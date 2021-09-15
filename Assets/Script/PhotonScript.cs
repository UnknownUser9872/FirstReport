using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonScript : MonoBehaviourPunCallbacks
{
    //방 제목을 정할 빌드
    public InputField inputField;
    public Canvas canvas;
    public GameObject floor;
    string gameVersion = "1";  //게임 버전
    private void Awake()
    {
        //자동 레벨 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        //버전 설정
        PhotonNetwork.GameVersion = gameVersion;
        //서버 연결
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    public void CreateRoomButton()  //방 생성 기능
    {
        if (inputField.text != "")
        {
            PhotonNetwork.CreateRoom(inputField.text, new RoomOptions { MaxPlayers = 4 }); //최대인원
        }
    }
    public void JoinRoomButton()
    {
        if (inputField.text != "")
        {
            PhotonNetwork.JoinRoom(inputField.text);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnJoinedRoom()  //방에 들어왔을때 실행되는 함수
    {
        canvas.gameObject.SetActive(false);
        floor.gameObject.SetActive(true);
        PhotonNetwork.Instantiate("Robot Kyle", Vector3.zero,Quaternion.identity,0);     //큐브를 0의 위치에 생성한다.
    }
}