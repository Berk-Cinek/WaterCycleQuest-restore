using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float speed = 2f;            // Movement speed of the monster
    public float pathUpdateInterval = 1f; // Time interval to recalculate the path
    public float targetReachThreshold = 0.1f; // Distance to consider a node "reached"

    private Transform player;          // Reference to the player
    private Pathfinding pathfinding;   // Reference to the pathfinding system
    private List<PathNode> currentPath; // Current path to the player
    private int currentPathIndex;      // Index for tracking the path
    private float pathUpdateTimer;     // Timer for path recalculation

    void Start()
    {
        // Manually create an instance of Pathfinding with grid dimensions
        pathfinding = new Pathfinding(100, 100); 

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has a 'Player' tag.");
            return;
        }

        // Initialize variables
        currentPath = null;
        currentPathIndex = 0;
        pathUpdateTimer = pathUpdateInterval;
    }
    void Update()
    {
        if (player == null || pathfinding == null)
            return;

        // Update path periodically
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0f)
        {
            UpdatePath();
            pathUpdateTimer = pathUpdateInterval; // Reset the timer
        }

        // Follow the path
        FollowPath();
    }

    void UpdatePath()
    {
        // Get the monster's current grid position
        Vector3 monsterWorldPosition = transform.position;
        pathfinding.GetGrid().GetXY(monsterWorldPosition, out int monsterX, out int monsterY);

        // Get the player's current grid position
        Vector3 playerWorldPosition = player.position;
        pathfinding.GetGrid().GetXY(playerWorldPosition, out int playerX, out int playerY);

        // Calculate the path to the player
        currentPath = pathfinding.FindPath(monsterX, monsterY, playerX, playerY);
        currentPathIndex = 0; // Reset the path index

        // Debug log the path length
        if (currentPath != null)
        {
            Debug.Log($"Path updated. Path length: {currentPath.Count}");
        }
        else
        {
            Debug.Log("No path found to the player.");
        }
    }

    void FollowPath()
    {
        // If no path or path is completed, stop movement
        if (currentPath == null || currentPathIndex >= currentPath.Count)
            return;

        // Get the target node in the path
        PathNode targetNode = currentPath[currentPathIndex];
        Vector3 targetWorldPosition = new Vector3(
            targetNode.x * pathfinding.GetGrid().GetCellSize(),
            targetNode.y * pathfinding.GetGrid().GetCellSize(),
            0
        );

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, speed * Time.deltaTime);

        // Check if the monster reached the target node
        if (Vector3.Distance(transform.position, targetWorldPosition) < targetReachThreshold)
        {
            currentPathIndex++; // Move to the next node in the path
        }
    }
}

