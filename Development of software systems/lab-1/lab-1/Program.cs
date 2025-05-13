namespace MyApplication
{

    class Point
    {
        public Point(double x_value, double y_value)
        {
            x = x_value;
            y = y_value;
        }

        // Конструктор копирования
        public Point(Point other)
        {
            x = other.x;
            y = other.y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
        public override bool Equals(object obj)
        {
            if (obj is Point other)
            {
                const double epsilon = 1e-9;
                return Math.Abs(x - other.x) < epsilon && Math.Abs(y - other.y) < epsilon;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // Используем хэш-код от округлённых значений с учетом точности
            const double precision = 1e9;
            int hashX = (int)(x * precision);
            int hashY = (int)(y * precision);
            return hashX ^ hashY;
        }

        public double x { get; }
        public double y { get; }

    }

    class Segment
    {
        public Segment(double x1_value, double y1_value, double x2_value, double y2_value)
        {
            point_1 = new Point(x1_value, y1_value);
            point_2 = new Point(x2_value, y2_value);
        }
        public Segment(Point point_1_value, Point point_2_value)
        {
            point_1 = point_1_value;
            point_2 = point_2_value;
        }

        public Point point_1 { get; }
        public Point point_2 { get; }
    }

    class Rectangle
    {
        private Rectangle(Point point_1_value, Point point_2_value, Point point_3_value, Point point_4_value)
        {
            point_1 = point_1_value;
            point_2 = point_2_value;
            point_3 = point_3_value;
            point_4 = point_4_value;
        }

        private static bool CanCreate(Point p1, Point p2, Point p3, Point p4)
        {
            if (p1.Equals(p2) || p1.Equals(p3) || p1.Equals(p4) ||
                                 p2.Equals(p3) || p2.Equals(p4) ||
                                                  p3.Equals(p4))
            {
                return false; 
            }

            double d12 = MathGeometrySolver.Distance(p1, p2);             // стороны
            double d14 = MathGeometrySolver.Distance(p1, p4);
            double d23 = MathGeometrySolver.Distance(p2, p3);
            double d34 = MathGeometrySolver.Distance(p3, p4);

            double d13 = MathGeometrySolver.Distance(p1, p3);             // диагонали
            double d24 = MathGeometrySolver.Distance(p2, p4);


            bool is_opposite_sides_equal = MathGeometrySolver.IsEqual(d12, d34) &&
                                           MathGeometrySolver.IsEqual(d23, d14) &&
                                           MathGeometrySolver.IsEqual(d13, d24);

            bool is_angles90 = MathGeometrySolver.IsAngle90(p1, p2, p3) &&
                               MathGeometrySolver.IsAngle90(p2, p3, p4) &&
                               MathGeometrySolver.IsAngle90(p3, p4, p1) &&
                               MathGeometrySolver.IsAngle90(p4, p1, p2);

            return is_opposite_sides_equal && is_angles90;
        }

        // (static factory method)
        public static Rectangle Create(Point point_1_value, Point point_2_value, Point point_3_value, Point point_4_value)
        {
            if (!CanCreate(point_1_value, point_2_value, point_3_value, point_4_value)) return null;

            return new Rectangle(point_1_value, point_2_value, point_3_value, point_4_value);
        }
        public static Rectangle Create(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            Point p3 = new Point(x3, y3);
            Point p4 = new Point(x4, y4);

            return Create(p1, p2, p3, p4);
        }

        public Point point_1 { get; }
        public Point point_2 { get; }
        public Point point_3 { get; }
        public Point point_4 { get; }
    }

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


        static double InputNumFromSTDIN(string S)
        {
            var consoleInput = new ConsoleInputProvider();
            var consoleDoubleReader = new InputReader<double?>(consoleInput, new NullableDoubleParser());

            while (true)
            {
                double? consoleDouble = consoleDoubleReader.Read("Введите число: ");
                if (consoleDouble != null)
                {
                    return (double) consoleDouble;
                }

                Console.WriteLine("ошибка");
            }
        }

        static int InputNumFromSTDIN()
        {
            var consoleInput = new ConsoleInputProvider();
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




        static Rectangle ManualFillRectangle()
        {
            Rectangle rect = null;

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
                Point intersection = MathGeometrySolver.FindIntersection(seg, rectSegment,ref is_overlap);
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
            Console.WriteLine("Выберите способ заполнения:");
            Rectangle rect = null;
            Segment seg = null;
            bool exit = false;
            while (!exit)
            {
                int command = 0;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int)Сommands.file:
                        exit = true;
                        break;

                    case (int)Сommands.console:
                        rect = ManualFillRectangle();
                        seg = ManualFillSegment();
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
            // Проверяем, что фигуры были созданы
            if (rect == null || seg == null)
            {
                Console.WriteLine("Ошибка: фигуры не были созданы!");
                return;
            }
            var ss = IntersectionFinder(rect, seg);
            Console.WriteLine("Found intersections:");
            foreach (var point in ss)
            {
                Console.WriteLine(point);
            }


        }



    }

    static class MathGeometrySolver
    {
        public static double Distance(in Point p1, in Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }

        public static bool IsEqual( double a, double b, double epsilon = 0.0001)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static bool IsAngle90(in Point p1, in Point p2, in Point p3)
        {
            // Вектор от p2 к p1
            double dx1 = p1.x - p2.x;
            double dy1 = p1.y - p2.y;

            // Вектор от p2 к p3
            double dx2 = p3.x - p2.x;
            double dy2 = p3.y - p2.y;

            // Скалярное произведение векторов
            double dotProduct = dx1 * dx2 + dy1 * dy2;

            // Если скалярное произведение равно нулю, угол прямой
            return IsEqual(dotProduct, 0);
        }

        public static bool HasOverlap(in Segment s1, in Segment s2)
        {
            if (!AreColinear(s1, s2))
                return false;

            double minX1 = Math.Min(s1.point_1.x, s1.point_2.x);
            double maxX1 = Math.Max(s1.point_1.x, s1.point_2.x);
            double minX2 = Math.Min(s2.point_1.x, s2.point_2.x);
            double maxX2 = Math.Max(s2.point_1.x, s2.point_2.x);

            double minY1 = Math.Min(s1.point_1.y, s1.point_2.y);
            double maxY1 = Math.Max(s1.point_1.y, s1.point_2.y);
            double minY2 = Math.Min(s2.point_1.y, s2.point_2.y);
            double maxY2 = Math.Max(s2.point_1.y, s2.point_2.y);

            bool overlapX = maxX1 >= minX2 && maxX2 >= minX1;
            bool overlapY = maxY1 >= minY2 && maxY2 >= minY1;

            return overlapX && overlapY;
        }
        private static bool AreColinear(in Segment s1, in Segment s2)
        {
            return IsColinear(s1.point_1, s1.point_2, s2.point_1) &&
                   IsColinear(s1.point_1, s1.point_2, s2.point_2);
        }
        private static bool IsColinear(in Point a, in Point b, in Point c)
        {
            double cross = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
            return Math.Abs(cross) < 1e-8;
        }

        public static Point? FindIntersection(in Segment s1, in Segment s2,ref bool is_overlap)
        {
            double x1 = s1.point_1.x, y1 = s1.point_1.y;
            double x2 = s1.point_2.x, y2 = s1.point_2.y;
            double x3 = s2.point_1.x, y3 = s2.point_1.y;
            double x4 = s2.point_2.x, y4 = s2.point_2.y;

            double denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            // Если знаменатель 0, значит отрезки параллельны или совпадают
            if (Math.Abs(denominator) < 1e-10)
            {
                if (HasOverlap(s1, s2))
                {
                    Console.WriteLine("Отрезки накладываются");
                    is_overlap = true;
                }

                return null;
            }

            double t = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
            double u = ((x3 - x1) * (y2 - y1) - (y3 - y1) * (x2 - x1)) / denominator;

            // Если t и u в пределах [0, 1], значит отрезки пересекаются
            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                double x = x1 + t * (x2 - x1);
                double y = y1 + t * (y2 - y1);
                return new Point(x, y);
            }

            return null; // Отрезки не пересекаются
        }
    }
}






/* TO DO

рабочий прямоугольник

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