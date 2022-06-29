using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//힐팩 아이템
public class HealPack : MonoBehaviour
{
    public float fHealth = 20.0f;//체력 회복 수치

    public void Use(GameObject target)
    {
        // 전달받은 게임 오브젝트로부터 PlayerStatManager 컴포넌트 get
        PlayerStatManager life = target.GetComponent<PlayerStatManager>();

        if (life != null)
        {
            // 체력 회복 실행
            life.Heal(fHealth);
        }

        //사용후 아이템 파괴
        Destroy(gameObject);
    }
}
