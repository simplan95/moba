using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 

//적(미니언) 전투 관련 스크립트
public class EnemyCombat : MonoBehaviour
{
    public Targetable targetedEnemy;//타깃된 적
    private Animator animator;//에니메이터
    private NavMeshAgent navMeshAgent; //내비메시
    private EnemyStatManager enemyStatManager;//적 스탯 매니저
    private AudioSource audioPlayer; // 오디오 소스 컴포넌트

    private bool bCanNomalAttack = true;//일반 공격 가능여부
    public GameObject projPrefab;//발사체
    public Transform projSpawnPoint;//발사체 스폰위치
    public float fAttackRange;//공격범위
    public float fDetectRange = 10.0f;//탐지범위
    public AudioClip meleeAttackSound;//근점 공격 사운드
    public enum AttackType { melee, range };//공격타입 enum
    public AttackType eAttackType;//공격타입

    public LayerMask Target; // 탐색 대상 레이어

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyStatManager = GetComponent<EnemyStatManager>();
        audioPlayer = GetComponent<AudioSource>();

        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(DetectTarget());
    }

    // Update is called once per frame
    void Update()
    {
        //타깃이 null일 때
        if (targetedEnemy == null)
        {
            bCanNomalAttack = true;//노말공격 가능 true
            animator.SetBool("NomalAttack", false);//애니메이터 NomalAttack false
        }
        //타깃이 null이 아닐 때
        else if (targetedEnemy != null)
        {
            if (!targetedEnemy.IsCanTargetable())//타깃이 타깃 가능 상태가 아닐 때 타깃 null 및 return
            {
                targetedEnemy = null;
                return;
            }

            //타깃 방향으로 회전
            Quaternion rotationToLookat = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
            float rotationY = rotationToLookat.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, rotationY, 0);

            //타깃이 사정거리 안에 있을 때 공격 실행
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) <= fAttackRange)
            {
                if (bCanNomalAttack)
                {
                    StartCoroutine(NomalAttackInterval());//일반공격 코루틴 실행
                }
            }
        }

        //NavMeshAgent의 magnitude와 Speed의 비율을 구해서 해당 캐릭터의 속도를 (0.0~1.0) 구함 
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;

        //애니메이터의 Speed 값을 설정
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    //공격 실행(공격 에니메이션에서 이벤트로 등록함)
    public void IguanaAttack()
    {
        if (targetedEnemy != null)//타깃이 null이 아닐 때
        {
            if (eAttackType == AttackType.melee)//공격타입이 근점일 때
            {
                {
                    audioPlayer.PlayOneShot(meleeAttackSound);//밀리공격 사운드 재생
                    targetedEnemy.GetComponent<CharacterStatManager>().TakeDamage(enemyStatManager.fAttackDamage);//플레이어 테이크데미지실행
                }
            }
            if (eAttackType == AttackType.range)//공격타입이 원거리 일 때
            {
                {
                    //발사체 오너 설정 및 데미지 설정
                    projPrefab.GetComponent<SkillProj>().SetOwnerPlayer(gameObject);
                    projPrefab.GetComponent<SkillProj>().fDamage = enemyStatManager.fAttackDamage;

                    //발사체 방향
                    Quaternion rotateTo = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                    //발사체 소환
                    Instantiate(projPrefab, projSpawnPoint.transform.position, rotateTo);
                }
            }
        }
    }

    private IEnumerator DetectTarget()
    {
        // 살아 있는 동안 무한 루프
        while (!enemyStatManager.bDead)
        {
            if (targetedEnemy != null)//타깃 활성화시
            {
                //타깃 위치로 이동
                navMeshAgent.SetDestination(targetedEnemy.transform.position);
                navMeshAgent.stoppingDistance = fAttackRange;
            }
            else if (targetedEnemy == null)//타깃 비활성화시
            {
                //타깃 미탐지시 캐릭터 주변 일정 범위 랜덤 이동위치 선정
                Vector3 moveTo = GetRandomPointOnNavMesh(gameObject.transform.position, fDetectRange);

                //위치로 이동
                navMeshAgent.SetDestination(moveTo);
                navMeshAgent.stoppingDistance = 0.0f;

                // 모든 콜라이더들을 순회하면서 타깃 찾기
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, fDetectRange, Target);

                // 모든 콜라이더들을 순회하면서 타깃탐색
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    Targetable targetEntity = colliders[i].GetComponent<Targetable>();

                    // 타깃이 플레이어 타입 반환시
                    if (targetEntity.GetComponent<Targetable>() != null && targetEntity.GetComponent<Targetable>().IsCanTargetable() && targetEntity.eTargetType == Targetable.EnemyType.player)
                    {
                        // 추적 대상 설정
                        targetedEnemy = targetEntity;
                        break;
                    }
                }
            }

            // 0.1초 주기로 처리 반복
            yield return new WaitForSeconds(0.1f);
        }
    }

    //타깃 미탐지시 캐릭터 주변 일정 범위 랜덤 이동위치 선정
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤 위치 지정
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // 내비메시 샘플링의 결과 정보를 저장
        NavMeshHit hit;

        // maxDistance 반경 안에서, randomPos에 가장 가까운 내비메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // 찾은 점 반환
        return hit.position;
    }

    private IEnumerator NomalAttackInterval()
    {
        bCanNomalAttack = false;//노말공격 가능 false
        animator.SetBool("NomalAttack", true);//애니메이터 NomalAttack true

        // 공격시간만큼 기다림
        yield return new WaitForSeconds(enemyStatManager.fAttackTime + 0.1f);

        if (targetedEnemy == null)//타깃이 null일때 노말공격 준비 비활성화
        {
            bCanNomalAttack = true;//노말공격 가능 true
            animator.SetBool("NomalAttack", false);//애니메이터 NomalAttack false
        }
    }
}
