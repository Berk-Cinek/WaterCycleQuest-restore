using UnityEngine;
using UnityEngine.UI;

public class SkillBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image invincibilityImage;
    [SerializeField] private Image freezeImage;
    [SerializeField] private Image bulletFrenzyImage;

    public void UpdateInvincibilityCooldown(float normalizedTime)
    {
        invincibilityImage.fillAmount = normalizedTime;
    }

    public void UpdateFreezeCooldown(float normalizedTime)
    {
        freezeImage.fillAmount = normalizedTime;
    }

    public void UpdateBulletFrenzyCooldown(float normalizedTime)
    {
        bulletFrenzyImage.fillAmount = normalizedTime;
    }
}
