using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject camera;
    private float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X") * speed, 0f, Space.Self);
        camera.transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);
    }
}
