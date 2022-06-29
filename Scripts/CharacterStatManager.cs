using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//델리게이트

//캐릭터 스탯 매니저 스크립트(사망여부, 데미지처리, 스탯)
public class CharacterStatManager : MonoBehaviour
{
    public float fCurrentHealth { get; protected set; } // 현재 체력
    public bool bDead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    public float fMaxHealth;//최대체력
    public float fAttackDamage;//노말 공격대미지
    public float fAttackSpeed;//공격 스피드
    public float fAttackTime;//공격시간


    protected virtual void OnEnable()
    {

    }


    public virtual void TakeDamage(float damage)
    {
        // 데미지만큼 체력 감소
        fCurrentHealth -= damage;

        // 체력이 0 이하 및 아직 죽지 않았다면 사망 처리 실행
        if (fCurrentHealth <= 0 && !bDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        // 캐릭터 사망
        bDead = true;
    }
}
