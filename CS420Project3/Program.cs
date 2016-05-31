using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CS420Project3
{
    enum Coordinates { A, B, C, D, E, F, G, H };
    class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
    // TODO: possibly merge moveList and takenSpaces since they'll be storing the same data.
    class Board
    {
        private int[,] board;
        private int size { get; }
        private List<string> moveList;
        private List<Coordinate> takenSpaces;
        private List<Coordinate> nextSpaces;

        public Board()
        {
            board = new int[8, 8];
            size = (int)Math.Sqrt(board.Length);
            moveList = new List<string>();
            takenSpaces = new List<Coordinate>();
            nextSpaces = new List<Coordinate>();
        }

        public Board(Board oldBoard)
        {
            this.board = oldBoard.getBoard();
            this.size = oldBoard.size;
            this.moveList = oldBoard.getMoveList();
            this.takenSpaces = oldBoard.getTakenSpaces();
            this.nextSpaces = oldBoard.getNextSpaces();
        }

        public void printMoveList()
        {
            Console.WriteLine("Player   Opponent");
            for (int i = 0; i < moveList.Count; i += 2)
            {
                Console.WriteLine(moveList[i] + "       " + moveList[i + 1]);
            }
        }

        public void printBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8");
            foreach (string coordinateName in Enum.GetNames(typeof(Coordinates)))
            {
                Console.Write("{0} ", coordinateName);
                for (int i = 0; i < size; i++)
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
            Console.WriteLine();
        }

        public int[,] getBoard()
        {
            return board;
        }

        public List<string> getMoveList()
        {
            return moveList;
        }

        public List<Coordinate> getTakenSpaces()
        {
            return this.takenSpaces;
        }

        public void printTakenSpaces()
        {
            string stringSpace;
            Console.WriteLine("Taken Spaces:");
            Console.WriteLine("-------------");
            foreach (Coordinate space in takenSpaces)
            {
                stringSpace = Enum.GetName(typeof(Coordinates), space.x) + (space.y + 1);
                Console.WriteLine(stringSpace);
            }
            Console.WriteLine();
        }

        public List<Coordinate> getNextSpaces()
        {
            return this.nextSpaces;
        }

        public void printNextSpaces()
        {
            string stringSpace;
            Console.WriteLine("Next Spaces:");
            Console.WriteLine("-------------");
            foreach (Coordinate space in nextSpaces)
            {
                stringSpace = Enum.GetName(typeof(Coordinates), space.x) + (space.y + 1);
                Console.WriteLine(stringSpace);
            }
            Console.WriteLine();
        }

        public bool setPiece(int piece, string location)
        {
            bool success = false;
            int x = (int)((Coordinates)Enum.Parse(typeof(Coordinates), location[0].ToString().ToUpper()));
            int y = ((int)Char.GetNumericValue(location[1])) - 1;
            if (board[x, y] == 0)
            {
                board[x, y] = piece;
                string move = location[0].ToString().ToUpper() + y;
                Coordinate temp = new Coordinate(x, y);
                takenSpaces.Add(temp);
                moveList.Add(move);
                success = true;
                int winner = checkWin(temp);
                printWinner(winner);
                nextSpaces.RemoveAll(item => item.x == x && item.y == y);
                int x1 = x;
                int y1 = y;

                x1 = x;
                y1 = y + 1;
                if (y1 < size)
                {
                    if (board[x1, y1] == 0)
                    {
                        nextSpaces.Add(new Coordinate(x1, y1));
                    }
                }
                x1 = x;
                y1 = y - 1;
                if (y1 >= 0)
                {
                    if (board[x1, y1] == 0)
                    {
                        nextSpaces.Add(new Coordinate(x1, y1));
                    }
                }
                x1 = x + 1;
                y1 = y;
                if (x1 < size)
                {
                    if (board[x1, y1] == 0)
                    {
                        nextSpaces.Add(new Coordinate(x1, y1));
                    }
                }
                x1 = x - 1;
                y1 = y;
                if (x1 >= 0)
                {
                    if (board[x1, y1] == 0)
                    {
                        nextSpaces.Add(new Coordinate(x1, y1));
                    }
                }
            }
            else
            {
                Console.WriteLine("Placement failed - Space already used.");
            }
            return success;
        }

        public int checkWin(Coordinate lastMove)
        {
            int winner = 0;
            int tempWinner = 0;
            int count = 0;

            // Horizontal check
            tempWinner = board[lastMove.x, 0];
            for (int i = 0; i < size; i++)
            {
                if (board[lastMove.x, i] == tempWinner && tempWinner != 0)
                {
                    count++;
                }

                else
                {
                    count = 1;
                    tempWinner = board[lastMove.x, i];
                }

                if (count >= 4 && tempWinner != 0)
                {
                    winner = tempWinner;
                    break;
                }

            }
            //Vertical check
            tempWinner = board[0, lastMove.y];
            for (int i = 0; i < size; i++)
            {
                if (board[i, lastMove.y] == tempWinner && tempWinner != 0)
                {
                    count++;
                }

                else
                {
                    count = 1;
                    tempWinner = board[i, lastMove.y];
                }

                if (count >= 4 && tempWinner != 0)
                {
                    winner = tempWinner;
                    break;
                }
            }
            return winner;
        }

        public void printWinner(int winner)
        {
            if (winner != 0)
            {
                if (winner == 1)
                {
                    Console.WriteLine("X is the winner!\n");
                }
                else // winner == 2, O wins
                {
                    Console.WriteLine("O is the winner!\n");
                }
            }
            else
            {
 //               Console.WriteLine("No winner detected\n");
            }
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            Board testBoard = new Board();
            testBoard.printBoard();
            testBoard.setPiece(1, "D5");
            testBoard.printBoard();
            testBoard.printNextSpaces();
            testBoard.setPiece(2, "E5");
            testBoard.printBoard();
            testBoard.printNextSpaces();
            testBoard.setPiece(1, "d6");
            testBoard.printBoard();
            testBoard.printNextSpaces();
            testBoard.setPiece(2, "d7");
            testBoard.setPiece(1, "d4");
            testBoard.setPiece(1, "d3");
            testBoard.printBoard();
            Thread.Sleep(40000);
        }

/*        public string minmaxDecision(Board board)
        {
            int value = maxValue(board);
            string[] successors
        }

        public int maxValue(Board board)
        {

        }*/

 /*       string makemove(Board board)
        {
            int[,] workingBoard = board.getBoard();
            int best = -20000, score, mi, mj;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (b[i][j] == 0)
                    {
                        b[i][j] = 1; // make move on board
                        score = min(depth - 1);
                        if (score > best) { mi = i; mj = j; best = score; }
                        b[i][j] = 0; // undo move
                    }
                }
            }
            cout << "my move is " << mi << " " << mj << endl;
            b[mi][mj] = 1;
        }
        int min(int depth)
        {
            int best = 20000, score;
            if (check4winner() != 0) return (check4winner());
            if (depth == 0) return (evaluate());
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (b[i][j] == 0)
                    {
                        b[i][j] = 2; // make move on board
                        score = max(depth - 1);
                        if (score < best) best = score;
                        b[i][j] = 0; // undo move
                    }
                }
            }
            return (best);
        }

        int max(int depth)
        {
            int best = -20000, score;
            if (check4winner() != 0) return (check4winner());
            if (depth == 0) return (evaluate());
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (b[i][j] == 0)
                    {
                        b[i][j] = 1; // make move on board
                        score = min(depth - 1);
                        if (score > best) best = score;
                        b[i][j] = 0; // undo move
                    }
                }
            }
            return (best);
        }*/

    }
}
