using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI; // AI, 내비게이션 

//적(미니언) 스탯 매니저 
public class EnemyStatManager : CharacterStatManager//캐릭터 스탯 매니저 상속
{
    public CharacterStats scriptableEnemyStats;//스크립터블 스탯
    private Targetable targetable;//타깃 스크립트
    private Animator animator;//에니메이터
    private NavMeshAgent navMeshAgent; //내비메시
    private EnemyCombat enemyCombat;//EnemyCombat 스크립트
    private CapsuleCollider capsuleCollider;//캡슐 콜라이더


    public Slider enemyHealthSlider;//게임내 적 체력바

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        targetable = GetComponent<Targetable>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<EnemyCombat>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        targetable.SetTargetable(true);//타깃 가능활성화

        bDead = false;//초기상태는 생존
        fMaxHealth = scriptableEnemyStats.fMaxHealth;//최대체력 설정
        fCurrentHealth = fMaxHealth;//현재체력 설정
        fAttackDamage = scriptableEnemyStats.fAttackDamage;//노말 공격대미지
        fAttackSpeed = scriptableEnemyStats.fAttackSpeed;//공격 스피드
        fAttackTime = scriptableEnemyStats.fAttackTime;//공격시간

        //onDeath 이벤트 추가
        onDeath += GiveExpToPlayer;
        onDeath += PlayDeadSequence;

        //적 체력바 설정
        enemyHealthSlider.maxValue = fMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //적 체력 업데이트
        enemyHealthSlider.value = fCurrentHealth;
    }

    //플레이어에게 경험치 추가
    void GiveExpToPlayer()
    {
        //LastHitGameObject 확인 및 플레이어 인지 확인
        if (targetable.LastHitGameObject != null && targetable.LastHitGameObject.GetComponent<Targetable>().IsPlayer())
        {
            //자신을 처치한 플레이어에게 경험치 추가
            targetable.LastHitGameObject.GetComponent<CharacterLevelManager>().SetExp(Random.Range(10, 15));
        }
    }

    //캐릭터 죽음 및 일정시간뒤 Destroy
    void PlayDeadSequence()
    {

        // 사망 애니메이션 재생
        animator.SetTrigger("Dead");
        targetable.SetTargetable(false);//타깃 가능 비활성화

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        enemyCombat.enabled = false;//캐릭터 컴뱃 스크립트 비활성화
        targetable.enabled = false;//Targetable 스크립트 비활성화
        capsuleCollider.isTrigger = true;

        Destroy(gameObject, 5.0f);//5초뒤 오브젝트 Destroy
    }
}
