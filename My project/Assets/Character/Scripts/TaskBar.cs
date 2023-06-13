using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TaskBar : MonoBehaviourPunCallbacks
{
    public Slider slider;
    public int maxTasks = 100;
    public int minTasks = 0;
    public int currentTasksCompleted;
    public TaskBar taskbar;

    public GameObject Win_panel;
    public Button Exit;
    private void Start()
    {
        currentTasksCompleted = minTasks;
        slider.minValue = 0;
        taskbar.SetMinTask(currentTasksCompleted);
    }
    public void Update()
    {

    }
    public void SetMaxTask(int tast)
    {
        slider.maxValue = tast;
        //slider.value = tast;
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




    public void TaskDone(int task)
    {
        currentTasksCompleted = task;
        taskbar.SetTasks(currentTasksCompleted);
        slider.value = currentTasksCompleted;
       
        if (slider.value >= slider.maxValue)
        {
            // Navigare către scena Main Menu
           // PhotonNetwork.LoadLevel("MainMenu");
           Win_panel.SetActive(true);
        }
       
    }

   

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Taskbar"))
        {
            TaskDone(int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["Taskbar"].ToString()));
        }
        if(propertiesThatChanged.ContainsKey("numberOfTasksPerPlayer"))
        {
            SetMaxTask(int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["numberOfTasksPerPlayer"].ToString())*(PhotonNetwork.CurrentRoom.Players.Count-1));
        }
    }
}