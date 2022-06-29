using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//AI

//캐릭터 이동관련 스크립트
public class CharacterMovement : MonoBehaviour
{
    public float fRotateSpeedMovement = 0.1f;//캐릭터 회전 속도
    public float fRotateVelocity;

    public NavMeshAgent navAgent;//내비 메시
    private CharacterCombat characterCombat;//케릭터 전투
    private Animator animator;//에니메이터
    private RaycastHit hit;//레이캐스트 
    public bool bMovingFirst { get; private set; }//이동상태 활성화 변수(이동 실시,탐색 미실시) 
    public bool bCanRotation { get; set; }//스킬 사용 중 캐릭터 로테이션 잠금


    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterCombat = GetComponent<CharacterCombat>();
        animator = GetComponent<Animator>();

        bCanRotation = true;
    }

    private void Update()
    {
        //NavMeshAgent의 magnitude와 Speed의 비율을 구해 해당 캐릭터의 속도를 (0.0~1.0) 구함 
        float speed = navAgent.velocity.magnitude / navAgent.speed;

        //애니메이터의 Speed 값을 설정
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        if (Input.GetMouseButtonDown(1))//마우스 우클릭시
        {
            //마우스 우클릭위치 지점으로 레이캐스트실행
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                    //bMovingFirst를 false(탐지 비활성화)
                    bMovingFirst = false;

                    //hit.point 지점으로 목적지 설정,정지거리 0
                    navAgent.SetDestination(hit.point);
                    navAgent.stoppingDistance = 0.0f;
            }
        }

        //목표 지점 도착시 bMovingFirst true(탐지 활성화)
        if (Vector3.Distance(transform.position, hit.point) <= 0.1)
        {
            bMovingFirst = true;
        }

    }

    public void CharacterRotateToTarget(Vector3 targetVec)
    {
        if (bCanRotation)//캐릭터 회전잠금이 false일 때만 회전
        {
            //캐릭터 회전방향 설정 및 부드럽게 회전
            Quaternion rotationToLookat = Quaternion.LookRotation(targetVec - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                                     rotationToLookat.eulerAngles.y,
                                                     ref fRotateVelocity,
                                                     fRotateSpeedMovement * Time.deltaTime);

            //캐릭터 rotationY 값으로 회전
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }

    }
}
