using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braaksma_Connect_Four
{
    class Player
    {
        private string _name;
        private int _designation;

        public Player(string name, int designation)
        {
            _name = name;
            _designation = designation;
        }
    }


    class AI : Player
    {
        private int _difficulty;
        public AI(string name, int designation, int difficulty) : base(name, designation)
        {
            _difficulty = difficulty;
        }
    }


    class Board
    {
        private List<char> _boardPiece;
        private int _rows;
        private int _columns;

        public Board(List<char> fullBoard, int rows, int columns)
        {
            _boardPiece = fullBoard;
            _rows = rows;
            _columns = columns;
        }
        public void DrawBoard()
        {
            for(int i = 0;i < _columns; i++)
            {
                for(int j = 0;j < _rows; j++)
                    Console.Write("{0} ", _boardPiece[i]);
                
                    Console.Write("\n");
            }
        }
    }


    class Token
    {

    }    


    class Controller
    {

    }


    class Program
    {
        static void Main(string[] args)
        {
            List<char> boardPiece = new List<char>();
            
            for(int i = 0; i < 44; i++) 
                boardPiece.Add('#');
            
            Board gameBoard = new Board(boardPiece, 7, 6);

            gameBoard.DrawBoard();            
        }
    }
}
