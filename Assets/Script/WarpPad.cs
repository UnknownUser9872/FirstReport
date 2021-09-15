using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPad : MonoBehaviour
{
    public GameObject AnotherWarpPad;
    public GameObject player;
    public static float coltime= 0;
    public static float fulltime=1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(coltime < 0f)
        {
            coltime = 0f;
        }
        else
        {
        coltime -= Time.deltaTime;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(coltime < 0.1f)
            {
                coltime = fulltime;
                player.gameObject.transform.position = AnotherWarpPad.transform.position;
            }
        }
    }
}