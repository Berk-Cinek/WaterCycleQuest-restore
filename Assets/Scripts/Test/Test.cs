using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    private void Start()
    {
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;

    }

    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {

    }


}
