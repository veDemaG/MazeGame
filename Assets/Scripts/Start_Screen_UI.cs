using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Start_Screen_UI : MonoBehaviour
{
    public Button backbtn;
    [SerializeField]
    public GameObject start_Panel;
    public GameObject worldSelection_Panel;
    public GameObject settings_Panel;
    public GameObject loading_Panel;
    public GameObject store_Panel;
    private int maxWorld = 4;
    public Button[] unlockBtns;
    public Button[] worlds;
    public static Start_Screen_UI instance;
    private int points;
    public Text points_txt;
    private Text msg;
    private GameObject msg_panel;
    private Button yes;
    private Button no;

    public Button on_btn;
    public Button off_btn;

    public Button jystick_btn;
    public Button btns;
    public Slider dif_slider;

    private void Awake()
    {
        instance = this;
        StartScreen_PanelManage(1);
        setUnlocksWorlds();
        
    }
    private void OnEnable()
    {
        Game_Manager.instance.ad_manager.back_music(false);

        msg = Game_Manager.instance.msg;
        msg_panel = Game_Manager.instance.msg_Panel;
        yes = Game_Manager.instance.yes;
        no = Game_Manager.instance.no;
        set_btText();
        msg_panel.SetActive(false);

        dif_slider.value = Game_Manager.instance.game_difficulty;
        controlls(Game_Manager.instance.game_Controller.Equals(1));
        sound(Game_Manager.instance.soundOPS.Equals(0));
        points = Game_Manager.instance.points;
        points_txt.text = "Coins: " + points.ToString();
        Debug.Log("Data Displayed");
    }
    public void quitGame()
    {
        //Game_Manager.instance.Save_game();
        Application.Quit();
    }
    private void set_btText()
    {
        unlockBtns[1].GetComponentInChildren<Text>().text = "Unlock For 100";
        unlockBtns[2].GetComponentInChildren<Text>().text = "Unlock For 250";
        unlockBtns[3].GetComponentInChildren<Text>().text = "Unlock For 500";
    }
    public void sound(bool on)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        if (on)
        {
            on_btn.interactable = false;
            off_btn.interactable = true;
            PlayerPrefs.SetInt("Sound", 0);
            Game_Manager.instance.ad_manager.mute_unmute(0);
        }else
        {
            on_btn.interactable = true;
            off_btn.interactable = false;
            PlayerPrefs.SetInt("Sound", 1);
            Game_Manager.instance.ad_manager.mute_unmute(1);

        }
        Game_Manager.instance.soundOPS = PlayerPrefs.GetInt("Sound");
    }
    public void controlls(bool joystick)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        if (joystick)
        {
            Game_Manager.instance.game_Controller =1;
            jystick_btn.interactable = false;
            btns.interactable = true;
            PlayerPrefs.SetInt("Control", 1);
        }
        else
        {
            Game_Manager.instance.game_Controller = 2;
            jystick_btn.interactable = true;
            btns.interactable = false;
            PlayerPrefs.SetInt("Control", 2);
        }
    }

    public void set_Game_difficulty(Slider sd)
    {

        Game_Manager.instance.game_difficulty = (int)sd.value;
        PlayerPrefs.SetInt("Difficulty", (int)sd.value);
    }

    public void StartScreen_PanelManage(int active)
    {
        if(instance==null)
        {
            Debug.Log("return");
            return;
        }
        start_Panel.SetActive(false);
        worldSelection_Panel.SetActive(false);
        settings_Panel.SetActive(false);
        loading_Panel.SetActive(false);
        store_Panel.SetActive(false);
       points_txt.text = "Coins: " +  points.ToString();
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        switch (active)
        {

            case 1:
                backbtn.gameObject.SetActive(false);
                start_Panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.play_screen;
                break;
            case 2:
                backbtn.gameObject.SetActive(true);
                worldSelection_Panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.world_selection;

                break;
            case 3:
                backbtn.gameObject.SetActive(false);
                loading_Panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.loading;

                break;
            case 4:
                settings_Panel.SetActive(true);

                break;
            case 5:
                store_Panel.SetActive(true);
                break;
            default:
                break;
        }

    }

    private void setUnlocksWorlds()
    {
        PlayerPrefs.SetInt("World_1", 1);
        for(int i=0;i<maxWorld;i++)
        {
            if (PlayerPrefs.GetInt("World_"+(i+1)) == 1)
            {
                //world 1 unlock
                worlds[i].interactable = true;
             if(unlockBtns[i]!=null)   unlockBtns[i].gameObject.SetActive(false);
            }else
            {
                worlds[i].interactable = false;
               //cange text of button
            }
        }
        
    }

    // set yes no Button event For Button Buying
    public bool msgBTN_Events(int action,int world,int p)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        if (action==1)
        {
            //yes
            PlayerPrefs.SetInt("World_" + world, 1);
            points = points - p;
            Game_Manager.instance.points = points;
            PlayerPrefs.SetInt("Points", points);
            if(points_txt!=null)points_txt.text = "Coins: " + points;
            msg.text = "Congratulation !! \n You Have Unloack World " + world;
            okPanel(false);
            setUnlocksWorlds();

            return true;
        }
        else if(action==3)
        {
            msg_panel.SetActive(false);
        }
        return false;
    }

    private void okPanel(bool error)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        if (error)
        {
            Debug.Log("not Enough Points !! " + points);
            msg.text = "Not Enough Coins !! \n\n <size=40><color=#ff0000ff>" + points+"</color></size>";
        }
        no.gameObject.GetComponentInChildren<Text>().text = "Ok";
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(true);
        no.onClick.AddListener(() => { var ac= msgBTN_Events(3,0,0); });
        msg_panel.SetActive(true);
    }

    private void unlock(int world,int p)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        bool action =false;
        msg.text = "Sure to Buy The World " + world + " For " + p + " Points ?";
        no.GetComponentInChildren<Text>().text = "NO";
        yes.GetComponentInChildren<Text>().text = "YES";
        no.gameObject.SetActive(true);
        yes.gameObject.SetActive(true);
        no.onClick.AddListener(() => { action = msgBTN_Events(3,world,p); });//no
        yes.onClick.AddListener(() => { action = msgBTN_Events(1,world,p); });//yes
        msg_panel.SetActive(true);
    }

    public void buyWorld(int world)
    {
       
        switch(world)
        {
            case 2:
                if (points >= 100)
                    unlock(2, 100);
                else
                    okPanel(true);

                break;
            case 3:
                if (points >= 250)
                    unlock(3, 250);
                else
                    okPanel(true);
                break;
            case 4:
                if (points >= 500)
                    unlock(4, 500);
                else
                    okPanel(true);

                break;
        }
    }

    public void load_worlds()
    {
        StartScreen_PanelManage(2);
    }
    
    public void selectWorld(int world)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        StartScreen_PanelManage(3);
        Game_Manager.instance.selected_world = world;
        Play_Game();
    }

    private void Play_Game()
    {
        Game_Manager.instance.game_state = Game_Manager.Game_State.playing;
        SceneManager.LoadSceneAsync(1);

    }
}
