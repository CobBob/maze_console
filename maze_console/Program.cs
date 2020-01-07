using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Below is non-standard stuff
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maze_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Random randSeed = new Random();
            ConsoleKeyInfo keyboard_input;
            Maze newMaze;

            int MazeWidth = 5;
            int MazeLength = 7;

            char char_keyboard_input;
            bool flag_ExitGame = false;

            do
            {
                newMaze = new Maze(MazeWidth += 6, MazeLength += 14,
                        true, randSeed.Next(10000));

                while (flag_ExitGame == false && newMaze.flag_wincondition == false)
                {
                    Console.Clear();
                    newMaze.DisplayPlayField();

                    Console.WriteLine("Use w,a,s,d to move the pawn to the goal, " +
                        "press q to quit");

                    keyboard_input = Console.ReadKey();
                    char_keyboard_input = keyboard_input.KeyChar;
                    switch (char_keyboard_input)
                    {
                        case 'w':
                            {
                                newMaze.MovePawn("UP");
                                break;
                            }
                        case 'a':
                            {
                                newMaze.MovePawn("LEFT");
                                break;
                            }
                        case 's':
                            {
                                newMaze.MovePawn("DOWN");
                                break;
                            }
                        case 'd':
                            {
                                newMaze.MovePawn("RIGHT");
                                break;
                            }
                        case 'q':
                            {
                                flag_ExitGame = true;
                                break;
                            }
                        case '[':
                            {
                                newMaze.flag_wincondition = true;
                                break;
                            }
                    }
                }

                if (newMaze.flag_wincondition == true)
                {
                    Console.Clear();
                    //Console.SetCursorPosition(0, 0);
                    newMaze.DisplayPlayField();
                    Console.WriteLine("You Win");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                }
            } while (flag_ExitGame == false);
        }
    }
}
