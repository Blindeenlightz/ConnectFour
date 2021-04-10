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
        public char Token { get; set; }


        public Player(string name, char token)
        {
            Name = name;
            Token = token;
        }

    }


    class AI : Player
    {
        private int _difficulty;
        public AI(string name, int difficulty, char token) : base(name = "Computer", token = '0')
        {
            _difficulty = difficulty;
        }
    }

    class Human : Player
    {
        private readonly int playerDesignation = 1;
        public Human(string name, char token) : base(name, token)
        {
            playerDesignation++;
        }
    }


    class Board
    {
        public List<char> Cells { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new List<char>();
            for (int i = 0; i <= Rows * Columns; i++)
            {
                Cells.Add('#');
            }
        }
        public void DrawBoard()
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                    Console.Write("{0} ", Cells[i]);

                Console.Write("\n");
            }
            for (int i = 0; i <= Columns; i++)
            {
                Console.Write("{0} ", i + 1);

            }
            Console.WriteLine("\n\nSelect a number between 1 - {0} to place your piece", Rows);
        }
    }


    class Gameplay
    {
        public static int turnCount;
        Board currentGameBoard;

        public Gameplay(Board currentGameBoard)
        {
            this.currentGameBoard = currentGameBoard;
        }

        public void Turn(Player currentPlayer)
        {
            Console.WriteLine("It's {0}'s turn!", currentPlayer.Name);
            turnCount++;
        }

        public void InsertToken(Player currentPlayer, int selection,  Board currentGameBoard) //doesn't change the global currentGameBoard
        {
            //Replace the blank square with the players token in the gameboard (<List>)
            for (int i = currentGameBoard.Cells.Count; i > 0 ; i--)
            {
                if(currentGameBoard.Cells[i - 1] == currentPlayer.Token)
                {
                    currentGameBoard.Cells.RemoveAt(i - 1);
                    currentGameBoard.Cells.Insert(i - 1 , currentPlayer.Token);
                }

                else
                    currentGameBoard.Cells.Add('#');
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<char> board = new List<char>();
            //Choose the size of board to play with
            int numberOfRows = 7;
            int numberOfColumns = 6;
            int choice;
            int numberPlayers;
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
            if(numberPlayers == 1)
            {
                    Console.WriteLine("Enter your name");
                    string player1Name = Console.ReadLine();

                    Console.WriteLine("What difficulty would you like? ( 1 / 2 / 3");
                    int difficulty = Console.Read();
                
                    //Validate difficulty
                    while(difficulty != 1 || difficulty != 2 || difficulty != 3)
                    {
                        Console.WriteLine("Please enter 1 or 2 or 3");
                        difficulty = Console.Read();
                    }
                
                    Player player1 = new Human(player1Name, 'X');
                    Player player2= new AI("Computer", difficulty, '0');

                    Board gameBoard = new Board(numberOfRows, numberOfColumns);

                do 
                {
                    gameBoard.DrawBoard();

                    //Get player selection
                    int.TryParse(Console.ReadLine(), out choice);

                    //Validate player selection
                    while (choice < 1 || choice > numberOfRows)
                    {
                        Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfRows);
                        int.TryParse(Console.ReadLine(), out choice);
                    }


                }while(!winner);
            }

            //Multiplayer Game
            else if(numberPlayers == 2)
            {
                    Console.WriteLine("Enter your name player 1");
                    string player1Name = Console.ReadLine();

                    Console.WriteLine("Enter your name player 2");
                    string player2Name = Console.ReadLine();

                    Player player1 = new Human(player1Name, 'X');
                    Player player2 = new Human(player2Name, 'X');
            
                    Board gameBoard = new Board(numberOfRows, numberOfColumns);
                    Gameplay multiplayerGame = new Gameplay(gameBoard);
                
                do
                {
                    if(Gameplay.turnCount % 2 == 0)
                        multiplayerGame.Turn(player1);
                    else
                        multiplayerGame.Turn(player2);

                    gameBoard.DrawBoard();

                    //Get player selection
                    int.TryParse(Console.ReadLine(), out choice);

                    //Validate player selection
                    while (choice < 1 || choice > numberOfRows)
                    {
                        Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfRows);
                        int.TryParse(Console.ReadLine(), out choice);
                    }

                    if (Gameplay.turnCount % 2 == 0)
                        multiplayerGame.InsertToken(player1, choice, gameBoard);
                    else
                        multiplayerGame.InsertToken(player2, choice, gameBoard);
                } while (!winner);
            }
        }
    }
}