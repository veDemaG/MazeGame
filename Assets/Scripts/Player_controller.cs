using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{

    public float speed = 3.0f;
    public float jumpSpeed = 5.0f;
    public int mobile_move;
    private Rigidbody rd;
    private Vector3 moveDirection = Vector3.zero;
    private bool allowMove = true;
   // Asset From Fenerax Studios
   // https://assetstore.unity.com/publishers/32730
    public VariableJoystick variableJoystick;

    float horizontal;
    float verticale;
    bool onground = true;
    private int lvv_coins=0;
    void Start()
    {
        rd = this.gameObject.GetComponent<Rigidbody>();
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onground)
        {
            jump();
        }
    }
    void FixedUpdate()
    {
        if (Game_Manager.instance.game_state == Game_Manager.Game_State.playing && allowMove)
        {
            horizontal = Input.GetAxis("Horizontal");
            verticale = Input.GetAxis("Vertical");
            
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                transform.position += new Vector3(horizontal, 0, verticale) * (Time.deltaTime * speed);
            }
#if UNITY_ANDROID
            else if(mobile_move!=0)
            {
                Debug.Log("Move");
                plyer_move();
            }
            else if (variableJoystick.Vertical != 0 || variableJoystick.Horizontal != 0)
            {
                transform.position += new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical) * (Time.deltaTime * speed);
            }
#endif           
        }
    }

   
    private void move_camera()
    {

        if(Input.GetKeyDown(KeyCode.LeftArrow) && transform.rotation.y > -90f)
        {
           transform.Rotate(new Vector3(0.0f, transform.rotation.y - 90f, 0.0f));
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) && transform.rotation.y < 90f)
        {
            transform.Rotate(new Vector3(0.0f, transform.rotation.y + 90f, 0.0f));
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(-45f, 0.0f, 0.0f));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(45f, 0.0f, 0.0f));

        }
       
    }
    public void jump()
    {
        if (onground)
        {
            onground = false;

            transform.GetComponent<Rigidbody>().velocity = Vector3.up * jumpSpeed;
            Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.jump_clip);
        }
    }
    public void set_mobile_move(int move)
    {
        mobile_move = move;
    }
    
    private void plyer_move()
    {
        switch (mobile_move)
        {
            //forward
            case 1:
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            //backword
            case 2:
                transform.position -= transform.forward * speed * Time.deltaTime;
                break;
            //left
            case 3:
                transform.position += Vector3.left * speed * Time.deltaTime;
                break;
            //right
            case 4:
                transform.position += Vector3.right * speed * Time.deltaTime;
                break;
            default:
                break;
        }
    }

    public void OnCollisionEnter (Collision other)
    {
        if (Game_Manager.instance.game_state == Game_Manager.Game_State.playing)
        {

            // for moving enemy ignoreations
            if (other.gameObject.layer == 8)
        {
                Physics.IgnoreCollision(other.collider,this.gameObject.GetComponent<Collider>());
        }
        //win
        if (other.gameObject.tag == "Finish")
        {
               UI_Controller.instance.doublePointBTN.interactable = true;
                UI_Controller.instance.rewardBTN.interactable = true;
                UI_Controller.instance.lvl_coin.text = "" + lvv_coins;
                UI_Controller.instance.UI_panel_mange(2);
                Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.win_clip);

                Level_mng.instance.cur_lvl++;
            Game_Manager.instance.Save_game();
        }
        else if (other.gameObject.tag == "Dead End")
        {
            UI_Controller.instance.UI_panel_mange(4);
                Game_Manager.instance.Save_game();
                Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.gameover_clip);

            }
            if (other.gameObject.name.Contains("Floor"))
            {
                onground = true;
                if (!allowMove) allowMove = true;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (Game_Manager.instance.game_state == Game_Manager.Game_State.playing)
        {

            if (other.gameObject.tag == "Enemy")
            {
                allowMove = false;
                Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.enemy_clip);
                gameObject.transform.position = Game_Manager.instance.get_span_point() + Vector3.up * 3;
            }
            else if (other.gameObject.tag == "coin")
            {
                other.GetComponent<Collider>().enabled = false;
                iTween.MoveTo(other.gameObject, iTween.Hash("y", 3, "time", 0.2f));
                Destroy(other.gameObject, 0.21f);
                lvv_coins++;
                Game_Manager.instance.ad_manager.playSound(Game_Manager.instance.ad_manager.coin_clip);

                Game_Manager.instance.points++;
                UI_Controller.instance.points_txt.text ="Points: "+ Game_Manager.instance.points.ToString();
            }
        }               
    }
}
