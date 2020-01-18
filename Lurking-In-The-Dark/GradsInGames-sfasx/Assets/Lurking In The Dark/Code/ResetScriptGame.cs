using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScriptGame : MonoBehaviour
{
    [Header("Reset Player Settings")]
    public GameObject Player = null;
    public Vector3 playerStartingPos;
    public GameObject Camera = null;
    public Vector3 cameraStartingPos;

    [Header("Reset Monster Settings")]
    public GameObject Monster = null;
    public Vector3 mosnterStartingPos;

    [Header("Other Reset Settings")]
    public GameObject[] PagesInGame = new GameObject[9];
    public Pages_Switch UiPages = null;

    [Header("Radar Effect Reset")]
    public float radius = 0.0f;
    public float fadeAmmount = 0.0f;
    public float width = 0.0f;

    public void ResetGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // if needed reset the movement components
        if (!Player.GetComponent<FpsMovement>().isActiveAndEnabled)
            Player.GetComponent<FpsMovement>().enabled = true;
        if (!Monster.GetComponent<MonsterMoving>().isActiveAndEnabled)
            Monster.GetComponent<MonsterMoving>().enabled = true;

        // Reset the radius Effects
        Player.GetComponent<RadarEffect>().playerRadius = radius;
        Player.GetComponent<RadarEffect>().playerFadeAmmount = fadeAmmount;
        Player.GetComponent<RadarEffect>().playerWidth = width;
        Player.GetComponent<RadarEffect>().SetPos();

        //Reset the Looking At Scripts
        Monster.GetComponent<LookingAtPlayer>().PlayerSpotted = false;
        Monster.GetComponent<LookingAtPlayer>().DamageCount = 0;

        //Reset All Objects In the Scene
        for (int i = 0; i < Monster.GetComponent<LookingAtPlayer>().Eyes.Length; i++)
        {
            Monster.GetComponent<LookingAtPlayer>().Eyes[i].GetComponent<Renderer>().material = Monster.GetComponent<LookingAtPlayer>().RedEyes;
        }
        Camera.transform.localPosition = cameraStartingPos;
        Camera.transform.localEulerAngles = new Vector3(0, 0, 0);
        Player.transform.localPosition = playerStartingPos;
        Monster.transform.localPosition = mosnterStartingPos;
        for (int i = 0; i < PagesInGame.Length; i++)
        {
            UiPages.CollectedPage[i] = false;
            PagesInGame[i].SetActive(true);
        }
        UiPages.CollectedPage[0] = true;
    }
}
