using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_effect : MonoBehaviour
{
   
   
    public cell[,] mazecells;
    private int cols, rows;
    
    public void apply_obs()
    {

        cols = mazecells.GetLength(1);
        rows = mazecells.GetLength(0);
        int df_lvl =(int)((Game_Manager.instance.game_difficulty* Level_mng.instance.cur_lvl)/2)/2;
        //Debug.Log(df_lvl+" lvl "+ Level_mng.instance.cur_lvl);
        for (int i = 0; i < df_lvl; i++)
        {
            moving_obstacle();
            resize_floor();
            remove_floors();
            move_floor();
        }
        
        for(int i=0;i<cols;i++)
        {
            coinsSpan();
        }
    }
    private void coinsSpan()
    {
        Instantiate(Game_Manager.instance.coinPrefeb, random_floor(false).transform.position + Vector3.up, Quaternion.identity);
    }
    GameObject random_floor(bool obs,bool checkspace=false)
    {
    redo:
        var r = Random.Range(0, rows);
        var c = Random.Range(0, cols);
        var tmp = mazecells[r,c ].floor;

        if (tmp.tag == "Finish" || tmp.tag == "spawn")
            if (obs && tmp.tag == "obs") goto redo;
            else goto redo;
        else if (!isrightSpace(r, c)&&checkspace)
            goto redo;
        
       if(obs) tmp.tag = "obs";
        return tmp;
    }
    private bool isrightSpace(int r,int c)
    {
      
            if ((c + 1) < cols && mazecells[r, c + 1].floor.tag=="obs")
            {
                return false;
            }else if ((c-1) >= 0 && mazecells[r, c - 1].floor.tag == "obs")
            {
                return false;
            }else if ((r+1)<rows&&mazecells[r+1, c].floor.tag == "obs")
            {
                return false;
            }else if ((r - 1) >= 0&&mazecells[r-1, c ].floor.tag == "obs")
            {
                return false;
            }
            else
            {
                return true;

            }
    }
    private void resize_floor()
    {
        random_floor(true,true).transform.localScale = new Vector3(1,0.2f,1);
    }
    private void remove_floors()
    {
        Destroy(random_floor(true,true));
    }
    private void move_floor()
    {
        random_floor(true).transform.position -= Vector3.up*0.5f;

    }
    private void moving_obstacle()
    {
        GameObject gb= Instantiate(Game_Manager.instance.enemy,
                                    random_floor(true).transform.position + Vector3.up*0.5f,
                                     Quaternion.identity);
    }

}
