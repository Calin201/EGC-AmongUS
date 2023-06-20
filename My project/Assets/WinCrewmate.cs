using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCrewmate : MonoBehaviourPunCallbacks
{
    public GameObject Win_panel;
    public Button Exit;
    public GameObject player;
    private void Start()
    {
        Exit.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame() called");

            Exxit();//Invoke("Exxit", 5.0f);

    }
    void Exxit()
    {
       player.GetComponent<RandomColor>().Exit();
    }
    public override void OnLeftRoom()
    {
        // Verificăm dacă jucătorul local este în cameră
        if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.LocalPlayer.IsLocal)
        {
            // Jucătorul local a părăsit camera
            Win_panel.SetActive(false);
            SceneManager.LoadScene("Main Menu");
        }
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
}
