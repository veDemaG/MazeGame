using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [Header("Mobile Controlls")]
    public GameObject btnControlls;
    public GameObject joystickCNT;
    public Text points_txt;
    public RawImage hintMap;
    public GameObject Puase_panel;
    public GameObject Mobile_UI_panel;
    public GameObject Info_panel;
    public GameObject win_panel;
    public GameObject loss_pael;
    public GameObject quiz_Panel;
    public Button quizBTN;
    public Button doublePointBTN;
    public Button rewardBTN;
    public Button hintBTN;
    public Text lvl_coin;
    public Button[] optionsBtn;
    int correct_op;
    public Text ques;
    public Text quiz_msg;

    public Player_controller plc;

    public static UI_Controller instance;

    private int ran_que;

    private void Awake()
    {
        instance = this;
        hintMap.gameObject.SetActive(false);
        lvl_coin.text = "";

    }

    public void SetUPUI()
    {
        UI_panel_mange(1);
        
        points_txt.text = "Coins: " + Game_Manager.instance.points.ToString();
        #if UNITY_ANDROID
        Debug.Log("Mobile Platform");
        if (Game_Manager.instance.game_Controller == 1)
        { joystickCNT.SetActive(true);
            btnControlls.SetActive(false);
        }
        else if(Game_Manager.instance.game_Controller == 2)
        {
            joystickCNT.SetActive(false);
            btnControlls.SetActive(true);
        }
        UI_panel_mange(5);
#endif
        plc = Game_Manager.instance.player.GetComponent<Player_controller>();
        plc.variableJoystick = joystickCNT.GetComponent<VariableJoystick>();

    }

    public void restart()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }

    private void set_quiz()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        quiz_msg.text = "Select Right Option And Double Your Point*";
    REDO:
        var tmp = Random.Range(0, Quetionaire.instance.rs.results.Count);
        if (tmp != ran_que) ran_que = tmp;
        else goto REDO;
        optionSetUP(true);
        ques.text = WebUtility.HtmlDecode(Quetionaire.instance.rs.results[ran_que].question);
        correct_op = Random.Range(0,4);
        optionsBtn[correct_op].GetComponentInChildren<Text>().text = WebUtility.HtmlDecode(Quetionaire.instance.rs.results[ran_que].correct_answer);
        int op = 0;
            for (int i = 0; i < optionsBtn.Length; i++)
            {
            optionsBtn[i].image.color = Color.white;
                if ( i != correct_op)
                {
                    optionsBtn[i].GetComponentInChildren<Text>().text = WebUtility.HtmlDecode(Quetionaire.instance.rs.results[ran_que].incorrect_answers[op]);
                    op++;
                }
            }
        Debug.Log("Crrect op :"+correct_op);
        // set text on quiz btn means SKIP
        quizBTN.GetComponentInChildren<Text>().text = "SKIP QUESTION";
        quizBTN.onClick.RemoveAllListeners();
        quizBTN.onClick.AddListener(() => { skipQueBTN(1); });
    }

    //1 for skip,2 for close  
    public void skipQueBTN(int action)
    {

        if (action == 1)
        {
            set_quiz();
        }
        else if (action == 2)
        {
            Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

            UI_panel_mange(2);
        }
    }

    public void select_option(int i)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        Debug.Log("btnindex : "+i);
        if(i==correct_op)
        {
            Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.doublleCoin_clip);

            quiz_msg.text = "Congratulation Right Option";
            optionsBtn[i].image.color = Color.green;
            quizBTN.GetComponentInChildren<Text>().text = "Close";
            var tmp = int.Parse(lvl_coin.text);
            Game_Manager.instance.points += tmp;
            Game_Manager.instance.Save_game();
            lvl_coin.text = (tmp * 2).ToString();
        }
        else
        {
            Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.wrong_option_clip);

            quiz_msg.text = "Opps Wrong Option !";
            optionsBtn[i].image.color = Color.red;
            optionsBtn[correct_op].image.color = Color.green;
            quizBTN.GetComponentInChildren<Text>().text = "Try NextTime";

        }
        quizBTN.onClick.RemoveAllListeners();
        quizBTN.onClick.AddListener(() => { skipQueBTN(2); });
        optionSetUP(false);
    }
    private void optionSetUP(bool isAvail)
    {
        if(isAvail)
        {
            foreach (var btn in optionsBtn)
                btn.interactable = true;
        }else
        {
            foreach (var btn in optionsBtn)
                btn.interactable = false;

        }
    }
    public void setBtnEvent(int move)
    {
        
        if(plc==null) plc = Game_Manager.instance.player.GetComponent<Player_controller>();

        plc.set_mobile_move(move);
    }

    public void jump_fun()
    {
        if (plc == null) plc = Game_Manager.instance.player.GetComponent<Player_controller>();

        plc.jump();
    }

    public void Nextlevel()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        Game_Manager.instance.game_state = Game_Manager.Game_State.playing;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
    /* "1=info,2=win,3=pause,4=loss,5=mobileUI") 
     */
    public void UI_panel_mange(int active)
    {
        Info_panel.SetActive(false);
        Puase_panel.SetActive(false);
        win_panel.SetActive(false);
        Mobile_UI_panel.SetActive(false);
        loss_pael.SetActive(false);
        quiz_Panel.SetActive(false);

        switch (active)
        {
            case 1:
                Info_panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.playing;
                break;
            case 2:
                win_panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.win;

                break;
            case 3:
                Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

                Puase_panel.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.puase;

                break;
            case 4:
                loss_pael.SetActive(true);
                Game_Manager.instance.game_state = Game_Manager.Game_State.loss;

                break;
            case 5:
                Info_panel.SetActive(true);
                Mobile_UI_panel.SetActive(true);
                break;
            case 6:
                set_quiz();
                doublePointBTN.interactable = false;
                quiz_Panel.SetActive(true);
                break;
            default:
                break;
        }

    }


    public void msg_PanelBTN(bool yes)
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        if (yes)
        {
            if (Game_Manager.instance.points > 5)
            {
                Game_Manager.instance.points -= 5;
                points_txt.text = "Coins: " + Game_Manager.instance.points;
                hintMap.gameObject.SetActive(true);
                Game_Manager.instance.msg_Panel.SetActive(false);
            }
            else
            {
                Game_Manager.instance.msg.text = "You Don't have Enough Coins !";
                Game_Manager.instance.yes.gameObject.SetActive(false);
                Game_Manager.instance.no.GetComponentInChildren<Text>().text = "OK";
                Game_Manager.instance.no.onClick.AddListener(()=> { msg_PanelBTN(false); });
                Game_Manager.instance.msg_Panel.SetActive(true);
            }
        }
        else
        {
            Game_Manager.instance.msg_Panel.SetActive(false);
        }
    }
    public void hint()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        Game_Manager.instance.yes.gameObject.SetActive(true);
        Game_Manager.instance.no.gameObject.SetActive(true);
        Game_Manager.instance.msg.text = "Get Top View Of Maze For 5 Coins";
        Game_Manager.instance.yes.GetComponentInChildren<Text>().text = "BUY";
        Game_Manager.instance.no.GetComponentInChildren<Text>().text = "CLOSE";
        Game_Manager.instance.yes.onClick.AddListener(() => { msg_PanelBTN(true); });
        Game_Manager.instance.no.onClick.AddListener(() => { msg_PanelBTN(false); });

        Game_Manager.instance.msg_Panel.SetActive(true);
    }
    public void resume()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        UI_panel_mange(1);
    }
    public void main_menu()
    {
        Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.clik_sound);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
