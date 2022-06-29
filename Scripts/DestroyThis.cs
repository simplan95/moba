using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//설정 시간뒤 파괴
public class DestroyThis : MonoBehaviour
{
    public float fDestroyTime = 3.0f;
    private float fCurrentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {

        fCurrentTime += Time.deltaTime;
        if (fCurrentTime > fDestroyTime)
            Destroy(gameObject);

    }
}
