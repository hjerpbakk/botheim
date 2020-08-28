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
        var blindAndForgetfull = new BlindAndForgetfull();
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
                    blindAndForgetfull.Move();
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
    class BlindAndForgetfull {
        readonly Random random;
        readonly Dictionary<string, string[]> legalStates;
        string prev;
        public BlindAndForgetfull() {
            random = new Random(42);
            prev = "W";
            legalStates = new Dictionary<string, string[]> {
                { "W", new [] { "N", "W", "S"} },
                { "N", new [] { "W", "E", "N"} },
                { "E", new [] { "N", "E", "S"} },
                { "S", new [] { "S", "E", "W"} }
            };
        }
        public void Move() {
            var move = random.Next(0, 3);
            Move(legalStates[prev][move]);
        }
        void Move(string direction) {
            prev = direction;
            Console.WriteLine(direction);
        }
    }
}