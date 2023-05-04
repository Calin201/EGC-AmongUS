using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RandomColor : MonoBehaviourPunCallbacks
{
    public Renderer renderer;
    public Material[] playerMaterials;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            // genereaza o culoare aleatoare pentru jucatorul curent
            int randomMaterialIndex = Random.Range(0, playerMaterials.Length);
            GetComponent<Renderer>().material = playerMaterials[randomMaterialIndex];

            // trimite culoarea aleatoare la ceilalti jucatori din camera
            photonView.RPC("SetPlayerColor", RpcTarget.OthersBuffered, randomMaterialIndex);
        }
    }

    [PunRPC]
    void SetPlayerColor(int materialIndex)
    {
        GetComponent<Renderer>().material = playerMaterials[materialIndex];
    }
}
