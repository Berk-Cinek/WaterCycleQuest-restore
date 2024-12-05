using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 2f; // Movement speed of the enemy
    [SerializeField] private float stoppingDistance = 0.2f; // Distance at which the enemy stops moving towards the player

    private Pathfinding pathfinding;
    private List<PathNode> currentPath;

    private void Start()
    {
     Pathfinding pathfinding = new Pathfinding(100, 100);
    }

    private void FixedUpdate()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform.position;

        int enemyX = Convert.ToInt32(enemyPosition.x);
        int enemyY = Convert.ToInt32(enemyPosition.y);
        int playerX = Convert.ToInt32(playerPosition.x);
        int playerY = Convert.ToInt32(playerPosition.y);

        // Calculate path if the enemy is far enough from the player
        if (Vector3.Distance(playerPosition, enemyPosition) > stoppingDistance)
        {
            currentPath = pathfinding.FindPath(enemyX, enemyY, playerX, playerY);

            // Move the enemy along the calculated path
            if (currentPath != null && currentPath.Count > 1)
            {
                PathNode nextNode = currentPath[1]; // First node is the current position
                Vector3 nextPosition = new Vector3(nextNode.x, nextNode.y) * pathfinding.GetGrid().GetCellSize();
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, enemySpeed * Time.deltaTime);
            }
        }
    }
}

