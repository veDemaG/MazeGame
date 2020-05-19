using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Serializable]
public class data
{
    int response_code;
    public List<Results> results;
}
[System.Serializable]
public class Results
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;
}
public class Quetionaire : MonoBehaviour
{
    public data rs;
    public TextAsset file;
    public static Quetionaire instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(loadQuestionaire());
        Debug.Log("Loading data");
    }
    public IEnumerator loadQuestionaire()
    {
        // using https://opentdb.com/ for free API

        WWW w = new WWW("https://opentdb.com/api.php?amount=100&difficulty=easy&type=multiple");

        yield return w;

        if (w.error != null)
        {
            Debug.Log("Error .. " + w.error);
            rs = JsonUtility.FromJson<data>(file.text);
            Debug.Log("Load Offline Database");
        }
        else
        {
            Debug.Log("Quetions Loaded...");
            rs = JsonUtility.FromJson<data>(w.text);
        }
    }
}
