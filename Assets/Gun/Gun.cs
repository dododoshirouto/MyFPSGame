using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public string gunName;
    public Magazine mag_pref;
    public Transform firePoint;
    public Transform magPoint;

    [Header("System Args")]
    public Magazine magazine;


    void Start()
    {

    }

    void Update()
    {

    }



    public int getBulletNum() {
        if (magazine == null) {
            return 0;
        }
        return magazine.bulletNum;
    }




    public void Fire()
    {

    }

    [Header("Action Preview Buttons")]
    [SerializeField, Button("EjectMag")]
    public bool ejectMagButton;
    public void EjectMag()
    {
        if (this.magazine == null)
        {
            return;
        }

        magazine.Eject();
        magazine = null;
    }

    [SerializeField, Button("SetMag")]
    bool setMagButton;
    public void SetMag()
    {
        if (magazine != null)
        {
            EjectMag();
        }

        magazine = Instantiate(mag_pref.gameObject, magPoint).GetComponent<Magazine>();
        magazine.parentGun = this;
        magazine.Set();
    }

    [SerializeField, Button("Cocking")]
    bool cockingButton;
    public void Cocking()
    {

    }
}
