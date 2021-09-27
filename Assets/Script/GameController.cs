using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviour
{
    public int whichPlayerIsBoss;
    public static GameController GC;
    public List<PlayerController> Players = new List<PlayerController>();
    void Awake()
    {
        GC = this;
    }
    IEnumerator ExampleCoroutine()//���ӽ��� 3���� ������ ���������� ������ �д�.  
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //�ʼ��� ����
        yield return new WaitForSeconds(3);
        //3�ʼ���
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        //�ʼ��� ��
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Start Pick Boss");
            //�������ϱ� ����
            PickBoss();
            //���� ���ϱ�
        }
    }
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }
    void PickBoss()
    {
        List<PlayerController> PlayerList = new List<PlayerController>(Players);
        //PlayerController�ް��ִ� ��� ����Ʈȭ ��Ű��
        whichPlayerIsBoss = Random.Range(0, PlayerList.Count);
        //���߿� �ѳ� ������ �����ֱ�
        Debug.Log("We have " + PlayerList.Count);
        //�����ִ� ������� ����
        Debug.Log("Boss Number is " + whichPlayerIsBoss);
        //����� �������� ������
        Players[whichPlayerIsBoss].GetComponent<PhotonView>().RPC("SetBoss", RpcTarget.All, true);
        //�ش� ����� ��������

    }
}
