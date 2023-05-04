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
    }



    // Update is called once per frame
    void Update()
    {
        if(nameText !=null)
        {
            nameText.transform.LookAt(Camera.main.transform.position);
            nameText.transform.Rotate(0, 180, 0);
        } 
    }
}
