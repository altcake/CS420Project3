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

        public Coordinate()
        {
            this.x = -1;
            this.y = -1;
        }
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
        private Coordinate lastMove; // Used in win detection
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
                lastMove = temp;
                moveList.Add(move);
                success = true;
                int winner = checkWin();
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

        public void removePiece(string location)
        {
            int x = (int)((Coordinates)Enum.Parse(typeof(Coordinates), location[0].ToString().ToUpper()));
            int y = ((int)Char.GetNumericValue(location[1])) - 1;
            board[x, y] = 0;
        }

        public int checkWin()
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
    }
    class Program
    {
        static void Main(string[] args)
        {
            Board testBoard = new Board();
            int winner = 0;
            string input;
            string humanMove;
            int human;
            int computer;
            Console.WriteLine("Who is taking the first turn {Human(h) or Computer(c)}: ");
            input = Console.ReadLine();
            if(input.ToUpper() == "C")
            {
                computer = 1;
                human = 2;
                testBoard.setPiece(computer, "D5");
            }
            else
            {
                computer = 2;
                human = 1;
            }

            while(winner == 0)
            {
                testBoard.printBoard();
                Console.Write("Please enter a space {A-H + 1-8 e.g. E5}: ");
                humanMove = Console.ReadLine();
                testBoard.setPiece(human, humanMove);
                winner = testBoard.checkWin();

                testBoard.setPiece(computer, minimaxDecision(testBoard, computer));
            }
            testBoard.printBoard();
            printWinner(winner);
            Thread.Sleep(5000);
        }

        public static void printWinner(int winner)
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

        public static string minimaxDecision(Board board, int player)
        {
            int depthLimit = 3;
            int max = -100000;

            string decision = null;
            List<Coordinate> nextSpaces = board.getNextSpaces();
            foreach (Coordinate space1 in nextSpaces)
            {
                int thing = calcMax(board, space1, 1, depthLimit);
                if (thing > max)
                {
                    max = thing;
                    decision = Enum.GetName(typeof(Coordinates), space1.x) + (space1.y + 1);
                }
            }

            return decision;
        }

        public static int calcMin(Board board, Coordinate space, int currentDepth, int depthLimit)
        {
            int min = 100000;
            if(currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = board.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int thing = calcMax(board, space1, currentDepth + 1, depthLimit);
                    if (thing < min)
                    {
                        min = thing;
                    }
                }
            }
            return min;
        }

        public static int calcMax(Board board, Coordinate space, int currentDepth, int depthLimit)
        {
            int max = -100000;
            if(currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = board.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int thing = calcMin(board, space1, currentDepth + 1, depthLimit);
                    if (thing > max)
                    {
                        max = thing;
                    }
                }
            }
            return max;
        }

        public static int calculateScore(Board board, string space, int player)
        {
            board.setPiece(player, space);
            int[,] activeBoard = board.getBoard();
            int size = (int)(Math.Sqrt(activeBoard.Length));
            int opponent = 0;
            int lastPiece = 0;
            int playerTotal = 0;
            int opponentTotal = 0;
            int count = 0;

            if(player == 1)
            {
                opponent = 2;
            }
            else
            {
                opponent = 1;
            }

            // Horizontal check
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; i < size; i++)
                {
                    if (activeBoard[i,j] == player && lastPiece == player)
                    {
                        count++;
                    }

                    else if (activeBoard[i,j] == opponent && lastPiece == opponent)
                    {
                        count++;
                    }

                    else if (activeBoard[i,j] == 0)
                    {
                        if(lastPiece == player)
                        {
                            playerTotal += (int)Math.Pow(10, count);
                        }
                        else if(lastPiece == opponent)
                        {
                            opponentTotal += (int)Math.Pow(10, count);
                        }
                        count = 0;
                    }

                    else if((activeBoard[i,j] == player || activeBoard[i,j] == opponent) && lastPiece == 0)
                    {
                        count = 1;
                    }
                }

                if (lastPiece == player)
                {
                    playerTotal += (int)Math.Pow(10, count);
                }
                else if (lastPiece == opponent)
                {
                    opponentTotal += (int)Math.Pow(10, count);
                }
                count = 0;
            }

            //Vertical check
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; i < size; i++)
                {
                    if (activeBoard[j,i] == player && lastPiece == player)
                    {
                        count++;
                    }

                    else if (activeBoard[j,i] == opponent && lastPiece == opponent)
                    {
                        count++;
                    }

                    else if (activeBoard[j,i] == 0)
                    {
                        if (lastPiece == player)
                        {
                            playerTotal += (int)Math.Pow(10, count);
                        }
                        else if (lastPiece == opponent)
                        {
                            opponentTotal += (int)Math.Pow(10, count);
                        }
                        count = 0;
                    }

                    else if ((activeBoard[j,i] == player || activeBoard[j,i] == opponent) && lastPiece == 0)
                    {
                        count = 1;
                    }
                }

                if (lastPiece == player)
                {
                    playerTotal += (int)Math.Pow(10, count);
                }
                else if (lastPiece == opponent)
                {
                    opponentTotal += (int)Math.Pow(10, count);
                }
                count = 0;
            }
            board.removePiece(space);
            return playerTotal - opponentTotal;
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
