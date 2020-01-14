using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarEffect : MonoBehaviour
{
    //-------------------------- public verables-------------------------//
    [Header("Over Rides")]
    public bool Animations = false;

    [Header("GameObjects")]
    public GameObject player;
    public GameObject monster;
    public Material mat;

    [Header("Large Pulse Settings")]
    public float speed = 10.0f;
    public float width = 1.0f;
    [Range(0, 100)]
    public float fadeTime = 0.0f;
    public AudioSource soundEffect = null;

    [Header("Viewing Circle Settings")]
    public float playerWidth = 1.0f;
    public float playerRadius = 1.0f;
    [Range(0, 100)]
    public float playerFadeAmmount = 3.0f;

    [Header("Breathing In Settings")]
    public float breathing = 0.0f;
    public bool breathingIn = true;
    public float breathingSpeed = 1.0f;
    public AudioSource breathingInSound = null;
    public AudioSource breathingOutSound = null;


    [Header("Mosnter Pulse Settings")]
    public float monsterWidth = 1.0f;
    public float monsterRadius = 1.0f;

    //-------------------------- private verables -----------------------//
    private float radius = 9999.0f;
    private float time = 1.0f;
    private bool InUi = false;


    void Start()
    {
        mat.SetVector("_PlayerPos", player.transform.position);
        mat.SetFloat("_PlayerRadius", playerRadius);
        mat.SetFloat("_FadeAmmount", playerFadeAmmount);
        breathing = 0.0f;
    }
    //-------------------------- UpdateMethod ---------------------------//
    void Update()
    {
        if (InUi == false)
        {

            
            if (Input.GetKey(KeyCode.Mouse0) && InUi == false|| Input.GetKey(KeyCode.Joystick1Button6) && InUi == false || breathingIn == true)
            {
                if (Animations == false)
                {
                    if (breathingIn == false)
                    {
                        breathingOutSound.Stop();
                        breathingInSound.PlayDelayed(0.1f);
                    }
                    breathingIn = true;
                    breathing += Time.deltaTime * breathingSpeed;
                    breathing = Mathf.Clamp(breathing, 0, playerRadius / 2);
                }
                mat.SetFloat("_PlayerRadius", playerRadius - breathing);
                mat.SetFloat("_FadeAmmount", Mathf.Clamp(playerFadeAmmount - breathing,0, playerRadius/2));
            }

            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Joystick1Button6))
            {
                if (Animations == false)
                {
                    if (breathingIn == true)
                    {
                        breathingInSound.Stop();
                        breathingOutSound.Play();
                    }
                    breathingIn = false;
                }
                mat.SetFloat("_PlayerRadius", playerRadius);
                mat.SetFloat("_FadeAmmount", playerFadeAmmount);
                breathing = 0.0f;
            }
        }
        else
        {
            breathingIn = false;
            breathingInSound.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            soundEffect.Play();
            //- Setting the width and center of the large pulse -//
            mat.SetFloat("_Width", width);
            mat.SetVector("_Center", player.transform.position);


            //- Setting the width and center of the monster pulse -//
            mat.SetFloat("_MonsterWidth",monsterWidth);
            mat.SetVector("_MonsterPos", monster.transform.position);

            //- resets the radius by line index -//
            radius = 0.0f;
            time = 1.0f;
        }
        else
        {
            mat.SetFloat("_PlayerWidth", playerWidth);
            mat.SetVector("_PlayerPos", player.transform.position);
        }
        //- updates all the radius's -//
        radius += speed * Time.deltaTime;
        monsterRadius += speed * Time.deltaTime;
        time -= Time.deltaTime / fadeTime;

        mat.SetFloat("_Radius", radius);
        mat.SetFloat("Time", time);
        mat.SetFloat("_MonsterRadius", monsterRadius);
    }

    public void UI()
    {
        if (InUi == false)
            InUi = true;
        else if (InUi == true)
            InUi = false;
        breathingInSound.Pause();
        breathingOutSound.Pause();
    }

    public void SetPos()
    {
        mat.SetVector("_PlayerPos", player.transform.position);
        mat.SetVector("_Center", player.transform.position);
        breathing = 0;
        breathingIn = false;
        mat.SetFloat("_PlayerRadius", playerRadius);
        mat.SetFloat("_FadeAmmount", playerFadeAmmount);
        monsterRadius = 0.0f;
        mat.SetFloat("_MonsterRadius", monsterRadius);
        mat.SetFloat("_MonsterWidth", 0);
    }

}
