using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;
using System.Linq;
using Photon.Realtime;

public class TaskManager : MonoBehaviourPunCallbacks
{
    

    public List<TMP_Text> tasks= new List<TMP_Text>();
    public TMP_Text taskPrefab;
    public RectTransform listPanel;
    // Start is called before the first frame update

    private void Awake()
    {
        if(!photonView.IsMine)
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
            foreach (string task in assignedTasks)
            {
                Debug.Log(task);
                assignedTaskText = Instantiate(taskPrefab, listPanel);
                assignedTaskText.text = task;
                tasks.Add(assignedTaskText);
            }

        }
    }

}
