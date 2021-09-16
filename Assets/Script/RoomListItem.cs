using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    RoomInfo info;  //���� ����Ÿ���� ������ ���

    public void SetUp(RoomInfo _info) //�� ���� �޾ƿ���
    {
        info = _info;
        text.text = _info.Name;
    }
    public void OnClick()
    {
        AnotherPhotonScriipt.Instance.JoinRoom(info); //���潺ũ��Ʈ �޼���� ���η� ����
    }
}
