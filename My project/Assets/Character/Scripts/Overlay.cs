using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class Overlay : MonoBehaviourPun
{
    [SerializeField] private TMP_Text nameText;
    public GameObject overlay;
    // Start is called before the first frame update
    void Awake()
    {
        if(photonView.IsMine) { overlay.SetActive(true); }
        else { overlay.SetActive(false); }
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
