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
    
    // Use this for initialization
    void Start () {
        // lets get the character controller so we can use it later
        cc = GetComponent<CharacterController>();
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
        // apply non relative movements i.e. gravity
        // add to velocity from accelleration
        vertVelocity += Physics.gravity.y*Time.deltaTime;
        if (cc.isGrounded) {
            // if the player is grounded pervent velocity from building up
            vertVelocity = Physics.gravity.y * Time.deltaTime;
        }

        if (Input.GetButton("Jump") && cc.isGrounded)
        {
            // if the player is on the ground and wants to jump then make it jump
            vertVelocity = jumpVelocity;
        }

        // apply velocity to the movement
        movement.y += vertVelocity * Time.deltaTime;

        //rotate as we move the mouse horizontally
        transform.Rotate(0, Input.GetAxis("Mouse X")*mouseSensitivity, 0);

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


    }

    void FixedUpdate() {
        // apply movement
        cc.Move(movement);
        // reset movement to zero
        movement = Vector3.zero;
    }
}
