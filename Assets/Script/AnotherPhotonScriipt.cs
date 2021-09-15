using UnityEngine;
using Photon.Pun;
using TMPro;   //텍스트에서 매쉬 프로 기능 사용

public class AnotherPhotonScriipt : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
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

    void Update()
    {
        
    }
}
