using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject graphics;

    private void Awake()
    {
        graphics.SetActive(false);//스폰포인트 위치값만 남기고 캡슐과 큐브는 지우기
    }
}
