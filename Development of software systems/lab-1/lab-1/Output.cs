// Интерфейсы вывода
using System.Text.Json;

public interface ILineOutputProvider
{
    void WriteLine(string text);
}

public interface IObjectOutputProvider
{
    void WriteObject<T>(T obj) where T : class;
}
// Объединяющий интерфейс вывода
public interface IOutputProvider : ILineOutputProvider, IObjectOutputProvider { }

// Реализация записи в файл
public class FileOutputProvider : IOutputProvider
{
    private readonly string _filePath;

    public FileOutputProvider(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteLine(string text)
    {
        File.AppendAllText(_filePath, text + Environment.NewLine);

    }

    public void WriteObject<T>(T obj) where T : class
    {

        string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
        
    }
}

// Реализация записи в консоль
public class ConsoleOutputProvider : ILineOutputProvider
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}