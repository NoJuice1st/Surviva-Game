using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    public float waitTime = 0;

    private void Start()
    {
        Invoke("DestroySelf", waitTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
