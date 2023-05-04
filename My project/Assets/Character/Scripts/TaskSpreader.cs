using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using UnityEngine;

public class TaskSpreader : MonoBehaviourPunCallbacks
{
    [SerializeField] public string[] taskList = { "Drop the lab", "ETH circuits", "Mouse algorithm", "Switch", "Make a game", "Delete the recycle bin", "Put the plush corectly" };

    private bool isMine;
    public int numberOfTasksPerPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
        if (!PhotonNetwork.IsMasterClient)
        {
            // Disable the script on non-master clients
            enabled = true;
            return;
        }
        isMine = photonView.IsMine;
        if (isMine)
        {
            AssignTasks();
        }
        else
        {
            GetComponent<TasksManager>().enabled = false;
        }
    }

    void AssignTasks()
    {
        Debug.Log("AssignTasks");
        List<string> taskListCopy = new List<string>(taskList);
        ShuffleList<string>(taskListCopy);
        List<string> tasksPerPlayer = taskListCopy.Take(numberOfTasksPerPlayer).ToList();
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["AssignedTasks"] = tasksPerPlayer.ToArray();
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
