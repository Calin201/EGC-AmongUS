using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCrewBase : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    protected virtual void RPC_Exit()
    {
        SceneManager.LoadScene("Main Menu");
        PhotonNetwork.Disconnect();
    }
}
