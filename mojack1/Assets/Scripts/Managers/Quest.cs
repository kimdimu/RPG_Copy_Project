using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Quest
{
    public int id;
    public string questName;
    public string description;
    public int recipent;
    public int requiredLevel;
    public Reward reward;
    public Task task;

    [Serializable]
    public class Reward
    {
        public float exp;
        public float money;
        public QuestItem[] items;
    }

    [Serializable]
    public class Task
    {
        public int[] talkTo;
        public QuestItem[] items;
        public QuestKill[] kills;
    }
    [Serializable]
    public class QuestItem
    {
        public int id;
        public int amount;
    }
    [Serializable]
    public class QuestKill
    {
        public int id;
        public int amount;
        public int playerCurrent;
    }
}
