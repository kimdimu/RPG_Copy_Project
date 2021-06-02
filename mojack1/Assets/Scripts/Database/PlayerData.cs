using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : MonoBehaviour
{
    public static Dictionary<int, ActiveQuest> activeQuests = new Dictionary<int, ActiveQuest>();
    public static Dictionary<int, MonsterKills> monstersKilled = new Dictionary<int, MonsterKills>();
    public static List<int> finishedQuests = new List<int>();

    public static void AddQuest(int id)
    {
        //해당 아이디의 퀘스트가 활성화 되어있다면.. (진행중이면?)
        if (activeQuests.ContainsKey(id)) return;

        //해당 아이디의 퀘스트를 참조할 객체 만듬.
        Quest quest = QuestManager.instance.questDictionary[id];
        //사전에 Add할 객체 만듬
        ActiveQuest newActiveQuest = new ActiveQuest();
        newActiveQuest.id = id;
        newActiveQuest.dataTaken = DateTime.Now.ToLongDateString();

        if(quest.task.kills.Length >0) //죽일 종류가 하나 이상 있다면.. 
        {
            //Add할 객체의 종류에 종류 길이만큼의 배열 할당
            newActiveQuest.kills = new Quest.QuestKill[quest.task.kills.Length];
            //종류 하나하나 돈다.
            foreach(Quest.QuestKill questKill in quest.task.kills)
            {
                //종류 할당
                newActiveQuest.kills[questKill.id] = new Quest.QuestKill();

                //부탁받은 종류가 데이터에 없다? 추가한다.
                if (!monstersKilled.ContainsKey(questKill.id))
                    monstersKilled.Add(questKill.id, new MonsterKills());

                newActiveQuest.kills[questKill.id].initialAmount = monstersKilled[questKill.id].amount;
            }

        }
        activeQuests.Add(id, newActiveQuest);
    }

    //어떤 몬스터를 얼마나 죽였나?
    public class MonsterKills
    {
        public int id;
        public int amount;
    }

    //어떤 퀘스트를 받았나?
    public class ActiveQuest
    {
        public int id;
        public string dataTaken;//언제 퀘스트를 받았나?
        public Quest.QuestKill[] kills;//죽일 몬스터 종류
    }
}
