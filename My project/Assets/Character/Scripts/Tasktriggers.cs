using Photon.Pun;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class Tasktriggers : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject lab;

    [SerializeField]
    private GameObject bin;

    private Collider2D labCollider;
    private Collider2D binCollider;


    private void Start()
    {
        labCollider = lab.GetComponent<BoxCollider2D>();
        binCollider = bin.GetComponent<BoxCollider2D>(); //ar merge cu asta dar ah 
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerEventData.position, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);

        if (labCollider.bounds.Intersects(binCollider.bounds))
        {
            lab.SetActive(false);
            GameObject taskObject = GameObject.Find("Drop the lab");
            Outline outline = taskObject.GetComponent<Outline>();
            outline.enabled = false;
            // score += 10;


            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["Taskbar"] = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["Taskbar"].ToString()) + 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(playerProperties);


        }

    }
}