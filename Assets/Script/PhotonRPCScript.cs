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
        Debug.Log("AAA");   //A키를 누르면 AAA가 뜨는 결과
    }
}
