using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour
{
    public Slider slider;
    public int maxTask = 100;
    public int minTask = 0;
    public int currentTask;
    public TaskBar taskbar;
    private void Start()
    {
        currentTask = minTask;
        slider.minValue = 0;
        taskbar.SetMinTask(currentTask);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TaskDone(10);
        }
    }
    public void SetMaxTask(int tast)
    {
        slider.maxValue = tast;
        slider.value = tast;
    }

    public void SetMinTask(int tast)
    {
        slider.minValue = tast;
        slider.value = tast;
    }

    public void SetTasks(int task)
    {
        slider.value = task;
    }
    void TaskDone(int task)
    {
        currentTask += task;
        taskbar.SetTasks(currentTask);
        slider.value = currentTask;
    }
    
}
