using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public delegate void InputEvent();
    public static event InputEvent KeyPressDown;
    public static event InputEvent OnPressUp;
    //public static event InputEvent OnTap;

    void Start()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            OnPressUp?.Invoke();
        }
        if (Input.anyKeyDown)
        {
            KeyPressDown?.Invoke();
           // print("Input Key: " + Input.inputString);
        }
    }
}
