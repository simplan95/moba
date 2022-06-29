using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라가 캐릭터를 따라가지 않고 커서 위치에 따라 카메라 이동
public class CameraUnlock : MonoBehaviour
{
    CameraScroll cameraScroll;//카메라 스크롤 스크립트(카메라 움직임 범위 제한값 가져오기용)
    public float fCameraSpeed = 20.0f;//카메라 움직임 속도
    public float fScreenBoundary = 0.9f;//스크린 바운더리

    void Start()
    {
        cameraScroll = GetComponent<CameraScroll>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = transform.position;

        //커서 위치가 위쪽 스크린 바운더리 영역에 있을 경우 카메라 위로
        if (Input.mousePosition.y >= Screen.height * fScreenBoundary && camPos.z < cameraScroll.fMaxZ)
        {
            camPos.z += fCameraSpeed * Time.deltaTime;
        }

        //커서 위치가 아래쪽 스크린 바운더리 영역에 있을 경우 카메라 아래로
        if (Input.mousePosition.y <= Screen.height * (1 - fScreenBoundary) && camPos.z > cameraScroll.fMinZ)
        {
            camPos.z -= fCameraSpeed * Time.deltaTime;
        }

        //커서 위치가 오른쪽 스크린 바운더리 영역에 있을 경우 카메라 오른쪽으로
        if (Input.mousePosition.x >= Screen.width * fScreenBoundary && camPos.x < cameraScroll.fMaxX)
        {
            camPos.x += fCameraSpeed * Time.deltaTime;
        }

        //커서 위치가 왼쪽 스크린 바운더리 영역에 있을 경우 카메라 왼쪽으로
        if (Input.mousePosition.x <= Screen.width * (1 - fScreenBoundary) && camPos.x > cameraScroll.fMinX)
        {
            camPos.x -= fCameraSpeed * Time.deltaTime;
        }

        transform.position = camPos;//카메라 위치 업데이트
    }
}
