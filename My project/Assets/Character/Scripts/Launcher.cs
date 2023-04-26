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
            options.MaxPlayers = 2;
            
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
    // Update is called once per frame
    void Update()
    {

    }
}
