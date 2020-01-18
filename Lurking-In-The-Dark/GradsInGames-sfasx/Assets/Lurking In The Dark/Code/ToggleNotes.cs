using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNotes : MonoBehaviour
{
    public GameObject ObjectToToggle = null;
    public void ToggleFunction()
    {
        if (ObjectToToggle.activeSelf == false)
            ObjectToToggle.SetActive(true);
        else
            ObjectToToggle.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleFunction();
        }
    }
}
