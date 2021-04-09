/*
 * Adding a turn counter to Gameplay() class negated the need for Player(int designation).
 * Added abstract to Player class and created AI & Human inherited classes.
 * Moved Token class to property of Player instead.
 * Added user selection to console (Move to a class later?)
 * Deleted Controller class (Might add it back later if needed)
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

        public Board(List<char> cells, int rows, int columns)
        {
            Cells = cells;
            Rows = rows;
            Columns = columns;
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
        static int turnCount;
        Board currentGameBoard;

        public Gameplay(Board currentGameBoard)
        {
            this.currentGameBoard = currentGameBoard;
        }

        public void Turn(Player currentPlayer)
        {
            if (turnCount != 0 && turnCount % 2 == 0)
                Console.WriteLine("Player 1's turn");
            else
                Console.WriteLine("Player 2's turn");

            turnCount++;
        }

        public void InsertToken(Player currentPlayer, int selection)
        {

            for (int i = selection; i < currentGameBoard.Columns; i++)
            {
                for (int j = 0; j < currentGameBoard.Rows; j++)
                    Console.Write("{0} ", currentGameBoard.Cells[i]);

                Console.Write("\n");
            }


        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<char> board = new List<char>();
            int numberOfRows = 7;
            int numberOfColumns = 6;
            int numberOfCells = numberOfRows * numberOfColumns;
            int choice;


            for (int i = 0; i <= numberOfCells; i++)
            {
                board.Add('#');
            }

            Board gameBoard = new Board(board, numberOfRows, numberOfColumns);

            gameBoard.DrawBoard();

            int.TryParse(Console.ReadLine(), out choice);

            while (choice < 1 || choice > numberOfRows)
            {
                Console.WriteLine("Please enter a valid number between 1 - {0}", numberOfRows);
                int.TryParse(Console.ReadLine(), out choice);
            }
        }
    }
}