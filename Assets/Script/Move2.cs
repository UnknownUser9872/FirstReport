using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class Move2 : MonoBehaviourPunCallbacks
{
    Rigidbody rb;
    public float power = 10f; //힘 값
    public GameObject playerObj;
    public float playerspeed = 10f;
    public float BeforeJumpPos;
    public Vector3 StartPos;
    [Range(0, 3)]
    public float enemySpawnCount = 0.5f; //1초동안

    void Start()
    {
        BeforeJumpPos = -4.94f;
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * playerspeed * Time.deltaTime);
                //playerObj.transform.position += Vector3.forward * playerspeed * Time.deltaTime; //로컬
                //playerObj.transform.Translate(Vector3.forward * playerspeed * Time.deltaTime, Space.Self);
                //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.forward * -playerspeed * Time.deltaTime);
                //playerObj.transform.position += Vector3.back * playerspeed * Time.deltaTime;
                //playerObj.transform.Translate(Vector3.back * playerspeed * Time.deltaTime, Space.Self);
                //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 180f, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(transform.right * -playerspeed * Time.deltaTime);
                //playerObj.transform.position += Vector3.left * playerspeed * Time.deltaTime;
                //playerObj.transform.Translate(Vector3.left *playerspeed * Time.deltaTime, Space.Self);
                //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 270f, 0));
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * playerspeed * Time.deltaTime);
                //playerObj.transform.position += Vector3.right * playerspeed * Time.deltaTime;
                //playerObj.transform.Translate(Vector3.right * playerspeed * Time.deltaTime, Space.Self);
                //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));
            }
            ////대각선회전
            //if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            //{
            //    //playerObj.transform.Translate(transform.forward * playerspeed * Time.deltaTime, Space.Self);
            //    //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, -45f, 0f));
            //}
            //if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            //{
            //    playerObj.transform.Translate(transform.forward * playerspeed * Time.deltaTime, Space.Self);
            //    //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 45f, 0f));
            //}
            //if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            //{
            //    playerObj.transform.Translate(transform.forward * playerspeed * Time.deltaTime, Space.Self);
            //    //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, -135f, 0f));
            //}
            //if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            //{
            //    playerObj.transform.Translate(transform.forward * playerspeed * Time.deltaTime, Space.Self);
            //    //playerObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 135f, 0f));
            //}
            if (Input.GetKey(KeyCode.Space) && this.transform.localPosition.y <= BeforeJumpPos)
            {
                BeforeJumpPos = transform.localPosition.y;
                this.GetComponent<Rigidbody>().
               AddForce(Vector3.up * power, ForceMode.Force);
            }
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    photonView.RPC("ColorChangeRandom",RpcTarget.All,0);
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    photonView.RPC("ColorChangeRandom", RpcTarget.All,Random.Range(0,4));
            //}
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")   //골 들어가면 씬넘김
        {
            SceneManager.LoadScene(1);
            PhotonNetwork.Instantiate("Ninja_Demo@T-Pose", Vector3.zero, Quaternion.identity, 0);
        }
        else if (collision.gameObject.tag == "Lava")
        {
            //시작지점에서 리스폰
            PhotonNetwork.Instantiate("Robot Kyle", Vector3.zero, Quaternion.identity, 0);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "SpeedPad")
        {
            StartCoroutine(SpeedPad());   //코루틴 함수 1회 실행
        }
    }
    IEnumerator SpeedPad()
    {
        playerspeed *= 1.5f;
        yield return new WaitForSeconds(enemySpawnCount);
    }
}