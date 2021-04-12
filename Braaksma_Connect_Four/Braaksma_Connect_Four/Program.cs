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

    }


    class Gameplay : Board
    {
        public static int turnCount;

        public Gameplay(int rows, int columns) : base(rows, columns)
        {
            turnCount = 0;
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
                for (int i = (Cells.Count - 1) - (Columns - selection); i > 0; i -= Columns)
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
            } while (columnFull);
        }

        public bool Winner(Player player)
        {

            for(int i = 0; i < Cells.Count; i++)
            {
                //Check horizontal
                //Validate a win is within range
                if(i + 3 < Cells.Count)
                {
                    if (Cells[i] == player.Token && Cells[i + 1] == player.Token && Cells[i + 2] == player.Token && Cells[i + 3] == player.Token)
                        return true;
                }

                //Check Vertical
                //Validate a win is within range
                if (i + (Columns * 3) < Cells.Count)
                {
                    if (Cells[i] == player.Token && Cells[i + Columns] == player.Token && Cells[i + (Columns * 2)] == player.Token && Cells[i + (Columns * 3)] == player.Token)
                        return true;
                }

                //if (i % 7 == 0)
                //{
                    for (int j = i; j < 3; j++)
                    {
                    
                        for (int k = i * j ; k < 4; k++)
                        {
                            //Validate a win is within range
                            if (k + ((Columns + 1) * 3) < Cells.Count)
                            {
                                //Check Diagonal
                                if (Cells[k] == player.Token && Cells[k + (Columns + 1)] == player.Token && Cells[k + ((Columns + 1) * 2)] == player.Token && Cells[k + ((Columns + 1) * 3)] == player.Token)
                                    return true;
                            }
                        }

                    }
                    for (int l = i + 3; l < i + 7; l++)
                    {
                        //Validate a win is within range
                        if (l + ((Columns - 1) * 3) < Cells.Count)
                        {
                            //Check reverse Diagonal
                            if (Cells[l] == player.Token && Cells[l + (Columns - 1)] == player.Token && Cells[l + ((Columns - 1) * 2)] == player.Token && Cells[l + ((Columns - 1) * 3)] == player.Token)
                                return true;
                        }
                    }
                //}
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
                    Console.WriteLine("Enter your name");
                    string player1Name = Console.ReadLine();

                    Console.WriteLine("What difficulty would you like? ( 1 / 2 / 3");
                    int difficulty = Console.Read();

                    //Validate difficulty
                    while (difficulty != 1 || difficulty != 2 || difficulty != 3)
                    {
                        Console.WriteLine("Please enter 1 or 2 or 3");
                        difficulty = Console.Read();
                    }

                    Player player1 = new Human(player1Name, "X");
                    Player player2 = new AI("Computer", difficulty, "0");

                    Gameplay singlePlayerGame = new Gameplay(numberOfRows, numberOfColumns);

                    do
                    {
                        singlePlayerGame.DrawBoard(player1.Token, player2.Token);
                        Console.WriteLine("\n\nSelect a number between 1 - {0} to place your piece", singlePlayerGame.Columns);

                        //Get player selection
                        int.TryParse(Console.ReadLine(), out choice);

                        //Validate player selection
                        while (choice < 1 || choice > numberOfColumns)
                        {
                            Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
                            int.TryParse(Console.ReadLine(), out choice);
                        }


                    } while (!winner);
                }


                //Multiplayer Game
                else if (numberPlayers == 2)
                {
                    Console.WriteLine("Enter your name player 1");
                    string player1Name = Console.ReadLine();

                    Console.WriteLine("Enter your name player 2");
                    string player2Name = Console.ReadLine();

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
                            Console.WriteLine("{0} and (1} have TIED!!", player1.Name, player2.Name);
                            break;
                        }

                        Gameplay.turnCount++;

                    } while (!winner);


                    if(winner)
                    {
                        multiplayerGame.DrawBoard(player1.Token, player2.Token);
                        Console.WriteLine("\nCongratulations {0}!!\nYOU WIN!!", theWinner);
                    }
         
                    
                    Console.WriteLine("Would you like to play agian? (Y / N)");
                    
                    if (Console.ReadLine().ToUpper().Substring(0,1) == "Y")
                        again = true;
                    else
                        again = false;
                }
            } while (again);
        }
    }
}