using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

    void Awake()
    {
        if (instance == null) instance = this;
        SetCallbacks();
    }


    public void ShowQuestInfoInInfo(Quest quest)
    {
        Debug.Log("QM ShowQuestInfo");

        //show q info panel
        UIController.instance.questInfo.gameObject.SetActive(true);
        //가져온 퀘를 받아서 진행중이지 않다면? SetActive true 
        UIController.instance.questInfoAcceptButton.gameObject.SetActive(!PlayerData.activeQuests.ContainsKey(quest.id)
                                                                        && !PlayerData.finishedQuests.Contains(quest.id));

        //클리어 버튼 숨긴다.
        UIController.instance.questInfoCompleteButton.gameObject.SetActive(false);

        //이전에 추가된 함수 삭제
        UIController.instance.questInfoAcceptButton.onClick.RemoveAllListeners();
        //새 함수 추가
        UIController.instance.questInfoAcceptButton.onClick.AddListener(() =>
        {
            //퀘스트 추가!
            PlayerData.AddQuest(quest.id);
            //hide q info panel
            UIController.instance.questInfo.gameObject.SetActive(false);

            ShowActiveQuestsInGrid();
        });

        #region UIText

        UIController.instance.questInfoContent.Find("Name").GetComponent<Text>().text = quest.questName;
        UIController.instance.questInfoContent.Find("Description").GetComponent<Text>().text = quest.description;

        string taskString = "Task:\n";
        if (quest.task.kills != null)
        {
            foreach (Quest.QuestKill qk in quest.task.kills)
            {
                int curKills = 0;
                if (PlayerData.activeQuests.ContainsKey(qk.id)&&PlayerData.monstersKilled.ContainsKey(qk.id))
                    curKills = PlayerData.monstersKilled[qk.id].amount - PlayerData.activeQuests[quest.id].kills[qk.id].initialAmount;

                taskString += "Slay " + (curKills) + "/" + qk.amount + " " + MonsterDatabase.monsters[qk.id] + ".\n";
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

        UIController.instance.questInfoContent.Find("Task").GetComponent<Text>().text = taskString;

        string rewardString = "Reward:\n";
        if (quest.reward.items != null)
        {
            foreach (Quest.QuestItem qi in quest.reward.items)
            {
                rewardString += qi.amount + " " + ItemDatabase.items[qi.id] + ".\n";
            }
        }
        if (quest.reward.exp > 0) rewardString += quest.reward.exp + " Exp.\n";
        if (quest.reward.money > 0) rewardString += quest.reward.money + " Money.\n";
        UIController.instance.questInfoContent.Find("Reward").GetComponent<Text>().text = rewardString;
        #endregion
    }
    public void LoadQuest(int id)
    {
        Quest newQuest =  JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("Json files/"+ id.ToString("00")).text);
        //파일이름 00 01 . . .
        questDictionary.Add(newQuest.id, newQuest);
        //Debug.Log(newQuest.id + newQuest.questName + "Load Success");
    }

    void SetCallbacks()
    {
        InputManager.KeyPressDown += KeyCallbacks;
    }

    void KeyCallbacks()
    {
        if(Input.inputString=="b")
        {
            ToggleQuestBook(!UIController.instance.questBook.gameObject.activeInHierarchy);
        }
    }

    void ToggleQuestBook(bool b)
    {
        UIController.instance.questBook.gameObject.SetActive(b);
        if (b) ShowActiveQuestsInGrid();
    }

    public void ShowActiveQuestsInGrid()
    {
        foreach(PlayerData.ActiveQuest aq in PlayerData.activeQuests.Values)
        {
            int i = aq.id;
            if (UIController.instance.questBookContent.Find(i.ToString()) != null)
                continue;
        Debug.Log("ShowActiveQuests");
            //create new Quest Button
            GameObject QuestButtonGo = Instantiate(Resources.Load("Prefabs/Quest_Button_Prefab") as GameObject);
            QuestButtonGo.name = questDictionary[i].id.ToString();
            QuestButtonGo.transform.SetParent(UIController.instance.questBookContent);
            QuestButtonGo.transform.localScale = Vector3.one;
            QuestButtonGo.transform.Find("Text").GetComponent<Text>().text = questDictionary[i].questName;
            int questid = new int();
            questid = i;
            QuestButtonGo.GetComponent<Button>().onClick.AddListener(() => { ShowQuestInfoInInfo(questDictionary[questid]); });
        }
    }
    public bool IsQuestAvailable(int questId, PlayerController player)
    {
        return (questDictionary[questId].requiredLevel <= player.level);
    }
    public bool IsQuestFinished(int questId)
    {
        Quest quest = questDictionary[questId];

        //뭔가를 죽여야한다면
        if(quest.task.kills.Length>0)
        {
            foreach(var questKill in quest.task.kills)
            {
                if (!PlayerData.activeQuests.ContainsKey(quest.id)||!PlayerData.monstersKilled.ContainsKey(questKill.id)) return false;

                int curKills = PlayerData.monstersKilled[questKill.id].amount -
                             PlayerData.activeQuests[quest.id].kills[questKill.id].initialAmount;
                //지금 잡은거보다 목표가 더 크면
                if (curKills < questKill.amount) return false;
            }
        }

        return true;
    }

    public void CreateJsonFile(string createpath, string filename, string jsondata)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Resources/Json files/{1}.json", createpath, filename), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsondata);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
        Debug.Log("CREATEJSONFILECLOSE");
    }
}
