
using System.Text.Json.Serialization;

    public static class MathGeometrySolver
    {
        public static double Distance(in Point p1, in Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }

        public static bool IsEqual(double a, double b, double epsilon = 0.0001)
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

        public static Point? FindIntersection(in Segment s1, in Segment s2, ref bool is_overlap)
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

    public class Point
    {
        [JsonConstructor]
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
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
        public override bool Equals(object? obj)
        {
            if (obj is Point other && obj != null)
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

    public class Segment
    {
        public Segment(double x1_value, double y1_value, double x2_value, double y2_value)
        {
            point_1 = new Point(x1_value, y1_value);
            point_2 = new Point(x2_value, y2_value);
            if (point_1 == null || point_2 == null)
            {
                throw new ArgumentNullException(nameof(point_1));
            }
            if (point_2.Equals(point_1))
            {
            Console.WriteLine("Точки совпадают");
                    throw new InvalidOperationException();
            }
        }

        [JsonConstructor]
        public Segment(Point point_1, Point point_2) // сделать как в поинте а то не десериализуется
        {

            if (point_1 == null||point_2 == null)
            {
                throw new ArgumentNullException(nameof(point_1));
            }
            if (point_2.Equals(point_1))
            {
            Console.WriteLine("Точки совпадают");
                throw new InvalidOperationException();
            }

            this.point_1 = point_1;
            this.point_2 = point_2;
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
        public static Rectangle? Create(Point point_1_value, Point point_2_value, Point point_3_value, Point point_4_value)
        {
            if (!CanCreate(point_1_value, point_2_value, point_3_value, point_4_value)) return null;

            return new Rectangle(point_1_value, point_2_value, point_3_value, point_4_value);
        }
        public static Rectangle? Create(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            Point p3 = new Point(x3, y3);
            Point p4 = new Point(x4, y4);

            return Create(p1, p2, p3, p4);
        }

        public RectangleDto ToDto() =>
            new RectangleDto(point_1, point_2, point_3, point_4);

        public static Rectangle? FromDto(RectangleDto? dto)
        {
            if (dto == null) return null;

            return Create(dto.p1, dto.p2, dto.p3, dto.p4);
        }

        public override bool Equals(object? obj)
        {
            return obj is Rectangle rectangle &&
                   EqualityComparer<Point>.Default.Equals(point_1, rectangle.point_1) &&
                   EqualityComparer<Point>.Default.Equals(point_2, rectangle.point_2) &&
                   EqualityComparer<Point>.Default.Equals(point_3, rectangle.point_3) &&
                   EqualityComparer<Point>.Default.Equals(point_4, rectangle.point_4);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(point_1, point_2, point_3, point_4);
        }

        public Point point_1 { get; }
        public Point point_2 { get; }
        public Point point_3 { get; }
        public Point point_4 { get; }

        public override string ToString()
        {
            return $"Points: {point_1}, {point_2}, {point_3}, {point_4}";
        }
}

 record RectangleDto( //(serialization surrogate)
    Point p1,
    Point p2,
    Point p3,
    Point p4
);