using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//��������Ʈ

//ĳ���� ���� �Ŵ��� ��ũ��Ʈ(�������, ������ó��, ����)
public class CharacterStatManager : MonoBehaviour
{
    public float fCurrentHealth { get; protected set; } // ���� ü��
    public bool bDead { get; protected set; } // ��� ����
    public event Action onDeath; // ����� �ߵ��� �̺�Ʈ

    public float fMaxHealth;//�ִ�ü��
    public float fAttackDamage;//�븻 ���ݴ����
    public float fAttackSpeed;//���� ���ǵ�
    public float fAttackTime;//���ݽð�


    protected virtual void OnEnable()
    {

    }


    public virtual void TakeDamage(float damage)
    {
        // ��������ŭ ü�� ����
        fCurrentHealth -= damage;

        // ü���� 0 ���� �� ���� ���� �ʾҴٸ� ��� ó�� ����
        if (fCurrentHealth <= 0 && !bDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {
            onDeath();
        }

        // ĳ���� ���
        bDead = true;
    }
}
