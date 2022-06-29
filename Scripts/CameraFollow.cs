using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶� ĳ���͸� ���󰡴� ��ũ��Ʈ
public class CameraFollow : MonoBehaviour
{
    CameraScroll cameraScroll;//ī�޶� ��ũ�� ��ũ��Ʈ(ī�޶� ������ ���� ���Ѱ� ���������
    public Transform player;//�ٶ� �÷��̾�
    private Vector3 cameraOffset;//�÷��̾ �ٶ󺸴� �÷��̾� ����

    [Range(0.01f, 1.0f)]//������ ����
    public float fSmoothness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //ī�޶� ���� ����
        cameraOffset = transform.position - player.transform.position;

        cameraScroll = GetComponent<CameraScroll>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //ī�޶���ġ ������Ʈ
        Vector3 NewPos = player.position + cameraOffset;

        if (NewPos.z < cameraScroll.fMaxZ && NewPos.z > cameraScroll.fMinZ)//Z�� ������������ Z�� ������Ʈ
        {
            Vector3 tempPos = transform.position;
            tempPos.z = Mathf.Lerp(transform.position.z, NewPos.z, fSmoothness);
            transform.position = tempPos;
        }
        if (NewPos.x < cameraScroll.fMaxX && NewPos.x > cameraScroll.fMinX)//X�� ������������ X�� ������Ʈ
        {
            Vector3 tempPos = transform.position;
            tempPos.x = Mathf.Lerp(transform.position.x, NewPos.x, fSmoothness);
            transform.position = tempPos;
        }
    }
}