using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonScript : MonoBehaviourPunCallbacks
{
    //�� ������ ���� ����
    public InputField inputField;
    public Canvas canvas;
    public GameObject floor;
    string gameVersion = "1";  //���� ����
    private void Awake()
    {
        //�ڵ� ���� ����ȭ
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        //���� ����
        PhotonNetwork.GameVersion = gameVersion;
        //���� ����
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    public void CreateRoomButton()  //�� ���� ���
    {
        if (inputField.text != "")
        {
            PhotonNetwork.CreateRoom(inputField.text, new RoomOptions { MaxPlayers = 4 }); //�ִ��ο�
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
    public override void OnJoinedRoom()  //�濡 �������� ����Ǵ� �Լ�
    {
        canvas.gameObject.SetActive(false);
        floor.gameObject.SetActive(true);
        PhotonNetwork.Instantiate("Robot Kyle", Vector3.zero,Quaternion.identity,0);     //ť�긦 0�� ��ġ�� �����Ѵ�.
    }
}