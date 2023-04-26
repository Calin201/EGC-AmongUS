using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

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
    // Start is called before the first frame update 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
