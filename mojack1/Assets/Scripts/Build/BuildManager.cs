using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    static public BuildManager instance;

    bool isBuild;
    public GameObject buildObj;
    private void Awake()
    {
        SetCallbacks();
        if (!instance) instance = this;
        buildObj = GameObject.Find("Build");
        buildObj.SetActive(false);
        isBuild = false;
    }

    public void OnOffBuildMode()
    {
        if(isBuild)
        {
            isBuild = false;
            PlayerController.main.enabled = true;
             UIController.instance.buildButtons.SetActive(false);
            buildObj.SetActive(false);
        }
        else
        {
            isBuild = true;
            PlayerController.main.enabled = false;
            UIController.instance.buildButtons.SetActive(true);
            buildObj.SetActive(true);
        }

    }
    public bool ReturnIsBuild()
    {
        return isBuild;
    }
    void SetCallbacks()
    {
        InputManager.KeyPressDown += KeyCallbacks;
    }

    void KeyCallbacks()
    {
        if (Input.inputString == "r")
        {
            OnOffBuildMode();
        }
    }
}
