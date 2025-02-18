// BattleshipRefactor -- A refactoring of BattleshipSimple

using System;


namespace BattleshipSimple
{
    class Program
    {
        static void Main(string[] args)
        {       
            ConsoleKeyInfo response;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"The Game of Battleship\n\nSpecify the size of the grid (5-26) or 'q': ");
                    int sizeOfGame;
                    int.TryParse(Console.ReadLine(), out sizeOfGame);
                    if (sizeOfGame > 26)
                    {
                        throw new Exception();
                    }
                    var game = new BattleShipGame(sizeOfGame);                    
                    game.Play();
                    game.Reset();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: Invalid input. Please enter a valid integer.");
                }
                Console.WriteLine("Do you want to play again (y/n)");
                response = Console.ReadKey();
            } while (response.Key == ConsoleKey.Y);
            
        }
    }
}
