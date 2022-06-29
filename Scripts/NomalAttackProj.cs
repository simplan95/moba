using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일반 공격 투사체 스크립트 (타깃을 따라감)
public class NomalAttackProj : MonoBehaviour
{
    public float fDamage = 10.0f;//발사체 데미지
    public Targetable target;//타깃
    public GameObject explosionEffect;//일반공격 타격 이펙트
    public GameObject ownerPlayer;//공격 발사체를 생성한 플레이어

    public float fVelocity = 10.0f;//발사체 속도
    public bool bStopProjectile = false;//발사체 멈춤 


    public void SetOwnerPlayer(GameObject Owner)//해당 발사체를 발사한 게임 오브젝트를 설정
    {
        ownerPlayer = Owner;
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)//타깃이 있을경우 타깃 추적
        {
            //타깃 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, fVelocity * Time.deltaTime);

            if (!bStopProjectile)
            {
                //발사체와 타깃의 거리가 0.2이하가 되면 타깃 데미지 및 자기자신 Destroy
                if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
                {
                    //적 테이크데미지
                    target.GetComponent<CharacterStatManager>().TakeDamage(fDamage);

                    //발사체를 쏜 플레이어를 설정
                    target.GetComponent<Targetable>().SetLastHitGameObject(ownerPlayer);

                    bStopProjectile = true;//발사체 멈춤

                    //피격 이펙트 생성
                    if (explosionEffect) Instantiate(explosionEffect, target.transform.position, Quaternion.identity);

                    Destroy(gameObject);//발사체파괴
                }
            }
        }
        else if (target == null)//타깃 추적 중 타깃이 없으면 자기자신을 Destroy
        {
            Destroy(gameObject);
        }
    }
}
