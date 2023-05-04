

using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform= Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation*Vector3.forward,mainCameraTransform.rotation*Vector3.up);
    }
}
