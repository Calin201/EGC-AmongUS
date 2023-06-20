using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RandomColor : MonoBehaviourPunCallbacks
{
    public Renderer renderer;
    public List<Material> playerMaterials;
    public GameObject Win_panel;
    public GameObject Lose_panel;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            // genereaza o culoare aleatoare pentru jucatorul curent
            int randomMaterialIndex = Random.Range(0, playerMaterials.Count);
            // trimite culoarea aleatoare la ceilalti jucatori din camera
            photonView.RPC("SetPlayerColor", RpcTarget.OthersBuffered, randomMaterialIndex);
        }
    }

    public void Exit()
    {
            photonView.RPC("RPC_Exit",RpcTarget.All);
    }
    [PunRPC]
    void RPC_Exit()
    {
        Win_panel.SetActive(false);
        Lose_panel.SetActive(false);
        SceneManager.LoadScene("Main Menu");

    }
    [PunRPC]
    void SetPlayerColor(int materialIndex)
    {
        GetComponent<Renderer>().material = playerMaterials[materialIndex];
        playerMaterials.RemoveAt(materialIndex);
    }
    public override void OnLeftRoom()
    {
        // Verificăm dacă jucătorul local este în cameră
        if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.LocalPlayer.IsLocal)
        {
            // Jucătorul local a părăsit camera
            Win_panel.SetActive(false);
            Lose_panel.SetActive(false);
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
