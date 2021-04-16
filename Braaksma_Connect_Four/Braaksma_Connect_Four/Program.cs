/*
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braaksma_Connect_Four
{
    public abstract class Player
    {
        public string Name { get; set; }
        public string Token { get; set; }


        public Player(string name, string token)
        {
            Name = name;
            Token = token;
        }

    }


    public class AI : Player
    {
        private int _difficulty;
        private bool checkedForMiddle = false;
        public AI(string name, int difficulty, string token) : base(name = "Computer", token = "0")
        {
            _difficulty = difficulty;
        }


        public int PickNextMove(Gameplay currentGame, Player opponent)
        {
            int middleChoice = 0;
            Gameplay tempHolder;
            List<int> notFull = new List<int>();

            for (int i = 0; i < currentGame.Columns; i++)
                notFull.Add(i + 1);

            //Create a simulation to test states without affecting original game
            Gameplay simulatedGame = new Gameplay(currentGame);

            //Easy Difficluty
            Random random = new Random();
            if (_difficulty == 1)
            {
                for (int i = 0; i < simulatedGame.Columns; i++)
                {
                    //Check if a column is full (to remove as option for random placement)
                    if (simulatedGame.Cells[i] != "|_|")
                        notFull.Remove(i + 1);
                }

                //If all strategies are null, place a random token in an available column
                int randomChoice = random.Next(notFull.Count);
                return notFull[randomChoice];
            }


            //Medium Difficulty
            else if (_difficulty == 2)
            {
                tempHolder = new Gameplay(simulatedGame);

                //Check if any next move will result in a win
                for (int i = 1; i <= simulatedGame.Columns; i++)
                {
                    //Check if a column is full (to remove as option for random placement)
                    if (simulatedGame.Cells[i - 1] != "|_|")
                        notFull.Remove(i);

                    if (simulatedGame.Cells[i - 1] == "|_|")
                        if (Simulation.SimulateNextMove(this, opponent, simulatedGame, i))
                            return i;

                    simulatedGame = new Gameplay(tempHolder);
                }

                //Check if any next move will result in opponent win
                for (int i = 1; i <= simulatedGame.Columns; i++)
                {
                    if (simulatedGame.Cells[i - 1] == "|_|")
                        if (Simulation.SimulateNextMove(opponent, this, simulatedGame, i))
                            return i;

                    simulatedGame = new Gameplay(tempHolder);
                }

                //If all strategies are null, place a random token in an available column
                int randomChoice = random.Next(notFull.Count);
                return notFull[randomChoice];
            }


            //Hard difficulty 
            else if (_difficulty == 3)
            {
                tempHolder = new Gameplay(simulatedGame);

                //Check for win in one first, then simulate two moves
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 1; j <= simulatedGame.Columns; j++)
                    {
                        //Check if a column is full (to remove as option for random placement)
                        if (simulatedGame.Cells[j - 1] != "|_|")
                            notFull.Remove(j);

                        //Check if next move will result in opponent win
                        if (i == 0)
                        {
                            for (int k = 1; k <= simulatedGame.Columns; k++)
                            {
                                if (simulatedGame.Cells[k - 1] == "|_|")
                                    if (Simulation.SimulateNextMove(opponent, this, simulatedGame, k))
                                        return k;

                                simulatedGame = new Gameplay(tempHolder);
                            }
                        }

                        //Check if next move will result in a win
                        if (simulatedGame.Cells[j - 1] == "|_|")
                            if (Simulation.SimulateNextMove(this, opponent, simulatedGame, j))
                                return j;

                        //Check for win in 2 moves
                        if (i == 1)
                        {
                            //Create a second simulation
                            Gameplay simulatedGame2 = new Gameplay(simulatedGame);

                            for (int k = 1; k <= simulatedGame2.Columns; k++)
                            {
                                //Check if any next move will result in a win
                                if (simulatedGame2.Cells[k - 1] == "|_|")
                                    if (Simulation.SimulateNextMove(this, opponent, simulatedGame2, k))
                                        return j;

                                //Reset the second simulated game
                                simulatedGame2 = new Gameplay(simulatedGame);
                            }
                        }
                        //Reset the simulated gameboard to it's original state
                        simulatedGame = new Gameplay(tempHolder);
                    }
                }
                //If all strategies are null, place a random token in an available column
                int randomChoice = random.Next(notFull.Count);
                return notFull[randomChoice];
            }


            //Very hard difficulty
            else
            {
                tempHolder = new Gameplay(simulatedGame);

                //If the middle column is available (and exists) on the first move take it
                if (simulatedGame.Columns % 2 != 0 && !checkedForMiddle)
                {
                    checkedForMiddle = true;
                    middleChoice = (simulatedGame.Cells.Count - 1) - ((simulatedGame.Columns / 2) + 1);
                    if (simulatedGame.Cells[middleChoice] == "|_|")
                    {
                        return (simulatedGame.Columns / 2) + 1;
                    }
                }
                checkedForMiddle = true;

                //Check for win in one first, then simulate the opponents move, then simulate following move
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 1; j <= simulatedGame.Columns; j++)
                    {
                        //Check if a column is full (to remove as option for random placement)
                        if (simulatedGame.Cells[j - 1] != "|_|")
                            notFull.Remove(j);

                        //Check if very next move will result in opponent win
                        if (i == 0)
                        {
                            for (int m = 1; m <= simulatedGame.Columns; m++)
                            {
                                if (simulatedGame.Cells[m - 1] == "|_|")
                                    if (Simulation.SimulateNextMove(opponent, this, simulatedGame, m))
                                        return m;

                                simulatedGame = new Gameplay(tempHolder);
                            }
                        }

                        //Check if next move will result in a win
                        if (simulatedGame.Cells[j - 1] == "|_|")
                        {
                            if (Simulation.SimulateNextMove(this, opponent, simulatedGame, j))
                                return j;

                            if (i >= 1)
                            {
                                //Create a second simulation
                                Gameplay simulatedGame2 = new Gameplay(simulatedGame);

                                for (int k = 1; k <= simulatedGame2.Columns; k++)
                                {
                                    if (simulatedGame2.Cells[k - 1] == "|_|")
                                        if (Simulation.SimulateNextMove(opponent, this, simulatedGame2, k))
                                            break;

                                    if (i == 2)
                                    {
                                        //Create a third simulation
                                        Gameplay simulatedGame3 = new Gameplay(simulatedGame2);

                                        for (int l = 1; l <= simulatedGame3.Columns; l++)
                                        {
                                            if (simulatedGame3.Cells[l - 1] == "|_|")
                                                if (Simulation.SimulateNextMove(this, opponent, simulatedGame3, l))
                                                    return j;

                                            //Reset the simulated gameboard to it's original state
                                            simulatedGame3 = new Gameplay(simulatedGame2);
                                        }
                                    }
                                    //Reset the simulated gameboard to it's original state
                                    simulatedGame2 = new Gameplay(simulatedGame);
                                }
                            }
                        }
                        //Reset the simulated gameboard to it's original state
                        simulatedGame = new Gameplay(tempHolder);
                    }
                }
                //If all strategies are null, place a random token in an available column
                int randomChoice = random.Next(notFull.Count);
                return notFull[randomChoice];
            }
        }



        public class Human : Player
        {
            public Human(string name, string token) : base(name, token) { }
        }


        public class Board
        {
            public List<string> Cells { get; set; }
            public int Rows { get; set; }
            public int Columns { get; set; }

            public Board(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
                Cells = new List<string>();
                for (int i = 0; i < Rows * Columns; i++)
                {
                    Cells.Add("|_|");
                }
            }

            //Copy Constructor
            public Board(Board toBeCopied)
            {
                Rows = (toBeCopied.Rows);
                Columns = toBeCopied.Columns;
                Cells = new List<string>();
                foreach (string item in toBeCopied.Cells)
                    Cells.Add(item);
            }

        }


        public class Gameplay : Board
        {
            public static int turnCount;

            public Gameplay(int rows, int columns) : base(rows, columns)
            {
                turnCount = 0;
            }

            //Copy Constructor
            public Gameplay(Gameplay toBeCopied) : base(toBeCopied)
            {

            }

            public void Turn(Player currentPlayer)
            {
                Console.WriteLine("It's {0}'s turn!\n", currentPlayer.Name);
            }

            public void DrawBoard(string player1Token, string player2Token)
            {
                int counter = 0;
                foreach (string item in Cells)
                {
                    if (counter % Columns == 0 && counter != 0)
                        Console.Write("\n");

                    //Add formatting for gamePieces
                    if (item == player1Token)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0}", item);
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        Console.ResetColor();
                    }

                    else if (item == player2Token)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("{0}", item);
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("{0}", item);
                        Console.ResetColor();
                    }

                    counter++;
                }

                Console.Write("\n");

                for (int i = 0; i < Columns; i++)
                {
                    if (i < 9)
                        Console.Write(" {0} ", i + 1);
                    else
                        Console.Write("{0} ", i + 1);
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            public void InsertToken(string token, int selection)
            {
                bool columnFull = true;
                do
                {
                    //Replace the blank square with the players token in the gameboard (<List>)
                    for (int i = (Cells.Count - 1) - (Columns - selection); i >= 0; i -= Columns)
                    {
                        if (Cells[i] == "|_|")
                        {
                            Cells[i] = token;
                            columnFull = false;
                            break;
                        }
                    }

                    if (columnFull)
                    {
                        Console.WriteLine("That column is full please pick another");
                        int.TryParse(Console.ReadLine(), out selection);
                    }

                    //Validate player selection
                    while (selection < 1 || selection > Columns)
                    {
                        Console.WriteLine("Please enter a valid number between 1 - {0}", Columns);
                        int.TryParse(Console.ReadLine(), out selection);
                    }

                } while (columnFull);
            }

            public bool Winner(Player player)
            {
                for (int i = 0; i < Rows; i++)
                {
                    //Check Vertical
                    for (int j = 0; j < i * Columns; j++)
                    {
                        //Validate a win is within range
                        if (j + (Columns * 3) < Cells.Count)
                            if (Cells[j] == player.Token && Cells[j + Columns] == player.Token && Cells[j + (Columns * 2)] == player.Token && Cells[j + (Columns * 3)] == player.Token)
                                return true;
                    }

                    //Check reverse diagonal
                    //This win can't take place starting form the first 3 columns
                    for (int j = (i * Columns) + 3; j < (i + 1) * Columns; j++)
                    {
                        //Validate a win is within range
                        if (j + ((Columns - 1) * 3) < (Cells.Count - 3))
                            if (Cells[j] == player.Token && Cells[j + (Columns - 1)] == player.Token && Cells[j + ((Columns - 1) * 2)] == player.Token && Cells[j + ((Columns - 1) * 3)] == player.Token)
                                return true;
                    }

                    //These wins can't take place starting from the last 3 columns
                    for (int j = i * Columns; j < ((i + 1) * Columns) - 3; j++)
                    {
                        //Check horizontal
                        //Validate a win is within range
                        if (j + 3 < Cells.Count)
                            if (Cells[j] == player.Token && Cells[j + 1] == player.Token && Cells[j + 2] == player.Token && Cells[j + 3] == player.Token)
                                return true;

                        //Check diagonal
                        //Validate a win is within range
                        if (j + ((Columns + 1) * 3) < Cells.Count)
                            if (Cells[j] == player.Token && Cells[j + (Columns + 1)] == player.Token && Cells[j + ((Columns + 1) * 2)] == player.Token && Cells[j + ((Columns + 1) * 3)] == player.Token)
                                return true;
                    }
                }
                return false;
            }
        }

        public static class Simulation
        {
            public static bool SimulateNextMove(Player player, Player opponent, Gameplay simulatedGame, int choice)
            {
                if (simulatedGame.Cells[choice - 1] == "|_|")
                {
                    //Simulate inserting a token
                    simulatedGame.InsertToken(player.Token, choice);

                    if (simulatedGame.Winner(player))
                        return true;
                }
                return false;
            }
        }




        class Program
        {
            static void Main(string[] args)
            {
                bool again = true;
                do
                {
                    //Choose the size of board to play with
                    int numberOfColumns;
                    int numberOfRows;
                    int choice;
                    int numberPlayers;
                    string theWinner = "";
                    bool winner = false;

                    //Limit in board size is based on the time to run the hardest algorithm, it could be higher but the wait time is too long
                    Console.WriteLine("How many columns would you like the board to have? (Min: 4 / Max: 25)");
                    int.TryParse(Console.ReadLine(), out numberOfColumns);

                    //Validate number of columns
                    while (numberOfColumns < 4 || numberOfColumns > 25)
                    {
                        Console.WriteLine("Please enter a valid number larger than 3 and smaller than 26");
                        int.TryParse(Console.ReadLine(), out numberOfColumns);
                    }

                    Console.WriteLine("How many rows would you like the board to have? (Min: 4 / Max: 25)");
                    int.TryParse(Console.ReadLine(), out numberOfRows);

                    //Validate number of rows
                    while (numberOfRows < 4 || numberOfRows > 25)
                    {
                        Console.WriteLine("Please enter a valid number larger than 3 and smaller than 26");
                        int.TryParse(Console.ReadLine(), out numberOfRows);
                    }

                    Console.WriteLine("One player game or Two? (1 / 2)");
                    int.TryParse(Console.ReadLine(), out numberPlayers);

                    //Validate number of players
                    while (numberPlayers < 1 || numberPlayers > 2)
                    {
                        Console.WriteLine("Please enter the number of players. Either 1 or 2");
                        int.TryParse(Console.ReadLine(), out numberPlayers);
                    }



                    //Single Player Game
                    if (numberPlayers == 1)
                    {
                        string player1Name = "";
                        int difficulty = 0;

                        do
                        {
                            Console.WriteLine("Enter your name");
                            player1Name = Console.ReadLine();
                        } while (player1Name == "");

                        Console.WriteLine("What difficulty would you like? ( 1 / 2 / 3 / 4)");
                        int.TryParse(Console.ReadLine(), out difficulty);

                        //Validate difficulty
                        while (difficulty < 1 || difficulty > 4)
                        {
                            Console.WriteLine("Please enter 1 or 2 or 3 or 4");
                            int.TryParse(Console.ReadLine(), out difficulty);
                        }

                        Human player1 = new Human(player1Name, "X");
                        AI player2 = new AI("Computer", difficulty, "0");

                        Gameplay singlePlayerGame = new Gameplay(numberOfRows, numberOfColumns);

                        //Randomly pick who goes first
                        Random rand = new Random();
                        Gameplay.turnCount = rand.Next(0, 2);
                        do
                        {
                            if (Gameplay.turnCount % 2 == 0)
                                singlePlayerGame.Turn(player1);
                            else
                                singlePlayerGame.Turn(player2);

                            singlePlayerGame.DrawBoard(player1.Token, player2.Token);

                            //Human player game
                            if (Gameplay.turnCount % 2 == 0)
                            {
                                Console.WriteLine("\nSelect a number between 1 - {0} to place your piece", singlePlayerGame.Columns);

                                //Get player selection
                                int.TryParse(Console.ReadLine(), out choice);

                                //Validate player selection
                                while (choice < 1 || choice > numberOfColumns)
                                {
                                    Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
                                    int.TryParse(Console.ReadLine(), out choice);
                                }

                                singlePlayerGame.InsertToken(player1.Token, choice);
                                winner = singlePlayerGame.Winner(player1);
                                if (winner)
                                    theWinner = player1.Name;
                            }

                            else
                            {
                                choice = player2.PickNextMove(singlePlayerGame, player1);
                                singlePlayerGame.InsertToken(player2.Token, choice);
                                winner = singlePlayerGame.Winner(player2);
                                if (winner)
                                    theWinner = player2.Name;
                            }

                            int numberOfCellsEmpty = singlePlayerGame.Cells.IndexOf("|_|");
                            if (numberOfCellsEmpty == -1)
                            {
                                singlePlayerGame.DrawBoard(player1.Token, player2.Token);
                                Console.WriteLine("\n{0} and {1} have\n\n**** TIED ****", player1.Name, player2.Name);
                                break;
                            }

                            Gameplay.turnCount++;
                        } while (!winner);


                        if (winner)
                        {
                            singlePlayerGame.DrawBoard(player1.Token, player2.Token);
                            Console.WriteLine("\nCongratulations {0}!!\n\n**** YOU WIN ****", theWinner);
                        }
                    }


                    //Multiplayer Game
                    else if (numberPlayers == 2)
                    {
                        string player1Name = "";
                        string player2Name = "";

                        do
                        {
                            Console.WriteLine("Enter your name player 1");
                            player1Name = Console.ReadLine();
                        } while (player1Name == "");

                        do
                        {
                            Console.WriteLine("Enter your name player 2");
                            player2Name = Console.ReadLine();
                        } while (player2Name == "");

                        Player player1 = new Human(player1Name, "X");
                        Player player2 = new Human(player2Name, "0");

                        Gameplay multiplayerGame = new Gameplay(numberOfRows, numberOfColumns);

                        do
                        {
                            if (Gameplay.turnCount % 2 == 0)
                                multiplayerGame.Turn(player1);
                            else
                                multiplayerGame.Turn(player2);

                            multiplayerGame.DrawBoard(player1.Token, player2.Token);
                            Console.WriteLine("\nSelect a number between 1 - {0} to place your piece", multiplayerGame.Columns);

                            //Get player selection
                            int.TryParse(Console.ReadLine(), out choice);

                            //Validate player selection
                            while (choice < 1 || choice > numberOfColumns)
                            {
                                Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
                                int.TryParse(Console.ReadLine(), out choice);
                            }

                            if (Gameplay.turnCount % 2 == 0)
                            {
                                multiplayerGame.InsertToken(player1.Token, choice);
                                winner = multiplayerGame.Winner(player1);
                                if (winner)
                                    theWinner = player1.Name;
                            }

                            else
                            {
                                multiplayerGame.InsertToken(player2.Token, choice);
                                winner = multiplayerGame.Winner(player2);
                                if (winner)
                                    theWinner = player2.Name;
                            }

                            int numberOfCellsEmpty = multiplayerGame.Cells.IndexOf("|_|");
                            if (numberOfCellsEmpty == -1)
                            {
                                multiplayerGame.DrawBoard(player1.Token, player2.Token);
                                Console.WriteLine("\n{0} and {1} have\n\n**** TIED ****", player1.Name, player2.Name);
                                break;
                            }

                            Gameplay.turnCount++;

                        } while (!winner);

                        if (winner)
                        {
                            multiplayerGame.DrawBoard(player1.Token, player2.Token);
                            Console.WriteLine("\nCongratulations {0}!!\n\n**** YOU WIN ****", theWinner);
                        }
                    }

                    Console.WriteLine("\n\nWould you like to play agian? (Y / N)");

                    if (Console.ReadLine().ToUpper().Substring(0, 1) == "Y")
                        again = true;
                    else
                        again = false;
                } while (again);
            }
        }
    }
}