using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Playercontroller : MonoBehaviourPunCallbacks
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

    public float jumpforce = 3f, gravity = 2.5f;

    public CharacterController charCon;

    public Transform groundcheckedPoint;
    private bool isgrounded;
    public LayerMask groundLayers;

    public TMP_Text names;
    public GameObject CanvasName;

   
    
    // Start is called before the first frame update
    void Start()
    {
       
        if(GetComponent<PhotonView>().IsMine == false)
        {
            CanvasName.SetActive(true);
            names.text = GetComponent<PhotonView>().Controller.NickName;
        }

        
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;

        

    }

   
   

    // Update is called once per frame
    void Update()
    {
        
        if (photonView)
        {


            mouseinput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseinput.x, transform.rotation.eulerAngles.z);
            verticalRotStore += mouseinput.y;
            verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 60f);

            if (invertLook)
            {
                viewPoint.rotation = Quaternion.Euler(verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
            }
            else
            {
                viewPoint.rotation = Quaternion.Euler(-verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);

            }

            movedirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            if (Input.GetKey(KeyCode.LeftShift))
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

            if (charCon.isGrounded)
            {
                movement.y = 0f;
            }

            isgrounded = Physics.Raycast(groundcheckedPoint.position, Vector3.down, .25f, groundLayers);

            if (Input.GetButtonDown("Jump") && isgrounded)
            {
                movement.y = jumpforce;
            }


            movement.y += Physics.gravity.y * Time.deltaTime;

            charCon.Move(movement * activeMoveSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
   
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            GetComponent<Playercontroller>().enabled = false;
        }

    }

    private void LateUpdate()
    {
        if(photonView.IsMine)
        {
            cam.transform.position = viewPoint.position;
            cam.transform.rotation = viewPoint.rotation;
        }
        
    }
}
