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
            //애니메이터가 달린다.
            myAnimator.SetBool("Run", true); //파라미터 walk을 true로 변경
        }
        else
        {
            //애니메이터가 멈춘다.
            myAnimator.SetBool("Run", false); //파라미터 walk을 flase로 변경
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
