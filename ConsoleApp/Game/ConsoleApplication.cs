using System;
using System.Runtime.InteropServices;
using ConsoleApp.Control;

namespace ConsoleApp
{
    public static class ConsoleApplication
    {
        private static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // required for correct display of ♦ symbol
            Console.CursorVisible = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // Unix can't resize window
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            }
            MenuManager menuManager = new MenuManager();
            menuManager.Start();
        }
    }
}