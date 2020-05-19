using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game_Manager : MonoBehaviour
{
    public enum Game_State
    {
        play_screen,
        world_selection,
        loading,
        playing,
        puase,
        win,
        loss
    }

    public Game_State game_state=Game_State.play_screen;

    public GameObject player_prefeb;
    public GameObject player;
    public static Game_Manager instance;
    private Vector3 span_point;
    private Vector3 finish_point;
    public GameObject enemy;
    public int selected_world;
    public int points;
    public GameObject coinPrefeb;
    public GameObject msg_Panel;
    public Text msg;
    public Button yes;
    public Button no;
    public int soundOPS;
    public int game_difficulty;
    public int game_Controller;//1 for joytick 2 for buttons
    public  Audio_Manger ad_manager;

    public void set_span_point(Vector3 span)
    {
        span_point = span;
    }
    public void set_fnish_point(Vector3 f_point)
    {
        finish_point = f_point;
    }
    public Vector3 get_span_point()
    {
        return span_point;
    }
    public Vector3 get_finish_point()
    {
        return finish_point;
    }
    private void Awake()
    {
        load_game_data();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
       // StartCoroutine(WaitForRequest());
    }
   
    public void load_game_data()
    {
       //PlayerPrefs.SetInt("Points", 2000);
        points = PlayerPrefs.GetInt("Points",0);
        game_difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        game_Controller = PlayerPrefs.GetInt("Control", 1);//joystick
        soundOPS=PlayerPrefs.GetInt("Sound", 0);
        ad_manager.isMute = soundOPS;
        Debug.Log("Data saved");
       
    }
    public void Save_game()
    {
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.SetInt("World_"+ Level_mng.instance.cur_wrld,1);
        PlayerPrefs.SetInt(Level_mng.instance.cur_wrld + "_Level", Level_mng.instance.cur_lvl);
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch(Game_Manager.instance.game_state)
            {
                case Game_Manager.Game_State.play_screen:
                   if (Start_Screen_UI.instance.settings_Panel.activeSelf|| Start_Screen_UI.instance.store_Panel.activeSelf) Start_Screen_UI.instance.StartScreen_PanelManage(1);
                    else Application.Quit();
                    break;
                case Game_Manager.Game_State.world_selection:
                    Start_Screen_UI.instance.StartScreen_PanelManage(1);
                    break;
                case Game_Manager.Game_State.loading:
                    //do nothing
                    break;
                case Game_Manager.Game_State.playing:
                    UI_Controller.instance.UI_panel_mange(3);

                    Game_Manager.instance.game_state = Game_Manager.Game_State.puase;

                    break;
                case Game_Manager.Game_State.puase:
                    UI_Controller.instance.UI_panel_mange(1);
                    break;
                case Game_Manager.Game_State.win:
                    if (UI_Controller.instance.quiz_Panel.activeSelf) UI_Controller.instance.UI_panel_mange(2);
                    else {
                        SceneManager.LoadSceneAsync(0);
                        Start_Screen_UI.instance.StartScreen_PanelManage(2);
                    }
                    break;
                case Game_Manager.Game_State.loss:
                    SceneManager.LoadSceneAsync(0);
                    Start_Screen_UI.instance.StartScreen_PanelManage(2);
                    break;

            }
        }
    }

    IEnumerator WaitForRequest()
    {
        string url = "ftp://rp80118%2540gmail.com@ftp.drivehq.com/Maze/maze_test.text";

        WWW testwww = new WWW(url);
        yield return testwww;

        // check for errors
        if (testwww.error == null)
        {
            Debug.Log("WWW Ok!: " + testwww.text);
        }
        else
        {
            Debug.Log("WWW Error: " + testwww.error);
        }
    }

}
