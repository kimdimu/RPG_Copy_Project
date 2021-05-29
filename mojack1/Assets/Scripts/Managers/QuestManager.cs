using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

    void Awake()
    {
        if (instance == null) instance = this;
        LoadQuests();
    }

    public void CreateJsonFile(string createpath, string filename, string jsondata)
    {
        Debug.Log("CREATEJSONFILE");
        FileStream fileStream = new FileStream(string.Format("{0}/Resources/Json files/{1}.json", createpath, filename), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsondata);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
        Debug.Log("CREATEJSONFILECLOSE");
    }

    void LoadQuests()
    {
        Quest newQuest =  JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("Json files/questtext").text);
        questDictionary.Add(newQuest.id, newQuest);
    }
}
