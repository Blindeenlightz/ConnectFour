﻿/*

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
            for (int i = 0; i < Rows * Columns; i++)
            {
                Cells.Add('#');
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
            Console.WriteLine("It's {0}'s turn!", currentPlayer.Name);
            turnCount++;
        }

        public void DrawBoard()
        {
            int counter = 0;
            foreach (char item in Cells)
            {       
                if(counter % Columns == 0 && counter != 0)
                    Console.Write("\n");

                Console.Write("{0} ", item);
                
                counter++;
            }

            Console.Write("\n");

            for (int i = 0; i <= Rows; i++)
            {
                Console.Write("{0} ", i + 1);

            }
            Console.WriteLine("\n\nSelect a number between 1 - {0} to place your piece", Columns);
        }

        public void InsertToken(Player currentPlayer, int selection) //doesn't change the global currentGameBoard
        {
            //Replace the blank square with the players token in the gameboard (<List>)
            for (int i = (Cells.Count - 1) - (Columns - selection); i > 0 ; i -= Columns)
            {
                if(Cells[i] != currentPlayer.Token)
                {
                    Cells[i] = currentPlayer.Token;
                    Console.WriteLine(Cells[i]);
                    break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<char> board = new List<char>();
            //Choose the size of board to play with
            int numberOfColumns = 7;
            int numberOfRows = 6;
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

                    Gameplay singlePlayerGame = new Gameplay(numberOfRows, numberOfColumns);


                do
                {
                    singlePlayerGame.DrawBoard();

                    //Get player selection
                    int.TryParse(Console.ReadLine(), out choice);

                    //Validate player selection
                    while (choice < 1 || choice > numberOfColumns)
                    {
                        Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
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
                    Player player2 = new Human(player2Name, '0');
            
                    Gameplay multiplayerGame = new Gameplay(numberOfRows, numberOfColumns);
                
                do
                {
                    if(Gameplay.turnCount % 2 == 0)
                        multiplayerGame.Turn(player1);
                    else
                        multiplayerGame.Turn(player2);

                    multiplayerGame.DrawBoard();

                    //Get player selection
                    int.TryParse(Console.ReadLine(), out choice);

                    //Validate player selection
                    while (choice < 1 || choice > numberOfColumns)
                    {
                        Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfColumns);
                        int.TryParse(Console.ReadLine(), out choice);
                    }

                    if (Gameplay.turnCount % 2 == 0)
                        multiplayerGame.InsertToken(player1, choice);
                    else
                        multiplayerGame.InsertToken(player2, choice);

                } while (!winner);
            }
        }
    }
}