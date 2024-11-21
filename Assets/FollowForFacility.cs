using Cinemachine;
using System.Collections;
using UnityEngine;

public class FollowForFacility : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;

    void Start()
    {
        StartCoroutine(FindAndFollowPlayer());
    }

    private IEnumerator FindAndFollowPlayer()
    {
        GameObject player = null;

        // Oyuncuyu bulana kadar bekle
        while (player == null)
        {
            player = GameObject.Find("Player(Clone)");
            yield return null; // Bir sonraki frame'e kadar bekle
        }

        Debug.Log("Player found!");
        _cam = GetComponent<CinemachineVirtualCamera>();
        _cam.Follow = player.transform;
    }
}
