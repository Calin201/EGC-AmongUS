using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public Transform viewPoint; //camera controll
    public float mouseSensitivity = 1f;
    private float verticalRotStore;
    private Vector2 mouseinput;

    public bool invertLook;
    public float moveSpeed = 5f, runSpeed = 8f;
    private float activeMoveSpeed;

    private Vector3 movedirection, movement;

    private Camera cam;
    private float yVel;

    public CharacterController charCon;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseinput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseinput.x, transform.rotation.eulerAngles.z);
        verticalRotStore += mouseinput.y;
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 60f);

        if (invertLook)
        {
            viewPoint.rotation = Quaternion.Euler(verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }else
        {
            viewPoint.rotation = Quaternion.Euler(-verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);

        }

        movedirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }
        float yvel = movement.y;
       
       

        movement = (transform.forward * movedirection.z + transform.right * movedirection.x).normalized * activeMoveSpeed;
        movement.y = yvel;

        if(charCon.isGrounded)
        {
            movement.y = 0f;
        }
        movement.y += Physics.gravity.y * Time.deltaTime;

        charCon.Move( movement * activeMoveSpeed * Time.deltaTime);


    }

    private void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }
}
