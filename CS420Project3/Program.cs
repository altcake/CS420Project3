using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CS420Project3
{
    
    enum Coordinates { A, B, C, D, E, F, G, H };
    enum Pieces { X = 1, O = 2 };
    class Coordinate
    {
        static Random r = new Random();
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
            this.size = oldBoard.size;
            this.board = new int[8,8];
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    board[i, j] = oldBoard.getBoard()[i, j];
                }
            }

            this.moveList = oldBoard.getMoveList().ConvertAll(move => move);
            this.takenSpaces = oldBoard.getTakenSpaces().ConvertAll(coordinate => new Coordinate(coordinate.x, coordinate.y));
            this.nextSpaces = oldBoard.getNextSpaces().ConvertAll(coordinate => new Coordinate(coordinate.x, coordinate.y));
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
            int x = (int)((Coordinates)Enum.Parse(typeof(Coordinates), location[0].ToString().ToUpper()));
            int y = ((int)Char.GetNumericValue(location[1])) - 1;
            return setPiece(piece, new Coordinate(x, y));
        }

        public bool setPiece(int piece, Coordinate location)
        {
            bool success = false;
            int x = location.x;
            int y = location.y;
            if (board[x, y] == 0)
            {
                board[x, y] = piece;
                string move = Enum.GetName(typeof(Coordinates), location.x) + (location.y + 1);
                Coordinate temp = new Coordinate(x, y);
                takenSpaces.Add(location);
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

        public bool testPiece(int piece, string location)
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
            string computerMove;
            string opponentMove;
            int human;
            int computer;
            Console.WriteLine("Who is taking the first turn {Human(h) or Computer(c)}: ");
            input = Console.ReadLine();
            if(input.ToUpper() == "C")
            {
                computer = 1;
                human = 2;
                computerMove = "D5";
                testBoard.setPiece(computer, computerMove);
                Console.WriteLine(Enum.GetName(typeof(Pieces), computer) + " to " + computerMove + "\n");

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
                opponentMove = Console.ReadLine();
                testBoard.setPiece(human, opponentMove);
                winner = testBoard.checkWin();
                if(winner > 0)
                {
                    break;
                }
                computerMove = minimaxDecision(testBoard, computer);
                testBoard.setPiece(computer, computerMove);
                Console.WriteLine(Enum.GetName(typeof(Pieces), computer) + " to " + computerMove + "\n");
                winner = testBoard.checkWin();
            }
            testBoard.printBoard();
            printWinner(winner);
            Console.Write("Press Enter to quit.");
            Console.ReadLine();
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
                // Console.WriteLine("No winner detected\n");
            }
        }

        public static string minimaxDecision(Board board, int player)
        {
            int depthLimit = 2;
            int max = -100000;
            
            string decision = null;
            List<Coordinate> nextSpaces = board.getNextSpaces();
            foreach (Coordinate space1 in nextSpaces)
            {
                int thing = calcMin(board, space1, 0, depthLimit, player);
                
                if (thing >= max)
                {
                    max = thing;
                    
                    decision = Enum.GetName(typeof(Coordinates), space1.x) + (space1.y + 1);
                    Console.WriteLine(max + " " + decision);
                }
            }
            //int lol =  r.Next(0, nextSpaces.Count-1);
            //decision = Enum.GetName(typeof(Coordinates), nextSpaces[lol].x) + (nextSpaces[lol].y + 1);
            return decision;
        }

        public static int calcMin(Board board, Coordinate space, int currentDepth, int depthLimit, int player)
        {
            int min = 100000;
            int opponent;
            Board tempBoard = new Board(board);
            if (player == 1)
            {
                opponent = 2;
            }
            else
            {
                opponent = 1;
            }
            tempBoard.setPiece(player, space);
            if(board.checkWin() > 0)
            {
                return min;
            }

            if(currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = board.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int thing = calcMax(board, space1, currentDepth + 1, depthLimit, player);
                    if (thing < min)
                    {
                        min = thing;
                    }
                }
            }
            else
            {
                min = calculateScore(board, space, player);
            }
            //if (currentDepth == 0) Console.WriteLine("MIN " + min);
            return min;
        }

        public static int calcMax(Board board, Coordinate space, int currentDepth, int depthLimit, int player)
        {
            int max = -100000;
            if (board.checkWin() > 0)
            {
                return max;
            }
            if (currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = board.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int thing = calcMin(board, space1, currentDepth + 1, depthLimit, player);
                    if (thing > max)
                    {
                        max = thing;
                    }
                }
            }
            else
            {
                max = calculateScore(board, space, player);
            }
            //Console.WriteLine("MAX " + max);
            return max;
        }

        public static int calculateScore(Board board, Coordinate space, int player)
        {
            string tempSpace = Enum.GetName(typeof(Coordinates), space.x) + (space.y + 1);
            board.testPiece(player, tempSpace);
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
                for (int j = 0; j < size; j++)
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
                            opponentTotal += (int)Math.Pow(10, count) + 1;
                        }
                        count = 0;
                    }

                    else if (activeBoard[i,j] == player && lastPiece == opponent)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }

                    else if (activeBoard[i,j] == opponent && lastPiece == player)
                    {
                        opponentTotal += (int)Math.Pow(10, count) + 1;
                        count = 1;
                    }

                    else if((activeBoard[i,j] == player || activeBoard[i,j] == opponent) && lastPiece == 0)
                    {
                        count = 1;
                    }
                    lastPiece = activeBoard[i, j];
                }

                if (lastPiece == player)
                {
                    playerTotal += (int)Math.Pow(10, count);
                }
                else if (lastPiece == opponent)
                {
                    opponentTotal += (int)Math.Pow(10, count) + 1;
                }
                count = 0;
            }
            lastPiece = 0;

            //Vertical check
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (activeBoard[j, i] == 0)
                    {
                        if (lastPiece == player)
                        {
                            playerTotal += (int)Math.Pow(10, count);
                        }
                        else if (lastPiece == opponent)
                        {
                            opponentTotal += (int)Math.Pow(10, count) + 1;
                        }
                        count = 0;
                    }
                    else if (activeBoard[j,i] == player && lastPiece == player)
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
                            opponentTotal += (int)Math.Pow(10, count) + 1;
                        }
                        count = 0;
                    }

                    else if (activeBoard[j,i] == player && lastPiece == opponent)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }

                    else if (activeBoard[j, i] == opponent && lastPiece == player)
                    {
                        opponentTotal += (int)Math.Pow(10, count) + 1;
                        count = 1;
                    }

                    else if ((activeBoard[j,i] == player || activeBoard[j,i] == opponent) && lastPiece == 0)
                    {
                        count = 1;
                    }
                    lastPiece = activeBoard[j, i];
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
            board.removePiece(tempSpace);
            //Console.WriteLine("Heuristics:\n" + playerTotal + " " + opponentTotal);
            return playerTotal - opponentTotal;
        }
    }
}
