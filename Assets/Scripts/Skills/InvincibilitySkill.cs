using System.Collections;
using UnityEngine;

public class InvincibilitySkill : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 5f;
    [SerializeField] private float cooldownTime = 60f;

    private bool isInvincible = false;
    private bool canActivate = true;
    private float cooldownTimer = 0f;

    private NewPlayerMovement player;
    private SkillBarUI skillBarUI;

    private void Awake()
    {
        player = GetComponent<NewPlayerMovement>();
        skillBarUI = FindObjectOfType<SkillBarUI>();

        if (player == null)
        {
            Debug.LogError("InvincibilitySkill requires a NewPlayerMovement component!");
        }
    }

    private void Update()
    {
        if (!canActivate)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                canActivate = true;
            }
        }

        UpdateCooldownUI();

        if (Input.GetKeyDown(KeyCode.Alpha1) && canActivate)
        {
            ActivateSkill();
        }
    }

    private void UpdateCooldownUI()
    {
        if (skillBarUI != null)
        {
            float normalizedTime = 1 - (cooldownTimer / cooldownTime);
            skillBarUI.UpdateInvincibilityCooldown(normalizedTime);
        }
    }

    private void ActivateSkill()
    {
        Debug.Log("Invincibility skill activated!");
        StartCoroutine(InvincibilityCoroutine());
        canActivate = false;
        cooldownTimer = cooldownTime;
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        player.SetInvincibility(true);
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        player.SetInvincibility(false);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

}
