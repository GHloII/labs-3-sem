namespace MyApplication
{
    class Program

    {

        static void Main(string[] args)
        {
            // show greets;
            int command;
            while (true)
            {
                Console.WriteLine("1 - запустить программу" +
                                "\n2 - выйти из программы  ");
                command = InputNumFromSTDIN();

                switch (command)
                {
                    case 1:
                        Console.WriteLine("запуск выполнен");
                        Menu();
                        break;
                    case 2:
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
                    case 1:

                        exit = true;
                        break;
                    case 2:

                        exit = true;
                        break;
                    case 3:

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
 * 
 */