using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    /*PhotonView myPV;
    int whichPlayerIsImposter;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<PhotonView>();
        myPV = GetComponent<PhotonView>();
        Debug.Log(myPV);
        if (PhotonNetwork.IsMasterClient)
        {
            PickImposter();
        }
    }
    void PickImposter()
    {
        whichPlayerIsImposter = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log(myPV);
        myPV.RPC("RPC_SyncImposter", RpcTarget.All, whichPlayerIsImposter);
        Debug.Log("Imposter " + whichPlayerIsImposter);
    }
    [PunRPC]
    void RPC_SyncImposter(int playerNumber)
    {
        whichPlayerIsImposter = playerNumber;
        Playercontroller.instance.BecomeImpostor(whichPlayerIsImposter);
    }
    */
}
