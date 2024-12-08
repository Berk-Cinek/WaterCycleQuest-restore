using System.Collections;
using UnityEngine;

public class BulletFrenzySkill : MonoBehaviour
{
    [SerializeField] private float shootingCooldownReductionDuration = 10f; 
    [SerializeField] private float skillCooldown = 60f; 
    private bool canActivateSkill = true;
    private float skillCooldownTimer = 0f;

    private ShootProjectiles shootProjectiles;

    private void Awake()
    {
        shootProjectiles = GetComponent<ShootProjectiles>();
        if (shootProjectiles == null)
        {
            Debug.LogError("BulletFrenzySkill requires a ShootProjectiles component!");
        }
    }

    private void Update()
    {
        
        if (!canActivateSkill)
        {
            skillCooldownTimer -= Time.deltaTime;
            if (skillCooldownTimer <= 0f)
            {
                skillCooldownTimer = 0;
                canActivateSkill = true;
                Debug.Log("You can now activate Bullet Frenzy again!");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            if (canActivateSkill)
            {
                ActivateSkill();
            }
            else
            {
                Debug.Log($"Bullet Frenzy is in cooldown! Time left: {Mathf.Ceil(skillCooldownTimer)} seconds.");
            }
        }
    }

    private void ActivateSkill()
    {
        Debug.Log("Bullet Frenzy skill activated!");
        StartCoroutine(BulletFrenzyCoroutine());
        canActivateSkill = false;
        skillCooldownTimer = skillCooldown;
    }

    private IEnumerator BulletFrenzyCoroutine()
    {
        
        Debug.Log("Activating reduced shooting cooldown!");
        shootProjectiles.SetShootingCooldown(0.1f);

        yield return new WaitForSeconds(shootingCooldownReductionDuration);

        
        shootProjectiles.SetShootingCooldown(2f);
        Debug.Log("Bullet Frenzy ended. Shooting cooldown reset to 2 seconds.");
    }
}
