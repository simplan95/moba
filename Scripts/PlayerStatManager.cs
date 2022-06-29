using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI; // AI, 내비게이션 

//플레이어 스탯 매니저
public class PlayerStatManager : CharacterStatManager//캐릭터 스탯 매니저 상속
{
    public CharacterStats scriptableStats;//스크립터블 스탯
    public Targetable targetable;//타깃 스크립트
    private CharacterCombat characterCombat;//캐릭터 전투 스크립트
    private CharacterMovement characterMovement;//캐릭터 이동 스크립트
    private Animator animator;//에니메이터
    private NavMeshAgent navMeshAgent;//내비메시
    private AudioSource audioPlayer;//플레이어 오디오 재생기

    public AudioClip getItemSound;//플레이어 아이템 획득 소리
    public AudioClip levelUpSound;//플레이어 레벨업 소리
    public Slider gameHealthSlider;//게임내 플레이어 체력바
    public Slider HUDHealthSlider;//HUD 체력바
    public Slider skillGaugageSlider;//스킬게이지 슬라이더

    [Header("Custom Stats")]
    [Range(0, 100)]
    public float fSkillGauge = 0.0f;//스킬게이지
    public float fSkillGaugeCoefficient { get; private set; }//스킬게이지 계수(스킬 데미지 및 범위 증가 보너스)

    public float fskill1Damage = 10.0f;//스킬1 데미지
    public float fskill2Damage = 10.0f;//스킬2 데미지
    public float fskill3Damage = 5.0f;//스킬3 데미지
    public float fskill4Damage = 100.0f;//스킬4 데미지

    // Start is called before the first frame update
    void Start()
    {
        targetable = GetComponent<Targetable>();
        characterCombat = GetComponent<CharacterCombat>();
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioPlayer = GetComponent<AudioSource>();

        targetable.SetTargetable(true);//타깃 가능활성화

        //ondeath 이벤트 추가
        onDeath += PlayDeadSequence;

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void OnEnable()
    {
        // CharacterStatManager OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        bDead = false;//초기상태는 생존
        fMaxHealth = scriptableStats.fMaxHealth;//최대체력 설정
        fCurrentHealth = fMaxHealth;//현재체력 설정
        fAttackDamage = scriptableStats.fAttackDamage;//노말 공격대미지
        fAttackSpeed = scriptableStats.fAttackSpeed;//공격 스피드
        fAttackTime = scriptableStats.fAttackTime;//공격시간
        fSkillGauge = 0.0f;//스킬게이지

        targetable.SetTargetable(true);//타깃 가능활성화

        //플레이어 체력바 최대체력 설정
        HUDHealthSlider.maxValue = fMaxHealth;
        gameHealthSlider.maxValue = fMaxHealth;

        //스킬게이지 최대치 설정
        skillGaugageSlider.maxValue = 100.0f;

    }


    // Update is called once per frame
    void Update()
    {
        //HUD 체력바와 플레이어 체력바를 바인딩 및 현제체력 업데이트
        HUDHealthSlider.value = fCurrentHealth;
        gameHealthSlider.value = HUDHealthSlider.value;

        if (fSkillGauge > 100.0f)
        {
            fSkillGauge = 100.0f;
        }
        if (fSkillGauge > 0.0f)
        {
            //스킬게이지가 0 이상 일때 일정시간마다 감소
            fSkillGauge -= 1.0f * Time.deltaTime;
        }

        skillGaugageSlider.value = fSkillGauge;//스킬게이지바 업데이트

        fSkillGaugeCoefficient = 1 + (fSkillGauge / 100);//스킬 게이지 계수 업데이트
    }

    void PlayDeadSequence()//플레이어 사망시
    {
        // 사망 애니메이션 재생
        animator.SetTrigger("Dead");
        targetable.SetTargetable(false);//타깃 가능 비활성화
        //animator.SetBool("MinionDead", true);//애니메이터 미니언 사망 true

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        characterCombat.targetedEnemy = null;//타깃 null
        characterCombat.enabled = false;//캐릭터 컴뱃 스크립트 비활성화
        characterMovement.enabled = false;//캐릭터 무브먼트 스크립트 비활성화
        targetable.enabled = false;//Targetable 스크립트 비활성화

        StartCoroutine(GameOver());//5초뒤 게임오버 화면 출력
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public void Heal(float amount)//캐릭터 체력회복
    {
        fCurrentHealth += amount;
        if (fCurrentHealth >= fMaxHealth)
        {
            fCurrentHealth = fMaxHealth;
        }
    }

    //레벨업 시 스탯 업
    public void StatsUp()//
    {
        //최대체력 증가
        fMaxHealth += 10.0f;
        HUDHealthSlider.maxValue = fMaxHealth;
        gameHealthSlider.maxValue = fMaxHealth;

        //노말공격및 스킬 데미지 증가
        fAttackDamage += 1.0f;
        fskill1Damage += 1.0f;
        fskill2Damage += 1.0f;
        fskill3Damage += 0.5f;

        //레벨업 사운드 재생
        audioPlayer.PlayOneShot(levelUpSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        //캐릭터 생존시 아이템 사용가능
        if (!bDead)
        {
            //HealPack 컴포넌트 가져오기
            HealPack item = other.GetComponent<HealPack>();

            //충돌한 대상이 HealPack스크립트를 반환시
            if (item != null)
            {
                //힐팩 사용
                item.Use(gameObject);
                //아이템 획득 소리 재생
                audioPlayer.PlayOneShot(getItemSound);
            }
        }
    }
}
