using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCheck : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] TMP_Text timetext;
    public int iTimer = 600;  //시간용 카운터
    public bool playStart = false;

    private void Start()
    {
        StartCoroutine(CoTimer());
    }
    private IEnumerator CoTimer()
    {
        if (iTimer == 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(2);
                playStart = false;
            }
        }
        while (true)
        {
            timetext.text = "Count Down : " + iTimer;
            yield return new WaitForSecondsRealtime(1f);
            iTimer--;
        }
    }
}
