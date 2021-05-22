﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    void Awake()
    {
        if (!instance)
        {
            Debug.Log("makeUIinstance");
            instance = this;
        }
    }

}
