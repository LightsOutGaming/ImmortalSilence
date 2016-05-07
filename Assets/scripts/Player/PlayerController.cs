using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    CharacterController cc; // the character controller
    Vector3 movement; // the planned movement for next Fixed Update
    public float speed; // the speed the player walks
    float vertVelocity; // the vertical velocity of the player (used mostly in gravety calculations)
    public float mouseSensitivity = 5; // the sensitivity of the mouse for mouselook
    float cameraRotX = 0; // the x rotation of the camera
    bool lockCursor = true; // should the cursor be locked? or not?
    public float jumpVelocity = 30; // how fast should the player jump?
    public float airControl = .1f; // how well can you move while in the air
    public float interactDistance = 30; //the range of the player's interaction
    public GameObject InventoryUI; // the gameobject containing all inventory ui elements
    bool controllsFrozen = false; // should the controlls be frozen?
    bool pausemenu = false; //is the pause menu being displayed?
    public GameObject pausemenuobject; //the object that contains the pause menu
    static public PlayerController instance; //the instance of the active player controller object;
    
    // Use this for initialization
    void Start () {
        // lets get the character controller so we can use it later
        cc = GetComponent<CharacterController>();
        //set the instance to the active playercontroller object
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (lockCursor)
        {
            // if the cursor should be locked then lock it and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            // otherwise unlock it and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Only apply controlls if they should not be frozen
        if (!controllsFrozen)
        {
            // movements that are relative to object rotation
            Vector3 relativeMovements = Vector3.zero;

            if (cc.isGrounded)
            {
                // forward and backward
                relativeMovements.z += Input.GetAxis("Vertical") * speed * Time.deltaTime;
                //left and right
                relativeMovements.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            }
            else {
                // forward and backward
                relativeMovements.z += Input.GetAxis("Vertical") * speed * Time.deltaTime * airControl;
                //left and right
                relativeMovements.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime * airControl;
            }

            //end of movements relative to object rotation
            //make movements relative to rotation
            relativeMovements = transform.rotation * relativeMovements;
            movement += relativeMovements;

            //rotate as we move the mouse horizontally
            transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);

            // lets do the vertical mouse look!
            // change the camera rotation by the vertical mouse movement
            cameraRotX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            // clamp the camera rotation to -60 and 60
            cameraRotX = Mathf.Clamp(cameraRotX, -60, 60);
            // get the eulerAngles from the camera's transform
            Vector3 eulerAngles = Camera.main.transform.rotation.eulerAngles;
            // set the camera x rotation
            eulerAngles.x = cameraRotX;
            // apply the modified rotation to the camera
            Camera.main.transform.rotation = Quaternion.Euler(eulerAngles);

            //Interaction code

            //was the interact button pressed down this frame
            if (Input.GetButtonDown("Interact"))
            {
                //the resulting racast info
                RaycastHit info;
                //lets cast a ray, and see if it hits anything
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, interactDistance))
                {
                    // the object we are currently scanning
                    Transform scanning = info.collider.transform;
                    //the interactable we found
                    Interactable interactable = null;
                    //keep looping untill we run out of parents or found an interactable
                    while (scanning != null && interactable == null)
                    {
                        //try to find an interactable
                        interactable = scanning.GetComponent<Interactable>();
                        //move on to the next parent
                        scanning = scanning.parent;
                    }
                    // if we found an interactable, call it.
                    if (interactable != null)
                        interactable.OnInteract(gameObject);
                }
            }
        }
        // apply non relative movements i.e. gravity
        // add to velocity from accelleration
        vertVelocity += Physics.gravity.y * Time.deltaTime;
        if (cc.isGrounded)
        {
            // if the player is grounded pervent velocity from building up
            vertVelocity = Physics.gravity.y * Time.deltaTime;
        }

        if (Input.GetButton("Jump") && cc.isGrounded && !controllsFrozen)
        {
            // if the player is on the ground and wants to jump then make it jump
            vertVelocity = jumpVelocity;
        }

        // apply velocity to the movement
        movement.y += vertVelocity * Time.deltaTime;


        //Inventory Management

        //check if pressed down inventory button this frame
        if (Input.GetButtonDown("Inventory")) {
            //toggle the inventory ui
            InventoryUI.SetActive(!InventoryUI.activeInHierarchy);
            //toggle cursor lock
            lockCursor = !InventoryUI.activeInHierarchy;
            //toggle frozen controlls
            controllsFrozen = InventoryUI.activeInHierarchy;
        }

        //pause menu management

        //check if the pausemenu button was pressed down this frame
        if (Input.GetButtonDown("PauseMenu")) {
            //toggle the pausemenu state
            pauseMenu(!pausemenu);
        }

    }

    void FixedUpdate() {
        // apply movement
        cc.Move(movement);
        // reset movement to zero
        movement = Vector3.zero;
    }

    //called when the pause menu should show up or go away
    public void pauseMenu(bool active) {
        //turn on/off the pause menu
        pausemenu = active;
        //freeze/unfreeze the player's controlls
        controllsFrozen = active;
        //lock/unlock the cursor
        lockCursor = !active;
        //activate/deactivate the menu
        pausemenuobject.SetActive(active);
    }
}
