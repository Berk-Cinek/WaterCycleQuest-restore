using System.Collections;
using System.Linq;
using UnityEngine;

public class FreezeSkill : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float skillCooldown = 60f;
    private bool canActivateSkill = true;
    private float skillCooldownTimer = 0f;

    private SkillBarUI skillBarUI;

    private void Awake()
    {
        skillBarUI = FindObjectOfType<SkillBarUI>();
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

        if (Input.GetKeyDown(KeyCode.Alpha3) && canActivateSkill)
        {
            ActivateSkill();
        }
    }

    private void UpdateCooldownUI()
    {
        if (skillBarUI != null)
        {
            float normalizedTime = 1 - (skillCooldownTimer / skillCooldown);
            skillBarUI.UpdateFreezeCooldown(normalizedTime);
        }
    }

    private void ActivateSkill()
    {
        Debug.Log("Freeze skill activated!");
        StartCoroutine(FreezeEnemiesCoroutine());
        canActivateSkill = false;
        skillCooldownTimer = skillCooldown;
    }

    private IEnumerator FreezeEnemiesCoroutine()
    {
        IFreezeable[] enemies = FindObjectsOfType<MonoBehaviour>().OfType<IFreezeable>().ToArray();

        foreach (var enemy in enemies)
        {
            enemy.SetFrozen(true);
        }

        Debug.Log("Enemies frozen.");

        yield return new WaitForSeconds(freezeDuration);

        foreach (var enemy in enemies)
        {
            enemy.SetFrozen(false);
        }

        Debug.Log("Enemies unfrozen.");
    }
}
