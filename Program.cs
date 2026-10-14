using BudgetTracker.Models;

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

            Console.WriteLine();
            Console.WriteLine("Transaktion erstellt:");
            Console.WriteLine($"{transaction.Type} - {transaction.Description} - {transaction.Amount:C}");
            break;

        case "2":
            Console.WriteLine("Transaktion entfernen kommt später.");
            break;

        case "3":
            Console.WriteLine("Bericht kommt später.");
            break;

        case "0":
            running = false;
            break;

        default:
            Console.WriteLine("Ungültige Auswahl.");
            break;
    }

    if (running)
    {
        Console.WriteLine();
        Console.WriteLine("Enter drücken, um zum Menü zurückzukehren.");
        Console.ReadLine();
    }
}
