
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
        } //merge 
        //mai trebuie adaugat + 1 pt taskbar 
        //sa nu mai poti intra pe el dupa ce e gata
        //si sa dispara outline cand e gata 
        //pun in panel ala cu verde task ul 

    }
}