using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipSimple
{
    internal class Grid
    {
        public int Size;
        public bool HasShipsLeft = true;
        private List<Tuple<char, int>> ShipPresets = new List<Tuple<char, int>> { Tuple.Create('A', 5), Tuple.Create('B', 4), Tuple.Create('D', 3), Tuple.Create('S', 3), Tuple.Create('P', 2) };       
        List<Ship> Ships = new List<Ship>();
        private List<Tuple<int, int>> UnUsableShipCords = new List<Tuple<int, int>>();
        private List<Tuple<int, int>> AllCoordinates = new List<Tuple<int, int>>();

        public List<Tuple<int, int>> userGuesses = new List<Tuple<int, int>>();
        public readonly char[] letterArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly Dictionary<char, ConsoleColor> letterColorDictionary = new Dictionary<char, ConsoleColor>()
        {
            { '.', ConsoleColor.Black },
            { 'S', ConsoleColor.Red },
            { 'P', ConsoleColor.Blue },
            { 'A', ConsoleColor.Yellow },
            { 'B', ConsoleColor.Cyan },
            {'D', ConsoleColor.Green }
        };

        public Grid(int size)
        {
            Size = size;
            Init();
        }

        private void Init()
        {
            AllCoordinates = PopulateAllCooridnates();
            UnUsableShipCords.Clear();
            CreateShips();
        }

        public void Reset()
        {
            AllCoordinates.Clear();
            HasShipsLeft = true;
            Ships.Clear();
            UnUsableShipCords.Clear();
            userGuesses.Clear();
            Console.Clear();
            Init();
        }

        public void Draw()
        {
            Console.Clear();
            DisplayTopRow();
            for (int i = 0; i < Size; i++)
            {
                DisplayGridRow(i, i == Size - 1);
            }           
        }

        public void DropBomb(int x, int y)
        {
            AddGuessToUserGuessesList(x, y);
            CheckIfAllShipsAreSunk();
        }

        private List<Tuple<int, int>> PopulateAllCooridnates()
        {
            List<Tuple<int, int>> totalCooridnates = new List<Tuple<int, int>>();

            for (int i = 1; i <= Size; i++)
            {
                for (int j = 1; j <= Size; j++)
                {
                    totalCooridnates.Add(Tuple.Create(i, j));
                }
            }
            return totalCooridnates;
        }
       
        public void CreateShips()
        {            
            for (int i = 0; i < ShipPresets.Count(); i++)
            {
                List<Tuple<int, int>> tempUnusableCords = UnUsableShipCords;
                CreateShipLocation(ShipPresets.ElementAt(i), tempUnusableCords);
            }
        }

        private void CreateShipLocation(Tuple<char, int> shipPreset, List<Tuple<int, int>> tempUnusableCords)
        {
            List<Tuple<int, int>> tempShipCords = new List<Tuple<int, int>>();
            Random rand = new Random();
            List<Ship> possibleShips = PossibleShipVariations(shipPreset);
            Ship ship = possibleShips.ElementAt(rand.Next(possibleShips.Count()));
            Ships.Add(ship);

            for (int i = 0; i < shipPreset.Item2; i++)
            {
                UnUsableShipCords.Add(ship.ShipCoordinates[i]);
            }
        }

        private List<Ship> PossibleShipVariations(Tuple<char, int> shipPreset)
        {
            List<Ship> possibleShips = new List<Ship>();
            for (int i = 0;i < AllCoordinates.Count();i++)
            {
                List<Tuple<int, int>> possibleEndCords = new List<Tuple<int, int>>();
                if (!UnUsableShipCords.Contains(AllCoordinates[i]))
                {
                    possibleEndCords = GetEndCordinateOptions(shipPreset.Item2 - 1, AllCoordinates[i].Item1, AllCoordinates[i].Item2);
                    if (possibleEndCords.Count() > 0)
                    {
                        for (int j = 0; j < possibleEndCords.Count(); j++)
                        {
                            Ship potentialShip = new Ship(shipPreset.Item2, shipPreset.Item1, PopulateShipCordinates(AllCoordinates[i], possibleEndCords[j]));
                            var unusable = potentialShip.ShipCoordinates.Intersect(UnUsableShipCords);
                            if (!unusable.Any())
                            {
                                possibleShips.Add(potentialShip);
                            }
                        }
                    }
                }
            }
            return possibleShips;
        }

        private List<Tuple<int, int>> GetEndCordinateOptions(int length, int startx, int starty)
        {
            List<Tuple<int, int>> endCordOptions = new List<Tuple<int, int>>();

            if (startx + length <= Size)
            {
                endCordOptions.Add(Tuple.Create(startx + length, starty));
            }
            if (startx - length > 0)
            {
                endCordOptions.Add(Tuple.Create(startx - length, starty));
            }
            if (starty + length <= Size)
            {
                endCordOptions.Add(Tuple.Create(startx, starty + length));
            }
            if (starty - length > 0)
            {
                endCordOptions.Add(Tuple.Create(startx, starty - length));
            }

            return endCordOptions;
        }

        private List<Tuple<int, int>> PopulateShipCordinates(Tuple<int, int> startCords, Tuple<int, int> endCords)
        {
            List<Tuple<int,int>> shipCords = new List<Tuple<int,int>>();
            if (startCords.Item1 != endCords.Item1)
            {
                if (startCords.Item1 < endCords.Item1)
                {
                    for (int i = startCords.Item1; i <= endCords.Item1; i++)
                    {
                        shipCords.Add(Tuple.Create(i, startCords.Item2));
                    }
                }
                else
                {
                    for (int i = startCords.Item1; i >= endCords.Item1; i--)
                    {
                        shipCords.Add(Tuple.Create(i, startCords.Item2));
                    }
                }
            }
            else
            {
                if (startCords.Item2 < endCords.Item2)
                {
                    for (int i = startCords.Item2; i <= endCords.Item2; i++)
                    {
                        shipCords.Add(Tuple.Create(startCords.Item1, i));
                    }
                }
                else
                {
                    for (int i = startCords.Item2; i >= endCords.Item2; i--)
                    {
                        shipCords.Add(Tuple.Create(startCords.Item1, i));
                    }
                }
            }
            return shipCords;
        }

        /// <summary>
        /// Method to always display the numbers on the top row
        /// </summary>
        private void DisplayTopRow()
        {
            Console.Write("  |");
            for (int i = 1; i <= Size; i++)
            {
                if (i >= 10)
                {
                    Console.Write($" {i}|");
                }
                else
                {
                    Console.Write($" {i} |");
                }
            }
            Console.WriteLine();
        }

        private void DisplayGridRow(int rowNumber, bool finalRow)
        {
            string gridBoxTopAndBottom = "#---";
            for (int i = 0; i < Size; i++)
            {
                if (i == 0)
                {
                    Console.Write("--");
                }
                Console.Write(gridBoxTopAndBottom);
                if (i == Size - 1)
                {
                    Console.Write("#"); 
                }
            }
            Console.WriteLine();
            for (int i = 0; i < Size; i++)
            {
                char coordinateValue = GetValueAtCoordinate(rowNumber, i);
                if (i == 0)
                {
                    Console.Write($"{letterArray[rowNumber]} ");
                }

                Console.Write($"|");
                if (!userGuesses.Contains(Tuple.Create(rowNumber, i)))
                {                   
                    Console.BackgroundColor = letterColorDictionary[coordinateValue];
                    if (coordinateValue.Equals('.'))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.Write($" {coordinateValue} ");
                }
                else
                {
                    if (!coordinateValue.Equals('.'))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(" X ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                if (i == Size - 1)
                {
                    Console.Write("|");
                }
            }
            if (finalRow)
            {
                Console.WriteLine();
                for (int i = 0; i < Size; i++)
                {
                    if (i == 0)
                    {
                        Console.Write("--");
                    }
                    Console.Write(gridBoxTopAndBottom);
                    if (i == Size - 1)
                    {
                        Console.Write("#");
                    }
                }
            }
            Console.WriteLine();
        }

        private char GetValueAtCoordinate(int xCord, int yCord)
        {
            foreach (var ship in Ships)
            {
                if (ship.ShipCoordinates.Contains(Tuple.Create(xCord + 1, yCord + 1)))
                {
                    return ship.Letter;
                }
            }
            return '.';
        }

        /// <summary>
        /// Adds the user guess to the List of userGuesses.
        /// </summary>
        /// <param name="guess"></param>
        private void AddGuessToUserGuessesList(int x, int y)
        {
            userGuesses.Add(Tuple.Create(x, y));
        }

        private void CheckIfAllShipsAreSunk()
        {
            foreach(var ship in Ships)
            {
                foreach(var cords in ship.ShipCoordinates)
                {
                    if (!userGuesses.Contains(Tuple.Create(cords.Item1 -1, cords.Item2-1)))
                    {
                        return;
                    }
                }
            }
            HasShipsLeft = false;
        }
    }
}
