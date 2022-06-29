using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 줌인 줌아웃 및 카메라 최대 최소 이동범위 설정
public class CameraScroll : MonoBehaviour
{
    public Camera cam;//카메라
    public float fZoomSpeed;//줌스피드
    private float fCamFOV;//카메라 FOV
    private float fMouseScrollInput;//마우스 휠 입력값

    //카메라 최대, 최소 위치
    public float fMaxZ { get; private set; }
    public float fMinZ { get; private set; }
    public float fMaxX { get; private set; }
    public float fMinX { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //현재 카메라 FOV 
        fCamFOV = cam.fieldOfView;

        fMaxZ = -12.0f;
        fMinZ = -44.0f;
        fMaxX = 6.5f;
        fMinX = -6.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 휠 입력값 
        fMouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        //카메라 FOV를 마우스 휠 스크롤에 따라서 조정 및 범위제한
        fCamFOV -= fMouseScrollInput * fZoomSpeed;
        fCamFOV = Mathf.Clamp(fCamFOV, 10f, 25f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fCamFOV, fZoomSpeed);
    }
}
