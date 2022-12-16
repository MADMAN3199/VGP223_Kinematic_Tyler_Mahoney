using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKinematic : MonoBehaviour
{
    Rigidbody rb;
    public int speed = 3;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);
        rb.MovePosition(transform.position + move * Time.deltaTime * speed);
    }
}
