using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    [SerializeField] int[] quests;
    [SerializeField] string[] dialogues;
    public int dialogueIndex = 0;
    private Quest quest;

    private void Start()
    {
        //foreach(int i in quests)
        {
            QuestManager.instance.LoadQuest(0);
        }

        //SetQuestExample();
        //QuestManager.instance.CreateJsonFile(Application.dataPath, "Jsontestfile", JsonUtility.ToJson(quest));
        //ShowQuestInfo(QuestManager.instance.questDictionary[0]);
        //print(JsonUtility.ToJson(quest));
    }
    public void ShowQuestInfo()
    {
        //Debug.Log("NPC ShowQuestInfo");
        foreach (int i in quests)
        {
            //Debug.Log("NPC ShowQuestInfo " + i);
            //Did the player finished this quest?
            if (!PlayerData.finishedQuests.Contains(i) &&
                //Do the player meet the requirements? 퀘스트 받을 수 있는 요구조건을 충족했나?
                QuestManager.instance.IsQuestAvailable(i, GameObject.Find("Player").GetComponent<PlayerController>()))
            {
                //Show the info of the quest
                QuestManager.instance.ShowQuestInfoInInfo(QuestManager.instance.questDictionary[quests[i]]);

            }
            break;
        } 
    }

    public void OnClick()
    {
                    Debug.Log("Click");
        ShowQuestInfo();
        ShowD();
        InputManager.OnPressUp += DialogueManager.instance.CloseDialBoxCallback;

        dialogueIndex++;
    }
  
    public void ShowD()
    {

        if (dialogueIndex > dialogues.Length - 1)
        {
            DialogueManager.instance.CloseDialBox();
            dialogueIndex = dialogues.Length - 2;
        }
        else
        {
            DialogueManager.instance.PrintOnDialogueBox(name + ": " + dialogues[dialogueIndex]);
            Debug.Log("ShowD");
        }
    }

    void SetQuestExample()
    {
        quest = new Quest();
        quest.questName = "Dimu's first Quest";
        quest.description = "Fucking Mummies Make Me Crazy.";
        quest.reward = new Quest.Reward();
        quest.reward.exp = 400;
        quest.task = new Quest.Task();
        quest.task.kills = new Quest.QuestKill[1];
        quest.task.kills[0] = new Quest.QuestKill();
        quest.task.kills[0].id = 0;
        quest.task.kills[0].amount = 10;
    }
}
