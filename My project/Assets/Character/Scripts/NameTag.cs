using Palmmedia.ReportGenerator.Core;

using Photon.Pun;

using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class NameTag : MonoBehaviourPun
{
    [SerializeField] private TMP_Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine) { return; }
        setName();
    }

    private void setName()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            nameText.text = PhotonNetwork.NickName;
        }
        else

        {
            nameText.text = GetComponent<PhotonView>().Owner.NickName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
