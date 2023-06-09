using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLampOn : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button Self;
    public Image lamp;
    // Start is called before the first frame update
    void Start()
    {
        Self.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool e =Button1.GetComponent<SwitchHandler>().state;
        e =e && Button2.GetComponent<SwitchHandler>().state;
        e= e && Button3.GetComponent<SwitchHandler>().state;
        e= e && Button4.GetComponent<SwitchHandler>().state;
        if(e==true)
        {
            Self.GetComponent<Image>().enabled = true;
        }
        else
        {
            Self.GetComponent<Image>().enabled = false;
        }
    }
    public void OnImageClick()
    {
        Debug.Log("Lampa aprinsa");
        lamp.GetComponent<Image>().sprite =  Resources.Load<Sprite>("lamp-on");
        GameObject taskObject = GameObject.Find("Switch");
        Outline outline = taskObject.GetComponent<Outline>();
        outline.enabled = false;
    }
    }
