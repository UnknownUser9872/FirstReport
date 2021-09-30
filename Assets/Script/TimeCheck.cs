using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCheck : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] TMP_Text timetext;
    public int iTimer = 600;  //�ð��� ī����

    private void Start()
    {
        StartCoroutine(CoTimer());
    }
    private IEnumerator CoTimer()
    {
        while (true)
        {
            timetext.text = "Count Down : " + iTimer;
            yield return new WaitForSecondsRealtime(1f);
            iTimer--;
        }
    }
}
