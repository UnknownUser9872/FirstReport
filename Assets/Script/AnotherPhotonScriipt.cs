using UnityEngine;
using Photon.Pun;
using TMPro;   //�ؽ�Ʈ���� �Ž� ���� ��� ���
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
    public static AnotherPhotonScriipt Instance; //�� ��ũ��Ʈ�� �޼���� ���� ���� ���� 
    private void Awake()
    {
        Instance = this; //�޼���� ���
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings(); //������ ���� ������ ���� ������ ������ ����
    }
    public override void OnConnectedToMaster()   //������ ���� ���� �� �۵�
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); //������ ������ ����� �κ�� ����
    }
    public override void OnJoinedLobby()   //�κ� ���� �� �۵�
    {
        AnotherMenuManager.Instance.OpenMenu("title");  //�κ� ������ Ÿ��Ʋ �޴� �ѱ�
        Debug.Log("Joind Lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
        //���û�� �̸��� �������� ���ڷ� �ٿ��ֱ�
    }
    public void CreateRoom()//�游���
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//�� �̸��� ���̸� �� �ȸ������
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);//���� ��Ʈ��ũ������� roomNameInputField.text�� �̸����� ���� �����.
        AnotherMenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public override void OnJoinedRoom()  //�濡 ������ �۵�
    {
        AnotherMenuManager.Instance.OpenMenu("room");   //�� �޴� ����
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;   //�� �� �̸� ǥ��
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            //���� �濡 ���� �濡�ִ� ��� ��� ��ŭ �̸�ǥ �߰� �ϱ�
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message) //�� ���� ���н� �۵�
    {
        errorText.text = "Room Creation Failed: " + message;
        AnotherMenuManager.Instance.OpenMenu("error"); //���� �޴� ����
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//�� ������ ���� ��Ʈ��ũ ���
        AnotherMenuManager.Instance.OpenMenu("loading"); //�ε�â ����
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//���� ��Ʈ��ũ�� JoinRoom��� �ش��̸��� ���� ������ �����Ѵ�. 
        AnotherMenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public override void OnLeftRoom()  //���� ������ ȣ��
    {
        AnotherMenuManager.Instance.OpenMenu("title");  //�� ������ ������ Ÿ��Ʋ �޴� ȣ��
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//������ �� ����Ʈ ���
    {
        foreach (Transform trans in roomListContent)//�����ϴ� ��� roomListContent
        {
            Destroy(trans.gameObject);//�븮��Ʈ ������Ʈ�� �ɶ����� �������
        }
        for (int i = 0; i < roomList.Count; i++)//�氹����ŭ �ݺ�
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate�� prefab�� roomListContent��ġ�� ������ְ� �� �������� i��° �븮��Ʈ�� �ȴ�. 
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)//�ٸ� �÷��̾ �濡 ������ �۵�
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //instantiate�� prefab�� playerListContent��ġ�� ������ְ� �� �������� �̸� �޾Ƽ� ǥ��. 
    }
}

