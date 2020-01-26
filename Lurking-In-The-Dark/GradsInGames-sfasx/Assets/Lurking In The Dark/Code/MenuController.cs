using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Game/Menu Objects")]
    public GameObject Menu = null;
    public GameObject Game = null;
    public GameObject MenuMain = null;
    public GameObject MenuOptions = null;
    public GameDifficultySettings difficultySettings = null;

    [Header("Menu Selector Options")]
    public int selectorCountMainMenu = 0;
    public int selectorCountOptionsMenu = 0;
    public GameObject[] MainMenuElements = new GameObject[3];
    public GameObject[] OptionMenuElements = new GameObject[4];

    public bool InOptions = false;

    private Vector3 lastMousePos = new Vector3();


    private bool temp = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 CurrentMouseMovement = lastMousePos - Input.mousePosition;
        lastMousePos = Input.mousePosition;

        string[] controllers = Input.GetJoystickNames();

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] == "Wireless Gamepad")
            {
                if (Input.GetAxis("HorizontalJoyStick") != 0.0f ||
                    Input.GetAxis("VerticalJoyStick") != 0.0f ||
                    Input.GetKeyDown(KeyCode.Joystick1Button0) == true ||
                    Input.GetKeyDown(KeyCode.Joystick1Button1) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton2) == true ||
                    Input.GetKeyDown(KeyCode.Joystick1Button3) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton6) == true ||
                    Input.GetKeyDown(KeyCode.JoystickButton7) == true ||
                    Input.GetAxis("VerticalJoyStickRight") != 0.0f ||
                    Input.GetAxis("HorizontalJoyStickRight") != 0.0f)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else if (CurrentMouseMovement.x > 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

            }
        }

        //- Move Selection -//
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (InOptions == false)
                selectorCountMainMenu = Mathf.Clamp(selectorCountMainMenu + 1, 0, 2);
            else
                selectorCountOptionsMenu = Mathf.Clamp(selectorCountOptionsMenu + 1, 0, 3);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (InOptions == false)
                selectorCountMainMenu = Mathf.Clamp(selectorCountMainMenu - 1, 0, 2);
            else
               selectorCountOptionsMenu = Mathf.Clamp(selectorCountOptionsMenu - 1 , 0, 3);
        }
        //- Select -//
        else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || temp == true)
        {
            if (InOptions == false)
            {
                switch (selectorCountMainMenu)
                {
                    case 0:
                        Game.GetComponent<ResetScriptGame>().ResetGame();
                        Menu.SetActive(false);
                        Game.SetActive(true);
                        temp = false;
                        break;
                    case 1:
                        InOptions = true;
                        MenuMain.SetActive(false);
                        MenuOptions.SetActive(true);
                        break;
                    case 2:
                        Menu.GetComponent<RadarMenu>().ShutDown();
                        break;
                }
            }
            else
            {
                switch (selectorCountOptionsMenu)
                {
                    case 0:
                        Easy();
                        break;
                    case 1:
                        Medium();
                        break;
                    case 2:
                        Hard();
                        break;
                    case 3:
                        InOptions = false;
                        MenuMain.SetActive(true);
                        MenuOptions.SetActive(false);
                        break;
                }
            }
        }

        //Update Seleted
        for (int i = 0; i < MainMenuElements.Length; i++)
        {
            MainMenuElements[i].SetActive(false);
        }
        for (int i = 0; i < OptionMenuElements.Length; i++)
        {
            OptionMenuElements[i].SetActive(false);
        }

        if (InOptions == false)
        {
            MainMenuElements[selectorCountMainMenu].SetActive(true);
        }
        else
        {
            OptionMenuElements[selectorCountOptionsMenu].SetActive(true);
        }
    }

    public void SetMenuSelector(int input)
    {
        InOptions = false;
        if (InOptions == false)
        {
            selectorCountMainMenu = input;
        }
        else
        {
            selectorCountOptionsMenu = input;
        }
    }

    public void SetOptionsSelector(int input)
    {
        InOptions = true;
        if (InOptions == false)
        {
            selectorCountMainMenu = input;
        }
        else
        {
            selectorCountOptionsMenu = input;
        }
    }

    public void Easy()
    {
        difficultySettings.Easy();
        InOptions = false;
        MenuMain.SetActive(true);
        MenuOptions.SetActive(false);
    }

    public void Medium()
    {
        difficultySettings.Normal();
        InOptions = false;
        MenuMain.SetActive(true);
        MenuOptions.SetActive(false);
    }

    public void Hard()
    {
        difficultySettings.Hard();
        InOptions = false;
        MenuMain.SetActive(true);
        MenuOptions.SetActive(false);
    }

    public void updateBool(bool tempIn)
    {
        temp = tempIn;
        Debug.Log("Temp");
    }

}
