using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject PlayerCamera;
    public GameObject PlayerModdel;
    public GameObject PlayerBody;
    public GameObject PlayerNeck;

    [Header("Sounds Effects")]
    public AudioSource walking;

    [Header("Movement Verables")]
    public float movementSpeed = 1.0f;
    public float jumpSpeed = 5.0f;
    public float gravity = 250.0f;
    public float lengthOfJump = 2.0f;

    [Header("Look Speed")]
    public float lookSpeed = 1.0f;

    //- private verable -//
    private Vector2 rotation = new Vector2(0, 0);
    private float jumpCounter = 0.0f;
    private bool spaceActive = false;

    private bool proControllerActive = false;
    private bool keyboardActive = true;
    private Animator animator;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        animator = PlayerModdel.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        string[] controllers = Input.GetJoystickNames();

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] == "Wireless Gamepad")
            {
                if (Input.GetAxis("HorizontalJoyStick") != 0.0f ||
                    Input.GetAxis("VerticalJoyStick") != 0.0f ||
                    Input.GetKeyDown(KeyCode.JoystickButton0) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton1) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton2) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton3) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton6) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton7) == true ||
                    Input.GetAxis("VerticalJoyStickRight") != 0.0f ||
                    Input.GetAxis("HorizontalJoyStickRight") != 0.0f)
                {
                    proControllerActive = true;
                    keyboardActive = false;
                }
                else
                {
                    keyboardActive = true;
                    proControllerActive = false;
                }
            }
        }

        if (proControllerActive == true)
            MoveCharacter("HorizontalJoyStick", "VerticalJoyStick", "VerticalJoyStickRight", "HorizontalJoyStickRight", KeyCode.Joystick1Button0, true); //REPLACED BUTTON REPLACE THIS
        else if (keyboardActive == true)
            MoveCharacter("Vertical", "Horizontal", "Mouse Y", "Mouse X", KeyCode.Space, false);
    }

    void MoveCharacter(string GetAxisVerticle, string GetAxisHorizontal, string LookXGetAxis, string LookYGetAxis, KeyCode JumpKey, bool Controller)
    {
        Vector3 moveDirection;
        if (Controller)
            moveDirection = new Vector3(Input.GetAxis(GetAxisHorizontal), 0, -Input.GetAxis(GetAxisVerticle));
        else
            moveDirection = new Vector3(Input.GetAxis(GetAxisHorizontal), 0, Input.GetAxis(GetAxisVerticle));

        if (moveDirection.x == 0.0f && moveDirection.y == 0.0f && moveDirection.z == 0.0f && jumpCounter < -0.3)
        {
            walking.Stop();
            animator.SetBool("Walking", false);
            animator.SetBool("Idle", true);
        }
        else if (moveDirection.x != 0.0f || moveDirection.y != 0.0f || moveDirection.z != 0.0f)
        {
            if (!walking.isPlaying)
                walking.Play();
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
        }
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= movementSpeed;

        if (Input.GetKeyDown(JumpKey) && spaceActive == false)
        {
            walking.Stop();
            jumpCounter = lengthOfJump;
            spaceActive = true;
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Jumping", true);
        }
        if (jumpCounter > -0.3)
        {
            jumpCounter -= 1 * Time.deltaTime;
            moveDirection.y += (jumpSpeed * jumpCounter) + Time.deltaTime;
        }
        else if (jumpCounter < -0.3)
        {
            jumpCounter = 0;
            spaceActive = false;
            animator.SetBool("Jumping", false);
        }
        moveDirection.y -= gravity * Time.deltaTime;

        rotation.y += Input.GetAxis(LookYGetAxis) * lookSpeed;

        if (Controller)
            rotation.x += Input.GetAxis(LookXGetAxis) * lookSpeed;
        else
            rotation.x += -Input.GetAxis(LookXGetAxis) * lookSpeed;

        rotation.x = Mathf.Clamp(rotation.x, -90, 90);

        PlayerNeck.transform.localEulerAngles = new Vector2(rotation.x,0); 
        PlayerBody.transform.eulerAngles = new Vector2(0, rotation.y);

        controller.Move(moveDirection * Time.deltaTime);
    }
}