using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{//�߻�Ŭ������ ����
    public ItemInfo itemInfo;
    public GameObject itemGameObject;

    public abstract void Use();
}
