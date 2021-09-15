using UnityEngine;
using Photon.Pun;
using TMPro;   //�ؽ�Ʈ���� �Ž� ���� ��� ���

public class AnotherPhotonScriipt : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
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

    void Update()
    {
        
    }
}
