using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Made by Illedan for Hjerpbakk.
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]);
        int playercount = int.Parse(inputs[2]);
        int foodcount = int.Parse(inputs[3]);
        int myid = int.Parse(inputs[4]);

        Console.Error.WriteLine(width);
        Console.Error.WriteLine(height);
        Console.Error.WriteLine(playercount);
        Console.Error.WriteLine(foodcount);

        var bot = new TheAnimal(width, height);

        // game loop
        while (true)
        {
            int aliveplayers = int.Parse(Console.ReadLine());
            for (int i = 0; i < aliveplayers; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int id = int.Parse(inputs[0]);
                int score = int.Parse(inputs[1]);
                int size = int.Parse(inputs[2]);
                string snake = inputs[3];

                if (id == myid) {
                    bot.Move(snake);
                    Console.Error.WriteLine(snake);
                }
            }
            for (int i = 0; i < foodcount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            //Console.WriteLine("E");
        }
    }

    class TheAnimal {
        readonly Random random;
        readonly Dictionary<string, string[]> legalStates;

        readonly int width;
        readonly int height;

        string prev;

        public TheAnimal(int width, int height) {
            this.width = width;
            this.height = height;
            random = new Random(42);
            prev = "W";
            legalStates = new Dictionary<string, string[]> {
                { "W", new [] { "N", "W", "S"} },
                { "N", new [] { "W", "E", "N"} },
                { "E", new [] { "N", "E", "S"} },
                { "S", new [] { "S", "E", "W"} }
            };
        }

        public void Move(string snake) {
            var parts = snake.Split(',');
            var head = new Position(int.Parse(parts[0]), int.Parse(parts[1]));

            string candidateMove;
            do {
                var i = random.Next(0, 3);
                candidateMove = legalStates[prev][i];
            } while (WillMoveKillMe(candidateMove, head));
            DoMove(candidateMove);
        }

        bool WillMoveKillMe(string candidateMove, Position head) {
            switch(candidateMove) {
                case "W":
                    if (head.X - 1 < 0) {
                        return true;
                    }

                    break;
                case "N":
                    if (head.Y - 1 < 0) {
                        return true;
                    }

                    break;
                case "E":
                    if (head.X + 1 >= width) {
                        return true;
                    }

                    break;
                case "S":
                    if (head.Y + 1 >= height) {
                        return true;
                    }

                    break;
            }

            return false;
        }

        void DoMove(string direction) {
            prev = direction;
            Console.WriteLine(direction);
        }

        struct Position {
            public Position(int x, int y) {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}