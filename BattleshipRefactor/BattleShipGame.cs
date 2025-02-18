// BattleshipRefactor -- A refactoring of BattleshipSimple
//
// TBD: You must create the Grid class used by this program
// TBD: It needs to be aded to the Project as a file: Grid.cs

using System;


namespace BattleshipSimple
{
    internal class BattleShipGame
    {
        private Grid grid;

        public BattleShipGame(int gridSize)
        {
            grid = new Grid(gridSize);

        }

        internal void Reset()
        {
            grid.Reset();

        }

        internal void Play()
        {
            while (grid.HasShipsLeft)
            {
                grid.Draw();

                //Read input from user into x, y
                int x, y;
                Console.WriteLine("Drop bomb at location (e.g., B3) or Q to quit: ");
                string guess = Console.ReadLine();
                if (guess.ToUpper().Equals("Q"))
                {
                    break;
                }
                try
                {
                    x = Array.IndexOf(grid.letterArray, Convert.ToChar(guess.Substring(0, 1).ToUpper()));
                    y = Convert.ToInt32(guess.Substring(1, guess.Length - 1)) - 1;
                    if (x < 0 || x > grid.Size || y < 0 || y >=  grid.Size)
                    {
                        DisplayGenericErrorMessage();
                    }                                      
                    else if (grid.userGuesses.Contains(Tuple.Create(x, y)))
                    {
                        DisplayDuplicateGuessErrorMessage();
                    }
                    else
                    {
                        grid.DropBomb(x, y);
                    }
                }
                catch (Exception)
                {
                    DisplayGenericErrorMessage();
                }
                Console.WriteLine("\n");
            }
            if (!grid.HasShipsLeft)
            {
                DisplayWinnerScreen();
            }
        }

        /// <summary>
        /// Displays a generic error message.
        /// </summary>
        private void DisplayGenericErrorMessage()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Incorrect guess format. Please try again (Press [Enter]).");
            Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Displays a duplicate guess error message.
        /// </summary>
        private void DisplayDuplicateGuessErrorMessage()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("You have already guessed this value. Please try again (Press [Enter]).");
            Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DisplayWinnerScreen()
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Clear();
            Console.WriteLine("                                  YOU WON!!");
            Console.WriteLine("Press [Enter]");
            Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }
    }
}
