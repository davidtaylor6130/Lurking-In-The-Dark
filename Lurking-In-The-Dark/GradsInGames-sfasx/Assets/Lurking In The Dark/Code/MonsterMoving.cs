using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoving : MonoBehaviour
{
    //- public var -//
    [Header("Game Objects")]
    public GameObject Player = null;
    public GameObject Monster = null;
    public GameObject NodeMaster = null;

    [Header("Starting weighting mush be the highest")]
    [Tooltip("This has to be higher than the largest weighting of the nodes")]
    public float DistanceToPlayerComparrision = 999.0f;
    [Tooltip("This has to be higher than the largest weighting of the nodes")]
    public float DistanceToMonsterComparrision = 999.0f;


    [Tooltip("This has to be higher than the largest weighting of the nodes")]
    public float DistanceFromPlayerComparrision = 0.0f;
    [Tooltip("This has to be higher than the largest weighting of the nodes")]
    public float DistanceFromMonsterComparrision = 0.0f;

    [Header("Array Of All Nodes")]
    [Tooltip("The Nodes must be a child of the NodeMaster Object")]
    public Node[] Nodes = null;
    [Header("Path Though Nodes")]
    public List<Node> route = null;

    [Header("Settings")]
    public float timeBeforeMove = 30.0f;

    //- private var -//
    [SerializeField] private float SingleNodeMoveTime = 0.5f;
    private float ElapsedTime = 0.0f;

    void Start()
    { 
        //new Node Array created by the amount of nodes
        Nodes = new Node[NodeMaster.transform.childCount];
        route = new List<Node>();
        // the Array is populated by the objects 
        for (int i = 0; i < Nodes.Length; i++)
            Nodes[i] = NodeMaster.transform.GetChild(i).transform.GetComponent<Node>();
    }

    void Update()
    {
        if (route.Count != 0)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
        //        if (route[route.Count - 1].closestToMonster == true)
          //          if (Monster.transform.position == Nodes[i].transform.position)
                //    {
            //            route.Clear();
              //      }

            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            for (int i = 0; i < Nodes.Length; i++)
                Nodes[i].PathWeighting = 0.0f;

            GoTo(GetPath(FindClosest(Monster.transform.position, false), FindClosest(Player.transform.position, true)));
        }
        else
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].PathWeighting = 0.0f;
                Nodes[i].traveled = false;
            }

            FindClosest(Player.transform.position, true);
            FindClosest(Monster.transform.position, false);

            FindFurthest(Player.transform.position, true);
            FindFurthest(Monster.transform.position, false);

            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > timeBeforeMove)
            {
                ElapsedTime = 0.0f;
                if (route.Count == 0)
                    GoTo(GetPath(FindClosest(Monster.transform.position, false), Nodes[Random.Range(0,32)]));
            }
            Debug.Log("RANDOM MOVE_______________________________");

        }
    }

    public List<Node> GetPath(Node StartingNodes, Node EndingNode)
    {
        for (int i = 0; i < route.Count; i++)
        {
            route[i].traveled = false;
        }  
        route.Clear();

        //route = new List<Node>;
        Node FromNode = null;
        Node NextNode = null;
        route.Add(StartingNodes);
        route[route.Count - 1].traveled = true;
        float LowestWeight = 99999999.0f;

        Debug.Log(route.Count - 1);

        while (route[route.Count - 1] != EndingNode)
        {
            for (int i = 0; i < route.Count; i++)
            {
                for (int k = 0; k < route[i].Connections.Length; k++)
                {
                    if (route[i].Connections[k].traveled == false)
                    {
                        //Debug.Log("Node : " + route[i].ToString() + " Weighting : " + route[i].Connections[k].PathWeighting + " Lowest Weighting : " + LowestWeight);
                        if (route[i].Connections[k].PathWeighting <= LowestWeight)
                        {
                            LowestWeight = route[i].Connections[k].PathWeighting;
                            FromNode = route[i];
                            NextNode = route[i].Connections[k];
                        }
                    }
                    else if (route[i].Connections[k].traveled == true)
                    {
                        route[i].Connections[k].PathWeighting += 9999.0f;
                    }
                }
            }
            if (NextNode != null)
            {
                //Debug.Log(NextNode + "NextNode");

                for (int i = 0; i < route.Count; i++)
                {
                    if (route[i] == FromNode && i != (route.Count - 1))
                    {
                        route[i + 1].traveled = false;
                        route.RemoveRange(i+1, route.Count - (i + 1));
                        route.Insert(i + 1, NextNode);
                        Debug.Log("ITS BEEN SMOOTHED");
                        break;
                    }
                    else if (i == (route.Count - 1))
                    {
                        route.Add(NextNode);
                        break;
                    }
                }

                route.Add(NextNode);
                Debug.Log(route.Count - 1);
                route[route.Count - 1].traveled = true;
                LowestWeight = 99999999.0f;
                NextNode = null;
                
            }
            else
            {
                Debug.Log("PATHFINDING ERROR PLEASE REPORT");
                break;
            }

            if (NextNode == EndingNode)
                break;
        }
        return route; 
    }

    public Node FindClosest(Vector3 PositionToCheckAgainst, bool IsItPlayer)
    {
        Node returnNode = null;
        //Loops though all nodes to calculate the distance to player

        if (IsItPlayer)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                //Calculates the distance from the player to node  and makes it positive
                Nodes[i].distanceToPlayer = new Vector3(Mathf.Abs(PositionToCheckAgainst.x - Nodes[i].gameObjectRefrance.transform.position.x),
                    Mathf.Abs(PositionToCheckAgainst.y - Nodes[i].gameObjectRefrance.transform.position.y),
                    Mathf.Abs(PositionToCheckAgainst.z - Nodes[i].gameObjectRefrance.transform.position.z));
                //if the node is the closes to the player the average distance to player
                //this is calculates into a global verable
                if (Nodes[i].closestToPlayer)
                    DistanceToPlayerComparrision = ((Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2);

            }
            //Loops though all nodes to check the distance to see whats the lowest
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].distanceToPlayerAvrage = (Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2;
                // checks the sistance and recalculates the distance to player global verable and toggles the bool 

                if (Nodes[i].distanceToPlayerAvrage <= DistanceToPlayerComparrision)
                {
                    DistanceToPlayerComparrision = ((Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2);
                    Nodes[i].closestToPlayer = true;
                    returnNode = Nodes[i];
                }
                else
                {
                    Nodes[i].closestToPlayer = false;
                }
                Nodes[i].PathWeighting += Nodes[i].distanceToPlayerAvrage;
            }
        }
        else if (IsItPlayer == false)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                //Calculates the distance from the monster to node  and makes it positive
                Nodes[i].distanceToMonster = new Vector3(Mathf.Abs(PositionToCheckAgainst.x - Nodes[i].gameObjectRefrance.transform.position.x),
                    Mathf.Abs(PositionToCheckAgainst.y - Nodes[i].gameObjectRefrance.transform.position.y),
                    Mathf.Abs(PositionToCheckAgainst.z - Nodes[i].gameObjectRefrance.transform.position.z));

                if (Nodes[i].closestToMonster)
                    DistanceToMonsterComparrision = ((Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2);

            }
            //Loops though all nodes to check the distance to see whats the lowest
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].distanceToMonsterAvrage = (Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2;
                // checks the sistance and recalculates the distance to monster global verable and toggles the bool 

                if (Nodes[i].distanceToMonsterAvrage <= DistanceToMonsterComparrision)
                {
                    DistanceToMonsterComparrision = ((Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2);
                    Nodes[i].closestToMonster = true;
                    returnNode = Nodes[i];
                }
                else
                {
                    Nodes[i].closestToMonster = false;
                }
                Nodes[i].PathWeighting += Nodes[i].distanceToMonsterAvrage / 2.0f;
            }
        }
        return returnNode;
    }

    public Node FindFurthest(Vector3 PositionToCheckAgainst, bool IsItPlayer)
    {
        Node returnNode = null;
        //Loops though all nodes to calculate the distance to player

        if (IsItPlayer)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                //Calculates the distance from the player to node  and makes it positive
                Nodes[i].distanceToPlayer = new Vector3(Mathf.Abs(PositionToCheckAgainst.x - Nodes[i].gameObjectRefrance.transform.position.x),
                    Mathf.Abs(PositionToCheckAgainst.y - Nodes[i].gameObjectRefrance.transform.position.y),
                    Mathf.Abs(PositionToCheckAgainst.z - Nodes[i].gameObjectRefrance.transform.position.z));
                //if the node is the closes to the player the average distance to player
                //this is calculates into a global verable
                if (Nodes[i].FurthestToPlayer)
                    DistanceFromPlayerComparrision = ((Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2);

            }
            //Loops though all nodes to check the distance to see whats the lowest
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].distanceToPlayerAvrage = (Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2;
                // checks the sistance and recalculates the distance to player global verable and toggles the bool 

                if (Nodes[i].distanceToPlayerAvrage >= DistanceFromPlayerComparrision)
                {
                    DistanceFromPlayerComparrision = ((Nodes[i].distanceToPlayer.x + Nodes[i].distanceToPlayer.z) / 2);
                    Nodes[i].FurthestToPlayer = true;
                    returnNode = Nodes[i];
                }
                else
                {
                    Nodes[i].FurthestToPlayer = false;
                }
                //Nodes[i].PathWeighting += Nodes[i].distanceToPlayerAvrage;
            }
        }
        else if (IsItPlayer == false)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                //Calculates the distance from the monster to node  and makes it positive
                Nodes[i].distanceToMonster = new Vector3(Mathf.Abs(PositionToCheckAgainst.x - Nodes[i].gameObjectRefrance.transform.position.x),
                    Mathf.Abs(PositionToCheckAgainst.y - Nodes[i].gameObjectRefrance.transform.position.y),
                    Mathf.Abs(PositionToCheckAgainst.z - Nodes[i].gameObjectRefrance.transform.position.z));

                if (Nodes[i].FurthestToMonster)
                    DistanceFromMonsterComparrision = ((Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2);

            }
            //Loops though all nodes to check the distance to see whats the lowest
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].distanceToMonsterAvrage = (Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2;
                // checks the sistance and recalculates the distance to monster global verable and toggles the bool 

                if (Nodes[i].distanceToMonsterAvrage >= DistanceFromMonsterComparrision)
                {
                    DistanceFromMonsterComparrision = ((Nodes[i].distanceToMonster.x + Nodes[i].distanceToMonster.z) / 2);
                    Nodes[i].FurthestToMonster = true;
                    returnNode = Nodes[i];
                }
                else
                {
                    Nodes[i].FurthestToMonster = false;
                }
                //Nodes[i].PathWeighting += Nodes[i].distanceToMonsterAvrage / 2.0f;
            }
        }
        return returnNode;
    }

    private IEnumerator DoMove(Vector3 position, Vector3 destination)
    {
        // Move between the two specified positions over the specified amount of time
        if (position != destination)
        {
            transform.rotation = Quaternion.LookRotation(destination - position, Vector3.up);

            Vector3 p = transform.position;
            float t = 0.0f;

            while (t < SingleNodeMoveTime)
            {
                t += (Time.deltaTime * 0.25f);
                p = Vector3.Lerp(position, destination, t / SingleNodeMoveTime);
                transform.position = p;
                yield return null;
            }
        }
    }

    private IEnumerator DoGoTo(List<Node> route)
    {

        Vector3 CurrentPosition = Monster.transform.position;
        // Move through each Node in the given route
        if (route != null)
        {
            for (int count = 0; count < route.Count; ++count)
            {
                Vector3 next = route[count].transform.position;
                yield return DoMove(Monster.transform.position, next);
            }
        }
    }

    public void GoTo(List<Node> route)
    {
        // Clear all coroutines before starting the new route so 
        // that clicks can interupt any current route animation
        StopAllCoroutines();
        StartCoroutine(DoGoTo(route));
    }
} 
