using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float verti = Input.GetAxis("Vertical");
        if(horiz != 0)
        {
            transform.Rotate(new Vector3(0,horiz*rotateSpeed*Time.deltaTime,0),Space.World);
        }
        if(verti != 0)
        {
            transform.Translate(transform.forward*moveSpeed*verti*Time.deltaTime,Space.World);
        }
    }
}
