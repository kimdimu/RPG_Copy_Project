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
        public QuestItem[] items;//아이템의 종류수
        public QuestKill[] kills;//몬스터의 종류 수
    }
    [Serializable]
    public class QuestItem
    {
        public int id;//아이템의 종류
        public int amount;//아이템의 개수
    }
    [Serializable]
    public class QuestKill
    {
        public int id;//몬스터의 종류
        public int amount;//죽일 몬스터의 개체 수
        public int initialAmount;//퀘스트 시작 시 저장할 몬스터 수
    }
}
