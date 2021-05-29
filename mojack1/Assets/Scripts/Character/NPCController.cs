using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    string[] dialogues;
    public int dialogueIndex = 0;
    private Quest quest;

    private void Start()
    {
        //SetQuestExample();
        //QuestManager.instance.CreateJsonFile(Application.dataPath, "Jsontestfile", JsonUtility.ToJson(quest));

        ShowQuestInfo(QuestManager.instance.questDictionary[0]);
        //print(JsonUtility.ToJson(quest));
    }

    void ShowQuestInfo(Quest quest)
    {
        Transform info = GameObject.Find("Canvas/QuestInfo2/Background/Info/Viewport/Content").transform;
        info.Find("Name").GetComponent<Text>().text = quest.questName;
        info.Find("Description").GetComponent<Text>().text = quest.description;

        string taskString = "Task:\n";
        if(quest.task.kills!=null)
        {
            foreach (Quest.QuestKill qk in quest.task.kills)
            {
                taskString += "Slay " + qk.amount + " " + MonsterDatabase.monsters[qk.id] + ".\n";
            }
        }
        if (quest.task.items != null)
        {
            foreach (Quest.QuestItem qi in quest.task.items)
            {
                taskString += "Bring " + qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
            }
        }
        if (quest.task.talkTo != null)
        {
            foreach (int id in quest.task.talkTo)
            {
                taskString += "Talk to " + NPCDatabase.npcs[id] + ".\n";
            }
        }

        info.Find("Task").GetComponent<Text>().text = taskString;

        string rewardString = "Reward:\n";
        if (quest.reward.items!= null)
        {
            foreach (Quest.QuestItem qi in quest.reward.items)
            {
                rewardString += qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
            }
        }
        if (quest.reward.exp > 0) rewardString += quest.reward.exp + " Exp.\n";
        if (quest.reward.money > 0) rewardString += quest.reward.money + " Money.\n";
        info.Find("Reward").GetComponent<Text>().text = rewardString;
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
