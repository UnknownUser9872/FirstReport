using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.blue;
    }
    void Update()
    {
        
    }
}
