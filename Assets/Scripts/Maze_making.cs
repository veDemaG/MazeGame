
using UnityEngine;

public class Maze_making : MonoBehaviour
{
    private int currentRow = 0;
    private int currentColumn = 0;
    private bool allgood=false;
    private cell[,] maze_cells;
    private int rows, columns;
    public Maze_making(cell[,] cells)
    {
        maze_cells = cells;
        rows = cells.GetLength(0);
        columns = cells.GetLength(1);
    }
    // Start is called before the first frame update
    public void Making_maze()
    {
        maze_cells[currentRow, currentColumn].isVisited = true;
        while (!allgood)
        {
            find_deadend();
            processed_unVisited();
        }
      
        //Game_Manager.instance.payer.transform.position = ;
        Game_Manager.instance.player = Instantiate(Game_Manager.instance.player_prefeb, Game_Manager.instance.get_span_point() + Vector3.up,Quaternion.identity);
        Game_Manager.instance.player.SetActive(true);
        UI_Controller.instance.SetUPUI();
        //obstacle data set
        Obstacle_effect obs = new Obstacle_effect();
        obs.mazecells = maze_cells;
        obs.apply_obs();
    }

    #region DEAD END FINIDING
    private void find_deadend()
    {
          while (RouteStillAvailable(currentRow, currentColumn))
            {
                int direction = Random.Range(1, 5);//THE BEST RANDOM

                if (direction == 1 && CellIsAvailable(currentRow - 1, currentColumn))
                {
                    // North
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn].top);
                    DestroyWallIfItExists(maze_cells[currentRow - 1, currentColumn].bottom);
                    currentRow--;
                }
                else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
                {
                    // South
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn].bottom);
                    DestroyWallIfItExists(maze_cells[currentRow + 1, currentColumn].top);
                    currentRow++;
                }
                else if (direction == 3 && CellIsAvailable(currentRow, currentColumn + 1))
                {
                    // east
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn].right);
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn + 1].left);
                    currentColumn++;
                }
                else if (direction == 4 && CellIsAvailable(currentRow, currentColumn - 1))
                {
                    // west
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn].left);
                    DestroyWallIfItExists(maze_cells[currentRow, currentColumn - 1].right);
                    currentColumn--;
                }

            maze_cells[currentRow, currentColumn].isVisited = true;
            }
    }

    private void DestroyWallIfItExists(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
    }

    private bool RouteStillAvailable(int row, int column)
    {
        int availableRoutes = 0;

        if (row > 0 && !maze_cells[row - 1, column].isVisited)
        {
            availableRoutes++;
        }

        if (row < rows - 1 && !maze_cells[row + 1, column].isVisited)
        {
            availableRoutes++;
        }

        if (column > 0 && !maze_cells[row, column - 1].isVisited)
        {
            availableRoutes++;
        }

        if (column <  columns - 1 && !maze_cells[row, column + 1].isVisited)
        {
            availableRoutes++;
        }

        return availableRoutes > 0;
    }

    private bool CellIsAvailable(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns && !maze_cells[row, column].isVisited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    private void processed_unVisited()
    {
        allgood = true;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (!maze_cells[r, c].isVisited && CellHasAnAdjacentVisitedCell(r, c))
                {
                    allgood = false; // still unvisited cell.
                    currentRow = r;
                    currentColumn = c;
                    DestroyAdjacentWall(currentRow, currentColumn);
                    maze_cells[currentRow, currentColumn].isVisited = true;
                    return; // Exit the function
                }
            }
        }
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while (!wallDestroyed)
        {
             int direction = Random.Range (1, 5);

            if (direction == 1 && row > 0 && maze_cells[row - 1, column].isVisited)
            {
                DestroyWallIfItExists(maze_cells[row, column].top);
                DestroyWallIfItExists(maze_cells[row - 1, column].bottom);
                wallDestroyed = true;
            }
            else if (direction == 2 && row < (rows - 2) && maze_cells[row + 1, column].isVisited)
            {
                DestroyWallIfItExists(maze_cells[row, column].bottom);
                DestroyWallIfItExists(maze_cells[row + 1, column].top);
                wallDestroyed = true;
            }
            else if (direction == 3 && column > 0 && maze_cells[row, column - 1].isVisited)
            {
                DestroyWallIfItExists(maze_cells[row, column].left);
                DestroyWallIfItExists(maze_cells[row, column - 1].right);
                wallDestroyed = true;
            }
            else if (direction == 4 && column < (columns - 2) && maze_cells[row, column + 1].isVisited)
            {
                DestroyWallIfItExists(maze_cells[row, column].right);
                DestroyWallIfItExists(maze_cells[row, column + 1].left);
                wallDestroyed = true;
            }
        }

    }

    private bool CellHasAnAdjacentVisitedCell(int row, int column)
    {
        int visitedCells = 0;

        // Look 1 row up (north) if we're on row 1 or greater
        if (row > 0 && maze_cells[row - 1, column].isVisited)
        {
            visitedCells++;
        }

        // Look one row down (south) if we're the second-to-last row (or less)
        if (row < (rows - 2) && maze_cells[row + 1, column].isVisited)
        {
            visitedCells++;
        }

        // Look one row left (west) if we're column 1 or greater
        if (column > 0 && maze_cells[row, column - 1].isVisited)
        {
            visitedCells++;
        }

        // Look one row right (east) if we're the second-to-last column (or less)
        if (column < (columns - 2) && maze_cells[row, column + 1].isVisited)
        {
            visitedCells++;
        }

        // return true if there are any adjacent visited cells to this one
        return visitedCells > 0;
    }

}
