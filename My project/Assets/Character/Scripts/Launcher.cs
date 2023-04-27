using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public void Awake()
    {
        instance = this;
    }

    public GameObject loadingScreen;
    public TMP_Text loadingtext;

    public GameObject menuButtons;

    public GameObject createRoomScreen;
    public TMP_InputField roomInput;

    public GameObject roomScreen;
    public TMP_Text roomNameText;

    public GameObject errorScreen;
    public TMP_Text errorText;

    public GameObject roombrowser;
    public Roombutton theRoombutton;
    private List<Roombutton> allRoombuttons= new List<Roombutton>();


    // Start is called before the first frame update 
    void Start()
    {
        CloseMenu();

        loadingScreen.SetActive(true);
        loadingtext.text = "Connecting To Network...";

        PhotonNetwork.ConnectUsingSettings();

    }
    void CloseMenu()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roombrowser.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
       

        PhotonNetwork.JoinLobby();
        loadingtext.text = "Joining lobby...";

    }

    public override void OnJoinedLobby()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public void OpenRoomCreate()
    {
        CloseMenu();
        createRoomScreen.SetActive(true);

    }

    public void CreateRoom()
    {
        if(!string.IsNullOrEmpty(roomInput.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 3;
            
            PhotonNetwork.CreateRoom(roomInput.text,options);

            CloseMenu();
            loadingtext.text = "Creating Room...";
            loadingScreen.SetActive(true);

        }
    }

    public override void OnJoinedRoom()
    {
        CloseMenu();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to create room" + message;
        CloseMenu();
        errorScreen.SetActive(true);

    }

    public void CloseErrorScreen()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenu();
        loadingtext.text = "Leaving Room...";
        loadingScreen.SetActive(true);

    }

    public override void OnLeftRoom()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public void OpenRoomBrowser()
    {
        CloseMenu();
        roombrowser.SetActive(true);
    }
    public void CloseRoomBrowser()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach (Roombutton rb in allRoombuttons)
        {
            Destroy(rb.gameObject);
        }
        allRoombuttons.Clear();
        theRoombutton.gameObject.SetActive(false);

        for (int i = 0; i < roomlist.Count; i++)
        {
            if (roomlist[i].PlayerCount != roomlist[i].MaxPlayers && !roomlist[i].RemovedFromList)
            {
                Roombutton newbutton = Instantiate(theRoombutton, theRoombutton.transform.parent);
                newbutton.SetButtonDetails(roomlist[i]);
                newbutton.gameObject.SetActive(true);

                allRoombuttons.Add(newbutton);
            }
        }
    }

    public void JoinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);
        CloseMenu();
        loadingtext.text = "Joining room...";
        loadingScreen.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
