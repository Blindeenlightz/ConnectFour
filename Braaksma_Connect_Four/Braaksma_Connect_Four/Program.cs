/* 
        
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braaksma_Connect_Four
{
    abstract class Player
    {
        public string Name { get; set; }
        public string Token { get; set; }


        public Player(string name, string token)
        {
            Name = name;
            Token = token;
        }

    }


    class AI : Player
    {
        private int _difficulty;
        public AI(string name, int difficulty, string token) : base(name = "Computer", token = "0")
        {
            _difficulty = difficulty;
        }

        public int PickNextMove(Gameplay currentGame, Player opponent)
        {
            //int nextMove = 1;
            Gameplay tempHolder;

            //Create a simulated game to test states without affecting original game
            Gameplay simulatedGame = new Gameplay(currentGame);

            Random random = new Random();
            if (_difficulty == 1)
            {
                return random.Next(1, 8);
            }


            //Medium Difficulty
            else if(_difficulty == 2)
            {

                for (int i = 0; i < simulatedGame.Rows; i++)
                {
                    //Check if any next move will result in a win
                    for (int j = 1; j <= simulatedGame.Columns; j++)
                    {
                        if (simulatedGame.Cells[j] == "|_|")
                        {
                            tempHolder = new Gameplay(simulatedGame);

                            //Simulate inserting a token
                            for (int k = (simulatedGame.Cells.Count - 1) - (simulatedGame.Columns - j); k >= 0; k -= simulatedGame.Columns)
                            {
                                if (simulatedGame.Cells[k] != this.Token && simulatedGame.Cells[k] == "|_|")
                                {
                                    simulatedGame.Cells[k] = this.Token;
                                    break;
                                }
                            }

                            //If the placement results in a win return it
                            if (simulatedGame.Winner(this))
                            {
                                Console.WriteLine("Win returned");
                                return j;
                            }

                            //Reset the simulated gameboard to it's original state
                            simulatedGame = new Gameplay(tempHolder);

                        }
                    }

                    //Check if any next move will result in opponent win
                    for (int j = 1; j <= simulatedGame.Columns; j++)
                    {
                        if (simulatedGame.Cells[j] == "|_|")
                        {
                            tempHolder = new Gameplay(simulatedGame);

                            //Simulate inserting a token
                            for (int k = (simulatedGame.Cells.Count - 1) - (simulatedGame.Columns - j); k >= 0; k -= simulatedGame.Columns)
                            {
                                if (simulatedGame.Cells[k] != opponent.Token && simulatedGame.Cells[k] == "|_|")
                                {
                                    simulatedGame.Cells[k] = opponent.Token;
                                    break;
                                }
                            }
                            Console.WriteLine();
                            simulatedGame.DrawBoard(this.Token, opponent.Token);

                            //If the placement results in a win block it
                            if (simulatedGame.Winner(opponent))
                            {
                                Console.WriteLine("Opponent Win Block");
                                return j;
                            }

                            //Reset the simulated gameboard to it's original state
                            simulatedGame = new Gameplay(tempHolder);

                        }
                    }

                }

                    //If all strategies are null, place a random token
                    return random.Next(1, 8);
            }

            //Hard difficulty 
            else
                return random.Next(1, 8);

        }
    }


    class Human : Player
    {
        private readonly int playerDesignation = 1;
        public Human(string name, string token) : base(name, token)
        {
            playerDesignation++;
        }
    }


    class Board
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


    class Gameplay : Board
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
                if(item == player1Token || item == player2Token)
                    Console.Write("|{0}|", item);
                
                else
                    Console.Write("{0}", item);

                counter++;
            }

            Console.Write("\n");

            for (int i = 0; i <= Rows; i++)
            {
                Console.Write(" {0} ", i + 1);

            }
        }

        public void InsertToken(Player currentPlayer, int selection)
        {
            bool columnFull = true;
            do
            {
                //Replace the blank square with the players token in the gameboard (<List>)
                for (int i = (Cells.Count - 1) - (Columns - selection); i >= 0; i -= Columns)
                {
                    if (Cells[i] != currentPlayer.Token && Cells[i] == "|_|")
                    {
                        Cells[i] = currentPlayer.Token;
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

            for(int i = 0; i < Rows; i++)
            {
                //Check Vertical
                for (int j = 0; j < i * Columns; j++)
                {
                    //Validate a win is within range
                    if (j + (Columns * 3) < Cells.Count)
                    {
                        if (Cells[j] == player.Token && Cells[j + Columns] == player.Token && Cells[j + (Columns * 2)] == player.Token && Cells[j + (Columns * 3)] == player.Token)
                        {
                            Console.WriteLine("Vertical Win!");
                            return true;
                        }
                    }
                }
           

                //Check reverse diagonal
                //This win can't take place starting form the first 3 columns
                for (int j = (i * Columns) + 3; j < (i + 1) * Columns; j++)
                {
                    //Validate a win is within range
                    if (j + ((Columns - 1) * 3) < (Cells.Count - 3))
                    {
                        if (Cells[j] == player.Token && Cells[j + (Columns - 1)] == player.Token && Cells[j + ((Columns - 1) * 2)] == player.Token && Cells[j + ((Columns - 1) * 3)] == player.Token)
                        {
                            Console.WriteLine("Reverse Diagonal Win!");
                            return true;
                        }
                    }
                }
                

                //These wins can't take place starting from the last 3 columns
                for (int j = i * Columns; j < ((i + 1) * Columns) - 3; j++)
                {
                    //Check horizontal
                    //Validate a win is within range
                    if (j + 3 < Cells.Count)
                    {
                        if (Cells[j] == player.Token && Cells[j + 1] == player.Token && Cells[j + 2] == player.Token && Cells[j + 3] == player.Token)
                        {
                            Console.WriteLine("Horizontal Win!");
                            return true;
                        }
                    }

                    //Check diagonal
                    //Validate a win is within range
                    if (j + ((Columns + 1) * 3) < Cells.Count)
                    {
                        if (Cells[j] == player.Token && Cells[j + (Columns + 1)] == player.Token && Cells[j + ((Columns + 1) * 2)] == player.Token && Cells[j + ((Columns + 1) * 3)] == player.Token)
                        {
                            Console.WriteLine("Diagonal Win!");
                            return true;
                        }
                    }
                }
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
                int numberOfColumns = 7;
                int numberOfRows = 6;
                int choice;
                int numberPlayers;
                string theWinner;
                bool winner = false;

                Console.WriteLine("One player game or Two? (1 / 2)");
                int.TryParse(Console.ReadLine(), out numberPlayers);

                //Validate number of players
                while (numberPlayers < 1 && numberPlayers > 2)
                {
                    Console.WriteLine("Please enter the number of players. Either 1 or 2");
                    int.TryParse(Console.ReadLine(), out numberPlayers);
                    Console.WriteLine(numberPlayers);
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

                    Console.WriteLine("What difficulty would you like? ( 1 / 2 / 3");
                    int.TryParse(Console.ReadLine(), out difficulty);

                    //Validate difficulty
                    while (difficulty < 1 || difficulty > 3)
                    {
                        Console.WriteLine("Please enter 1 or 2 or 3");
                        int.TryParse(Console.ReadLine(), out difficulty);
                    }

                    Human player1 = new Human(player1Name, "X");
                    AI player2 = new AI("Computer", difficulty, "0");

                    Gameplay singlePlayerGame = new Gameplay(numberOfRows, numberOfColumns);

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
                            Console.WriteLine("\n\nSelect a number between 1 - {0} to place your piece", singlePlayerGame.Columns);

                            //Get player selection
                            int.TryParse(Console.ReadLine(), out choice);

                            //Validate player selection
                            while (choice < 1 || choice > numberOfColumns)
                            {
                                Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
                                int.TryParse(Console.ReadLine(), out choice);
                            }

                            singlePlayerGame.InsertToken(player1, choice);
                            winner = singlePlayerGame.Winner(player1);
                            theWinner = player1.Name;
                        }

                        else
                        {
                            choice = player2.PickNextMove(singlePlayerGame, player1);
                            singlePlayerGame.InsertToken(player2, choice);
                            winner = singlePlayerGame.Winner(player2);
                            theWinner = player2.Name;
                        }

                        //Console formatting
                        Console.WriteLine();
                        Console.WriteLine();

                        int numberOfCellsEmpty = singlePlayerGame.Cells.IndexOf("|_|");
                        if (numberOfCellsEmpty == -1)
                        {
                            Console.WriteLine("{0} and {1} have TIED!!", player1.Name, player2.Name);
                            break;
                        }

                        Gameplay.turnCount++;
                    } while (!winner);


                    if (winner)
                    {
                        singlePlayerGame.DrawBoard(player1.Token, player2.Token);
                        Console.WriteLine("\nCongratulations {0}!!\nYOU WIN!!", theWinner);
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
                        Console.WriteLine("\n\nSelect a number between 1 - {0} to place your piece", multiplayerGame.Columns);

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
                            multiplayerGame.InsertToken(player1, choice);
                            winner = multiplayerGame.Winner(player1);
                            theWinner = player1.Name;
                        }

                        else
                        {
                            multiplayerGame.InsertToken(player2, choice);
                            winner = multiplayerGame.Winner(player2);
                            theWinner = player2.Name;
                        }

                        //Console formatting
                        Console.WriteLine();
                        Console.WriteLine();

                        int numberOfCellsEmpty = multiplayerGame.Cells.IndexOf("|_|");
                        if (numberOfCellsEmpty == -1)
                        {
                            Console.WriteLine("{0} and {1} have TIED!!", player1.Name, player2.Name);
                            break;
                        }

                        Gameplay.turnCount++;

                    } while (!winner);


                    if(winner)
                    {
                        multiplayerGame.DrawBoard(player1.Token, player2.Token);
                        Console.WriteLine("\nCongratulations {0}!!\nYOU WIN!!", theWinner);
                    }
         
                    
                }
                    Console.WriteLine("Would you like to play agian? (Y / N)");
                    
                    if (Console.ReadLine().ToUpper().Substring(0,1) == "Y")
                        again = true;
                    else
                        again = false;
            } while (again);
        }
    }
}