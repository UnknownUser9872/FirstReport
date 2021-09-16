using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    RoomInfo info;  //포톤 리얼타임의 방정보 기능

    public void SetUp(RoomInfo _info) //방 정보 받아오기
    {
        info = _info;
        text.text = _info.Name;
    }
    public void OnClick()
    {
        AnotherPhotonScriipt.Instance.JoinRoom(info); //포톤스크립트 메서드로 조인룸 실행
    }
}
