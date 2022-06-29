using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSlider : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        //게임 내 체력바회전을  카메라쪽으로 고정
        transform.LookAt(Camera.main.transform);
    }
}
