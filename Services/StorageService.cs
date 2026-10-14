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

    // Entfernt eine Transaktion anhand ihrer Id aus den gespeicherten Dateien.
    public bool RemoveTransaction(Guid id)
    {
        string[] files = Directory.GetFiles(_dataDirectory, "*.json");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);

            List<Transaction> transactions =
                JsonSerializer.Deserialize<List<Transaction>>(json, _jsonOptions)
                ?? new List<Transaction>();

            var transaction = transactions.FirstOrDefault(t => t.Id == id);

            if (transaction is null)
            {
                continue;
            }

            transactions.Remove(transaction);

            string newJson = JsonSerializer.Serialize(transactions, _jsonOptions);
            File.WriteAllText(file, newJson);

            return true;
        }

        return false;
    }

}
