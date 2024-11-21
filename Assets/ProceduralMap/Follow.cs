using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private PrefabGenerator prefabGenerator;

    private CinemachineVirtualCamera _cam;
    void Start()
    {
        GameObject _player = prefabGenerator.PlacePlayer();
        _cam = GetComponent<CinemachineVirtualCamera>();
        _cam.Follow = _player.transform;
    }


}
