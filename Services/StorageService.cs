using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetTracker.Models;

namespace BudgetTracker.Services;

public class StorageService
{
    private readonly string _dataDirectory;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public StorageService(string dataDirectory)
    {
        _dataDirectory = dataDirectory;
        Directory.CreateDirectory(_dataDirectory);
    }

    // Speichert eine Transaktion in der JSON-Datei des jeweiligen Tages.
    public void SaveTransaction(Transaction transaction)
    {
        string fileName = $"{transaction.Timestamp:yyyy-MM-dd}.json";
        string filePath = Path.Combine(_dataDirectory, fileName);

        List<Transaction> transactions = new();

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            transactions =
                JsonSerializer.Deserialize<List<Transaction>>(json, _jsonOptions)
                ?? new List<Transaction>();
        }

        transactions.Add(transaction);

        string newJson = JsonSerializer.Serialize(transactions, _jsonOptions);
        File.WriteAllText(filePath, newJson);
    }
}
