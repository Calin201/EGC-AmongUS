using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class Roombutton : MonoBehaviour
{
    public TMP_Text buttontext;
    private RoomInfo info;

    public void SetButtonDetails(RoomInfo inputInfo)
    {
        info = inputInfo;
        buttontext.text = info.Name;

    }

    public void OpenRoom()
    {
        Launcher.instance.JoinRoom(info);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
