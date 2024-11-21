using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;
    private float cellSize = 1f;

    void Start()
    {
        pathfinding = new Pathfinding(100, 100);
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
                    new Vector3(path[i].x * cellSize, path[i].y * cellSize, 0),
                    new Vector3(path[i + 1].x * cellSize, path[i + 1].y * cellSize, 0),
                    Color.green, 2f
                    );
                }

            }
        }
    }
}
