using Photon.Pun;
using UnityEngine;

public class PhotonRPCScript : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    void Update()
    {
      
    }
    [PunRPC]
    public void RPCDebug()
    {
        Debug.Log("AAA");   //AŰ�� ������ AAA�� �ߴ� ���
    }
}
