using System;
using System.Linq;
using System.Text;
using MineSweeper;

namespace MineSweeperConsole
{
    internal class Program
    {
        static readonly int Length = 8;
        private const int Height = 8;
        private const int Bomb = 10;
        private static bool _exit;
        private static int _inputX;
        private static int _inputY;

        private static void Main(string[] args)
        {
           
            var game = new Manager(Length, Height, Bomb);

                                    
            //var data = game.MineFields.Where(x => x.HasBomb);
            //foreach (var mineField in data)
            //{
            //    mineField.HasBomb = false;
            //}

            //game.MineFields.First(g => g.X == 7 && g.Y == 0).HasBomb = true;
            //game.MineFields.First(g => g.X == 6 && g.Y == 1).HasBomb = true;
            //game.MineFields.First(g => g.X == 5 && g.Y == 2).HasBomb = true;
            //game.MineFields.First(g => g.X == 4 && g.Y == 3).HasBomb = true;
            //game.MineFields.First(g => g.X == 3 && g.Y == 4).HasBomb = true;
            //game.MineFields.First(g => g.X == 2 && g.Y == 5).HasBomb = true;
            //game.MineFields.First(g => g.X == 1 && g.Y == 6).HasBomb = true;
            //game.MineFields.First(g => g.X == 0 && g.Y == 7).HasBomb = true;

            //game.SetBombZones();

            while (!_exit)
            {
                Draw(game);

                Status(game);
                                    
                Input(game);
            }
        }

        private static void Status(Manager game)
        {
            if (game.MineExploded())
            {
                Console.WriteLine("Game Over!");
                Console.ReadKey();
                _exit = true;
            }            
        }

        private static void Input(Manager game)
        {
            var input = Console.ReadLine();
            Console.Clear();

            if (input != null)
                try
                {
                    var values = input.Split(' ');

                    _inputX = int.Parse(values[0]);
                    _inputY = int.Parse(values[1]);

                    if (values.Length > 2)
                        game.PlantFlag(_inputX, _inputY);
                    else
                        game.PickField(_inputX, _inputY);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        private static void Draw(Manager game)
        {
            
            Console.Write("   ");
            for (var x = 0; x < Length; x++)
            {
                Console.Write("[{0}]", x);
            }
            Console.WriteLine("");
            for (var x = 0; x < Length; x++)
            {
                Console.Write("[{0}]", x);
                for (var y = 0; y < Height; y++)
                    try
                    {
                        var field = game.MineFields.First(f => f.X == x && f.Y == y);

                        //if (field.HasBomb)
                        //    Console.Write("[X]");
                        //else if (field.BombsNearMe <= 0)
                        //    Console.Write("[ ]");
                        //else
                        //    Console.Write("[{0}]", field.BombsNearMe);




                        if (field.HasExploded())
                            Console.Write("[X]");
                        else if (field.IsOpenField && field.BombsNearMe > 0)
                            Console.Write("[{0}]", field.BombsNearMe);
                        else if (field.IsOpenField && field.BombsNearMe == 0)
                            Console.Write("[ ]");
                        else if (field.HasFlag)
                            Console.Write("[*]");
                        else
                            Console.Write("[{0}]", (char)182);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                Console.WriteLine("");
            }
        }
    }
}