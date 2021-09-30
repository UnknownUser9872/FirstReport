using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView PV;
    public PlayerController playerController;
    void Awake()
    {
        PV = GetComponent<PhotonView>();  
    }
    public override void Use()
    {
        Debug.Log("using gun" + itemInfo.itemName);
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity,LayerMask.GetMask("Player")))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot" , RpcTarget.All,hit.point,hit.normal);  //������ ������ �˷��ֱ�
            playerController.currentHealth += 10;
        }
    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject BloodObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(BloodObj, 10f);
            //�Ѿ��ڱ��� 10�� �Ŀ� �������.
            BloodObj.transform.SetParent(colliders[0].transform);
            //�ֺ��� �ִ� �ݸ����� �θ�� �����Ѵ�.
            //�θ� ������� �ڽİ�ü�鵵 �� ������Ƿ� �Ѿ��ڱ��� �յ� ���ٴϴ°� �ذ��Ҽ� �ִ�.
        }
    }
}
