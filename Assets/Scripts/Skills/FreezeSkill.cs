using System.Collections;
using System.Linq;
using UnityEngine;

public class FreezeSkill : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float skillCooldown = 60f;
    private bool canActivateSkill = true;
    private float skillCooldownTimer = 0f;

    private void Update()
    {
        if (!canActivateSkill)
        {
            skillCooldownTimer -= Time.deltaTime;
            if (skillCooldownTimer <= 0f)
            {
                skillCooldownTimer = 0;
                canActivateSkill = true;
                Debug.Log("Freeze skill cooldown complete. Skill is ready to use.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (canActivateSkill)
            {
                ActivateSkill();
            }
            else
            {
                Debug.Log($"Cannot activate Freeze skill yet. Still in cooldown. Remaining time: {skillCooldownTimer:F2} seconds.");
            }
        }
    }

    private void ActivateSkill()
    {
        Debug.Log("Freeze skill activated!");
        StartCoroutine(FreezeEnemiesCoroutine());
        canActivateSkill = false;
        skillCooldownTimer = skillCooldown;
        Debug.Log($"Skill put on cooldown for {skillCooldown} seconds.");
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
