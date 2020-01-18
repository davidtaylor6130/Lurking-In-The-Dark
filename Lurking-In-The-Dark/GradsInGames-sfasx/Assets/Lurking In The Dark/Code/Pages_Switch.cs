using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pages_Switch : MonoBehaviour
{
    [Header("GameObject Page Array")]
    public GameObject[] Pages = null;
    public GameObject[] blankPages = null;

    [Header("PagesCollected")]
    public bool[] CollectedPage = new bool[9];

    public int LookingAtPage = 0;

    void start()
    {
        for (int i = 0; i < CollectedPage.Length; i++)
        {
            CollectedPage[i] = false;
        }
    }

    public void PageFound(int index)
    {
        CollectedPage[index] = true;
        Pages[index].SetActive(false);
    }

    public void DownAPage()
    {
        LookingAtPage--;
        LookingAtPage = Mathf.Clamp(LookingAtPage,0,8);
    }

    public void UpAPage()
    {
        LookingAtPage++;
        LookingAtPage = Mathf.Clamp(LookingAtPage,0,8);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpAPage();
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DownAPage();
        }

        for (int i = 0; i < Pages.Length; i++)
        {
            if (i == LookingAtPage && CollectedPage[i] == true)
            {
                Pages[i].SetActive(true);
            }
            else
            {
                Pages[i].SetActive(false);
            }

            if (i == LookingAtPage && CollectedPage[i] == true)
            {
                blankPages[i].SetActive(false);
            }
            else if (i != LookingAtPage)
            {
                blankPages[i].SetActive(false);
            }
            else
            {
                blankPages[i].SetActive(true);
            }
        }
    }
}
