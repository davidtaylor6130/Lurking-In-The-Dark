using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Input;
using System.Drawing;

public class LookingAtPlayer : MonoBehaviour
{
    [Header("Monster Objects")]
    public bool PlayerSpotted = false;
    public GameObject monster = null;
    public GameObject monsterObject = null;

    [Header("Player Objects")]
    public GameObject Camera = null;
    public GameObject Player = null;
    public GameObject PlayerObject = null;
    public GameObject monsterSnapToObject = null;

    [Header("Viewing Range")]
    [Range(1, 5)]
    public float viewingRange = 0.0f;

    [Header("Monster Audio")]
    public AudioSource damageAudioSource = null;
    public AudioSource baseMonsterNoise = null;

    [Header("Player Audio")]
    public AudioSource playerDeath = null;
    public AudioSource heavyBreathing = null;

    [Header("Monster Eye's")]
    public Material BlueEyes = null;
    public Material RedEyes = null;
    public GameObject[] Eyes = new GameObject[5];
    public int DamageCount = 0;

    [Header("Monster Eating Settings")]
    public Animator eating = null;
    public Animator blinking = null;
    public Animator blinking2 = null;

    public float speedOfCamera = 1.0f;
    public GameObject finalCameraPosition = null;
    public FpsMovement PlayerMovementDisable = null;

    [Header("Menu/Game")]
    public GameObject menu = null;
    public GameObject game = null;

    //- private verables -//
    private RadarEffect Radar = null;
    private MonsterMoving MonsterMoving = null;
    private bool alreadyClicked = false;
    private bool IsCursorVisable = false;


    //alreadyClicked Prevents AnyChance of double clicks on the mouse

    void Start()
    {
        Radar = Player.GetComponent<RadarEffect>();
        MonsterMoving = monster.GetComponent<MonsterMoving>();
        // sets scripts to allow me to acsess them at a later time
        IsCursorVisable = false;
    }

    void OnTriggerStay(Collider objectCollided)
    {
        if (objectCollided.gameObject.name == "Player")
        {
            // can the monster see the player (Holding breath mechanic)
            if (Radar.breathing < viewingRange && DamageCount != Eyes.Length && damageAudioSource.isPlaying == false)
            {
                // if statement prevents music overlapping
                if (!playerDeath.isPlaying && PlayerSpotted == false)
                {
                    heavyBreathing.Stop();
                    playerDeath.Play();
                    monsterObject.transform.rotation = Quaternion.LookRotation(-Vector3.Normalize(monsterObject.transform.position - Camera.transform.position), Vector3.up);
                }

                Camera.transform.LookAt(Eyes[4].transform);

                blinking.SetBool("Dead", true);
                blinking2.SetBool("Dead", true);
                eating.SetBool("Dead", true);

                PlayerSpotted = true;
                // sets new lookrotation vector then its applyed to the monster

                Camera.gameObject.transform.position = Vector3.Lerp(Camera.gameObject.transform.position, finalCameraPosition.transform.position, speedOfCamera);
                PlayerMovementDisable.enabled = false;
                MonsterMoving.enabled = false;
                MonsterMoving.StopMonster();

                if (!playerDeath.isPlaying)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    menu.SetActive(true);
                    game.SetActive(false);
                }
            }
            else
            {
                //Checks if the monster cant see the player and if the large pulse is activated
                if (Input.GetKeyUp(KeyCode.Mouse1) && alreadyClicked == false || Input.GetKeyUp(KeyCode.Joystick1Button7) && alreadyClicked == false)
                {
                    Eyes[DamageCount].GetComponent<Renderer>().material = BlueEyes;
                    alreadyClicked = true;
                    
                    //Noises
                    baseMonsterNoise.Stop();
                    damageAudioSource.Play();

                    //sets new lookrotation vector then is applyed to the monster
                    monsterObject.transform.LookAt(Camera.transform);

                    //Increases Damage count thats used to acsess eyes and keep chat
                    DamageCount++;
                    if (DamageCount == Eyes.Length)
                    {
                        Radar.breathing = 0.0f;
                        Radar.Animations = true;
                        //Get Monster Into Punching Position
                        monsterObject.transform.position = monsterSnapToObject.transform.position;

                        //Pause Player Moving Script
                        PlayerMovementDisable.enabled = false;

                        //Pause Monster Moving Script
                        MonsterMoving.enabled = false;

                        Camera.transform.LookAt(Eyes[4].transform);

                        monsterObject.transform.LookAt(Camera.transform);

                        //Play the PunchMonster animation
                        PlayerObject.GetComponent<Animator>().SetBool("Walking", false);
                        PlayerObject.GetComponent<Animator>().SetBool("Idle", false);
                        PlayerObject.GetComponent<Animator>().SetBool("Jumping", false);
                        PlayerObject.GetComponent<Animator>().SetBool("MonsterDeath", true);
                        blinking.SetBool("Dead", true);
                        blinking2.SetBool("Dead", true);
                    }
                    else
                    {
                        //Talk to the monster moving script and uses fucntion to create a path 
                        //This path is then executed
                        MonsterMoving.GoTo(MonsterMoving.GetPath(MonsterMoving.FindClosest(MonsterMoving.Monster.transform.position, false), MonsterMoving.FindFurthest(MonsterMoving.Player.transform.position, true)));
                    }
                }
                else if (!damageAudioSource.isPlaying && DamageCount == Eyes.Length)
                {
                    IsCursorVisable = true;
                    Cursor.lockState = CursorLockMode.None;
                    Radar.Animations = false;
                    menu.SetActive(true);
                    game.SetActive(false);
                }
                else
                {
                    alreadyClicked = false;
                }
            }
        }
        Cursor.visible = IsCursorVisable;
    }
}
