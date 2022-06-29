using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 오브젝트를 따라감
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
