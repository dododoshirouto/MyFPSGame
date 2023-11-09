using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{
    InputActionAsset actions;
    PlayerController playerController;
    public Gun equippedGun;
    public GunPointState gunPointState = GunPointState.Hand;

    public GunPointSet[] gunPointSets;

    public float gunPosAnimRate = 0.1f;


    void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!equippedGun)
        {
            return;
        }

        FireControl();
        SetGunPointState();
        SetAndEjectMag();
    }

    void FixedUpdate()
    {
        if (!equippedGun)
        {
            return;
        }

        var handPoint = Array.Find(gunPointSets, x => x.name == "Hand").point;
        handPoint.localRotation = playerController.neckBonePitch.localRotation;

        SetGunToGunPoint();
    }



    void FireControl()
    {

        if (actions["Fire"].WasPressedThisFrame())
        {
            equippedGun.Fire();
        }
    }

    void SetAndEjectMag()
    {
        if (actions["SetMag"].WasPressedThisFrame())
        {
            equippedGun.SetMag();
        }
        if (actions["EjectMag"].WasPressedThisFrame())
        {
            equippedGun.EjectMag();
        }
    }


    void SetGunToGunPoint()
    {
        var targetPoint = Array.Find(gunPointSets, x => x.name == gunPointState.ToString()).point;

        if (equippedGun.transform.parent != targetPoint)
        {
            equippedGun.transform.parent = targetPoint;
        }

        var nowGunPos = targetPoint.position;
        var nowGunRot = targetPoint.rotation;
        equippedGun.transform.position = Vector3.Lerp(equippedGun.transform.position, nowGunPos, gunPosAnimRate);
        equippedGun.transform.rotation = Quaternion.Lerp(equippedGun.transform.rotation, nowGunRot, gunPosAnimRate);
    }

    void SetGunPointState()
    {
        if (actions["Aim"].IsPressed())
        {
            gunPointState = GunPointState.Aim;
            return;
        }

        gunPointState = GunPointState.Hand;
    }


    public enum GunPointState
    {
        None,
        Aim,
        Hand,
        Look,
    }



    [System.Serializable]
    public class GunPointSet
    {
        public string name;
        public Transform point;
    }
}
