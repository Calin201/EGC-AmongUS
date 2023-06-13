using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCrewmate : MonoBehaviourPunCallbacks
{
    public GameObject Win_panel;
    public Button Exit;

    private void Start()
    {
        Exit.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame() called");

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.InRoom)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    // Jucătorul local este gazda și părăsește camera
                    PhotonNetwork.LeaveRoom();
                }
                else
                {
                    // Jucătorul local nu este gazda și părăsește camera
                    photonView.RPC("LeaveRoom", RpcTarget.MasterClient);
                }
            }

            Win_panel.SetActive(false);
            SceneManager.LoadScene("Main Menu");
        }
    }

    [PunRPC]
    private void LeaveRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        Win_panel.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            // Jucătorul local a părăsit camera
            Win_panel.SetActive(false);
            SceneManager.LoadScene("Main Menu");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer == newMasterClient)
        {
            // Jucătorul local este noul gazdă
        }
        else
        {
            // Gazda anterioară s-a deconectat
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                // Jucătorul local este noul gazdă și părăsește camera
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                // Jucătorul local nu este gazdă și părăsește camera
                photonView.RPC("LeaveRoom", RpcTarget.MasterClient);
            }
        }

        Win_panel.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
}
