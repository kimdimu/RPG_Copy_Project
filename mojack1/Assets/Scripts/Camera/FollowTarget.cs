using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float followTime;
    public Vector3 offset;
    void Start()
    {
        
    }

    void Update()
    {
        iTween.MoveUpdate(this.gameObject, iTween.Hash("position", target.position+offset, "time", followTime, "easetype", iTween.EaseType.easeInOutSine));
    }
}
