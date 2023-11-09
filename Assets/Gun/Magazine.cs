using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [Header("Magazine Settings")]
    public int maxBullet = 6;

    public int bulletNum = 0;

    [Header("System Args")]
    public Gun parentGun;

    public Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {

    }



    public void Eject()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        this.transform.SetParent(null);
    }

    public void Set()
    {

        this.transform.SetParent(null);
        transform.parent = parentGun.magPoint;
        transform.position = parentGun.magPoint.position;
        transform.rotation = parentGun.magPoint.rotation;
    }
}
