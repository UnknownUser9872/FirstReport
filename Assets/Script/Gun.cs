using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item  //아이템스크립트에서 정보받아줌
{
    public abstract override void Use();
    public GameObject bulletImpactPrefab; //맞은자국 만들기
}
