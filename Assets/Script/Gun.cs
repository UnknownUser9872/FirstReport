using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item  //�����۽�ũ��Ʈ���� �����޾���
{
    public abstract override void Use();
    public GameObject bulletImpactPrefab; //�����ڱ� �����
}
