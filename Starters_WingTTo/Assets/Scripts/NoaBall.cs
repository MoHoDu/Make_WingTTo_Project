using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoaBall : MonoBehaviour
{
    public float ballVec = 1;
    public float speed = 1;

    void Awake()
    {
        GameObject playerObj = GameObject.Find("Player");
        speed = playerObj.GetComponent<Rigidbody>().velocity.x * 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("block"))
        {
            ballVec = -1;
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (ballVec == 1)
        {
            GameObject playerObj = GameObject.Find("Player");
            speed = playerObj.GetComponent<Rigidbody>().velocity.x * 2f;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(speed * ballVec, 0, 0);
    }

    private void LateUpdate()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (Vector3.Distance(transform.position, playerObj.transform.position) > 100)
        {
            Destroy(this.gameObject);
        }
    }
}
