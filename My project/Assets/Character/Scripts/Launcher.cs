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
    public TMP_Text roomNameText, playerNameLabel;
    private List<TMP_Text> allPlayerName = new List<TMP_Text>();

    public GameObject errorScreen;
    public TMP_Text errorText;

    public GameObject roombrowser;
    public Roombutton theRoombutton;
    private List<Roombutton> allRoombuttons= new List<Roombutton>();

    public GameObject nameInputScreen;
    public TMP_Text nameInputText;

    private bool hasSetNickname;
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
        nameInputScreen.SetActive(false);
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

        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();

        if(!hasSetNickname)
        {
            CloseMenu();
            nameInputScreen.SetActive(true);

            if(PlayerPrefs.HasKey("playerName"))
            {
                nameInputText.text = PlayerPrefs.GetString("playerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
        }


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
        ListAllPlayers();

    }

    private void ListAllPlayers()
    {
        foreach (TMP_Text player in allPlayerName)
        {
            Destroy(player.gameObject);

           
        }

        allPlayerName.Clear();

        Player[] players = PhotonNetwork.PlayerList;
        for(int i=0; i<players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLabel.text= players[i].NickName;
            newPlayerLabel.gameObject.SetActive(true);

            allPlayerName.Add(newPlayerLabel);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPlayerLabel.text = newPlayer.NickName;
        newPlayerLabel.gameObject.SetActive(true);

        allPlayerName.Add(newPlayerLabel);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        ListAllPlayers();
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

    public void SetNickName()
    {
        if(!string.IsNullOrEmpty(nameInputText.text))
        {
            PhotonNetwork.NickName = nameInputText.text;

            PlayerPrefs.SetString("playerName", nameInputText.text);


            CloseMenu();
            menuButtons.SetActive(true);

            hasSetNickname = true;
        }
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
