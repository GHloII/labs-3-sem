// Интерфейсы

using System.Text.Json;

public interface ILineInputProvider
{
    string? ReadLine(string prompt = "");
}

public interface IObjectInputProvider
{
    T? ReadObject<T>(string prompt = "") where T : class; 
}

// Объединяющий интерфейс, наследующий оба
public interface IInputProvider : ILineInputProvider, IObjectInputProvider
{
}

public interface IParser<T>
{
  
    bool TryParse(string input, out T value);
}


// Реализация чтения из файла (поддерживает оба способа)
public class FileInputProvider : IInputProvider
{
    private readonly string _filePath;

    public FileInputProvider(string filePath)
    {
        _filePath = filePath;
    }

    public string? ReadLine(string prompt = "")
    {
        if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
        using var reader = new StreamReader(_filePath);
        return reader.ReadLine();  // читаем только первую строку
    }

    public T? ReadObject<T>(string prompt = "") where T : class
    {
        try
        {
            if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception)
        {
            Console.WriteLine($"Ошибка чтения JSON");
            return null;
        }
    }
}


// Реализация чтения из консоли
public class ConsoleInputProvider : ILineInputProvider
{
    public string? ReadLine(string prompt = "")
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            Console.Write(prompt);
        }
        return Console.ReadLine();
    }
    public T? ReadObject<T>(string prompt = "") where T : class
    {
        Console.WriteLine("ReadObject не поддерживается для консоли.");
        return null;
    }
}

// Парсеры
public class IntParser : IParser<int>
{
    public bool TryParse(string input, out int value)
    {
        return int.TryParse(input, out value);
    }
}

public class DoubleParser : IParser<double>
{
    public bool TryParse(string input, out double value)
    {
        return double.TryParse(input, out value);
    }
}
public class NullableDoubleParser : IParser<double?>
{
    public bool TryParse(string input, out double? value)
    {
        bool result = double.TryParse(input, out double temp);
        value = result ? temp : null;
        return result;
    }
}
public class NullableIntParser : IParser<int?>
{
    public bool TryParse(string input, out int? value)
    {
        bool result = int.TryParse(input, out int temp);
        value = result ? temp : null;
        return result;
    }
}

public class FilePathParser : IParser<string>
{
    public bool TryParse(string input, out string value)
    {
        value = input.Trim();

        // Проверка существования директории
        string? dir = Path.GetDirectoryName(value);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Console.WriteLine("Указанная директория не существует.");
            return false;
        }

        try
        {
            // Попытка открыть (или создать) файл для записи в режиме Append и тут же закрыть
            using (var stream = new FileStream(value, FileMode.OpenOrCreate, FileAccess.Write))
            {

                stream.Close(); 
            }

            //value = Path.GetFullPath(value);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа для записи в файл.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка проверки пути: {ex.Message}");
            return false;
        }
    }
}


// Универсальный InputReader<T> это мы вообще не трогаем сделать только для строк наверно
public class InputReader<T>
{
    private readonly ILineInputProvider inputProvider_;
    private readonly IParser<T> parser_;

    public InputReader(ILineInputProvider inputProvider, IParser<T> parser)
    {
        inputProvider_ = inputProvider;
        parser_ = parser;
    }

    public T? Read(string prompt = "")
    {
        
        string? input = inputProvider_.ReadLine(prompt);

        if (input != null && parser_.TryParse(input, out T value))
        {
            return value;
        }

        
        return default!;
    }


}


