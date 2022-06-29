using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶� ���� �ܾƿ� �� ī�޶� �ִ� �ּ� �̵����� ����
public class CameraScroll : MonoBehaviour
{
    public Camera cam;//ī�޶�
    public float fZoomSpeed;//�ܽ��ǵ�
    private float fCamFOV;//ī�޶� FOV
    private float fMouseScrollInput;//���콺 �� �Է°�

    //ī�޶� �ִ�, �ּ� ��ġ
    public float fMaxZ { get; private set; }
    public float fMinZ { get; private set; }
    public float fMaxX { get; private set; }
    public float fMinX { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //���� ī�޶� FOV 
        fCamFOV = cam.fieldOfView;

        fMaxZ = -12.0f;
        fMinZ = -44.0f;
        fMaxX = 6.5f;
        fMinX = -6.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //���콺 �� �Է°� 
        fMouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        //ī�޶� FOV�� ���콺 �� ��ũ�ѿ� ���� ���� �� ��������
        fCamFOV -= fMouseScrollInput * fZoomSpeed;
        fCamFOV = Mathf.Clamp(fCamFOV, 10f, 25f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fCamFOV, fZoomSpeed);
    }
}
