using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CS420Project3
{
    enum Coordinates { A, B, C, D, E, F, G, H };
    class Board
    {
        private int[,] board;
        private List<string> moveList;

        public Board()
        {
            board = new int[8, 8];
        }

        public Board(Board oldBoard)
        {
            this.board = oldBoard.getBoard();
            this.moveList = oldBoard.getMoveList();
        }

        void printMoveList()
        {
            Console.WriteLine("Player   Opponent");
            for (int i = 0; i < moveList.Count; i += 2)
            {
                Console.WriteLine(moveList[i] + "   " + moveList[i + 1]);
            }
        }

        public void printBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8");
            foreach (string coordinateName in Enum.GetNames(typeof(Coordinates)))
            {
                Console.Write("{0} ", coordinateName);
                for (int i = 0; i < Math.Sqrt(board.Length); i++)
                {
                    if (board[(int)((Coordinates)Enum.Parse(typeof(Coordinates), coordinateName)), i] == 1)
                    {
                        Console.Write("X ");
                    }
                    else if (board[(int)((Coordinates)Enum.Parse(typeof(Coordinates), coordinateName)), i] == 2)
                    {
                        Console.Write("O ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
        }

        public int[,] getBoard()
        {
            return board;
        }

        public List<string> getMoveList()
        {
            return moveList;
        }

/*        public bool setPiece(int piece, string location)
        {
            bool success = false;
            if(board[(int)((Coordinates)Enum.Parse(typeof(Coordinates), location.ElementAt(0))
        }*/
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            Board testBoard = new Board();
            testBoard.printBoard();
            Thread.Sleep(5000);
        }
    }
}
