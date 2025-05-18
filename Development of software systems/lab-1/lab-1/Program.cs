using System;
using System.IO;
namespace MyApplication
{

    class Program
    {
        enum Сommands // возможно разделить по смыслу
        {
            start = 1,
            exit = 2,
            file = 1,
            console = 2,
        }

        static void Main()
        {
            // show greets;
            int command = 0;
            while (true)
            {
                Console.WriteLine("1 - запустить программу" +
                                  "\n" +
                                  "2 - выйти из программы ");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.start:
                        Console.WriteLine("запуск выполнен");
                        Menu();
                        break;

                    case (int)Сommands.exit:
                        Console.WriteLine("выход выполнен");
                        return;

                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
        }

        static string InputFilePathFromSTDIN(string S, bool record = false)
        {
            ILineInputProvider consoleInput = new ConsoleInputProvider();
            var filePathReader = new InputReader<string>(consoleInput, new FilePathParser());

            

            while (true)
            {
                string? consoleString = filePathReader.Read(S);
                if (consoleString == null) {
                    Console.WriteLine("ошибка");
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

        static double InputNumFromSTDIN(string S)
        {
            ILineInputProvider consoleInput = new ConsoleInputProvider();
            var consoleDoubleReader = new InputReader<double?>(consoleInput, new NullableDoubleParser());

            while (true)
            {
                double? consoleDouble = consoleDoubleReader.Read("Введите число "+ S);
                if (consoleDouble != null)
                {
                    return (double) consoleDouble;
                }

                Console.WriteLine("ошибка");
            }
        }

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

        static void ClassToFile<T>( T obj, string filePath) where T : class
        {
            var file_out = new FileOutputProvider(filePath);

            if (obj is Rectangle rect)
            {
                file_out.WriteObject(rect.ToDto());
                return;
            }
            file_out.WriteObject(obj);
        }

        static void StringToFile(string filePath,string s)
        {
            var file_out = new FileOutputProvider(filePath);
            Console.WriteLine(s);
            file_out.WriteLine(s);
        }

        static object? FileToClass<T>(string filePath) where T : class
        {
            IInputProvider file_in = new FileInputProvider(filePath);
            T? obj = file_in.ReadObject<T>();

            return obj;
        }

        static Rectangle ManualFillRectangle()
        {
            Rectangle? rect = null;

            while (rect == null)
            {
                Console.WriteLine("Введите координаты прямоугольника по часовой стрелке");

                rect = Rectangle.Create(InputNumFromSTDIN("x1: "), InputNumFromSTDIN("y1: "),
                                        InputNumFromSTDIN("x2: "), InputNumFromSTDIN("y2: "),
                                        InputNumFromSTDIN("x3: "), InputNumFromSTDIN("y3: "),
                                        InputNumFromSTDIN("x4: "), InputNumFromSTDIN("y4: "));

                if (rect == null)
                    Console.WriteLine("Не возможно создать прямоугольник для данных точек \n");
            }
            return rect;
        }
        static Segment ManualFillSegment()
        {
            Console.WriteLine("Введите координаты отрезка");

            var seg = new Segment(InputNumFromSTDIN("x1: "), InputNumFromSTDIN("y1: "),
                                  InputNumFromSTDIN("x2: "), InputNumFromSTDIN("y2: "));

            return seg;
        }

        static HashSet<Point> IntersectionFinder(Rectangle rect, Segment seg)
        {
            var rectSegments = new List<Segment>
            {
                new Segment(rect.point_1, rect.point_2),
                new Segment(rect.point_2, rect.point_3),
                new Segment(rect.point_3, rect.point_4),
                new Segment(rect.point_4, rect.point_1)
            };

            var intersections = new HashSet<Point>();

            foreach (var rectSegment in rectSegments)
            {
                bool is_overlap = false;
                Point? intersection = MathGeometrySolver.FindIntersection(seg, rectSegment,ref is_overlap);
                if (intersection != null)
                {
                    intersections.Add(intersection);
                }
                if (is_overlap)
                {
                    intersections.Clear();
                    break;
                }
            }
            return intersections;
        }

        static void Menu()
        {
            Rectangle? rect = null;
            Segment? seg = null;
            bool exit = false;


            Console.WriteLine("Выберите способ заполнения Прямоугольника:");
            while (!exit)
            {
                int command = 0;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.file:
                        var s = InputFilePathFromSTDIN("Введите путь до файла: ");
                        if (s != null)
                        {
                            var dto = FileToClass<RectangleDto>(s);
                            if (dto != null)    
                            rect = Rectangle.FromDto((RectangleDto)dto);
                        }
                        exit = true;
                        break;

                    case (int)Сommands.console: 
                        rect = ManualFillRectangle();
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
            exit = false;
            
            Console.WriteLine("Выберите способ заполнения Отрезка:");
            while (!exit)
            {
                int command = 0;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.file:
                        var s = InputFilePathFromSTDIN("Введите путь до файла: ");
                        if (s != null)
                        {
                            seg = (Segment?)FileToClass<Segment>(s);
                        }
                        exit = true;
                        break;

                    case (int)Сommands.console: //возможно сделать проверку на не налл
                        seg = ManualFillSegment();
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
            exit = false;

            // Проверяем, что фигуры были созданы
            if (rect == null || seg == null)
            {
                Console.WriteLine("Ошибка: фигуры не были созданы!");
                return;
            }


            var ss = IntersectionFinder(rect, seg);
            string points_string="";
            Console.WriteLine("Found intersections:");
            foreach (var point in ss)
            {
                Console.WriteLine(point);
                points_string += point.ToString();
            }



            Console.WriteLine("\nСохранение результата:");
            while (!exit)
            {
                int command;
                Console.WriteLine("1 - сохранить прямоугольник в файл" +
                                "\n2 - сохранить отрезок в файл" +
                                "\n3 - сохранить результат в файл" +
                                "\n4 - не сохранять и продолжить"+
                                "\n5 - выход из программы");

                command = InputNumFromSTDIN();
                string? s;
                switch (command)
                {
                    case 1:
                        s = InputFilePathFromSTDIN("Введите путь до файла: ",true);
                        if (s != null)
                        {
                            ClassToFile(rect, s);
                        }
                        
                        break;

                    case 2:
                        s = InputFilePathFromSTDIN("Введите путь до файла: ", true);
                        if (s != null)
                        {
                            ClassToFile(seg, s);
                        }
                        break;

                    case 3:
                        s = InputFilePathFromSTDIN("Введите путь до файла: ", true);
                        if (s != null)
                        {
                            StringToFile( s,points_string);
                        }
                        break;

                    case 4:
                        exit = true;
                        break;

                    case 5:
                        exit = true;
                        return;

                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
            exit = false;

        }
    }
}






/* TO DO
проверка файлнейма на адекватность
рабочий прямоугольник

1
1
rect.txt
1
seg.txt





1
2

3
12
7
16
14
9
10
5

2
15
14
15




 */