using UnityEditor;
using UnityEngine;

public class cell
{
    public GameObject floor;
    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
    public bool isVisited = false;
}

public class MazeAlgoritham : MonoBehaviour
{

    public GameObject wall;
    public GameObject floor;
    public GameObject[] floors;
    public GameObject[] walls;
    public float wall_length = 2f;
    public int maze_row = 5;
    public int maze_clmn = 5;
   public Cell_genrating g_cell;
    public static MazeAlgoritham instance;
    int df_lvl;


    public void set_current_Walls(int w)
    {
        wall = walls[w].gameObject;
        floor = floors[w].gameObject;
    }
    private void set_difficulty(int max_size)
    {
        int lvl = Level_mng.instance.cur_lvl;
        int r_lvl = lvl;
        int c_lvl = lvl;
        r_lvl = lvl % (5 - df_lvl);
        c_lvl = lvl % (6 - df_lvl);

        if(maze_clmn<max_size&&maze_row<max_size)
        {
            if(r_lvl==0)
            {
                maze_row++;
            }
            if(c_lvl==0)
            {
                maze_clmn++;
            }
            
        }else
        {
            Debug.Log("Max Maze");
        }
    }
    private void maze_dificulty()
    {
        
        switch(Level_mng.instance.cur_wrld)
        {
            //Desert
            case 0:
                set_difficulty(15);
                break;
            //forest
            case 1:
                set_difficulty(20);
                break;
            //sea
            case 2:
                set_difficulty(25);
                break;
            //lawa
            case 3:
                set_difficulty(20);
                break;
        }
    }
    public void Load_Maze()
    {
        set_current_Walls(Game_Manager.instance.selected_world);
        g_cell = new Cell_genrating();
        g_cell.set_data(maze_clmn, maze_row, wall, floor, wall_length);
        g_cell.genrate_Cell();
    }
    private void get_maze_size()
    {
        maze_clmn = PlayerPrefs.GetInt(Level_mng.instance.cur_wrld+"_clmn", 5);
        maze_row = PlayerPrefs.GetInt(Level_mng.instance.cur_wrld + "_row", 5);

    }
    private void save_maze_size()
    {
        PlayerPrefs.SetInt(Level_mng.instance.cur_wrld + "_clmn", maze_clmn);
        PlayerPrefs.SetInt(Level_mng.instance.cur_wrld + "_row", maze_row);
    }
    private void OnDisable()
    {
     if(Level_mng.instance!=null)save_maze_size();
    }
    void OnEnable ()
    {
        df_lvl = Game_Manager.instance.game_difficulty;
        get_maze_size();
        instance = this;
        maze_dificulty();
        Load_Maze();
	}
	
}
