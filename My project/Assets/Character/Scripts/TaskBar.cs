using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour
{
    public Slider slider;
    public int maxTasks = 100;
    public int minTasks = 0;
    public int currentTasksCompleted;
    public TaskBar taskbar;
    private void Start()
    {
        currentTasksCompleted = minTasks;
        slider.minValue = 0;
        taskbar.SetMinTask(currentTasksCompleted);
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
        currentTasksCompleted += task;
        taskbar.SetTasks(currentTasksCompleted);
        slider.value = currentTasksCompleted;
    }
    
}
