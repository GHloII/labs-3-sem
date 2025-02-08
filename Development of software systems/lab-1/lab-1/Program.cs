using System;

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
                        //menu();
                        break;
                    case 2:
                        Console.WriteLine("выход выполнен");
                        return;
                    default:
                        Console.WriteLine("команда не найдена");;
                        break;
                }
            }
        }


        static int InputNumFromSTDIN()
        {

            while (true) {
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

        }

        

    }
}