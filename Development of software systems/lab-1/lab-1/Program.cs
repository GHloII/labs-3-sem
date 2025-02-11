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






Если отрезки **пересекаются**, можно найти **координаты точки пересечения**.  

---

## **1. Уравнения отрезков**
Каждый отрезок можно представить в **параметрическом виде**:  
Пусть у нас есть два отрезка:
- **Первый отрезок**: \((x_1, y_1) \to (x_2, y_2)\)
- **Второй отрезок**: \((x_3, y_3) \to (x_4, y_4)\)

Параметрическое уравнение отрезка:  
\[
x = x_1 + t (x_2 - x_1)
\]
\[
y = y_1 + t (y_2 - y_1)
\]
где \( t \) меняется от 0 до 1.

Для второго отрезка аналогично:
\[
x = x_3 + u (x_4 - x_3)
\]
\[
y = y_3 + u (y_4 - y_3)
\]
где \( u \) меняется от 0 до 1.

---

## **2. Решение системы уравнений**
Равенство координат даёт систему:
\[
x_1 + t (x_2 - x_1) = x_3 + u (x_4 - x_3)
\]
\[
y_1 + t (y_2 - y_1) = y_3 + u (y_4 - y_3)
\]
Решаем относительно \( t \) и \( u \):

\[
t = \frac{(x_3 - x_1)(y_3 - y_4) - (y_3 - y_1)(x_3 - x_4)}
        {(x_1 - x_2)(y_3 - y_4) - (y_1 - y_2)(x_3 - x_4)}
\]

\[
u = \frac{(x_3 - x_1)(y_1 - y_2) - (y_3 - y_1)(x_1 - x_2)}
        {(x_1 - x_2)(y_3 - y_4) - (y_1 - y_2)(x_3 - x_4)}
\]

Если \( 0 \leq t \leq 1 \) и \( 0 \leq u \leq 1 \), то отрезки пересекаются.  
Подставляем найденное \( t \) в уравнение отрезка:

\[
x = x_1 + t (x_2 - x_1)
\]
\[
y = y_1 + t (y_2 - y_1)
\]

---

## **3. Код на Python**
```python
def find_intersection(x1, y1, x2, y2, x3, y3, x4, y4):
    """Находит точку пересечения двух отрезков, если она существует"""
    
    # Вычисляем знаменатель
    denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4)

    # Если знаменатель 0, значит отрезки параллельны или совпадают
    if denominator == 0:
        return None  

    # Вычисляем параметры t и u
    t = ((x3 - x1) * (y3 - y4) - (y3 - y1) * (x3 - x4)) / denominator
    u = ((x3 - x1) * (y1 - y2) - (y3 - y1) * (x1 - x2)) / denominator

    # Если t и u в пределах [0, 1], значит отрезки пересекаются
    if 0 <= t <= 1 and 0 <= u <= 1:
        # Вычисляем координаты точки пересечения
        x = x1 + t * (x2 - x1)
        y = y1 + t * (y2 - y1)
        return (x, y)

    return None  # Отрезки не пересекаются

# Примеры
print(find_intersection(1, 1, 5, 5, 1, 5, 5, 1))  # (3.0, 3.0)
print(find_intersection(1, 1, 5, 5, 6, 6, 10, 10))  # None (не пересекаются)
```

---

## **4. Разбор работы кода**
1. **Сначала проверяется, параллельны ли отрезки**. Если знаменатель \( 0 \), то пересечения нет.  
2. **Вычисляются параметры \( t \) и \( u \)** по формулам.  
3. **Если \( 0 \leq t \leq 1 \) и \( 0 \leq u \leq 1 \)**, значит, отрезки пересекаются, и мы считаем точку пересечения.  
4. **Если пересечения нет, возвращается `None`**.  

Этот алгоритм работает за **O(1)** и используется в компьютерной графике, геометрии, играх и системах навигации.
 * 
 * 
 * 
 */