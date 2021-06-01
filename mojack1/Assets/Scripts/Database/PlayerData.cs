using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static List<int> activeQuests = new List<int>();
    public static List<int> finishedQuests = new List<int>();

    public static void AddQuest(int id)
    {
        //해당 아이디의 퀘스트가 활성화 되어있다면.. (진행중이면?)
        if (activeQuests.Contains(id)) return;
        activeQuests.Add(id);
    }

    public class MonsterKills
    {

    }
}
