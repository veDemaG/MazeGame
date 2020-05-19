using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector3 startpos;
    public float speed = 1.2f;
    RaycastHit hit;
    int raycast_length = 1;
    int hitcount = 0;
   
    private void FixedUpdate()
    {
        if (Game_Manager.instance.game_state == Game_Manager.Game_State.playing)
        {
           if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycast_length))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);

                if (hitcount >= 3)
                {
                    transform.Rotate(new Vector3(0, transform.rotation.y + 180, 0));
                    hitcount = 0;
                }
                else
                {
                    transform.Rotate(new Vector3(0, 90, 0));
                    hitcount++;
                }

            }
           
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("Floor"))
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
      
    }

   
}