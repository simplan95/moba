using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ų ����ü ��ũ��Ʈ
public class SkillProj : MonoBehaviour
{
    private Rigidbody rigidBody;//������ �ٵ�
    public GameObject ownerObject;//�ش� ��ų�� ����� �÷��̾�
    public GameObject explosionEffect;//��Ʈ ����Ʈ
    public enum ProjType { block, penetration };//����Ÿ�� enum
    public ProjType eProjType;//����Ÿ��


    public float fMaxDistance = 8.0f;//��ų �����Ÿ�
    private float fCurrentDistance = 0.0f;//���� �����Ÿ�
    public float fStartSpeed = 5.0f;//��ų �߻�ü �ӵ�
    public float fDamage;//�ٸ���



    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        //�߻�ü�� ������ �ӵ��� �̵�
        rigidBody.velocity = transform.forward * fStartSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        //����Ÿ��� �����Ӹ��� ������Ʈ
        fCurrentDistance += fStartSpeed * Time.deltaTime;

        //�߻�ü�� max�Ÿ��� �ٴٸ��� Destroy
        if (fCurrentDistance >= fMaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Targetable>() != null && other.GetComponent<Targetable>().IsCanTargetable() && other.gameObject != ownerObject)
        {
            //�߻�ü Ÿ���� �����ϰ��
            if (eProjType == ProjType.penetration)
            {
                //�߻�ü�� �� �÷��̾ ����
                other.GetComponent<Targetable>().SetLastHitGameObject(ownerObject);

                //�� ����ũ������
                other.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                //�ǰ� ����Ʈ ����
                if (explosionEffect) Instantiate(explosionEffect, other.transform.position, other.transform.rotation);
            }
            //�߻�ü Ÿ���� ����� ���
            else if (eProjType == ProjType.block)
            {
                //�߻�ü�� �� �÷��̾ ����
                other.GetComponent<Targetable>().SetLastHitGameObject(ownerObject);

                //�� ����ũ������
                other.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                //�ǰ� ����Ʈ ����
                if (explosionEffect) Instantiate(explosionEffect, other.transform.position, other.transform.rotation);

                //�߻�ü �ı�
                Destroy(gameObject);
            }
        }
    }

    public void SetOwnerPlayer(GameObject owner)//�߻�ü�� ������ ĳ���� ���� 
    {
        ownerObject = owner;
    }
}
