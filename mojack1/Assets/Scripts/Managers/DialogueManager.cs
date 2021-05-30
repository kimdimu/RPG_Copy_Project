using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private Transform dialBox;
    private Text dialText;
    void Start()
    {
        if (instance == null) instance = this;


        dialBox = UIController.instance.canvas.Find("DialogueBox");
        dialText = dialBox.Find("Background/Text").GetComponent<Text>();
    }

    public void PrintOnDialogueBox(string text)
    {
        AnimationEvents.OnOffMove();
        dialBox.gameObject.SetActive(true);
        Debug.Log("PrintDB");
        dialText.text = text;
    }
    public void CloseDialBox()
    {
        Debug.Log("cloasdb");
        dialBox.gameObject.SetActive(false);
        AnimationEvents.OnOffMove();
    }

    public void CloseDialBoxCallback()
    {
        Debug.Log("cloasdbCallback");
        CloseDialBox();
        InputManager.OnPressUp -= CloseDialBoxCallback;
    }
}
