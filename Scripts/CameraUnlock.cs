using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶� ĳ���͸� ������ �ʰ� Ŀ�� ��ġ�� ���� ī�޶� �̵�
public class CameraUnlock : MonoBehaviour
{
    CameraScroll cameraScroll;//ī�޶� ��ũ�� ��ũ��Ʈ(ī�޶� ������ ���� ���Ѱ� ���������)
    public float fCameraSpeed = 20.0f;//ī�޶� ������ �ӵ�
    public float fScreenBoundary = 0.9f;//��ũ�� �ٿ����

    void Start()
    {
        cameraScroll = GetComponent<CameraScroll>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = transform.position;

        //Ŀ�� ��ġ�� ���� ��ũ�� �ٿ���� ������ ���� ��� ī�޶� ����
        if (Input.mousePosition.y >= Screen.height * fScreenBoundary && camPos.z < cameraScroll.fMaxZ)
        {
            camPos.z += fCameraSpeed * Time.deltaTime;
        }

        //Ŀ�� ��ġ�� �Ʒ��� ��ũ�� �ٿ���� ������ ���� ��� ī�޶� �Ʒ���
        if (Input.mousePosition.y <= Screen.height * (1 - fScreenBoundary) && camPos.z > cameraScroll.fMinZ)
        {
            camPos.z -= fCameraSpeed * Time.deltaTime;
        }

        //Ŀ�� ��ġ�� ������ ��ũ�� �ٿ���� ������ ���� ��� ī�޶� ����������
        if (Input.mousePosition.x >= Screen.width * fScreenBoundary && camPos.x < cameraScroll.fMaxX)
        {
            camPos.x += fCameraSpeed * Time.deltaTime;
        }

        //Ŀ�� ��ġ�� ���� ��ũ�� �ٿ���� ������ ���� ��� ī�޶� ��������
        if (Input.mousePosition.x <= Screen.width * (1 - fScreenBoundary) && camPos.x > cameraScroll.fMinX)
        {
            camPos.x -= fCameraSpeed * Time.deltaTime;
        }

        transform.position = camPos;//ī�޶� ��ġ ������Ʈ
    }
}
