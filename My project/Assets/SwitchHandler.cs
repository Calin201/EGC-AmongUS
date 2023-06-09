using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class SwitchHandler : MonoBehaviour
{
    public Button button;
    //public Image image;
    public bool state =  RandomNumberGenerator.GetInt32(0,100)<50;
    private void Start()
    {
        //button.onClick.AddListener(OnImageClick);
    }

    public void OnImageClick()
    {
        Debug.Log("Image clicked: " + button.name+" "+state);
        // Efectuează acțiunile corespunzătoare în funcție de imaginea apăsată
        Sprite imageOff = Resources.Load<Sprite>("switch-off");
        Sprite imageOn = Resources.Load<Sprite>("switch-on");
        Image da = button.GetComponentInChildren<Image>();
        
        state = !state;
        if (state == true)
        {
            
            da.sprite = imageOn;
        }
        else
        {

            da.sprite = imageOff;
        }
    }
}
