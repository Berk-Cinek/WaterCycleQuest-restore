using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField] private Transform pfBullet;
    [SerializeField] private float cooldownTime = 2f; 
    private float lastShootTime = 0f; 

    private void Awake()
    {
        
        GetComponent<PlayerAimWeapon>().OnShoot += PlayerShootProjectiles_OnShoot;
    }

    private void PlayerShootProjectiles_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        Debug.Log("Inside Event");

        if (Time.time - lastShootTime >= cooldownTime)
        {
            Debug.Log("Shoot");

            Transform bulletTransform = Instantiate(pfBullet, e.gunEndPointPosition, Quaternion.identity);

           
            Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;

            
            bulletTransform.GetComponent<Bullet>().Setup(shootDir, moveSpeed: 10f, damage: 10);

            
            lastShootTime = Time.time;
        }
        else
        {
            
            Debug.Log("Cooldown in effect, please wait before shooting again.");
        }
    }
}
