using UnityEngine;
using UnityEngine.UI;

public class SkillBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image invincibilityImage;
    [SerializeField] private Image freezeImage;
    [SerializeField] private Image bulletFrenzyImage;

    /// <summary>
    /// Update the cooldown bar for Invincibility.
    /// </summary>
    public void UpdateInvincibilityCooldown(float normalizedTime)
    {
        invincibilityImage.fillAmount = normalizedTime;
    }

    /// <summary>
    /// Update the cooldown bar for Freeze skill.
    /// </summary>
    public void UpdateFreezeCooldown(float normalizedTime)
    {
        freezeImage.fillAmount = normalizedTime;
    }

    /// <summary>
    /// Update the cooldown bar for Bullet Frenzy.
    /// </summary>
    public void UpdateBulletFrenzyCooldown(float normalizedTime)
    {
        bulletFrenzyImage.fillAmount = normalizedTime;
    }
}
