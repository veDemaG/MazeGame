using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Follow : MonoBehaviour
{
    private GameObject objectToFollow;

    public float speed = 2.0f;
   public Vector3 ini_pos;
  

    private void Start()
    {

        // ini_pos = transform.position;
        objectToFollow = Game_Manager.instance.player;
    }
    void FixedUpdate()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y+ini_pos.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x+ini_pos.x, interpolation);
        position.z = Mathf.Lerp(this.transform.position.z, objectToFollow.transform.position.z+ini_pos.z, interpolation);

        this.transform.position = position;
    }
}
