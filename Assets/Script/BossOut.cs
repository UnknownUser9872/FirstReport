using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOut : MonoBehaviour
{
    public bool playStart = false;

    void Start()
    {

    }

    void Update()
    {
        if (playStart)
        {
            if (transform.childCount == 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(2);
                    playStart = false;
                }
            }
        }
    }
}