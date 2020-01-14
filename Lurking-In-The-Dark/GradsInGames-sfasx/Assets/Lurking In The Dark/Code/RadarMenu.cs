using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarMenu : MonoBehaviour
{
    [Header("View Circle Menu")]
    public float Width = 1.0f;
    public float radius = 1.0f;
    public float FadeAmount = 1.0f;

    [Header("Game Objects")]
    public GameObject player;
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        //- setting radar effect for the menu -//
        mat.SetFloat("_PlayerWidth", Width);
        mat.SetVector("_PlayerPos", player.transform.position);
        mat.SetFloat("_PlayerRadius", radius);
        mat.SetFloat("_FadeAmmount", FadeAmount);
        //- reseting shader effect -// 

        mat.SetFloat("Time", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        mat.SetFloat("_PlayerWidth", Width);
        mat.SetVector("_PlayerPos", player.transform.position);
        mat.SetFloat("_PlayerRadius", radius);
        mat.SetFloat("_FadeAmmount", FadeAmount);

        mat.SetFloat("Time", 0);
    }

    public void ShutDown()
    {
        Application.Quit();
    }
}
