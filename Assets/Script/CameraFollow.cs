using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetPlayer;
    public GameObject targetPos;
    public float cameraSpeed, halfHeight;
    public Camera cam;
    public Vector3 targetPosition;
    private float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraMove());
    }

    // Update is called once per frame
    void Update()
    {

        if (targetPos.gameObject != null)
        {
            // target 위치 찾기
            targetPosition.Set(targetPos.transform.position.x, targetPos.transform.position.y, targetPos.transform.position.z);
            // target 위치로 카메라 속도에 맞게 이동
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, cameraSpeed);
            //this.transform.LookAt(targetPlayer.transform.position);
        }
        if (this.transform.position.y <= 3.5f)
        {
            cam.transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);
        }
    }
    IEnumerator CameraMove()
    {
        float i = 0;
        while (i < 3f)
        {
            this.transform.LookAt(targetPlayer.transform.position);
            yield return new WaitForSeconds(0.001f);
            i += 0.001f;
        }
    }
}
