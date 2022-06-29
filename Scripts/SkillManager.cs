using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//AI
using UnityEngine.UI;//UI

//스킬 구현 관련 스크립트
public class SkillManager : MonoBehaviour
{
    private enum SkillType { Skill1, Skill2, Skill3, Skill4 };//스킬 타입 enum
    public Vector3 skillPointer { get; private set; }//스킬 포인터
    public Vector3 skillPointerInRange { get; private set; }//범위내 스킬 포인터(UI 업데이트)
    public bool bCanUseSkill { get; private set; }//스킬 사용 가능여부


    private SkillUIManager skillUI;//스킬 UI스크립트
    private CharacterMovement charaMovement;//캐릭터 Movement 스크립트
    private Animator animator;//에니메이터
    private PlayerStatManager playerStatManager;//플레이어 스탯
    private NavMeshAgent navAgent;//내비 메시
    private Quaternion rotateTo;//스킬 방향
    private SkillType eSkillSet;//현재 스킬타입

    public Transform projSpawnPoint;//발사체 스폰위치
    public LayerMask Target; // 탐색 대상 레이어

    [Header("Skill1")]
    public KeyCode skill1;//스킬 버튼
    public float fCoolDown1 = 5.0f;//쿨다운 시간
    public GameObject skill1Proj;//스킬 1발사체
    public bool bIsCoolDown1 { get; private set; }//스킬 쿨다운 여부
    public float fCurrentCoolDown1 { get; private set; }//현재 스킬 쿨다운시간
    public bool bSkill1Ready { get; private set; }//스킬 준비


    [Header("Skill2")]
    public KeyCode skill2;
    public float fCoolDown2 = 8.0f;//스킬 2 쿨다운 시간
    public GameObject skill2Prefab;//스킬 2 프리팹
    public float fMaxSkill2Range;//스킬2 범위
    private Vector3 skill2Position;//스킬 2프리팹 소환 위치
    public bool bIsCoolDown2 { get; private set; }//스킬 사용 가능여부
    public float fCurrentCoolDown2 { get; private set; }//현재 스킬 쿨다운시간
    public bool bSkill2Ready { get; private set; }//스킬 준비

    [Header("Skill3")]
    public KeyCode skill3;//스킬버튼
    public float fCoolDown3 = 10.0f;//스킬 3 쿨다운 시간
    public GameObject skill3Prefab;//스킬 3 프리팹
    public float fMaxSkill3ExistenceTime = 4.0f;//스킬 3 최대 지속 시간
    private float fCurrentSkill3ExistenceTime;//스킬 3 현재 지속 시간
    private bool bSkill3Activate;//스킬3 데미지 활성화 여부
    public float fSkill3DamageInterval = 1.0f;//스킬3 데미지 시간 간격
    private float fSkill3DamageIntervalUpdate;//스킬3 데미지 시간 간격 업데이트
    public bool bIsCoolDown3 { get; private set; }//스킬 사용 가능여부
    public float fCurrentCoolDown3 { get; private set; }//현재 스킬 쿨다운시간
    public bool bSkill3Ready { get; private set; }//스킬 준비

    [Header("Skill4")]
    public KeyCode skill4;//스킬버튼
    public GameObject skill4Prefab;//스킬 4 프리팹
    public bool bSkill4Ready { get; private set; }//스킬 준비

    // Start is called before the first frame update
    void Start()
    {
        skillUI = GetComponent<SkillUIManager>();
        charaMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        playerStatManager = GetComponent<PlayerStatManager>();
        navAgent = GetComponent<NavMeshAgent>();

        bCanUseSkill = true;
    }

    // Update is called once per frame
    void Update()
    {
        //스크린 상의 마우스 위치를 레이변수에 할당 및 Hit 변수 선언
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //스킬 포인터 업데이트를 위한 raycast 실행
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
        {
            //레이캐스트 실행 후 해당 지점에 skillPointer위치 업데이트
            skillPointer = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        //스킬포인터의 방향설정
        Vector3 hitPosDir = skillPointer - transform.position;
        //캐릭터의 타깃지점 방향을 정규화  
        Vector3 hitPosDirNomal = hitPosDir.normalized;
        //캐릭터와 타깃지점의 거리
        float fDistance = Vector3.Distance(skillPointer, transform.position);
        //캐릭터와 타깃지점의 거리를 구한 후 사거리와 비교
        fDistance = Mathf.Min(fDistance, fMaxSkill2Range);

        //캐릭터 위치에서 타깃지점까지 더한 위치를 구함
        Vector3 newhitPos = transform.position + (hitPosDirNomal * fDistance);
        //y축 위치는 고정시킴
        newhitPos = new Vector3(newhitPos.x, 0.3f, newhitPos.z);
        //skill2Canvas 위치 및 skill2 프리팹 위치 설정
        skillPointerInRange = newhitPos;

        Skill1_Update();//스킬 1 업데이트
        Skill2_Update();//스킬 2 업데이트
        Skill3_Update();//스킿 3 업데이트
        Skill4_Update();//스킿 3 업데이트
    }

    private IEnumerator SkillAnimation()
    {
        animator.SetBool("SkillAttack", true);//스킬 애니메이션 활성화
        charaMovement.bCanRotation = false;//스킬 사용 중엔 캐릭터 회전을 잠금
        navAgent.angularSpeed = 0.0f;
        bCanUseSkill = false;//스킬 사용 불가

        //스킬 애니메이션 시간만큼 기다림
        yield return new WaitForSeconds(1.6f);

        animator.SetBool("SkillAttack", false);//스킬 애니메이션 비활성화
        charaMovement.bCanRotation = true;//캐릭터 회전잠금 해제
        navAgent.angularSpeed = 720.0f;
        bCanUseSkill = true;//스킬 사용 가능
    }

    public void SkillActivate()
    {
        switch (eSkillSet)
        {
            case SkillType.Skill1:
                //스킬 데미지 설정(기본 데미지 * 스킬 게이지 계수)
                skill1Proj.GetComponent<SkillProj>().fDamage = playerStatManager.fskill1Damage * playerStatManager.fSkillGaugeCoefficient;
                //스킬 발사체 오너 설정
                skill1Proj.GetComponent<SkillProj>().SetOwnerPlayer(gameObject);
                //스킬게이지 계수에 따라 스킬의 크기 설정
                skill1Proj.transform.localScale = new Vector3(1.5f * playerStatManager.fSkillGaugeCoefficient,
                                                              1.5f * playerStatManager.fSkillGaugeCoefficient,
                                                              1.5f * playerStatManager.fSkillGaugeCoefficient);
                //스킬 발사체 생성
                Instantiate(skill1Proj, projSpawnPoint.transform.position, rotateTo);
                break;

            case SkillType.Skill2:
                //스킬게이지 계수에 따라 스킬의 크기 설정
                skill2Prefab.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient,
                                                                1f * playerStatManager.fSkillGaugeCoefficient,
                                                                1f * playerStatManager.fSkillGaugeCoefficient);
                //스킬2 프리팹 생성
                Instantiate(skill2Prefab, skill2Position, rotateTo);
                //해당 범위 탐지 후 데미지처리
                SphereOverlap(skill2Position, 1.5f * playerStatManager.fSkillGaugeCoefficient, playerStatManager.fskill2Damage);
                break;

            case SkillType.Skill3:
                //DoNothing
                break;

            case SkillType.Skill4:
                //스킬 4 프리팹 소환
                Instantiate(skill4Prefab, transform.position, rotateTo);
                //범위 탐지 후 데미지 처리
                SphereOverlap(transform.position, 5.0f, playerStatManager.fskill4Damage);

                bSkill4Ready = false;
                break;

        }

    }

    //해당 지역 탐색후 데미지 처리
    void SphereOverlap(Vector3 pos, float range, float damage)
    {
        //설정 지역 유닛3 만큼 탐지 실행
        Collider[] colliders =
        Physics.OverlapSphere(pos, range, Target);

        // 모든 콜라이더들을 순회하면서 타깃 찾기
        for (int i = 0; i < colliders.Length; i++)
        {
            // 콜라이더로부터 컴포넌트 Get
            Targetable targetEntity = colliders[i].GetComponent<Targetable>();

            // targetEntity 정상 반환 및 타깃 가능일 때
            if (targetEntity.GetComponent<Targetable>() != null && targetEntity.GetComponent<Targetable>().IsCanTargetable())
            {
                //타깃을 마지막으로 때린 게임 오브젝트 설정
                targetEntity.GetComponent<Targetable>().SetLastHitGameObject(gameObject);
                //타깃 데미지 처리(기본 데미지 * 스킬 게이지 계수)
                targetEntity.GetComponent<CharacterStatManager>().TakeDamage(damage * playerStatManager.fSkillGaugeCoefficient);

            }
        }
    }

    void Skill1_Update()
    {
        //스킬 활성화 및 쿨다운이 아닐때
        if (Input.GetKey(skill1) && bIsCoolDown1 == false && bCanUseSkill)
        {
            //스킬1 준비완료 및 나머지 스킬 준비 해제
            eSkillSet = SkillType.Skill1;
            bSkill1Ready = true;
            bSkill2Ready = false;
            bSkill3Ready = false;
            bSkill4Ready = false;
        }
        //좌클릭 감지 및 스킬 준비 완료일 때
        if (Input.GetMouseButtonDown(0) && bSkill1Ready)
        {
            //스킬게이지 증가
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //캐릭터가 스킬을 쏘는 방향
            rotateTo = Quaternion.LookRotation(skillPointer - transform.position);
            //캐릭터를 발사체가 나가는 방향으로 설정
            transform.eulerAngles = new Vector3(0, rotateTo.eulerAngles.y, 0);

            //스킬 코루틴 실행
            StartCoroutine(SkillAnimation());

            fCurrentCoolDown1 = fCoolDown1;//현재 쿨다운 시간 초기화
            bIsCoolDown1 = true;//쿨다운 활성화
            bSkill1Ready = false;//스킬 준비 해제
        }
        //스킬 쿨다운 중일 때
        if (bIsCoolDown1)
        {
            //쿨다운 감소
            fCurrentCoolDown1 -= Time.deltaTime;

            //쿨다운 완료시
            if (fCurrentCoolDown1 <= 0.0f)
            {
                fCurrentCoolDown1 = 0.0f;
                bIsCoolDown1 = false;
            }
        }
    }

    void Skill2_Update()
    {
        //스킬 활성화 및 쿨다운이 아닐때
        if (Input.GetKey(skill2) && bIsCoolDown2 == false && bCanUseSkill)
        {
            eSkillSet = SkillType.Skill2;
            bSkill1Ready = false;
            bSkill2Ready = true;
            bSkill3Ready = false;
            bSkill4Ready = false;
        }
        //좌클릭 감지 및 스킬 준비 완료일 때
        if (Input.GetMouseButtonDown(0) && bSkill2Ready)
        {
            //스킬게이지 증가
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //캐릭터가 스킬을 쏘는 방향
            rotateTo = Quaternion.LookRotation(skillPointer - transform.position);
            //캐릭터를 발사체가 나가는 방향으로 설정
            transform.eulerAngles = new Vector3(0, rotateTo.eulerAngles.y, 0);

            //스킬2 프리팹 위치 설정
            skill2Position = skillPointerInRange;

            //스킬 코루틴 실행
            StartCoroutine(SkillAnimation());

            fCurrentCoolDown2 = fCoolDown2;//현재 쿨다운 시간 초기화
            bIsCoolDown2 = true;//쿨다운 활성화
            bSkill2Ready = false;//스킬 준비 해제
        }
        //스킬 쿨다운 중일 때
        if (bIsCoolDown2)
        {
            //쿨다운 감소
            fCurrentCoolDown2 -= Time.deltaTime;

            //쿨다운 완료시
            if (fCurrentCoolDown2 <= 0.0f)
            {
                fCurrentCoolDown2 = 0.0f;
                bIsCoolDown2 = false;
            }
        }
    }

    void Skill3_Update()
    {
        //스킬 활성화 및 쿨다운이 아닐때
        if (Input.GetKey(skill3) && bIsCoolDown3 == false && bCanUseSkill && bSkill3Ready == false)
        {
            eSkillSet = SkillType.Skill3;
            bSkill1Ready = false;
            bSkill2Ready = false;
            bSkill3Ready = true;
            bSkill4Ready = false;

            //스킬게이지 증가
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //스킬 아이콘 초기화
            skillUI.SkillImage3.fillAmount = 1;

            //오너 플레이어 설정 및 파괴시간 설정
            skill3Prefab.GetComponent<FollowThis>().SetOwnerPlayer(gameObject, fMaxSkill3ExistenceTime);
            //스킬게이지 계수에 따라 스킬의 크기 설정
            skill3Prefab.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient,
                                                            1f * playerStatManager.fSkillGaugeCoefficient,
                                                            1f * playerStatManager.fSkillGaugeCoefficient);
            //스킬 3 프리팹 소환
            Instantiate(skill3Prefab, transform.position, rotateTo);

            fCurrentCoolDown3 = fCoolDown3;//현재 쿨다운 시간 초기화
            bSkill3Activate = true;//스킬3 활성화

        }
        if (bSkill3Activate)
        {
            //일정 주기마다 해당범위 탐지후 데미지 처리
            if (Time.time >= fSkill3DamageIntervalUpdate + fSkill3DamageInterval)
            {
                SphereOverlap(transform.position, 3.0f * playerStatManager.fSkillGaugeCoefficient, playerStatManager.fskill3Damage);
                fSkill3DamageIntervalUpdate = Time.time;
            }

            //스킬 현재 지속시간 업데이트
            fCurrentSkill3ExistenceTime += Time.deltaTime;

            //현재 지속시간이 최대시간이 될 경우 
            if (fCurrentSkill3ExistenceTime >= fMaxSkill3ExistenceTime)
            {
                bSkill3Ready = false;//스킬 준비 해제
                bSkill3Activate = false;//스킬3 활성화 해제
                bIsCoolDown3 = true;//쿨다운 활성화
                fCurrentSkill3ExistenceTime = 0.0f;//스킬3 현재 지속시간 0
            }
        }
        //스킬 쿨다운 중일 때
        if (bIsCoolDown3)
        {
            //쿨다운 감소
            fCurrentCoolDown3 -= Time.deltaTime;

            //쿨다운 완료시
            if (fCurrentCoolDown3 <= 0.0f)
            {
                fCurrentCoolDown3 = 0.0f;
                bIsCoolDown3 = false;
            }
        }
    }

    void Skill4_Update()
    {
        //스킬 활성화 및 스킬 게이지가 90 이상일 때
        if (Input.GetKey(skill4) && bCanUseSkill && bSkill4Ready == false && playerStatManager.fSkillGauge >= 90.0f)
        {
            eSkillSet = SkillType.Skill4;
            bSkill1Ready = false;
            bSkill2Ready = false;
            bSkill3Ready = false;
            bSkill4Ready = true;

            //R 스킬 사용 후 스킬게이지 초기화
            if (playerStatManager.fSkillGauge >= 90.0f) playerStatManager.fSkillGauge = 0.0f;

            //스킬 코루틴 실행
            StartCoroutine(SkillAnimation());
        }
    }
}


