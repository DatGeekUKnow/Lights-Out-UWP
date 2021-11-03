// LIGHTS OUT GAME.CS WPF
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOutUWP
{
    public class LightsOutGame
    {
        private int gridSize = 3; // Number of cells in grid
        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers
        public const int MaxGridSize = 7;
        public const int MinGridSize = 3;
        // Returns the number of horizontal/vertical cells in the grid
        public int GridSize
        {
            get
            {
                return gridSize;
            }
            set
            {
                if (value >= MinGridSize && value <= MaxGridSize)
                {
                    gridSize = value;
                    grid = new bool[gridSize, gridSize];
                    NewGame();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("NumCells must be between " +
                    MinGridSize + " and " + MaxGridSize + ".");
                }
            }
        }
        public LightsOutGame()
        {
            rand = new Random(); // Initialize random number generator
            GridSize = MinGridSize;
        }
        // Randomizes the grid
        public void NewGame()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
        }
        // Returns the grid value at the given row and column
        public bool IsOn(int row, int col)
        {
            return grid[row, col];
        }
        // Inverts the selected light and all orthogonally adjacent lights
        public void FlipLight(int row, int col)
        {
            for (int r = row - 1; r <= row + 1; r++)
            {
                if (r >= 0 && r < gridSize)
                {
                    grid[r, col] = !grid[r, col];
                }
            }
            for (int c = col - 1; c <= col + 1; c++)
            {
                if (c >= 0 && c < gridSize && c != col)
                {
                    grid[row, c] = !grid[row, c];
                }
            }
        }
        // Returns true if all lights are off
        public bool IsGameOver()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    if (grid[r, c])
                    {
                    return false;
                    }
                }
            }
            return true;
        }
        // Turns off all lights except 3 in left-most corner.
        public void Cheat()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    grid[r, c] = false;
                }
            }
            FlipLight(0, 0);
        }
    }
}