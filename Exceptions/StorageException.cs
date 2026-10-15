namespace BudgetTracker.Exceptions;

// Wird ausgelöst, wenn beim Lesen oder Schreiben der Daten ein Fehler entsteht.
public class StorageException : Exception
{
    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
