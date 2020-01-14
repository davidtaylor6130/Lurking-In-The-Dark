using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePickUp : MonoBehaviour
{

    [Header("Game Object To Collide")]
    public GameObject Player = null;
    public GameObject PageUiElement = null;
    public GameObject self = null;
    public int pageNumber = 1;


    void OnTriggerEnter(Collider objectCollided)
    {
        Debug.Log("Trigger " + objectCollided.gameObject.name);
        if (objectCollided.gameObject.name == "Player")
        {
            PageUiElement.GetComponent<Pages_Switch>().PageFound(pageNumber);
            self.SetActive(false);
        }
    }
}
