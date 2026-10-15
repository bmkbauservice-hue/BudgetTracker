using BudgetTracker.Events;
using BudgetTracker.Models;

namespace BudgetTracker.Services;

public class TransactionService
{
    private readonly StorageService _storageService;

    public event EventHandler<TransactionAddedEventArgs>? TransactionAdded;

    public TransactionService(StorageService storageService)
    {
        _storageService = storageService;
    }

    public void AddTransaction(Transaction transaction)
    {
        _storageService.SaveTransaction(transaction);

        TransactionAdded?.Invoke(
            this,
            new TransactionAddedEventArgs(transaction));
    }
}
