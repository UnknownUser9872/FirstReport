using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;  //�⺻ ������ �� ������ ����
    //���콺���� �ٴ¼ӵ� �ȴ¼ӵ� ������ �ٱ�ȱ�ٲܶ� ���ӽð�
    float verticalLookRotation;
    bool grounded=true;//������ ���� �ٴ�üũ
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;//���� �̵��Ÿ�

    PhotonView PV;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            Equipitem(0);  //�����Ҷ� �� ������ 1�� ������ ����
        }
        else
        { 
            Destroy(GetComponentInChildren<Camera>().gameObject); //�����ƴϸ� ī�޶� ���ֱ�
            Destroy(rb); //�����ƴϸ� ������ٵ� ���ֱ�
        }
    }
    void Update()
    {
        if (!PV.IsMine)
        {
            return;   //���� �ƴϸ� �۵�����
        }
        Look();
        Move();
        Jump();
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))//ToString���� �ϸ� �Է¹޴� String�� ���ڷ� ǥ���� �� �ִ�. 
            {
                Equipitem(i);
                //����Ű 1 2������ ������ ���� ����
                break;
            }
        }
    }
    void Look()
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
        if (Input.GetKeyDown(KeyCode.Space) && grounded)//�������� �����̽��� ������
        {
            rb.AddForce(transform.up * jumpForce);//�����¸�ŭ���� ������
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;  //�����ƴϸ� �۵�����
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        //�̵��ϴ°Ŵ� ��� ���� moveAmount��ŭ�� �����Ƚð�(0.2��)���ٿ� ���缭
    }
    void Equipitem(int _index)
    {
        if (_index == previousItemIndex)
        {
            return; //�Է¹��� ���ڰ� �Ʊ� ���� ���ڶ� ������ �ƹ��ϵ� ���Ͼ
        }
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true); //itemindex ��° ������on
        if (previousItemIndex != -1) //���� �ʱ� ���� �ƴ϶��
        {
            items[previousItemIndex].itemGameObject.SetActive(false); //���� �Ʊ� ���� ���� off
        }
        previousItemIndex = itemIndex; //���ѻ���Ŭ
    }
}
