using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������
public class HealPack : MonoBehaviour
{
    public float fHealth = 20.0f;//ü�� ȸ�� ��ġ

    public void Use(GameObject target)
    {
        // ���޹��� ���� ������Ʈ�κ��� PlayerStatManager ������Ʈ get
        PlayerStatManager life = target.GetComponent<PlayerStatManager>();

        if (life != null)
        {
            // ü�� ȸ�� ����
            life.Heal(fHealth);
        }

        //����� ������ �ı�
        Destroy(gameObject);
    }
}
