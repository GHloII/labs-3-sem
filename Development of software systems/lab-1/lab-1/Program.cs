namespace MyApplication
{

    class Point
    {
        public Point(int x_value,int y_value)
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

        public int x { get; }
       public int y { get; }
    }

    class Segment
    {
        public Segment(int x1_value, int y1_value, int x2_value, int y2_value)
        {
           point_1 = new Point(x1_value,y1_value);
           point_2 = new Point(x2_value,y2_value);
        }
        public Segment(Point point_1_value, Point point_2_value) // передеча по ссылке значений точек
        {
            point_1 = point_1_value;
            point_2 = point_2_value;
        }

        public Point point_1 { get; }
        public Point point_2 { get; }
    }

    class Rectangle
    {
        // сделать фактори конструктор


    }

    class Program
    {
        enum Сommands // возможно разделить по смыслу
        {
            start   = 1,
            exit    = 2,
            file    = 1,
            console = 2,
        }

        static void Main()
        {
            // show greets;
            int command = 0;
            while (true)
            {
                Console.WriteLine( "1 - запустить программу" +
                            "\n" + "2 - выйти из программы " );
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int) Сommands.start:
                        Console.WriteLine("запуск выполнен");
                        Menu();
                        break;
                    case (int) Сommands.exit:
                        Console.WriteLine("выход выполнен");
                        return;
                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }
        }


        static int InputNumFromSTDIN()
        {
            while (true)
            {
                string? str_num = Console.ReadLine();

                if (str_num != null && int.TryParse(str_num, out int num))
                {
                    return num;
                }

                Console.WriteLine("ошибка");
            }
        }

        static void Menu()
        {
            Console.WriteLine("Выберите способ заполнения:");

            bool exit = false;
            while (!exit)
            {
                int command = 0;
                Console.WriteLine("1 - заполнение  из файла\n2 - ввод из консоли\n3 - заполнение рандомными числами");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case (int) Сommands.file:

                        exit = true;
                        break;
                    case (int) Сommands.console:

                        exit = true;
                        break;
                    default:
                        Console.WriteLine("команда не найдена");
                        break;
                }
            }


        }



    }
}

/* TO DO
 *  class segment
 *  class прямоугольник
 *  2 функциии с пересечением отрезков и прямоугольника
 *  надо ли праверку на прямоугольность
 *  
 *  
  class Point
{
    public int X { get; }
    public int Y { get; }

    // Приватный конструктор
    private Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Статический метод для проверки
    public static bool CanCreate(int x, int y)
    {
        return x >= 0 && y >= 0;
    }

    // Фабричный метод для создания объекта с проверкой
    public static Point Create(int x, int y)
    {
        if (!CanCreate(x, y))
        {
            return null; // Возвращаем null, если нельзя создать объект
        }
        return new Point(x, y);
    }
}

Point p = Point.Create(-1, 5);
if (p != null)
{
    Console.WriteLine($"Создана точка с координатами: {p.X}, {p.Y}");
}
else
{
    Console.WriteLine("Не удалось создать точку.");
}


 * 
 * 
 * 
 */