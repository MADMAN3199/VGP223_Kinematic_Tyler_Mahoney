using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //MouseLook rotates transform based on mouse delta
    //min and max values can be used to constraining possible rotation


    public enum RotationAxis { MouseXandY = 0, MouseX = 1, MouseY = 2}
    public RotationAxis axes = RotationAxis.MouseXandY;

    public float senX = 15f;
    public float senY = 15f;

    public float minX = -360F;
    public float maxX = 360F;

    public float minY = -60F;
    public float maxY = 60F;

    float rotY = 0f;
    

    // Update is called once per frame
    void Update()
    {
       if (axes == RotationAxis.MouseXandY)
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * senX;
            rotY += Input.GetAxis("Mouse Y") * senY;
            rotY = Mathf.Clamp(rotY, minY, maxY);

            transform.localEulerAngles = new Vector3(-rotY, rotX, 0);
        }
       else if (axes == RotationAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * senX, 0); 
        }
       else
        {
            rotY += Input.GetAxis("Mouse Y") * senY;
            rotY = Mathf.Clamp(rotY, minY, maxY);

            transform.localEulerAngles = new Vector3(-rotY, transform.localEulerAngles.y, 0);
        }
    }
}
