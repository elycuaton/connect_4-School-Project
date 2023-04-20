using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class connectBoard
{
    private readonly char[,] board = new char[7, 6];
    private bool gameOver = false;

    public connectBoard()
    {
        // Initialize the board with empty spaces
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                board[i, j] = ' ';
            }
        }
    }
    public bool IsColumnAvailable(int col)
    {
        for (int j = 0; j < 6; j++)
        {
            if (board[col, j] == ' ')
            {
                return true;
            }
        }
        return false;
    }

    public void DropPiece(int col, char symbol)
    {
        for (int j = 5; j >= 0; j--)
        {
            if (board[col, j] == ' ')
            {
                board[col, j] = symbol;
                return;
            }
        }
    }

    public bool CheckForWin()
    {
        // Check for horizontal win
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (board[i, j] != ' ' && board[i, j] == board[i + 1, j] && board[i + 1, j] == board[i + 2, j] && board[i + 2, j] == board[i + 3, j])
                {
                    return true;
                }
            }
        }

        // Check for vertical win
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] != ' ' && board[i, j] == board[i, j + 1] && board[i, j + 1] == board[i, j + 2] && board[i, j + 2] == board[i, j + 3])
                {
                    return true;
                }
            }
        }

        // Check for diagonal win (top-left to bottom-right)
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] != ' ' && board[i, j] == board[i + 1, j + 1] && board[i + 1, j + 1] == board[i + 2, j + 2] && board[i + 2, j + 2] == board[i + 3, j + 3])
                {
                    return true;
                }
            }
        }

        // Check for diagonal win (bottom-left to top-right)
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; j < 6; j++)
            {
                if (board[i, j] != ' ' && board[i, j] == board[i + 1, j - 1] && board[i + 1, j - 1] == board[i + 2, j - 2] && board[i + 2, j - 2] == board[i + 3, j - 3])
                {
                    return true;
                }
            }
        }

        return false;        // Implementation...
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < 7; i++)
        {
            if (board[i, 0] == ' ')
            {
                // If there's at least one empty cell in the top row, the board is not full
                return false;
            }
        }

        // All cells in the top row are occupied, so the board is full
        return true;
    }

    public void DisplayBoard()
    {
        Console.Clear();
        Console.WriteLine(" 1 2 3 4 5 6 7");
        Console.WriteLine("+-------------+");
        for (int j = 0; j < 6; j++)
        {
            Console.Write("|");
            for (int i = 0; i < 7; i++)
            {
                Console.Write(board[i, j] + "|");
            }
            Console.WriteLine();
        }
        Console.WriteLine("+-------------+");
    }
    public char GetBoardCell(int col, int row)
    {
        return board[col, row];
    }

    public void SetBoardCell(int col, int row, char symbol)
    {
        board[col, row] = symbol;
    }
}

abstract class Player
{
    public int Number { get; private set; }
    public char Symbol { get; private set; }

    public Player(int number, char symbol)
    {
        Number = number;
        Symbol = symbol;
    }

    public abstract int GetColumnFromUser(connectBoard board);
}

 class HumanPlayer : Player
{
    public HumanPlayer(int number, char symbol) : base(number, symbol)
    {
    }

    public override int GetColumnFromUser(connectBoard board)
    {
        while (true)
        {
            Console.Write($"Player {Number}, choose a column (1-7): ");
            if (int.TryParse(Console.ReadLine(), out int col) && col >= 1 && col <= 7 && board.IsColumnAvailable(col - 1))
            {
                return col - 1; 
            }
            Console.WriteLine("Invalid column. Please try again.");
        }
    }
}

class Game
{
    private connectBoard board;
    private Player[] players;
    private int currentPlayerIndex;
    private bool gameOver;

    public Game()
    {
        board = new connectBoard();
        players = new Player[] { new HumanPlayer(1, 'X'), new HumanPlayer(2, 'O') };
        currentPlayerIndex = 0;
        gameOver = false;
    }

    public void Play()
    {
        while (!gameOver)
        {
            board.DisplayBoard();

            int col = players[currentPlayerIndex].GetColumnFromUser(board);

            animatePiece(col);
            DropPiece(col);

            if (CheckForWin())
            {
                board.DisplayBoard();
                Console.WriteLine($"Player {players[currentPlayerIndex].Number} wins!");
                gameOver = true;
            }
            else if (IsBoardFull())
            {
                board.DisplayBoard();
                Console.WriteLine("The game is a tie.");
                gameOver = true;
            }
            else
            {
                SwitchTurn();
            }
        }
    }
    
    private void animatePiece(int col)
    {
        int x = 0;
        for (int j = 5; j >= 0; j--)
        {
            if (board.GetBoardCell(col, j) == ' ')
            {
                x = j;
                break;
            }
        }

        for (int j = 0; j < x; j++)
        {
            if (board.GetBoardCell(col, j) == ' ')
            {
                board.SetBoardCell(col, j, players[currentPlayerIndex].Symbol);
                board.DisplayBoard();
                Thread.Sleep(200);
                board.SetBoardCell(col, j, ' ');
            }
        }
    }
    private void DropPiece(int col)
    {
        board.DropPiece(col, players[currentPlayerIndex].Symbol);
    }

    private bool CheckForWin()
    {
        return board.CheckForWin();
    }

    private bool IsBoardFull()
    {
        return board.IsBoardFull();
    }

    private bool IsColumnAvailable(int col)
    {
        return board.IsColumnAvailable(col);
    }
    private void SwitchTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
    }
}


class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Play();
    }
}

