using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;//path�������

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;//����� ����
    GameObject controller; //PlayerContriller�� ���̴� ĸ��

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)//�� ���� ��Ʈ��ũ�̸�
        {
            CreateController();//�÷��̾� ��Ʈ�ѷ� �ٿ��ش�. 
        }
    }
    void CreateController()//�÷��̾� ��Ʈ�ѷ� �����
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        Debug.Log("Instantiated Player Controller");
        controller=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerController"),spawnpoint.position,spawnpoint.rotation,0,new object[] { PV.ViewID});
        //���� �����տ� �ִ� �÷��̾� ��Ʈ�ѷ��� �� ��ġ�� �� ������ ����
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
    }
}