using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour
{
    public bool closestToPlayer;
    public bool closestToMonster;

    public bool FurthestToPlayer;
    public bool FurthestToMonster;

    public GameObject gameObjectRefrance;

    public Vector3 distanceToPlayer;
    public Vector3 distanceToMonster;
    public float distanceToPlayerAvrage = 0.0f;
    public float distanceToMonsterAvrage = 0.0f;

    public Node[] Connections = new Node[4];
    public float PathWeighting = 0.0f;
    public bool traveled = false;

    public Node(GameObject ChildNode, bool isItCloses)
    {
        closestToPlayer = isItCloses;
        gameObjectRefrance = ChildNode;
    }
}
