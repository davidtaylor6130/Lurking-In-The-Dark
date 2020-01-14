using UnityEngine;

public class MusicEffects : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Player;
    public float maxSound = 0.25f;
    public float deadZoneRadius = 0.0f;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float volume;
        // The code calulates the distance between the two positions of the monster/ the player then it converts any output to positive and then it avrages it.
        volume = ((Mathf.Abs(Monster.transform.position.x - Player.transform.position.x) - deadZoneRadius + 
            Mathf.Abs(Monster.transform.position.z - Player.transform.position.z) - deadZoneRadius) / 100.0f);
        // The volume is set with a limit from the users set maxSound to 0 using a mathf command
        music.volume = Mathf.Clamp(volume, 0, maxSound);
    }
}
