using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;


    void Start()
    {
        Pathfinding pathfinding = new Pathfinding(100, 100);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mousWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null){
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(
                        new Vector3(path[i].x, path[i].y) * 5f,  // Scale the node's position
                        new Vector3(path[i + 1].x, path[i + 1].y) * 5f,  // Scale the next node's position
                        Color.green
                    );
                }

            }
        }
    }
}
