using BudgetTracker.Models;
using BudgetTracker.Services;
using BudgetTracker.Exceptions;

var storageService = new StorageService("data");
var transactionService = new TransactionService(storageService);
var loggerService = new LoggerService();

transactionService.TransactionAdded += loggerService.OnTransactionAdded;

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("=== Budget Tracker ===");
    Console.WriteLine();
    Console.WriteLine("1 - Transaktion hinzufügen");
    Console.WriteLine("2 - Transaktion entfernen");
    Console.WriteLine("3 - Bericht anzeigen");
    Console.WriteLine("0 - Beenden");
    Console.WriteLine();

    Console.Write("Auswahl: ");
    string? input = Console.ReadLine();

    try
    {
        switch (input)
        {
        case "1":
            TransactionType type;

            while (true)
            {
                Console.Write("Typ (Income/Expense): ");
                string? typeInput = Console.ReadLine();

                if (Enum.TryParse(typeInput, true, out type))
                {
                    break;
                }

                Console.WriteLine("Ungültiger Typ.");
            }

            Console.Write("Beschreibung: ");
            string description = Console.ReadLine() ?? string.Empty;

            decimal amount;

            while (true)
            {
                Console.Write("Betrag: ");
                string? amountInput = Console.ReadLine();

                if (decimal.TryParse(amountInput, out amount) && amount > 0)
                {
                    break;
                }

                Console.WriteLine("Bitte einen gültigen positiven Betrag eingeben.");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.Now,
                Type = type,
                Description = description,
                Amount = amount
            };

            transactionService.AddTransaction(transaction);

            Console.WriteLine();
            Console.WriteLine("Transaktion erstellt:");
            Console.WriteLine($"{transaction.Type} - {transaction.Description} - {transaction.Amount:C}");
            break;

        case "2":
            Console.Write("Id der Transaktion: ");
            string? idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                Console.WriteLine("Ungültige Id.");
                break;
            }

            bool removed = storageService.RemoveTransaction(id);

            if (removed)
            {
                Console.WriteLine("Transaktion wurde entfernt.");
            }
            else
            {
                Console.WriteLine("Keine Transaktion mit dieser Id gefunden.");
            }

            break;

        case "3":
            Console.Write("Startdatum (yyyy-MM-dd): ");
            string? fromInput = Console.ReadLine();

            Console.Write("Enddatum (yyyy-MM-dd): ");
            string? toInput = Console.ReadLine();

            if (!DateOnly.TryParse(fromInput, out DateOnly from) ||
                !DateOnly.TryParse(toInput, out DateOnly to))
            {
                Console.WriteLine("Ungültiges Datum.");
                break;
            }

            var transactions = storageService
                .LoadTransactions(from, to)
                .ToList();

            Console.WriteLine();
            Console.WriteLine("Filter:");
            Console.WriteLine("0 - Alle");
            Console.WriteLine("1 - Income");
            Console.WriteLine("2 - Expense");
            Console.Write("Auswahl: ");

            string? filterInput = Console.ReadLine();

            if (filterInput == "1")
            {
                transactions = transactions
                    .Where(t => t.Type == TransactionType.Income)
                    .ToList();
            }
            else if (filterInput == "2")
            {
                transactions = transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .ToList();
            }
            else if (filterInput != "0")
            {
                Console.WriteLine("Ungültige Auswahl.");
                break;
            }

            Console.WriteLine();

            foreach (var item in transactions)
            {
                Console.WriteLine(
                    $"{item.Timestamp:yyyy-MM-dd} | {item.Type} | {item.Description} | {item.Amount:C}");
            }

            decimal income = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            decimal expenses = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            decimal balance = income - expenses;

            Console.WriteLine();
            Console.WriteLine($"Einnahmen: {income:C}");
            Console.WriteLine($"Ausgaben:  {expenses:C}");
            Console.WriteLine($"Saldo:     {balance:C}");

            break;

        case "0":
            running = false;
            break;

        default:
            Console.WriteLine("Ungültige Auswahl.");
            break;
        }
    }
    catch (StorageException ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Fehler: {ex.Message}");
    }
    catch (Exception)
    {
        Console.WriteLine();
        Console.WriteLine("Ein unerwarteter Fehler ist aufgetreten.");
    }

    if (running)
    {
        Console.WriteLine();
        Console.WriteLine("Enter drücken, um zum Menü zurückzukehren.");
        Console.ReadLine();
    }
}
