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
    IEnumerator ExampleCoroutine()//게임시작 3초후 술래가 정해지도록 지연을 둔다.  
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //초세기 시작
        yield return new WaitForSeconds(3);
        //3초세기
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        //초세기 끝
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Start Pick Boss");
            //술래정하기 시작
            PickBoss();
            //술래 정하기
        }
    }
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }
    void PickBoss()
    {
        List<PlayerController> PlayerList = new List<PlayerController>(Players);
        //PlayerController달고있는 놈들 리스트화 시키기
        whichPlayerIsBoss = Random.Range(0, PlayerList.Count);
        //그중에 한놈 술래로 정해주기
        Debug.Log("We have " + PlayerList.Count);
        //현재있는 사람숫자 보고
        Debug.Log("Boss Number is " + whichPlayerIsBoss);
        //몇번이 술래인지 말해줌
        Players[whichPlayerIsBoss].GetComponent<PhotonView>().RPC("SetBoss", RpcTarget.All, true);
        //해당 사람이 술래가됨

    }
}
