using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ϲ� ���� ����ü ��ũ��Ʈ (Ÿ���� ����)
public class NomalAttackProj : MonoBehaviour
{
    public float fDamage = 10.0f;//�߻�ü ������
    public Targetable target;//Ÿ��
    public GameObject explosionEffect;//�Ϲݰ��� Ÿ�� ����Ʈ
    public GameObject ownerPlayer;//���� �߻�ü�� ������ �÷��̾�

    public float fVelocity = 10.0f;//�߻�ü �ӵ�
    public bool bStopProjectile = false;//�߻�ü ���� 


    public void SetOwnerPlayer(GameObject Owner)//�ش� �߻�ü�� �߻��� ���� ������Ʈ�� ����
    {
        ownerPlayer = Owner;
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)//Ÿ���� ������� Ÿ�� ����
        {
            //Ÿ�� ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, fVelocity * Time.deltaTime);

            if (!bStopProjectile)
            {
                //�߻�ü�� Ÿ���� �Ÿ��� 0.2���ϰ� �Ǹ� Ÿ�� ������ �� �ڱ��ڽ� Destroy
                if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
                {
                    //�� ����ũ������
                    target.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                    //�߻�ü�� �� �÷��̾ ����
                    target.GetComponent<Targetable>().SetLastHitGameObject(ownerPlayer);

                    bStopProjectile = true;//�߻�ü ����

                    //�ǰ� ����Ʈ ����
                    if (explosionEffect) Instantiate(explosionEffect, target.transform.position, Quaternion.identity);

                    Destroy(gameObject);//�߻�ü�ı�
                }
            }
        }
        else if (target == null)//Ÿ�� ���� �� Ÿ���� ������ �ڱ��ڽ��� Destroy
        {
            Destroy(gameObject);
        }
    }
}
