using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isSafe;

    private void OnTriggerEnter(Collider other) {
        if(other.name.Contains("SafeArea"))
        {
            isSafe = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.name.Contains("SafeArea"))
        {
            isSafe = false;
        }
    }
}
