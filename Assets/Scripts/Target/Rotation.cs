using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform firingPoint;
    public float fireRate;
    private float timeToFire;
    public float rotateSpeed = 0.0025f;
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            GetTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

}
