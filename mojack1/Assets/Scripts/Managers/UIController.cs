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
    public Button questInfoCompleteButton;

    public Transform questBook;
    public Transform questBookContent;
    public Button questBookCancelButton;

    void Awake()
    {
        if (!instance) instance = this;
        canvas = GameObject.Find("Canvas").transform;

        questInfo = canvas.Find("QuestInfo2");
        questInfoContent = questInfo.Find("Background/Info/Viewport/Content");
        questInfoAcceptButton = questInfo.Find("Background/Buttons/Accept").GetComponent<Button>();
        questInfoCompleteButton = questInfo.Find("Background/Buttons/Complete").GetComponent<Button>();
        questInfoCancelButton = questInfo.Find("Background/Buttons/Cancel").GetComponent<Button>();
        questInfoCancelButton.onClick.AddListener(() => questInfo.gameObject.SetActive(false));

        questBook = canvas.Find("QuestGrid");
        questBookContent = questBook.Find("Background/Info/Viewport/Content");
        questBookCancelButton = questBook.Find("Background/Buttons/Cancel").GetComponent<Button>();
    }

}
