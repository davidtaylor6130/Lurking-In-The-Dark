using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficultySettings : MonoBehaviour
{
    [Header("Script")]
    public MonsterMoving monsterMoving = null;
    public RadarEffect radarEffect = null;

    [Header("Easy")]
    public float MonsterMovementSpeedEasy = 0.0f;
    public float WidthEasy = 0.0f;
    public float RadiusEasy = 0.0f;
    public float FadeEasy = 0.0f;

    [Header("Normal")]
    public float MonsterMovementSpeedNormal = 0.0f;
    public float WidthNormal = 0.0f;
    public float RadiusNormal = 0.0f;
    public float FadeNormal = 0.0f;

    [Header("Hard")]
    public float MonsterMovementSpeedHard = 0.0f;
    public float WidthHard = 0.0f;
    public float RadiusHard = 0.0f;
    public float FadeHard = 0.0f;

    private difficultyRating difficulty = difficultyRating.easy ;

    private enum difficultyRating
    {
        easy,
        normal,
        hard
    }

    public void Easy()
    {
        monsterMoving.speed = MonsterMovementSpeedEasy;
        radarEffect.playerFadeAmmount = FadeEasy;
        radarEffect.playerRadius = RadiusEasy;
        radarEffect.playerWidth = WidthEasy;
        difficulty = difficultyRating.easy;
    }

    public void Normal()
    {
        monsterMoving.speed = MonsterMovementSpeedNormal;
        radarEffect.playerFadeAmmount = FadeNormal;
        radarEffect.playerRadius = RadiusNormal;
        radarEffect.playerWidth = WidthNormal;
        difficulty = difficultyRating.normal;
    }

    public void Hard()
    {
        monsterMoving.speed = MonsterMovementSpeedHard;
        radarEffect.playerFadeAmmount = FadeHard;
        radarEffect.playerRadius = RadiusHard;
        radarEffect.playerWidth = WidthHard;
        difficulty = difficultyRating.hard;
    }

    public void Reset()
    {
        switch (difficulty)
        {
            case difficultyRating.easy:
                Easy();
                break;
            case difficultyRating.normal:
                Normal();
                break;
            case difficultyRating.hard:
                Hard();
                break;
        }
    }

}
