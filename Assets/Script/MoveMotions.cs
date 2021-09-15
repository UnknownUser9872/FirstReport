using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMotions : MonoBehaviour
{
    public Animator myAnimator;
    public float BeforeJumpPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
        Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)||
         Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)||
         Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)||
         Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)||
         Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            //�ִϸ����Ͱ� �޸���.
            myAnimator.SetBool("Run", true); //�Ķ���� walk�� true�� ����
        }
        else
        {
            //�ִϸ����Ͱ� �����.
            myAnimator.SetBool("Run", false); //�Ķ���� walk�� flase�� ����
        }
        if (Input.GetKey(KeyCode.Space) && this.transform.localPosition.y <= BeforeJumpPos)
        {
            BeforeJumpPos = this.transform.localPosition.y;
            myAnimator.SetBool("Jump", true);
        }
        else
        {
            myAnimator.SetBool("Jump",false);
        }
    }
}
