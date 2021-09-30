using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;//���� ���� Ŭ���̾�Ʈ�� ���� �ؽ����̺� ���
using Photon.Realtime;
using static GameController;  //���� ��Ʈ�ѷ� ���
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks/*�ٸ� ���� ���� �޾Ƶ��̱�*/, IDamageable//�������̽��ҷ�����
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;
    public bool isBoss;
    Renderer capsuleColor;
    public int itemIndex;
    public int previousItemIndex = -1;//�⺻ ������ �� ������
    //���콺���� �ٴ¼ӵ� �ȴ¼ӵ� ������ �ٱ�ȱ�ٲܶ� ���ӽð�
    float verticalLookRotation;
    bool grounded;//������ ���� �ٴ�üũ
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;//���� �̵��Ÿ�
    const float maxHealth = 100f; //Ǯ��
    public float currentHealth = maxHealth; //������
    public static PlayerController playerController;
    public TMP_Text healthText;
    public TMP_Text bossText;
    public Canvas canvas;
    bool isMove;
    public float speed = 10f;
    bool turnCamera;

    PlayerManager playerManager;   //�÷��̾�Ŵ��� ����
    Rigidbody rb;
    PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        isBoss = false;  //�ʱ� ���� ����
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        playerController = GetComponent<PlayerController>();  //�÷��̾� ��Ʈ�ѷ��� ����
        GC.Players.Add(this);  //������Ʈ�ѷ� ��Ͽ� ��׵� �߰�
        Debug.Log("Add complete");  //�߰��Ϸ�
    }

    void Start()
    {
        this.transform.parent = GameObject.FindWithTag("Playercheck").transform;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //���� �ƴϸ� ī�޶� ���ֱ�
            Destroy(rb);
            //���žƴϸ� ������ �ٵ� �����ֱ�
            canvas.gameObject.SetActive(false);
        }
        capsuleColor = gameObject.GetComponent<Renderer>();  //������ ���� ������, ó������ �ƹ��� ����ȳ�
    }

    void Update()
    {
        if (!PV.IsMine)
            return;//�����ƴϸ� �۵�����
        Jump();
        Look();
        if (isBoss == true)
        {
            turnCamera = true;
            Move();
            healthText.text = ("Current Health : " + currentHealth.ToString());
            bossText.text = ("Catch All");
            PV.RPC("RPC_SetColor", RpcTarget.AllBuffered);

            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))//ToString���� �ϸ� �Է¹޴� String�� ���ڷ� ǥ���� �� �ִ�. 
                {
                    EquipItem(i);
                    //����Ű 1 2������ ������ ���� ����
                    break;
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)//���콺 ��ũ�� �����̸�
            {
                if (itemIndex >= items.Length - 1)//���� ������ ��ϳ��� �ٴٸ���
                {
                    EquipItem(0);//��ó�� ����������
                }
                else
                {
                    EquipItem(itemIndex + 1);//�ƴϸ� ���� ����������
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)//���콺 ��ũ�� �ݴ�� �����̸�
            {
                if (itemIndex <= 0)//������ ��� ��ó������ �ڷΰ���?
                {
                    EquipItem(items.Length - 1);//�� �� ����������
                }
                else
                {
                    EquipItem(itemIndex - 1);//�ƴϸ� ���� ����������
                }
            }
            if (Input.GetMouseButtonDown(0) && (items[0].itemGameObject.activeSelf == true || items[1].itemGameObject.activeSelf == true)) //���콺 ��Ŭ����
            {
                if (items[0].itemGameObject.activeSelf == true)
                {
                    TakeDamage(5);  //������ ����1 ���� 5����
                }
                if (items[1].itemGameObject.activeSelf == true)
                {
                    TakeDamage(10);  //������ ����2 ���� 10����
                }
                items[itemIndex].Use();//����ִ� ������ ���
                Debug.Log("Shoot");
            }
        }
        else
        {
            bossText.text = ("Run Away");
            if (Input.GetKey(KeyCode.Q))
            {
                SetMove(false);
                turnCamera = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                turnCamera = true;
                SetMove(true);
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                turnCamera = true;
                Look();
                gameObject.transform.Rotate(new Vector3(0,90f,0));
            }
        }
        if (transform.position.y < -10f) //�� ������ ������
        {
            Die();
        }
    }

    void Look()
    {
        if (turnCamera == true)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
            //���콺 �����̴� ����*�ΰ�����ŭ ���� �����̱�
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
            //���콺 �����̴� ����*�ΰ�����ŭ ���� �� �ޱ�
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            //y�� -90������ 90���� ������ ����
            cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
            //���� ������ ī�޶� ������
        }
        else
        {
            cameraHolder.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
            //���콺 �����̴� ����*�ΰ�����ŭ ���� �����̱�
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
            //���콺 �����̴� ����*�ΰ�����ŭ ���� �� �ޱ�
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            //y�� -90������ 90���� ������ ����
            cameraHolder.transform.localEulerAngles = new Vector3((Vector3.left * verticalLookRotation).x, cameraHolder.transform.localEulerAngles.y, cameraHolder.transform.localEulerAngles.z);
            //���� ������ ī�޶� ������
        }
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //���������� �������� ũ��� 1�� �븻������
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        //���� ����Ʈ�� ������ �ٴ¼ӵ�, �������� �ȴ¼ӵ����ϱ�
        //smoothTime��ŭ�� ���ļ� �̵����ֱ�. 
    }

    void Jump()
    {
        print(grounded);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)//�������� �����̽��� ������
        {
            rb.AddForce(transform.up * jumpForce);//�����¸�ŭ���� ������
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;//�Է¹��� ���ڰ� �Ʊ� ���� ���ڶ� �Ȱ����� �ƹ��ϵ� �����ش�.  
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);//itemIndex���� ������ on
        if (previousItemIndex != -1)//���� �ʱ� ���°� �ƴ϶��
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
            //���� �Ʊ� ���� �������� off
        }
        previousItemIndex = itemIndex;//���� ����Ŭ

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            //���� hash[itemindex]�� ȣ��Ǹ� ���� �����۹�ȣ�� ȣ��ȴ�.    
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //�������� ��� ������� ���� ���糢�� �ִ� ������ ��ȣ�� �˷��ش�.  
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //�ٸ� �÷��̾��� ������ ��ȣ �޾Ƶ��̱�
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)//������ �ƴ϶� �ٸ�������϶�
        {
            EquipItem((int)changedProps["itemIndex"]);
            //�����ִ� ������ ���� �޾Ƶ��̱�
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void SetMove(bool _isMove)
    {
        isMove = _isMove;
        if (_isMove == true)
        {
            Move();
        }
        else
        {

        } 
    }
    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;//�����ƴϸ� �۵�����
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        //�̵��ϴ°Ŵ� ��� ���� moveAmount��ŭ�� �����Ƚð�(0.2��)���ٿ� ���缭
    }

    public void TakeDamage(float damage)
    {
        //IDamageable �������̽��� �ִ� �Լ� ������
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        //���ظ� ��������� �ش� �̸��� ���� �Լ��� RpcTakget(������ ��� �÷��̾�)���� ���� �ǵ��� ȣ��
        //Rpc�� ���� ���� ���ظ� ��ο��� �����Ѵ�.
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        //��ο��� ���޵�
        if (!PV.IsMine)
        {
            return;   //�������� �� �ƴϸ� ����ȵ�
        }
        Debug.Log("took damage" + damage);
        currentHealth -= damage;
        healthText.text = "Current Health : " + currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GC.Players.Remove(this);
        playerManager.Die();
    }
    [PunRPC]
    void SetBoss(bool _isBoss)
    {
        //������ �����ִ� Rpc
        isBoss = _isBoss;
        Debug.Log("Boss " + isBoss);
        items[0].itemGameObject.SetActive(true);
        previousItemIndex = 0;
        this.transform.parent = GameObject.FindWithTag("Bosscheck").transform;
    }
    [PunRPC]
    void RPC_SetColor()   //���� �� ����
    {
        capsuleColor.material.color = Color.black;
    }
}
