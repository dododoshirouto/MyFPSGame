using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    public bool onGround = false;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Ground") {
            onGround = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ground") {
            onGround = false;
        }
    }
}
