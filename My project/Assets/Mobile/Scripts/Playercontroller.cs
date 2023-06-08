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
    public GameObject button2UI;

    public GameObject ETH_Canvas;
    public GameObject Drop_The_Lab;
    public GameObject Switch;

    private bool panelActive = false;
    private bool isColliding = false;
    private bool isCurrentTaskOutlineActive = false;
    private bool doingTask = false;
    private string nume;

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
        button2UI.SetActive(true);



    }



    // Update is called once per frame
    void Update()
    {
        if (photonView)
        {
            //movement
            {
                mouseinput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
                if (!panelActive)
                {
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
                    if(!panelActive)
                    {
                        Cursor.lockState = CursorLockMode.Locked;

                    }
                }
            }
            //kill command
            if (Input.GetKeyDown(KeyCode.K))
            {
                photonView.RPC("KillTarget", RpcTarget.All);
            }

            if (Input.GetKeyDown(KeyCode.E) && isCurrentTaskOutlineActive)
            {
                panelActive = !panelActive;
                if(nume=="ETH circuits")
                {
                    ETH_Canvas.SetActive(panelActive);
                }
                else
                    if(nume=="Drop the lab")
                {
                    Drop_The_Lab.SetActive(panelActive);
                }
                else
                {
                    if(nume=="Switch")
                    {
                        Switch.SetActive(panelActive);
                    }
                }
               
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                doingTask = !doingTask;

            }
        }
    }

    private bool IsTaskOutlineActive(string taskName)
    {
        GameObject taskObject = GameObject.Find(taskName);

        if (taskObject != null)
        {
            Outline outline = taskObject.GetComponent<Outline>();
            if (outline != null && outline.enabled)
            {
                // Task-ul are outline activ
                return true;
            }
        }

        // Task-ul nu are outline activ sau nu a fost găsit în scenă
        return false;
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            //moving the camera with us
            cam.transform.position = viewPoint.position;
            cam.transform.rotation = viewPoint.rotation;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        Image buttonImage = panel.GetComponentInChildren<Image>();
        if (other.tag == "Player")
        {
            Playercontroller tempTarget = other.GetComponent<Playercontroller>();
            if (instance.role == "Imposter")
            {
                if (tempTarget.role == "Imposter")
                {
                    //Imposter colided
                    return;
                }
                else
                {

                    //Crewmate assinged
                    target = tempTarget;

                    Sprite imposterSprite = Resources.Load<Sprite>("kill");
                    if (imposterSprite != null)
                    {

                        buttonImage.sprite = imposterSprite;
                        panel.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError("Failed to load ImposterSprite!");
                    }
                }


            }
        }
        if (other.CompareTag("TaskItem") && instance.role == "Crewmate")
        {
           
            // Image buttonImage = panel.GetComponentInChildren<Image>();
            string taskName = other.gameObject.name;
            if (instance.role == "Crewmate")
            {
                    if(IsTaskOutlineActive(taskName))
                {
                    panel.SetActive(true);
                    Sprite defaultSprite = Resources.Load<Sprite>("qvorpkw1cxy51");
                    if (defaultSprite != null)
                    {
                        buttonImage.sprite = defaultSprite;
                    }
                    else
                    {
                        Debug.LogError("Failed to load default sprite!");
                    }
                }
            }
        }
        if (other.CompareTag("TaskItem") && instance.role == "Crewmate")
        {
            isColliding = true;
            string taskName = other.gameObject.name;
            if (IsTaskOutlineActive(taskName))
            {
                isCurrentTaskOutlineActive = true;
                nume = taskName;

            }
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TaskItem")
        {
            panel.SetActive(false);

            //Debug.Log(taskObject.name);
        }

        if (other.tag == "Player")
        {
            panel.SetActive(false);

            //Debug.Log(taskObject.name);
        }
        if (other.CompareTag("TaskItem") && instance.role == "Crewmate")
        {
            isColliding = false;
            isCurrentTaskOutlineActive = false;
            Cursor.visible = false;
            ETH_Canvas.SetActive(false);
            Drop_The_Lab.SetActive(false);
            Switch.SetActive(false);
            doingTask = false;
            panelActive = false;
            nume = " ";
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

