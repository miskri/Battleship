using System;

namespace ConsoleApp
{
    public class ConsoleApplication
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleMenu menu = new ConsoleMenu();
            menu.Start();
        }
    }
}