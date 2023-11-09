using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{
    InputActionAsset actions;
    public Gun equippedGun;
    public GunPointState gunPointState = GunPointState.Hand;

    public GunPointSet[] gunPointSets;

    public float gunPosAnimRate = 0.1f;


    void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    void Update()
    {
        if (!equippedGun)
        {
            return;
        }

        SetAndEjectMag();

        SetGunPointState();
        SetGunToGunPoint();
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
        var nowGunPos = Array.Find(gunPointSets, x => x.name == gunPointState.ToString()).point.position;
        var nowGunRot = Array.Find(gunPointSets, x => x.name == gunPointState.ToString()).point.rotation;
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
