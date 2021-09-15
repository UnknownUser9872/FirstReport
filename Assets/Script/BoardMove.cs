using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class BoardMove : MonoBehaviourPunCallbacks
{
    public GameObject JumpPos;
    public float power = 10f; //Èû °ª
    public float playerspeed = 3f;
    public float BeforeJumpPos;
    public GameObject playerObj;
    public Animator myAnimator;
    public Text Lifetext;
    public int Life = 3;
    public Text Startext;
    public int star = 0;
    [Range(0, 3)]
    public float aboutSpeedTime = 0.5f; //0.5ÃÊ µ¿¾È

    void Start()
    {

    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerObj.transform.Translate(Vector3.forward * playerspeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerObj.transform.Translate(Vector3.back * playerspeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.A))
            {
                playerObj.transform.Translate(Vector3.left * playerspeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                playerObj.transform.Translate(Vector3.right * playerspeed * Time.deltaTime, Space.Self);
            }
            if (this.transform.position.y <= 3.5f)
            {
                //this.transform.Rotate(0f, Input.GetAxis("Mouse X") * speed, 0f, Space.Self);
                playerObj.transform.Rotate(0f, Input.GetAxis("Mouse X") * playerspeed, 0f, Space.Self);
            }
            if (Life == 0)
            {
                Destroy(playerObj);
                Destroy(Lifetext);
                Destroy(Startext);
                PhotonNetwork.Instantiate("Ninja_Demo@T-Pose", Vector3.zero, Quaternion.identity, 0);
                Instantiate(Lifetext);
                Instantiate(Startext);
                Life = 3;
                star = 0;
            }
        }
    }
    private void OnGUI()
    {
        Lifetext.text = "" + Life;
        Startext.text = "" + star;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pad"&&Input.GetKey(KeyCode.Space) && this.transform.localPosition.y <= BeforeJumpPos)
        {
            BeforeJumpPos = JumpPos.transform.localPosition.y;
            this.GetComponent<Rigidbody>().
           AddForce(Vector3.up * power, ForceMode.Force);
        }
        if (collision.gameObject.tag == "Star")
        {
            Destroy(collision.other.gameObject);
            star++;
        }
        else if (collision.gameObject.tag == "NiddlePad")
        {
            Life--;
        }
        else if (collision.gameObject.tag == "RedPad")
        {
            StartCoroutine(RedPad());
        }
        else if (collision.gameObject.tag == "MintPad")
        {
            StartCoroutine(MintPad());
        }
    }
    IEnumerator RedPad()
    {
        playerspeed--;
        yield return new WaitForSeconds(aboutSpeedTime);
    }
    IEnumerator MintPad()
    {
        playerspeed++;
        yield return new WaitForSeconds(aboutSpeedTime);
    }
}