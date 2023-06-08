using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class SwitchHandler : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        Button button = image.GetComponent<Button>();
        button.onClick.AddListener(OnImageClick);
    }

    public  void OnImageClick()
    {
        Debug.Log("Image clicked: " + image.name);
        // Efectuează acțiunile corespunzătoare în funcție de imaginea apăsată
        Sprite imposterSprite = Resources.Load<Sprite>("switch-on");
        image.sprite = imposterSprite;
    }
}
