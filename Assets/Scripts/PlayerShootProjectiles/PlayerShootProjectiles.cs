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
        NewPlayerMovement playerMovement = GetComponent<NewPlayerMovement>();
        if (playerMovement.DialogueUI.IsOpen)
        {
            Debug.Log("Cannot shoot while dialogue is open.");
            return;
        }

        Debug.Log("Inside Event");

        if (Time.time - lastShootTime >= cooldownTime)
        {
            Debug.Log("Shoot");

            Transform bulletTransform = Instantiate(pfBullet, e.gunEndPointPosition, Quaternion.identity);

            Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;

            int playerDamage = playerMovement.GetDamage();

            bulletTransform.GetComponent<Bullet>().Setup(shootDir, moveSpeed: 10f, damage: playerDamage);

            lastShootTime = Time.time;
        }
        else
        {
            Debug.Log("Cooldown in effect, please wait before shooting again.");
        }
    }

    public void SetShootingCooldown(float newCooldownTime)
    {
        cooldownTime = newCooldownTime;
        Debug.Log($"Shooting cooldown is now: {cooldownTime} seconds.");
    }
}
