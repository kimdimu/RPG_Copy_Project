using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Stats stats;
    Vector3 moveAmount;
    public float speed;
    void Start()
    {
        stats = new Stats();
        stats.HP = 5;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            moveAmount.x = speed*Time.deltaTime;
            transform.position += moveAmount;
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveAmount.x = -speed * Time.deltaTime;
            transform.position += moveAmount;
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveAmount.z = speed * Time.deltaTime;
            transform.position += moveAmount;
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveAmount.z = -speed * Time.deltaTime;
            transform.position += moveAmount;
            return;
        }
        moveAmount.z = 0;
        transform.position += moveAmount;

        if (Input.GetKey(KeyCode.C))
        {

        }
    }


}
