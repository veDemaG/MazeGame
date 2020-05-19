using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_mng : MonoBehaviour
{
    [SerializeField]
    public int cur_lvl;
    public int cur_wrld;

    public GameObject[] worlds;
    public GameObject[] floors;
    public GameObject[] walls;
    public static Level_mng instance;
    private void OnEnable()
    {
        instance = this;
        set_level_filds(Game_Manager.instance.selected_world);
        load_world(cur_wrld);

    }
    private void OnDisable()
    {
        if (MazeAlgoritham.instance != null)
        {
            PlayerPrefs.SetInt(cur_wrld + "_clmn", MazeAlgoritham.instance.maze_clmn);
            PlayerPrefs.SetInt(instance.cur_wrld + "_row", MazeAlgoritham.instance.maze_clmn);
        }
        instance = null;
        Destroy(this);
    }
    private void set_level_filds(int selected_world)
    {
            cur_wrld = selected_world;
            cur_lvl = PlayerPrefs.GetInt(cur_wrld + "_Level", 1);
        Debug.Log(cur_wrld+"_lvl "+cur_lvl);
    }

    public void Update_Level()
    {
        cur_lvl++;
    }

    public void Update_World()
    {
        if(cur_wrld<=worlds.Length)
        {
            cur_wrld++;
        }
        else { return; }
    }
    public void load_world(int world)
    {
        if (world > cur_wrld)
            return;
        set_level_filds(world);
        for(int i=0;i<worlds.Length; i++)
        {
            worlds[i].SetActive(false);
        }
        worlds[world].SetActive(true);
        //lightinig.instance.Load();
    }
    
}
