using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Transform canvas;
    void Awake()
    {
        if (!instance)
            instance = this;
        canvas = GameObject.Find("Canvas").transform;
    }

}
