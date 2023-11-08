using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public Gun equippedGun;
    public GunPoint gunPoint = GunPoint.None;

    void Start()
    {

    }

    void Update()
    {
        SetGunToGunPoint();
    }


    void SetGunToGunPoint() {
        // equippedGun.transform.position =
    }


    public enum GunPoint {
        None,
        Aim,
        Hand,
        Look,
    }
}
