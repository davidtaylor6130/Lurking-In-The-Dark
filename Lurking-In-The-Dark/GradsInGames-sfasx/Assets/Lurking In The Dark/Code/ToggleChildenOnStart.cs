using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildenOnStart : MonoBehaviour
{
    //- this just keeps the ai nodes out the way but allows me to quickly reanable them -//
    [Header("Setting's Toggles")]
    public bool DebugMode = false;
    public GameObject PerantObject = null;

    [Header("ToggleOnStart")]
    public bool Showing = false;
    public GameObject self = null;

    void Start()
    {
        if (DebugMode == false)
        {
            if (self != null)
                self.SetActive(Showing);

            if (PerantObject != null)
                for (int i = 0; i < transform.childCount; i++)
                    PerantObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
