using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class TasksManager : MonoBehaviour
{
    public string[] tasksList = { "Drop the lab", "ETH circuits", "Mouse algorithm", "Switch" };

    public List<TMP_Text> tasks = new List<TMP_Text>();
    public TMP_Text taskPrefab;
    public RectTransform listPanel;
    // Start is called before the first frame update
    void Start()
    {
        float offsetY = 0;
        foreach (string task in tasksList)
        {
            TMP_Text newTask = Instantiate(taskPrefab, transform);
            newTask.text = task;
           // newTask.rectTransform.localPosition = new Vector3(0, offsetY, 0);
            tasks.Add(newTask);
            offsetY -= newTask.preferredHeight + 3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
