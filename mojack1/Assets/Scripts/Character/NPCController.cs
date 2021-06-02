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

                //퀘스트 다했다면 버튼 on
                if (QuestManager.instance.IsQuestFinished(i))
                {
                    UIController.instance.questInfoCompleteButton.gameObject.SetActive(true);
                    UIController.instance.questInfoCompleteButton.onClick.AddListener(() =>
                    {
                    ReceiveConpleteQuest(QuestManager.instance.questDictionary[quests[i]]);
                    PlayerData.activeQuests.Remove(i);//활성화에서 빼고
                    PlayerData.finishedQuests.Add(i); //끝낸 목록에 보낸다.
                    UIController.instance.questInfoCompleteButton.onClick.RemoveAllListeners();//버튼 이벤트 삭제한다.
                    UIController.instance.questInfo.gameObject.SetActive(false);//UI 끈다.

                    GameObject QuestButtonGo = UIController.instance.questBookContent.Find(i.ToString()).gameObject;
                    Destroy(QuestButtonGo);//왜 못없애지 transform을 gameobject로 바꾸니까 없어짐.
                        //이거 없애면 안되고 완료 탭 만들어서 거기로 옮겨야됨.
                    });
                }
            }
                break;
        }
    }
    void ReceiveConpleteQuest(Quest quest)
    {
        if (quest.reward.exp > 0) PlayerController.main.SetExp(quest.reward.exp);
        if(quest.reward.items.Length>0)
        {
            foreach(var item in quest.reward.items)
            {
                print("You get: ("+item.amount+")x"+ItemDatabase.items[item.id]);
                //인벤토리에 추가.
            }
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
        quest.task.kills = new Quest.QuestKill[1]; //몬스터 종류 1개
        quest.task.kills[0] = new Quest.QuestKill();
        quest.task.kills[0].id = 0; //0번 몬스터
        quest.task.kills[0].amount = 10; // 10번 죽여라
    }
}
