
namespace MyApplication
{
    class Program  
    {
        enum Сommands 
        {
            start = 1,
            exit = 2,
            file = 1,
            console = 2,
        }
        enum SaveOption
        {
            SaveRectangle = 1,
            SaveSegment = 2,
            SaveResult = 3,
            SkipAndContinue = 4,
            Exit = 5
        }


        static void Main()
        {
            Greeting_Text();
            int command;
            while (true)
            {
                Console.WriteLine("1 - запустить программу" +
                                  "\n" +
                                  "2 - выйти из программы ");
                command = ProgramInputOutput.InputNumFromSTDIN();

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
        public static void Greeting_Text()
        {
            Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
            Console.WriteLine("|Автор программы:".PadRight(129) + "|");
            Console.WriteLine("|\tИванов Глеб Игоревич".PadRight(123) + "|");
            Console.WriteLine("|\tГруппа №433".PadRight(123) + "|");
            Console.WriteLine("|\tВариант №10".PadRight(123) + "|");
            Console.WriteLine("|Назначение программы:".PadRight(129) + "|");
            Console.WriteLine("|\tДля заданного прямоугольника и отрезка на плоскости определить, пересекаются ли они.Найти координаты точек пересечения.  | ");
            Console.WriteLine("|________________________________________________________________________________________________________________________________|\n");
        }

        static Rectangle ManualFillRectangle()
        {
            Rectangle? rect = null;

            while (rect == null)
            {
                Console.WriteLine("Введите координаты прямоугольника по часовой стрелке");

                rect = Rectangle.Create(ProgramInputOutput.InputNumFromSTDIN("x1: "), ProgramInputOutput.InputNumFromSTDIN("y1: "),
                                        ProgramInputOutput.InputNumFromSTDIN("x2: "), ProgramInputOutput.InputNumFromSTDIN("y2: "),
                                        ProgramInputOutput.InputNumFromSTDIN("x3: "), ProgramInputOutput.InputNumFromSTDIN("y3: "),
                                        ProgramInputOutput.InputNumFromSTDIN("x4: "), ProgramInputOutput.InputNumFromSTDIN("y4: "));

                if (rect == null)
                    Console.WriteLine("Не возможно создать прямоугольник для данных точек \n");
            }
            return rect;
        }
        static Segment? ManualFillSegment()
        {
            Segment seg;
            Console.WriteLine("Введите координаты отрезка");
            try
            {
                seg = new Segment(ProgramInputOutput.InputNumFromSTDIN("x1: "), ProgramInputOutput.InputNumFromSTDIN("y1: "),
                                      ProgramInputOutput.InputNumFromSTDIN("x2: "), ProgramInputOutput.InputNumFromSTDIN("y2: "));
            }
            catch (Exception)
            {
                return null;
            }
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


            while (!exit)
            {
            Console.WriteLine("Выберите способ заполнения Прямоугольника:");
                int command = 0;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли");
                command = ProgramInputOutput.InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.file:
                        var s = ProgramInputOutput.InputFilePathFromSTDIN("Введите путь до файла: ");
                        if (s != null)
                        {
                            var dto = ProgramInputOutput.FileToClass<RectangleDto>(s);
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
                // Проверяем, что фигуры были созданы
                if (rect == null )
                {
                    Console.WriteLine("Ошибка: Прямоугольник не был создан!");
                    exit = false;

                }
            }
            exit = false;
            
            while (!exit)
            {
            Console.WriteLine("Выберите способ заполнения Отрезка:");
                int command;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли");
                command = ProgramInputOutput.InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.file:
                        var s = ProgramInputOutput.InputFilePathFromSTDIN("Введите путь до файла: ");
                        if (s != null)
                        {
                            seg = (Segment?)ProgramInputOutput.FileToClass<Segment>(s);
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
                // Проверяем, что фигуры были созданы
                if (rect == null || seg == null)
                {
                    Console.WriteLine("Ошибка: Отрезок не был создан!");
                    exit = false;

                }

            }
            exit = false;


            var ss = IntersectionFinder(rect, seg);
            string points_string = "";
            Console.WriteLine("Found intersections:");
            foreach (var point in ss)
            {
                Console.WriteLine(point);
                points_string += point.ToString();
            }



            Console.WriteLine("\nСохранение результата:");
            while (!exit)
            {
                Console.WriteLine("1 - сохранить прямоугольник в файл" +
                                  "\n2 - сохранить отрезок в файл" +
                                  "\n3 - сохранить результат в файл" +
                                  "\n4 - выход из программы");

                var command = ProgramInputOutput.InputNumFromSTDIN();
                string? s;
                switch ((SaveOption)command)
                {
                    case SaveOption.SaveRectangle:
                        s = ProgramInputOutput.InputFilePathFromSTDIN("Введите путь до файла: ", true);
                        if (s != null)
                        {
                            ProgramInputOutput.ClassToFile(rect, s);
                        }
                        break;

                    case SaveOption.SaveSegment:
                        s = ProgramInputOutput.InputFilePathFromSTDIN("Введите путь до файла: ", true);
                        if (s != null)
                        {
                            ProgramInputOutput.ClassToFile(seg, s);
                        }
                        break;

                    case SaveOption.SaveResult:
                        s = ProgramInputOutput.InputFilePathFromSTDIN("Введите путь до файла: ", true);
                        if (s != null)
                        {
                            ProgramInputOutput.StringToFile(s, points_string);
                        }
                        break;

                    case SaveOption.SkipAndContinue:
                        exit = true;
                        break;

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