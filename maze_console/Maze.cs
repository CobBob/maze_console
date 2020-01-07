using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace maze_console
{
    class Maze
    {

        private int mazeWidht;
        private int mazeLength;
        private char[,] playField;

        private Random randInt;

        //used for determining if it is possible to break a wall
        private readonly int[,] allPossibleDirections = 
            { {0, 0, 1, -1 },{1, -1, 0, 0 } };
        private int xPosAdjAt1;
        private int yPosAdjAt1;
        private int xPosAdjAt2;
        private int yPosAdjAt2;

        //Used for determining if the stack gets too high
        private int currentStack = 0;
        public int biggestStack = 0;

        //Characters used to display various items
        private char chaOpen = '\u0020';
        private char chaWall = '\u2588';
        private char chaHead = 'O';
        private char chaPawn = 'P';
        private char chaGoal = 'G';

        //Used for the location of the goal and the Pawn
        private int xPos_Pawn;
        private int yPos_Pawn;
        private int xPos_Goal;
        private int yPos_Goal;
        public bool flag_wincondition;

        private bool flag_ShowGeneration;

        //private char chaOpen = '0';
        //private char chaWall = '1';

        public Maze(int amazeWidht, int amazeLength, bool aflag_ShowGeneration, int aSeed)
        {
            flag_ShowGeneration = aflag_ShowGeneration;
            flag_wincondition = false;

            mazeWidht = amazeWidht;
            mazeLength = amazeLength;
            playField = new char[mazeWidht, mazeLength];
            randInt = new Random(aSeed);

            for (int i = 0; i < mazeWidht; i++)
            {
                for (int j = 0; j < mazeLength; j++)
                {
                    playField[i, j] = chaWall;
                }
            }
            WallBreaker(1, 1);
            xPos_Pawn = 1;
            yPos_Pawn = 1;
            xPos_Goal = mazeWidht  - 2;
            yPos_Goal = mazeLength - 2;
            playField[xPos_Pawn, yPos_Pawn] = chaPawn;
            playField[xPos_Goal, yPos_Goal] = chaGoal;
        }

        private void WallBreaker(int xPos, int yPos)
        {
            //set the current location to accessable
            playField[xPos, yPos] = chaOpen;

            if(flag_ShowGeneration == true)
            {
                playField[xPos, yPos] = chaHead;
                DisplayPlayField();
                playField[xPos, yPos] = chaOpen;
                Thread.Sleep(10);
            }

            int[] indexPosWalls;
            int selectedDirection;
            int xPos_NextToBreak;
            int yPos_NextToBreak;
            
            do
            {
                //Chooses a wall from the available lists
                //get the list of index directions of walls that can be broken
                indexPosWalls = CreateListAvaialableWalls(xPos, yPos);
                
                biggestStack = Math.Max(biggestStack, currentStack);

                //if there are walls that can be broken
                if (indexPosWalls.Length > 0)
                {
                    //if there are more than 2 walls a selection can be made
                    if(indexPosWalls.Length == 1)
                    {
                        selectedDirection = indexPosWalls[0];
                    }
                    else
                    {
                        selectedDirection = indexPosWalls[
                            randInt.Next(0, indexPosWalls.Length)];
                    }
                    xPos_NextToBreak = xPos + allPossibleDirections[0, selectedDirection];
                    yPos_NextToBreak = yPos + allPossibleDirections[1, selectedDirection];
                    playField[xPos_NextToBreak, yPos_NextToBreak] = chaOpen;

                    xPos_NextToBreak += allPossibleDirections[0, selectedDirection];
                    yPos_NextToBreak += allPossibleDirections[1, selectedDirection];


                    
                    if (currentStack >= 1500)
                    {
                        return;
                    }
                    else
                    {
                        currentStack++;
                        WallBreaker(xPos_NextToBreak, yPos_NextToBreak);
                        currentStack--;
                    }
                }
            } while (indexPosWalls.Length-1 > 0);
        }

        public void DisplayPlayField()
        {
            //Console.Clear();
            //Displays the playfield
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            string stringToDisplay = "";
            for (int i = 0; i < mazeWidht; i++)
            {
                for (int j = 0; j < mazeLength; j++)
                {
                    stringToDisplay = stringToDisplay + playField[i, j];
                }
                stringToDisplay = stringToDisplay + "\n";
            }
            Console.WriteLine(stringToDisplay);
        }

        private int[] CreateListAvaialableWalls(int xPos,int yPos)
        {
            List<int> indexValidWalls = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                xPosAdjAt1 = xPos + allPossibleDirections[0, i];
                yPosAdjAt1 = yPos + allPossibleDirections[1, i];

                xPosAdjAt2 = xPos + allPossibleDirections[0, i] 
                    + allPossibleDirections[0, i];
                yPosAdjAt2 = yPos + allPossibleDirections[1, i]
                    + allPossibleDirections[1, i];


                if (xPosAdjAt2 >= 0 && xPosAdjAt2 < mazeWidht &&
                        yPosAdjAt2 >= 0 && yPosAdjAt2 < mazeLength )
                {
                    //Console.WriteLine(xPosAdjAt1);
                    //Console.WriteLine(yPosAdjAt1);
                    //Console.WriteLine(xPosAdjAt2);
                    //Console.WriteLine(yPosAdjAt2);
                    if (playField[xPosAdjAt1, yPosAdjAt1] == chaWall &&
                        playField[xPosAdjAt2, yPosAdjAt2] == chaWall)
                    {
                        indexValidWalls.Add(i);
                    }
                }
            }
            int[] result = indexValidWalls.ToArray();
            return result;
        }
        public void MovePawn(string moveDirection)
        {
            playField[xPos_Pawn, yPos_Pawn] = chaOpen;
            switch (moveDirection)
            {
                case "UP":
                    if(playField[xPos_Pawn - 1, yPos_Pawn] != chaWall)
                    {
                        xPos_Pawn--;
                    }
                    break;
                case "DOWN":
                    if (playField[xPos_Pawn + 1, yPos_Pawn] != chaWall)
                    {
                        xPos_Pawn++;
                    }
                    break;
                case "LEFT":
                    if (playField[xPos_Pawn , yPos_Pawn - 1] != chaWall)
                    {
                        yPos_Pawn--;
                    }
                    break;
                case "RIGHT":
                    if (playField[xPos_Pawn, yPos_Pawn + 1] != chaWall)
                    {
                        yPos_Pawn++;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input in 'MovePawn' method");
                    Console.ReadLine();
                    break;
            }
            playField[xPos_Pawn, yPos_Pawn] = chaPawn;

            if (xPos_Pawn == xPos_Goal && yPos_Pawn == yPos_Goal)
            {
                flag_wincondition = true;
            }

        }
    }
}