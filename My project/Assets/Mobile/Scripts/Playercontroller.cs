using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using Photon.Realtime;
using Unity.VisualScripting;

public class Playercontroller : MonoBehaviourPun
{
    public static Playercontroller instance;
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

    //Amongus lines
    public string role;
    Playercontroller target;
    [SerializeField] Collider myColider;

    bool isDead;

   public GameObject panel; 

    private void Awake()
    {
        if (photonView.IsMine)
        {
            //asigning role Imposter/Crewmate
            role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        }
        else
        {
            //disabling all player controllers but mine
            GetComponent<Playercontroller>().enabled = false;
        }
        //setting the nickname to nametag
        setName();
    }

    // Start is called before the first frame update
    void Start()
    {
        //if the session is not mine
        if (GetComponent<PhotonView>().IsMine == false)
        {
            //turn on the top name
            CanvasName.SetActive(true);
        }
        if (photonView.IsMine == true)
        {
            instance = this;
            //turn on the top name
            CanvasName.SetActive(true);
        }
        //lock the cursor and set the camera
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;


    }

   

    // Update is called once per frame
    void Update()
    {
        if (photonView)
        {
            //movement
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
            }
            //end movement

            //mouse locking/unlocking
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
            //kill command
            if(Input.GetKeyDown(KeyCode.K)) 
            {
                photonView.RPC("KillTarget",RpcTarget.All);
            }
        }
    }

    private void LateUpdate()
    {
        if(photonView.IsMine)
        {
            //moving the camera with us
            cam.transform.position = viewPoint.position;
            cam.transform.rotation = viewPoint.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Playercontroller tempTarget = other.GetComponent<Playercontroller>();   
            if(instance.role== "Imposter")
            {
                if(tempTarget.role== "Imposter")
                {
                    //Imposter colided
                    return;
                }
                else
                {
                    //Crewmate assinged
                    target = tempTarget;
                }
            }
        }
        if(other.CompareTag("TaskItem"))
        {
            panel.SetActive(true);
            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TaskItem")
        {
            panel.SetActive(false);

            //Debug.Log(taskObject.name);
        }
    }

    [PunRPC]
    void KillTarget()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //only aplying for you
        if (target == null)
        {
            //No target in range
            return;
        }
        else
        {
            //a target in range
            if (target.isDead)
            {
                //a dead target in range
                return;
            }
            //asigning target 
            instance.transform.position = target.transform.position;
            //killing target
            target.Die();
            //unasigning target
            target = null;
        }

        Debug.Log("KILLED");
    }
    public void Die()
    {
        //if (photonView.IsMine)
        //{
            instance.isDead = true;
            //WIP maybe the player will be disabled
            instance.myColider.enabled = false;
            //WIP we make him black for now
            photonView.RPC("DieChanges", RpcTarget.All);
        //}
    }
    [PunRPC]
    void DieChanges()
    {
        GetComponent<Renderer>().material.color = Color.black;
        instance.names.text += "DEAD";
    }
    private void setName()
    {
        if (photonView.IsMine)
        {
            names.text = PhotonNetwork.NickName + " " + PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        }
        else
        {
            names.text = GetComponent<PhotonView>().Owner.NickName + " " + GetComponent<PhotonView>().Owner.CustomProperties["Role"];
        }
    }
}

