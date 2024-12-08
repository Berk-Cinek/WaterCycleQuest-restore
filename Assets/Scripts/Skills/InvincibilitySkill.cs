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

    private void Awake()
    {
        player = GetComponent<NewPlayerMovement>();
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
                Debug.Log("You can use invincibility again!");
            }
        }

       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (canActivate)
            {
                ActivateSkill();
            }
            else
            {
                Debug.Log($"Invincibility skill is on cooldown! Time left: {Mathf.Ceil(cooldownTimer)} seconds.");
            }
        }
    }

    private void ActivateSkill()
    {
        if (isInvincible || player == null) return;

        Debug.Log("Invincibility skill activated!");
        StartCoroutine(InvincibilityCoroutine());
        canActivate = false;
        cooldownTimer = cooldownTime; 
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        player.SetInvincibility(true); 

        
        Debug.Log("Player is invincible!");

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        player.SetInvincibility(false); 
        Debug.Log("Invincibility ended!");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
