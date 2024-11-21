using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform playerTransform;
    public void PlacePlayer()
    {
        // Oyuncuyu merkeze yerleştir
        Instantiate(player, new Vector3(playerTransform.position.x, playerTransform.position.y, 0), Quaternion.identity);
    }
}
