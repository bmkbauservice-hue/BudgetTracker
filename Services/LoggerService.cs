using BudgetTracker.Events;

namespace BudgetTracker.Services;

public class LoggerService
{
    public void OnTransactionAdded(object? sender, TransactionAddedEventArgs e)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"Log: {e.Transaction.Timestamp:yyyy-MM-dd HH:mm} | " +
            $"{e.Transaction.Type} | " +
            $"{e.Transaction.Description} | " +
            $"{e.Transaction.Amount:C}");
    }
}
