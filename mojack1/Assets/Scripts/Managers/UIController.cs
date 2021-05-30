using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Transform canvas;

    //Quest Info
    public Transform questInfo;
    public Transform questInfoContent;
    public Button questInfoAcceptButton;
    public Button questInfoCancelButton;
    public Transform questGridContent;


    void Awake()
    {
        if (!instance) instance = this;

        canvas = GameObject.Find("Canvas").transform;
        questInfo = canvas.Find("QuestInfo2");
        questInfoContent = questInfo.Find("Background/Info/Viewport/Content");
        questInfoAcceptButton = questInfo.Find("Background/Buttons/Accept").GetComponent<Button>();
        questInfoCancelButton = questInfo.Find("Background/Buttons/Cancel").GetComponent<Button>();
        questInfoCancelButton.onClick.AddListener(() => questInfo.gameObject.SetActive(false));

        questGridContent = canvas.Find("QuestGrid/Background/Info/Viewport/Content");
    }

}
