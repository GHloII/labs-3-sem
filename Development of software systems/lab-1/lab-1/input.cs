// Интерфейсы
public interface IInputProvider
{
    string? ReadLine(string prompt = "");
}

public interface IParser<T>
{
  
    bool TryParse(string input, out T value);
}

// Реализация чтения из файла
public class FileInputProvider : IInputProvider
{
    private readonly StreamReader _reader;
    private int _lineNumber = 0;

    public FileInputProvider(string filePath)
    {
        _reader = new StreamReader(filePath);
    }

    public string? ReadLine(string prompt = "")
    {
        if (!_reader.EndOfStream)
        {
            _lineNumber++;
            return _reader.ReadLine();
        }
        return null; // Или выбросить исключение
    }
}

// Реализация чтения из консоли
public class ConsoleInputProvider : IInputProvider
{
    public string? ReadLine(string prompt = "")
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            Console.Write(prompt);
        }
        return Console.ReadLine();
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

// Универсальный InputReader<T> это мы вообще не трогаем
public class InputReader<T>
{
    private readonly IInputProvider _inputProvider;
    private readonly IParser<T> _parser;

    public InputReader(IInputProvider inputProvider, IParser<T> parser)
    {
        _inputProvider = inputProvider;
        _parser = parser;
    }

    public T? Read(string prompt = "")
    {
        
        string? input = _inputProvider.ReadLine(prompt);

        if (input != null && _parser.TryParse(input, out T value))
        {
            return value;
        }

        
        return default!;
    }


}


