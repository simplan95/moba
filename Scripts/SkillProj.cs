using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 투사체 스크립트
public class SkillProj : MonoBehaviour
{
    private Rigidbody rigidBody;//리지드 바디
    public GameObject ownerObject;//해당 스킬을 사용한 플레이어
    public GameObject explosionEffect;//히트 이펙트
    public enum ProjType { block, penetration };//공격타입 enum
    public ProjType eProjType;//공격타입


    public float fMaxDistance = 8.0f;//스킬 사정거리
    private float fCurrentDistance = 0.0f;//현재 사정거리
    public float fStartSpeed = 5.0f;//스킬 발사체 속도
    public float fDamage;//다마게



    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        //발사체를 설정한 속도로 이동
        rigidBody.velocity = transform.forward * fStartSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        //현재거리를 프레임마다 업데이트
        fCurrentDistance += fStartSpeed * Time.deltaTime;

        //발사체가 max거리에 다다르면 Destroy
        if (fCurrentDistance >= fMaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Targetable>() != null && other.GetComponent<Targetable>().IsCanTargetable() && other.gameObject != ownerObject)
        {
            //발사체 타입이 관통일경우
            if (eProjType == ProjType.penetration)
            {
                //발사체를 쏜 플레이어를 설정
                other.GetComponent<Targetable>().SetLastHitGameObject(ownerObject);

                //적 테이크데미지
                other.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                //피격 이펙트 생성
                if (explosionEffect) Instantiate(explosionEffect, other.transform.position, other.transform.rotation);
            }
            //발사체 타입이 블록일 경우
            else if (eProjType == ProjType.block)
            {
                //발사체를 쏜 플레이어를 설정
                other.GetComponent<Targetable>().SetLastHitGameObject(ownerObject);

                //적 테이크데미지
                other.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                //피격 이펙트 생성
                if (explosionEffect) Instantiate(explosionEffect, other.transform.position, other.transform.rotation);

                //발사체 파괴
                Destroy(gameObject);
            }
        }
    }

    public void SetOwnerPlayer(GameObject owner)//발사체를 생성한 캐릭터 설정 
    {
        ownerObject = owner;
    }
}
