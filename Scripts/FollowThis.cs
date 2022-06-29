using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ������Ʈ�� ����
public class FollowThis : MonoBehaviour
{
    public GameObject ownerObject;
    public DestroyThis destroy;

    private void Start()
    {
        destroy = GetComponent<DestroyThis>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ownerObject!=null)
        {
            transform.position = ownerObject.transform.position;
        }
    }

    public void SetOwnerPlayer(GameObject owner,float maxTime)
    {
        ownerObject = owner;
        destroy.fDestroyTime = maxTime;
    }
}
