using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using UnityEngine;

public class TaskSpreader : MonoBehaviourPunCallbacks
{
    [SerializeField] public string[] taskList;// = { "Drop the lab", "ETH circuits", "Mouse algorithm", "Switch", "Make a game", "Delete the recycle bin", "Put the plush corectly" };
    [SerializeField] public GameObject listOfTasks;
    
    private bool isMine;
    public int numberOfTasksPerPlayer;
    // Start is called before the first frame update
    private void Awake()
    {
        var myObject = GameObject.Find("Tasks");
        Transform myObjectTransform = myObject.transform;
        var myObjectList = new List<GameObject>(myObjectTransform.GetComponentsInChildren<Transform>().Select(t => t.gameObject));
        myObjectList.Remove(myObject);
        taskList=myObjectList.Select(t => t.name).ToArray();
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AssignTasks();
        }

    }

    void AssignTasks()
    {
        Debug.Log("AssignTasks");
        List<int> playerIds=new List<int>(PhotonNetwork.CurrentRoom.Players.Keys);
        ShuffleList<int>(playerIds);
        List<string> taskListCopy = new List<string>(taskList);
        foreach (int id in playerIds)
        {
            if (!PhotonNetwork.CurrentRoom.Players[id].CustomProperties.ContainsKey("AssignedTasks"))
            {
                ShuffleList<string>(taskListCopy);
                List<string> tasksPerPlayer = taskListCopy.Take(numberOfTasksPerPlayer).ToList();
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
                playerProperties["AssignedTasks"] = tasksPerPlayer.ToArray();
                PhotonNetwork.CurrentRoom.Players[id].SetCustomProperties(playerProperties);
            }
        }
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
