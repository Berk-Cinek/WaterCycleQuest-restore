using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    private Vector3 startPosition;
    private float moveSpeed;
    private int damage;
    private float range;

    public void Setup(Vector3 shootDir, float moveSpeed = 100f, int damage = 10, float range = 10f)
    {
        this.shootDir = shootDir;
        this.moveSpeed = moveSpeed;
        this.damage = damage;
        this.range = range;

        startPosition = transform.position; 
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
    }

    private void Update()
    {
        
        transform.position += shootDir * moveSpeed * Time.deltaTime;

        
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Target target = collider.GetComponent<Target>();
        if (target != null)
        {
            target.Damage(damage); 
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("Bullet hit something else: " + collider.name); 
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return n < 0 ? n + 360 : n;
    }
}


