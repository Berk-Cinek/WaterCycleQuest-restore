using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private Animator aimAnimator;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");

    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();

    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);

    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);

    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);

        return worldPosition;

    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }

        else
        {
            aimLocalScale.y = +1f;
        }

        aimTransform.localScale = aimLocalScale;



    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            aimAnimator.SetTrigger("Shoot");
            OnShoot?.Invoke(this, new OnShootEventArgs { gunEndPointPosition = aimGunEndPointTransform.position, shootPosition = mousePosition, });

        }

    }
}
