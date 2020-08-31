using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/**
 * Made by Illedan for Hjerpbakk.
 **/
class Player {
    static void Main(string[] args) {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]);
        int playercount = int.Parse(inputs[2]);
        int foodcount = int.Parse(inputs[3]);
        int myid = int.Parse(inputs[4]);

        var bot = new Opportunist(width, height, myid);

        // game loop
        while (true) {
            var snakes = new string[playercount];
            var food = new string[foodcount];
            int aliveplayers = int.Parse(Console.ReadLine());
            for (int i = 0; i < aliveplayers; i++) {
                inputs = Console.ReadLine().Split(' ');
                int id = int.Parse(inputs[0]);
                int score = int.Parse(inputs[1]);
                int size = int.Parse(inputs[2]);
                string snake = inputs[3];
                snakes[id] = snake;
            }

            for (int i = 0; i < foodcount; i++) {
                inputs = Console.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
                food[i] = $"{x},{y}";
            }

            bot.MoveSnake(snakes, food);
        }
    }

    class Opportunist {
        readonly Random random;
        readonly Position[] walls;
        readonly int id;

        // The Opportunist strives to stay alive next turn, but if it can can
        // accomplish it while eating food, it will do so.
        public Opportunist(int width, int height, int id) {
            this.id = id;
            random = new Random(42);
            walls = GenerateWalls(width, height);
        }

        public void MoveSnake(string[] snakes, string[] food) {
            var me = ParsePositions(snakes[id]);
            Console.Error.WriteLine("Me: " + string.Join(',', me.Select(p => p.ToString())));
            
            var dangers = snakes.Where(s => s != null).SelectMany(s => ParsePositions(s)).Concat(walls).ToArray();
            Console.Error.WriteLine("Dangers: " + string.Join('|', dangers.Select(p => p.ToString())));

            var treats = food.SelectMany(s => ParsePositions(s)).ToArray();
            Console.Error.WriteLine("Food: " + string.Join('|', treats.Select(p => p.ToString())));
            
            var potentialMoves = GenerateValidMoves(me[0], dangers);
            Console.Error.WriteLine(string.Join('|', potentialMoves.Select(p => p.ToString())));
            var foodEatingMove = potentialMoves.FirstOrDefault(m => treats.Any(f => m.NewPosition.Equals(f)));

            Console.Error.WriteLine("Foodeating: " + foodEatingMove.Direction);
            if (!string.IsNullOrEmpty(foodEatingMove.Direction)) {
                Console.WriteLine(foodEatingMove.Direction);    
            } else {
                var choice = random.Next(0, potentialMoves.Length);
                Console.WriteLine(potentialMoves[choice].Direction);
            }
        }

        Move[] GenerateValidMoves(Position head, Position[] dangers) {
            var candidateMoves = new Move[] {
                new Move("W", new Position(head.X - 1, head.Y)),
                new Move("N", new Position(head.X, head.Y - 1)),
                new Move("E", new Position(head.X + 1, head.Y)),
                new Move("S", new Position(head.X, head.Y + 1)),
            };

            return candidateMoves.Where(m => !dangers.Any(d => d.Equals(m.NewPosition))).ToArray();
        }

        Position[] ParsePositions(string snake) {
            var parts = snake.Split(',');
            var positions = new List<Position>(parts.Length / 2);
            for (int i = 0; i < parts.Length; i += 2) {
                positions.Add(new Position(int.Parse(parts[i]), int.Parse(parts[i + 1])));
            }

            return positions.ToArray();
        }

        Position[] GenerateWalls(int width, int height) {
            var walls = new List<Position>();
            for (int i = 0; i < width; i++) {
                walls.Add(new Position(i, -1));
                walls.Add(new Position(i, height));
            }

            for (int i = 0; i < height; i++) {
                walls.Add(new Position(-1, i));
                walls.Add(new Position(width, i));
            }

            return walls.ToArray();
        }
    }

    public struct Position : IEquatable<Position> {
        public Position(int x, int y) {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public bool Equals(Position other) {
            return X == other.X && Y == other.Y;
        }

        public override string ToString() {
            return $"{X},{Y}";
        }
    }

    public struct Move {
        public Move(string direction, Position newPosition) {
            Direction = direction;
            NewPosition = newPosition;
        }

        public string Direction { get; }
        public Position NewPosition { get; }


        public override string ToString() {
            return $"{Direction}: {NewPosition.X},{NewPosition.Y}";
        }
    }
}