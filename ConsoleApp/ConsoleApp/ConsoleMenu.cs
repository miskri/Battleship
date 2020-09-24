using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class ConsoleMenu
    {
        private int MenuLength = 25;

        public void Start()
        {
            MenuLevel currentLevel = new MenuLevel("Main Menu", MenuLevelsContainer.GetSubmenuList("Main Menu"));
            RenderMenu(currentLevel, 0);
        }

        private void RenderMenu(MenuLevel level, int selected)
        {
            List<string> submenuList = level.SubmenuList;
            Console.Clear();
            Console.WriteLine(new string('=', MenuLength));
            for (int i = 0; i < submenuList.Count; i++)
            {
                if (i == selected)
                {
                    Console.Write("|");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" " + submenuList[i] + new string(' ', MenuLength - 3 - submenuList[i].Length));
                    Console.ResetColor();
                    Console.WriteLine("|");
                    continue;
                }

                Console.WriteLine("| " + submenuList[i] + new string(' ', MenuLength - 3 - submenuList[i].Length) + "|");
            }

            Console.WriteLine(new string('=', MenuLength));
            Console.SetCursorPosition(MenuLength - 2, selected + 1);
            EventListener(level, selected, submenuList.Count);
        }

        private void EventListener(MenuLevel level, int selected, int submenuCount)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow when selected - 1 >= 0:
                    RenderMenu(level, --selected);
                    break;

                case ConsoleKey.DownArrow when selected + 1 < submenuCount:
                    RenderMenu(level, ++selected);
                    break;

                case ConsoleKey.Enter:
                    if (level.SubmenuList[selected] == "Exit")
                    {
                        Console.Clear();
                    }
                    else if (level.SubmenuList[selected] == "Back")
                    {
                        level.SetTitle(level.PreviousMenu[^1]);
                        level.RemoveLastPreviousMenu();
                        level.SetSubmenuList(MenuLevelsContainer.GetSubmenuList(level.LevelTitle));
                        RenderMenu(level, 0);
                    }
                    else if (MenuLevelsContainer.GetSubmenuList(level.SubmenuList[selected]) != null)
                    {
                        if (level.SubmenuList[selected] == "Main Menu")
                        {
                            level.ClearPreviousMenu();
                        }
                        else
                        {
                            level.AddPreviousMenu(level.LevelTitle);
                        }
                        level.SetTitle(level.SubmenuList[selected]);
                        level.SetSubmenuList(MenuLevelsContainer.GetSubmenuList(level.LevelTitle));
                        RenderMenu(level, 0);
                    }
                    else
                    {
                        EventListener(level, selected, submenuCount);
                    }
                    break;

                default:
                    EventListener(level, selected, submenuCount);
                    break;
            }
        }
    }
}