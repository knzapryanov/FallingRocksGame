using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FallingRocks
{
    class FallingRocks
    {
        struct Position
        {
            public Position(int row, int col)
            {
                this.Row = row;
                this.Col = col;
            }

            public int Row;
            public int Col;
        }

        static void Main(string[] args)
        {
            Console.BufferHeight = Console.WindowHeight;
            Position[] dwarf = new Position[3];
            for (int i = 0; i < 3; i++)
            {
                dwarf[i].Row = Console.WindowHeight - 1;
                dwarf[i].Col = (Console.WindowWidth / 2) - 3 + i;
            }

            Console.SetCursorPosition(dwarf[0].Col, dwarf[0].Row);
            Console.Write("(");
            Console.SetCursorPosition(dwarf[1].Col, dwarf[1].Row);
            Console.Write("O");
            Console.SetCursorPosition(dwarf[2].Col, dwarf[2].Row);
            Console.Write(")");

            Position[] directions = new Position[]
            {
                new Position(0, 1),  // right
                new Position(0, -1),  // left
                new Position(0, 0)   // neutral
            };
            byte right = 0;
            byte left = 1;
            byte neutral = 2;

            int direction = neutral;

            Random randomGenerator = new Random();
            List<Position> obstaclesPositions = new List<Position>();
            obstaclesPositions.Add(new Position(0, randomGenerator.Next(Console.WindowWidth - 3)));

            foreach (var obstacle in obstaclesPositions)
            {
                Console.SetCursorPosition(obstacle.Col, obstacle.Row);
                Console.Write("+");
            }

            int score = 0;
            double sleepTime = 130;
            while (true)
            {

                int dwarfLeftMostSymbolCol = dwarf[0].Col + directions[direction].Col;
                int dwarfRightMostSymbolCol = dwarf[2].Col + directions[direction].Col;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (dwarfRightMostSymbolCol < Console.WindowWidth - 3)
                        {
                            direction = right;
                        }
                        Console.SetCursorPosition(dwarf[0].Col, dwarf[0].Row);
                        Console.Write(" ");
                    }
                    else if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (dwarfLeftMostSymbolCol > 0)
                        {
                            direction = left;
                        }
                        Console.SetCursorPosition(dwarf[2].Col, dwarf[2].Row);
                        Console.Write(" ");
                    }
                }
                    dwarf[0].Col += directions[direction].Col;
                    dwarf[1].Col += directions[direction].Col;
                    dwarf[2].Col += directions[direction].Col;

                    Console.SetCursorPosition(dwarf[0].Col, dwarf[0].Row);
                    Console.Write("(");
                    Console.SetCursorPosition(dwarf[1].Col, dwarf[1].Row);
                    Console.Write("O");
                    Console.SetCursorPosition(dwarf[2].Col, dwarf[2].Row);
                    Console.Write(")");

                direction = neutral;

                for (int i = 0; i < obstaclesPositions.Count; i++)
                {
                    if (obstaclesPositions[i].Row <= Console.WindowHeight - 1)
                    {
                        Console.SetCursorPosition(obstaclesPositions[i].Col, obstaclesPositions[i].Row);
                        Console.Write(" ");
                        // Create new position for the obstacle which is the same col but one row down to generate movement
                        obstaclesPositions[i] = new Position(obstaclesPositions[i].Row + 1, obstaclesPositions[i].Col);
                    }
                    else
                    {
                        obstaclesPositions.RemoveAt(i);
                    }
                }
                                
                obstaclesPositions.Add(new Position(0, randomGenerator.Next(Console.WindowWidth - 3)));

                foreach (var obstacle in obstaclesPositions)
                {
                    if (dwarf.Contains(obstacle))
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Game over noob!");
                        Console.WriteLine("Your score is: {0}", score);
                        Console.WriteLine("Obstacles count: {0}", obstaclesPositions.Count);
                        Thread.Sleep(1000);
                        return;
                    }
                    else if (obstacle.Row <= Console.WindowHeight - 1)
                    {
                        Console.SetCursorPosition(obstacle.Col, obstacle.Row);
                        Console.Write("+");                        
                    }
                }

                score++;
                Thread.Sleep((int)sleepTime);
                // Lower the sleepTime to increse the difficulty
                sleepTime -= 0.1;
                // Also on each 30 points
                if (score % 30 == 0)
                {
                    // Add score/30 obstacles wave
                    for (int i = 0; i < score / 30; i++)
                    {
                        obstaclesPositions.Add(new Position(0, randomGenerator.Next(Console.WindowWidth - 3)));
                    }
                }
            }
        }
    }
}
