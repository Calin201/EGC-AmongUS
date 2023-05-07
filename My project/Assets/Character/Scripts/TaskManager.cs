using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;
using System.Linq;
using Photon.Realtime;

public class TaskManager : MonoBehaviourPunCallbacks
{
    public GameObject tasksOnMap;
    private GameObject myObject;
    public List<GameObject> myObjectList;
    public List<TMP_Text> tasks= new List<TMP_Text>();
    public TMP_Text taskPrefab;
    public RectTransform listPanel;
    // Start is called before the first frame update

    private void Awake()
    {
       // tasksOnMap = GameObject.Find("ETH circuits");
        myObject = GameObject.Find("Tasks");
        Transform myObjectTransform = myObject.transform;

        myObjectList = new List<GameObject>(myObjectTransform.GetComponentsInChildren<Transform>().Select(t => t.gameObject));
        myObjectList.Remove(myObject);

        if (!photonView.IsMine)
        {
            GetComponent<TaskManager>().enabled= false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("AssignedTasks"))
        {
            if (targetPlayer.UserId!=PhotonNetwork.LocalPlayer.UserId) return;
            string[] customProps = (string[])PhotonNetwork.LocalPlayer.CustomProperties["AssignedTasks"];
            if (customProps == null || customProps.Length <= 0)
                return;

            List<string> tasccs = ((string[])PhotonNetwork.LocalPlayer.CustomProperties["AssignedTasks"]).ToList<string>();

            List<string> assignedTasks = ((string[])PhotonNetwork.LocalPlayer.CustomProperties["AssignedTasks"]).ToList<string>();

            Debug.Log("S-a schimbat si s-a propagat");
            TMP_Text assignedTaskText;

            Debug.Log(assignedTasks.Count);

            tasks = new List<TMP_Text>(assignedTasks.Count);
            int i = 0;
            foreach (string task in assignedTasks)
            {
                //Debug.Log("outline: "+);
                foreach(GameObject tasks in myObjectList)
                {
                    if (tasks.name == task)
                    {
                        tasks.SetActive(true);
                        tasks.GetComponent<Outline>().enabled = true;

                    }
                }
                Debug.Log(task);
                assignedTaskText = Instantiate(taskPrefab, listPanel);
                assignedTaskText.text = task;
                tasks.Add(assignedTaskText);
            }

        }
    }

}
