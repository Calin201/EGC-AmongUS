using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    [SerializeField] private float minDist = 0.6f;

    private LineRenderer lineRenderer;
    private Vector3 previousPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        previousPoint = transform.position;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 currentPoz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPoz.z = 0f;
            if (Vector3.Distance(currentPoz ,previousPoint) >minDist)
            {
                if(previousPoint==transform.position)
                {
                    lineRenderer.SetPosition(0, currentPoz);
                }
                else
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPoz);
                }
                previousPoint = currentPoz;
            }
        }
        
    }
}
