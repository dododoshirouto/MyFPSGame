using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public string gunName;
    public Magazine mag_pref;
    public Collider magCheckTrigger;
    public Transform firePoint;
    public Transform magPoint;
    public Transform slider;
    public Transform slidePoint;
    public float slideMoveRate = 0.2f;
    Vector3 firstSlidePos;
    Quaternion firstSlideRot;
    bool doSlide = false;
    bool doSlideStopper = false;

    public ParticleSystem[] fireParticles;

    [Header("System Args")]
    public Magazine magazine;


    HintText hintMagEject;
    HintText hintMagSet;
    HintText hintSlide;
    HintText hintFire;


    void Start()
    {
        if (slider && slidePoint)
        {
            firstSlidePos = slider.localPosition;
            firstSlideRot = slider.localRotation;
        }

        var mgc = magCheckTrigger.gameObject.AddComponent<MagCheck>();
        mgc.gun = this;

        hintMagEject = HintText.CreateText(magPoint, "B", "Eject Mag");
        hintMagEject.SetVisible(false);
        hintMagSet = HintText.CreateText(magPoint, "G", "Set Mag");
        hintMagSet.SetVisible(false);
        hintSlide = HintText.CreateText(slidePoint, "R", "Slide");
        hintSlide.SetVisible(false);
        hintFire = HintText.CreateText(firePoint, "Mouse L", "Fire");
        hintFire.SetVisible(false);
    }

    void Update()
    {
        MoveSlide();
    }

    void OnDestroy()
    {
        hintMagEject.Remove();
        hintMagSet.Remove();
        hintSlide.Remove();
        hintFire.Remove();
    }



    public int GetBulletNum()
    {
        if (magazine == null)
        {
            hintMagSet.SetVisible(true);
            return 0;
        }
        if (magazine.gameObject.TryGetComponent<Rigidbody>(out var tmp_rb))
        {
            return 0;
        }
        return magazine.bulletNum;
    }




    public void Fire()
    {
        if (GetBulletNum() <= 0)
        {
            if (magazine) hintMagEject.SetVisible(true);
            return;
        }
        if (doSlide || doSlideStopper)
        {
            hintSlide.SetVisible(true);
            return;
        }


        for (int i = 0; i < fireParticles.Length; i++)
        {
            fireParticles[i].Play();
        }

        magazine.bulletNum--;
        doSlide = true;
    }

    public void MoveSlide()
    {
        if (!slider || !slidePoint)
        {
            return;
        }

        if (doSlide)
        {
            slider.localPosition = Vector3.Lerp(slider.localPosition, slidePoint.localPosition, slideMoveRate);
            slider.localRotation = Quaternion.Lerp(slider.localRotation, slidePoint.localRotation, slideMoveRate);
            if (Vector3.Distance(slider.localPosition, slidePoint.localPosition) < 0.0001f)
            {
                doSlide = false;
                hintSlide.SetVisible(false);
            }
        }
        else if (Vector3.Distance(slider.localPosition, firstSlidePos) > 0.0001f)
        {
            if (doSlideStopper) return;
            if (GetBulletNum() > 0)
            {
                slider.localPosition = Vector3.Lerp(slider.localPosition, firstSlidePos, slideMoveRate);
                slider.localRotation = Quaternion.Lerp(slider.localRotation, firstSlideRot, slideMoveRate);
            }
            else
            {
                doSlideStopper = true;
            }
        }
    }

    public void DoSliding()
    {
        doSlide = true;
        if (GetBulletNum() > 0) doSlideStopper = false;
        else
        {
            if (magazine)
                hintMagEject.SetVisible(true);
            else
                hintMagSet.SetVisible(true);
        }
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
        // magazine = null;
        hintMagEject.SetVisible(false);
    }

    [SerializeField, Button("SetMag")]
    bool setMagButton;
    public void SetMag()
    {
        if (magazine != null)
        {
            // EjectMag();
            return;
        }

        magazine = Instantiate(mag_pref.gameObject, magPoint).GetComponent<Magazine>();
        magazine.parentGun = this;
        magazine.Set();
        hintMagSet.SetVisible(false);
    }

    [SerializeField, Button("Cocking")]
    bool cockingButton;
    public void Cocking()
    {

    }



    public class MagCheck : MonoBehaviour
    {
        public Gun gun;

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == gun.magazine.gameObject)
            {
                gun.magazine = null;
            }
        }
    }
}
