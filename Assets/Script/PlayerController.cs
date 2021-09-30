using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;//현재 게임 클라이언트가 쓰는 해쉬테이블 사용
using Photon.Realtime;
using static GameController;  //게임 컨트롤러 사용
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks/*다른 포톤 반응 받아들이기*/, IDamageable//엔터페이스불러오기
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;
    public bool isBoss;
    Renderer capsuleColor;
    public int itemIndex;
    public int previousItemIndex = -1;//기본 아이템 값 없도록
    //마우스감도 뛰는속도 걷는속도 점프힘 뛰기걷기바꿀때 가속시간
    float verticalLookRotation;
    bool grounded;//점프를 위한 바닥체크
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;//실제 이동거리
    const float maxHealth = 100f; //풀피
    public float currentHealth = maxHealth; //지금피
    public static PlayerController playerController;
    public TMP_Text healthText;
    public TMP_Text bossText;
    public Canvas canvas;
    bool isMove;
    public float speed = 10f;
    bool turnCamera;

    PlayerManager playerManager;   //플레이어매니저 선언
    Rigidbody rb;
    PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        isBoss = false;  //초기 술래 끄기
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        playerController = GetComponent<PlayerController>();  //플레이어 컨트롤러를 선언
        GC.Players.Add(this);  //게임컨트롤러 목록에 얘네들 추가
        Debug.Log("Add complete");  //추가완료
    }

    void Start()
    {
        this.transform.parent = GameObject.FindWithTag("Playercheck").transform;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            //내꺼 아니면 카메라 없애기
            Destroy(rb);
            //내거아니면 리지드 바디 없애주기
            canvas.gameObject.SetActive(false);
        }
        capsuleColor = gameObject.GetComponent<Renderer>();  //술래만 무기 가지게, 처음에는 아무도 무기안낌
    }

    void Update()
    {
        if (!PV.IsMine)
            return;//내꺼아니면 작동안함
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
                if (Input.GetKeyDown((i + 1).ToString()))//ToString으로 하면 입력받는 String을 숫자로 표현할 수 있다. 
                {
                    EquipItem(i);
                    //숫자키 1 2번으로 아이템 장착 가능
                    break;
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)//마우스 스크롤 움직이면
            {
                if (itemIndex >= items.Length - 1)//만약 아이템 목록끝에 다다르면
                {
                    EquipItem(0);//맨처음 아이템으로
                }
                else
                {
                    EquipItem(itemIndex + 1);//아니면 다음 아이템으로
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)//마우스 스크롤 반대로 움직이면
            {
                if (itemIndex <= 0)//아이템 목록 맨처음보다 뒤로가면?
                {
                    EquipItem(items.Length - 1);//맨 끝 아이템으로
                }
                else
                {
                    EquipItem(itemIndex - 1);//아니면 이전 아이템으로
                }
            }
            if (Input.GetMouseButtonDown(0) && (items[0].itemGameObject.activeSelf == true || items[1].itemGameObject.activeSelf == true)) //마우스 좌클릭시
            {
                if (items[0].itemGameObject.activeSelf == true)
                {
                    TakeDamage(5);  //술래는 무기1 사용시 5까임
                }
                if (items[1].itemGameObject.activeSelf == true)
                {
                    TakeDamage(10);  //술래는 무기2 사용시 10까임
                }
                items[itemIndex].Use();//들고있는 아이템 사용
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
        if (transform.position.y < -10f) //맵 밖으로 나가면
        {
            Die();
        }
    }

    void Look()
    {
        if (turnCamera == true)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
            //마우스 움직이는 정도*민감도만큼 각도 움직이기
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
            //마우스 움직이는 정도*민감도만큼 각도 값 받기
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            //y축 -90도에서 90도만 값으로 받음
            cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
            //받은 각도로 카메라도 돌려줌
        }
        else
        {
            cameraHolder.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
            //마우스 움직이는 정도*민감도만큼 각도 움직이기
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
            //마우스 움직이는 정도*민감도만큼 각도 값 받기
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            //y축 -90도에서 90도만 값으로 받음
            cameraHolder.transform.localEulerAngles = new Vector3((Vector3.left * verticalLookRotation).x, cameraHolder.transform.localEulerAngles.y, cameraHolder.transform.localEulerAngles.z);
            //받은 각도로 카메라도 돌려줌
        }
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //벡더방향을 가지지만 크기는 1로 노말라이즈
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        //왼쪽 쉬프트가 누르면 뛰는속도, 나머지는 걷는속도로하기
        //smoothTime만큼에 걸쳐서 이동해주기. 
    }

    void Jump()
    {
        print(grounded);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)//땅위에서 스페이스바 누르면
        {
            rb.AddForce(transform.up * jumpForce);//점프력만큼위로 힘받음
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;//입력받은 숫자가 아까 받은 숫자랑 똑같으면 아무일도 안해준다.  
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);//itemIndex번쨰 아이템 on
        if (previousItemIndex != -1)//만약 초기 상태가 아니라면
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
            //내가 아까 꼈던 아이템은 off
        }
        previousItemIndex = itemIndex;//무한 사이클

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            //이제 hash[itemindex]가 호출되면 현재 아이템번호가 호출된다.    
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //포톤으로 모든 사람에게 내가 현재끼고 있는 아이템 번호를 알려준다.  
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //다른 플레이어의 아이템 번호 받아들이기
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)//내꺼가 아니라 다른사람꺼일때
        {
            EquipItem((int)changedProps["itemIndex"]);
            //끼고있는 아이템 정보 받아들이기
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
            return;//내꺼아니면 작동안함
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        //이동하는거는 계산 끝난 moveAmount만큼만 고정된시간(0.2초)마다에 맞춰서
    }

    public void TakeDamage(float damage)
    {
        //IDamageable 인터페이스에 있는 함수 재정의
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        //피해를 입힌사람이 해당 이름을 가진 함수를 RpcTakget(지금은 모든 플레이어)에게 적용 되도록 호출
        //Rpc를 통해 받은 피해를 모두에게 전달한다.
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        //모두에게 전달됨
        if (!PV.IsMine)
        {
            return;   //피해입은 놈 아니면 실행안됨
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
        //술래를 정해주는 Rpc
        isBoss = _isBoss;
        Debug.Log("Boss " + isBoss);
        items[0].itemGameObject.SetActive(true);
        previousItemIndex = 0;
        this.transform.parent = GameObject.FindWithTag("Bosscheck").transform;
    }
    [PunRPC]
    void RPC_SetColor()   //술래 색 변경
    {
        capsuleColor.material.color = Color.black;
    }
}
