using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 전투 관련 스크립트
public class CharacterCombat : MonoBehaviour
{
    public Targetable targetedEnemy;//타깃된 적
    private CharacterMovement charaMovement;//캐릭터 무브먼트 스크립트
    private PlayerStatManager playerStatManager;//플레이어 스탯 스크립트
    private PlayerInputManager playerInputManaget;//플레이어 인풋 스크립트
    private Animator animator;//에니메이터

    public bool bCanNomalAttack = true;//일반 공격 가능시
    public float fAttackRange = 6.0f;//공격범위
    public float fDetectRange = 6.0f;//탐지범위
    public GameObject projPrefab;//노말공격 발사체
    public Transform projSpawnPoint;//노말공격 발사체 스폰위치

    public LayerMask Target; // 탐색 대상 레이어

    // Start is called before the first frame update
    void Start()
    {
        charaMovement = GetComponent<CharacterMovement>();
        playerStatManager = GetComponent<PlayerStatManager>();
        animator = GetComponent<Animator>();
        playerInputManaget = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //타깃이 null일때  노말공격 준비 해제
        if (targetedEnemy == null)
        {
            ReleaseAttackReady();
        }

        //타깃이 null이면서 캐릭터 이동,탐색 활성화에 따라 탐색실시 
        if (targetedEnemy == null && charaMovement.bMovingFirst || targetedEnemy == null && playerInputManaget.bMovingAndSearch)
        {
            TargetDetect();
        }

        //타깃이 null이 아닐 때 공격실행
        else if (targetedEnemy != null)
        {
            if (!targetedEnemy.IsCanTargetable())//타깃이 타깃 불가상태일 때 null및 return
            {
                targetedEnemy = null;
                return;
            }

            //타깃을 향해 캐릭터가 회전
            charaMovement.CharacterRotateToTarget(targetedEnemy.transform.position);

            //플레이어 캐릭터의 목적지를 타깃으로 설정 및 fAttackRange를 정지거리로 설정
            charaMovement.navAgent.SetDestination(targetedEnemy.transform.position);
            charaMovement.navAgent.stoppingDistance = fAttackRange;

            //타깃이 사정거리 안에 있을 때 일반공격실행
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) <= fAttackRange)
            {
                if (bCanNomalAttack)
                {
                    StartCoroutine(NomalAttackInterval());//일반공격 코루틴 실행
                }
            }
        }
    }

    private IEnumerator NomalAttackInterval()
    {
        ReadyToAttack();//노말공격 준비 활성화

        //공격시간 만큼 기다림
        yield return new WaitForSeconds(playerStatManager.fAttackTime);

        if (targetedEnemy == null)//타깃이 null일때 노말공격 준비 비활성화
        {
            ReleaseAttackReady();
        }
    }

    //에니메이션 노말어택 이벤트 발생시 실행할 메서드 구현
    public void NonmalAttack()
    {
        if (targetedEnemy != null)//타깃이 null이 아닐 때
        {
            if (targetedEnemy.GetComponent<Targetable>() != null)//Targetable 스크립트를 반환할 때
            {
                playerStatManager.fSkillGauge += 5.0f;//스킬게이지 상승
                SpawnRangedProj(targetedEnemy);//발사체 소환            
            }
        }
    }

    void SpawnRangedProj(Targetable targetEnemy)
    {
        //노말 발사체 설정(데미지, 타깃, 공격을 실시한 플레이어)
        projPrefab.GetComponent<NomalAttackProj>().fDamage = playerStatManager.fAttackDamage;
        projPrefab.GetComponent<NomalAttackProj>().target = targetEnemy;
        projPrefab.GetComponent<NomalAttackProj>().SetOwnerPlayer(gameObject);

        //발사체 소환
        Instantiate(projPrefab, projSpawnPoint.transform.position, Quaternion.identity);
    }


    public void TargetDetect()
    {
        //캐릭터 주변 설정범위 만큼 탐지 실행
        Collider[] colliders =
        Physics.OverlapSphere(transform.position, fDetectRange, Target);

        // 모든 콜라이더들을 순회하면서 타깃 찾기
        for (int i = 0; i < colliders.Length; i++)
        {
            // 콜라이더로부터 컴포넌트 Get
            Targetable targetEntity = colliders[i].GetComponent<Targetable>();

            // targetEntity가 미니언일 경우
            if (targetEntity.GetComponent<Targetable>() != null
                && targetEntity.GetComponent<Targetable>().IsCanTargetable()
                && targetEntity.eTargetType == Targetable.EnemyType.minion)
            {
                // 추적 대상 설정
                targetedEnemy = targetEntity.GetComponent<Targetable>();
                break;
            }
        }
    }

    public void ReadyToAttack()//노말 공격 준비
    {
        bCanNomalAttack = false;//노말공격 가능 false
        animator.SetBool("NomalAttack", true);//애니메이터 NomalAttack true
    }

    public void ReleaseAttackReady()//노말공격 준비 비활성화
    {
        bCanNomalAttack = true;//노말공격 가능 true
        animator.SetBool("NomalAttack", false);//애니메이터 NomalAttack false
    }
}
