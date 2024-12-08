using System.Collections;
using UnityEngine;

public class BulletFrenzySkill : MonoBehaviour
{
    [SerializeField] private float shootingCooldownReductionDuration = 10f;
    [SerializeField] private float skillCooldown = 60f;

    private bool canActivateSkill = true;
    private float skillCooldownTimer = 0f;
    private SkillBarUI skillBarUI;
    private ShootProjectiles shootProjectiles;

    private void Awake()
    {
        shootProjectiles = GetComponent<ShootProjectiles>();
        skillBarUI = FindObjectOfType<SkillBarUI>();

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
            }
        }

        UpdateCooldownUI();

        if (Input.GetKeyDown(KeyCode.Alpha2) && canActivateSkill)
        {
            ActivateSkill();
        }
    }

    private void UpdateCooldownUI()
    {
        if (skillBarUI != null)
        {
            float normalizedTime = 1 - (skillCooldownTimer / skillCooldown);
            skillBarUI.UpdateBulletFrenzyCooldown(normalizedTime);
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
        Debug.Log("Bullet Frenzy ended.");
    }
}
