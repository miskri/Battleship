using System;

namespace ConsoleApp
{
    public class ConsoleApplication
    {
        private static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            // Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight); 
            MenuManager menuManager = new MenuManager();
            menuManager.Start();
        }
    }
}