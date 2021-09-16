using UnityEngine;
using Photon.Pun;
using TMPro;   //텍스트에서 매쉬 프로 기능 사용
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;

public class AnotherPhotonScriipt : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    public static AnotherPhotonScriipt Instance; //이 스크립트를 메서드로 쓰기 위해 선언 
    private void Awake()
    {
        Instance = this; //메서드로 사용
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings(); //설정한 포톤 서버에 따라 마스터 서버에 연결
    }
    public override void OnConnectedToMaster()   //마스터 서버 연결 시 작동
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); //마스터 서버에 연결시 로비로 연결
    }
    public override void OnJoinedLobby()   //로비에 연결 시 작동
    {
        AnotherMenuManager.Instance.OpenMenu("title");  //로비에 들어오면 타이틀 메뉴 켜기
        Debug.Log("Joind Lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
        //들어올사람 이름을 랜덤으로 숫자로 붙여넣기
    }
    public void CreateRoom()//방만들기
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//방 이름이 빈값이면 방 안만들어짐
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);//포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.
        AnotherMenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public override void OnJoinedRoom()  //방에 들어갔을때 작동
    {
        AnotherMenuManager.Instance.OpenMenu("room");   //룸 메뉴 열기
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;   //들어간 방 이름 표시
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            //내가 방에 들어가면 방에있는 사람 목록 만큼 이름표 뜨게 하기
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message) //방 생성 실패시 작동
    {
        errorText.text = "Room Creation Failed: " + message;
        AnotherMenuManager.Instance.OpenMenu("error"); //에러 메뉴 열기
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//방 떠나기 포톤 네트워크 기능
        AnotherMenuManager.Instance.OpenMenu("loading"); //로딩창 열기
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//포톤 네트워크의 JoinRoom기능 해당이름을 가진 방으로 접속한다. 
        AnotherMenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public override void OnLeftRoom()  //방을 떠나면 호출
    {
        AnotherMenuManager.Instance.OpenMenu("title");  //방 떠나기 성공시 타이틀 메뉴 호출
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//포톤의 룸 리스트 기능
    {
        foreach (Transform trans in roomListContent)//존재하는 모든 roomListContent
        {
            Destroy(trans.gameObject);//룸리스트 업데이트가 될때마다 싹지우기
        }
        for (int i = 0; i < roomList.Count; i++)//방갯수만큼 반복
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate로 prefab을 roomListContent위치에 만들어주고 그 프리펩은 i번째 룸리스트가 된다. 
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)//다른 플레이어가 방에 들어오면 작동
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //instantiate로 prefab을 playerListContent위치에 만들어주고 그 프리펩을 이름 받아서 표시. 
    }
}

