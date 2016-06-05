// CS 420 Project 3
// Created by Ara Malayan and Ethan Sitt

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
        public override String ToString()
        {
            return "" + Enum.GetName(typeof(Coordinates), x) + (y + 1);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                for (int j = 0; j < this.size; j++)
                    board[i, j] = oldBoard.getBoard()[i, j];

            this.moveList = oldBoard.getMoveList().ConvertAll(move => move);
            this.takenSpaces = oldBoard.getTakenSpaces().ConvertAll(coordinate => new Coordinate(coordinate.x, coordinate.y));
            this.nextSpaces = oldBoard.getNextSpaces().ConvertAll(coordinate => new Coordinate(coordinate.x, coordinate.y));
        }

        public void printMoveList()
        {
            Console.WriteLine("Player   Opponent");
            for (int i = 0; i < moveList.Count; i += 2)
                Console.WriteLine(moveList[i] + "       " + moveList[i + 1]);
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
                        Console.Write("X ");
                    else if (board[(int)((Coordinates)Enum.Parse(typeof(Coordinates), coordinateName)), i] == 2)
                        Console.Write("O ");
                    else
                        Console.Write("- ");
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
                if (y1 < size && board[x1, y1] == 0)
                    nextSpaces.Add(new Coordinate(x1, y1));

                x1 = x;
                y1 = y - 1;
                if (y1 >= 0 && board[x1, y1] == 0)
                    nextSpaces.Add(new Coordinate(x1, y1));

                x1 = x + 1;
                y1 = y;
                if (x1 < size && board[x1, y1] == 0)
                    nextSpaces.Add(new Coordinate(x1, y1));

                x1 = x - 1;
                y1 = y;
                if (x1 >= 0 && board[x1, y1] == 0)
                    nextSpaces.Add(new Coordinate(x1, y1));

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
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            int time;
            bool success = false;
            Console.WriteLine("How many seconds should the program search for?");
            time = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Who is taking the first turn {Human(h) or Computer(c)}: ");
            input = Console.ReadLine();
            if (input.ToUpper() == "C")
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

            while (winner == 0)
            {
                testBoard.printBoard();
                Console.Write("Please enter a space {A-H + 1-8 e.g. E5}: ");
                opponentMove = Console.ReadLine();
                success = testBoard.setPiece(human, opponentMove);
                while(!success)
                {
                    Console.WriteLine("Invalid move - space already taken");
                    Console.Write("Please enter a space {A-H + 1-8 e.g. E5}: ");
                    opponentMove = Console.ReadLine();
                    success = testBoard.setPiece(human, opponentMove);
                }
                winner = testBoard.checkWin();
                if (winner > 0)
                    break;
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

        public static bool bogoMove = false;
        //bogomove overrides all other moves

        public static int debugMode = 2;
        //0 = no debug
        //1 = print search tree
        //2 = print decision overrides

        public static int depthLimit = 3;
        //sets the depth of the search

        public static int printDepthLimit = 10;
        //sets the depth of the search

        public static Random r = new Random();
        //for all your bogo needs

        public static string minimaxDecision(Board board, int player)
        {
            int max = -100000;
            string decision = null;
            List<Coordinate> nextSpaces = board.getNextSpaces();
            foreach (Coordinate space1 in nextSpaces)
            {
                int newHeurstic = calcMin(board, space1, 0, player, max);
                if (newHeurstic > max)
                {
                    max = newHeurstic;
                    decision = Enum.GetName(typeof(Coordinates), space1.x) + (space1.y + 1);
                    if (debugMode == 2) Console.WriteLine(max + " " + decision);
                }
            }
            if (debugMode == 1) Console.WriteLine("MAX " + decision + " " + max);
            if (bogoMove) //random move from all valid moves chosen.  can be enabled if the ai is worse than a random number generator
            {
                int lol = r.Next(0, nextSpaces.Count - 1);
                decision = Enum.GetName(typeof(Coordinates), nextSpaces[lol].x) + (nextSpaces[lol].y + 1);
            }
            return decision;
        }

        public static int calcMin(Board board, Coordinate space, int currentDepth, int player, int max)
        {
            int min = 10000000;
            Board tempBoard = new Board(board);
            tempBoard.setPiece(player, space);

            if (currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = tempBoard.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int newHeurstic = calcMax(tempBoard, space1, currentDepth + 1, player, min);
                    if (newHeurstic < min)
                        min = newHeurstic;
                    else if (newHeurstic < max) //continue;
                        return min; //alpha beta pruning
                }
            }
            else
                min = calculateScore(tempBoard, player);
            if (debugMode == 1 && currentDepth <= printDepthLimit)
            {
                string printBreak = "";
                printBreak += "      ";
                for (int z = 0; z < currentDepth; z++) printBreak += "      ";
                Console.WriteLine(printBreak + "MIN " + space.ToString() + " " + min);
            }
            return min;
        }

        public static int calcMax(Board board, Coordinate space, int currentDepth, int player, int min)
        {
            int max = -10000000;
            Board tempBoard = new Board(board);
            tempBoard.setPiece(player, space);
            if (currentDepth <= depthLimit)
            {
                List<Coordinate> nextSpaces = tempBoard.getNextSpaces();
                foreach (Coordinate space1 in nextSpaces)
                {
                    int newHeurstic = calcMin(tempBoard, space1, currentDepth + 1, player, max);
                    if (newHeurstic > max)
                        max = newHeurstic;
                    else if (newHeurstic < min) //continue;
                        return max; //alpha beta pruning
                }
            }
            else
                max = calculateScore(tempBoard, player);
            if (debugMode == 1 && currentDepth <= printDepthLimit)
            {
                string printBreak = "";
                printBreak += "      ";
                for (int z = 0; z < currentDepth; z++) printBreak += "      ";
                Console.WriteLine(printBreak + "MAX " + space.ToString() + " " + max);
            }
            return max;
        }

        public static int calculateScore(Board board, int player)
        {
            int[,] activeBoard = board.getBoard();
            int size = (int)(Math.Sqrt(activeBoard.Length));
            int opponent = 0;
            int lastPiece = 0;
            int playerTotal = 0;
            int opponentTotal = 0;
            int count = 0;

            //determines opponent
            opponent = player == 1 ? 2 : 1;
            // Horizontal check
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (activeBoard[i, j] == 0)
                    {
                        if (lastPiece == player)
                            playerTotal += (int)Math.Pow(10, count);
                        else if (lastPiece == opponent)
                            opponentTotal += (int)Math.Pow(10, count);
                        count = 0;
                    }
                    else if (activeBoard[i, j] == lastPiece && activeBoard[i, j] != 0)
                        count++;
                    else if (activeBoard[i, j] == player && lastPiece == opponent)
                    {
                        opponentTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if (activeBoard[i, j] == opponent && lastPiece == player)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if ((activeBoard[i, j] == player || activeBoard[i, j] == opponent) && lastPiece == 0)
                        count = 1;
                    else
                        count = 0;
                    lastPiece = activeBoard[i, j];
                }

                if (lastPiece == player)
                    playerTotal += (int)Math.Pow(10, count);
                else if (lastPiece == opponent)
                    opponentTotal += (int)Math.Pow(10, count);
                count = 0;
                lastPiece = 0;
            }

            // Horizontal check2
            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = size - 1; j >= 0; j--)
                {
                    if (activeBoard[i, j] == 0)
                    {
                        if (lastPiece == player)
                            playerTotal += (int)Math.Pow(10, count);
                        else if (lastPiece == opponent)
                            opponentTotal += (int)Math.Pow(10, count);
                        count = 0;
                    }
                    else if (activeBoard[i, j] == lastPiece && activeBoard[i, j] != 0)
                        count++;
                    else if (activeBoard[i, j] == player && lastPiece == opponent)
                    {
                        opponentTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if (activeBoard[i, j] == opponent && lastPiece == player)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if ((activeBoard[i, j] == player || activeBoard[i, j] == opponent) && lastPiece == 0)
                        count = 1;
                    else
                        count = 0;
                    lastPiece = activeBoard[i, j];
                }

                if (lastPiece == player)
                    playerTotal += (int)Math.Pow(10, count);
                else if (lastPiece == opponent)
                    opponentTotal += (int)Math.Pow(10, count);
                count = 0;
                lastPiece = 0;
            }
            
            // vertical check
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (activeBoard[j, i] == 0)
                    {
                        if (lastPiece == player)
                            playerTotal += (int)Math.Pow(10, count);
                        else if (lastPiece == opponent)
                            opponentTotal += (int)Math.Pow(10, count);
                        count = 0;
                    }
                    else if (activeBoard[j, i] == lastPiece && activeBoard[j, i] != 0)
                        count++;
                    else if (activeBoard[j, i] == player && lastPiece == opponent)
                    {
                        opponentTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if (activeBoard[j, i] == opponent && lastPiece == player)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if ((activeBoard[j, i] == player || activeBoard[j, i] == opponent) && lastPiece == 0)
                        count = 1;
                    else
                        count = 0;
                    lastPiece = activeBoard[j, i];
                }

                if (lastPiece == player)
                    playerTotal += (int)Math.Pow(10, count);
                else if (lastPiece == opponent)
                    opponentTotal += (int)Math.Pow(10, count);
                count = 0;
                lastPiece = 0;
            }

            // vertical check2
            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = size - 1; j >= 0; j--)
                {
                    if (activeBoard[j, i] == 0)
                    {
                        if (lastPiece == player)
                            playerTotal += (int)Math.Pow(10, count);
                        else if (lastPiece == opponent)
                            opponentTotal += (int)Math.Pow(10, count);
                        count = 0;
                    }
                    else if (activeBoard[j, i] == lastPiece && activeBoard[j, i] != 0)
                        count++;
                    else if (activeBoard[j, i] == player && lastPiece == opponent)
                    {
                        opponentTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if (activeBoard[j, i] == opponent && lastPiece == player)
                    {
                        playerTotal += (int)Math.Pow(10, count);
                        count = 1;
                    }
                    else if ((activeBoard[j, i] == player || activeBoard[j, i] == opponent) && lastPiece == 0)
                        count = 1;
                    else
                        count = 0;
                    lastPiece = activeBoard[j, i];
                }

                if (lastPiece == player)
                    playerTotal += (int)Math.Pow(10, count);
                else if (lastPiece == opponent)
                    opponentTotal += (int)Math.Pow(10, count);
                count = 0;
                lastPiece = 0;
            }

            //Console.WriteLine("Heuristics:\n" + playerTotal + " " + opponentTotal);
            return playerTotal - opponentTotal;
        }
    }
}
