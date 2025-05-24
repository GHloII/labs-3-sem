namespace MyApplication
{
    internal class ProgramInputOutput
    {

        public static int InputNumFromSTDIN()
        {
            ILineInputProvider consoleInput = new ConsoleInputProvider();
            var consoleIntReader = new InputReader<int?>(consoleInput, new NullableIntParser());

            while (true)
            {
                int? value = consoleIntReader.Read("Введите число: ");
                if (value != null)
                {
                    return (int)value;
                }

                Console.WriteLine("ошибка");
            }
        }

        public static void ClassToFile<T>(T obj, string filePath) where T : class
        {
            var file_out = new FileOutputProvider(filePath);

            if (obj is Rectangle rect)
            {
                file_out.WriteObject(rect.ToDto());
                return;
            }
            file_out.WriteObject(obj);
        }

        public static object? FileToClass<T>(string filePath) where T : class
        {
            IInputProvider file_in = new FileInputProvider(filePath);
            T? obj = file_in.ReadObject<T>();

            return obj;
        }

        public static string InputFilePathFromSTDIN(string S, bool record = false)
        {
            ILineInputProvider consoleInput = new ConsoleInputProvider();
            var filePathReader = new InputReader<string>(consoleInput, new FilePathParser());



            while (true)
            {
                string? consoleString = filePathReader.Read(S);
                if (WindowsNameValidator.IsInvalidWindowsName(consoleString, out var reason))
                {
                    Console.WriteLine(reason);
                    Console.WriteLine("Ошибка, введите данные заново");
                    continue;
                }
                FileInfo fileInfo = new FileInfo(consoleString);

                if (record && fileInfo.Length > 0)
                {

                    Console.WriteLine("В файле есть данные! Перезаписать файл?\n 1 - перезаписать \n2 - не перезаписывать");
                    var command = InputNumFromSTDIN();

                    switch (command)
                    {
                        case 1:
                            Console.WriteLine("Файл будет перезаписан");
                            break;

                        case 2:
                            continue;


                        default:
                            Console.WriteLine("команда не найдена");
                            break;
                    }
                }

                return consoleString;
            }
        }


        public static double InputNumFromSTDIN(string S)
        {
            ILineInputProvider consoleInput = new ConsoleInputProvider();
            var consoleDoubleReader = new InputReader<double?>(consoleInput, new NullableDoubleParser());

            while (true)
            {
                double? consoleDouble = consoleDoubleReader.Read("Введите число " + S);
                if (consoleDouble != null)
                {
                    return (double)consoleDouble;
                }

                Console.WriteLine("ошибка");
            }
        }

        public static void StringToFile(string filePath, string s)
        {
            var file_out = new FileOutputProvider(filePath);
            Console.WriteLine(s);
            file_out.WriteLine(s);
        }
    }
}