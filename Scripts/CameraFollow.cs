using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라가 캐릭터를 따라가는 스크립트
public class CameraFollow : MonoBehaviour
{
    CameraScroll cameraScroll;//카메라 스크롤 스크립트(카메라 움직임 범위 제한값 가져오기용
    public Transform player;//바라볼 플레이어
    private Vector3 cameraOffset;//플레이어를 바라보는 플레이어 방향

    [Range(0.01f, 1.0f)]//움직임 감도
    public float fSmoothness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //카메라 방향 설정
        cameraOffset = transform.position - player.transform.position;

        cameraScroll = GetComponent<CameraScroll>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //카메라위치 업데이트
        Vector3 NewPos = player.position + cameraOffset;

        if (NewPos.z < cameraScroll.fMaxZ && NewPos.z > cameraScroll.fMinZ)//Z축 범위내에서만 Z축 업데이트
        {
            Vector3 tempPos = transform.position;
            tempPos.z = Mathf.Lerp(transform.position.z, NewPos.z, fSmoothness);
            transform.position = tempPos;
        }
        if (NewPos.x < cameraScroll.fMaxX && NewPos.x > cameraScroll.fMinX)//X축 범위내에서만 X축 업데이트
        {
            Vector3 tempPos = transform.position;
            tempPos.x = Mathf.Lerp(transform.position.x, NewPos.x, fSmoothness);
            transform.position = tempPos;
        }
    }
}