using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    void Awake()
    {

    }

    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, 10));
    }
}
