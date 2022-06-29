using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI;//AI

//플레이어 인풋 매니저 스크립트(카메라 고정,해제 및 어택땅 구현)
public class PlayerInputManager : MonoBehaviour
{
    public KeyCode CameraViewModeKey;//카메라 뷰모드변환 키
    public CameraUnlock cameraUnlock;//카메라 고정 해제 스크립트
    public CameraFollow cameraFollow;//카메라 고정 스크립트

    public KeyCode NomalAttackKey;//일반공격 키
    public Image NomalAttackRangeCircle;//범위서클 이미지
    private NavMeshAgent navAgent;//내비 메시
    private CharacterCombat characterCombat;//캐릭터 컴뱃
    private RaycastHit hit;//레이캐스트 결과
    public bool bMovingAndSearch { get; private set; }//이동 및 탐색 활성화 변수

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterCombat = GetComponent<CharacterCombat>();

        cameraUnlock.enabled = false;//CameraUnlock 비활성화
        cameraFollow.enabled = true;//CameraFollow 활성화

        NomalAttackRangeCircle.GetComponent<Image>().enabled = false;//일반공격 범위 이미지 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(CameraViewModeKey))//카메라 뷰모드 체인지
        {
            cameraUnlock.enabled = !cameraUnlock.enabled;//CameraUnlock 활성화 체인지
            cameraFollow.enabled = !cameraFollow.enabled;//CameraFollow 활성화 체인지
        }

        //이동 및 탐색(어택땅)
        NomalAttackUpdate();
    }

    void NomalAttackUpdate()
    {
        // 이동 및 탐색키 활성화
        if (Input.GetKey(NomalAttackKey))
        {
            NomalAttackRangeCircle.GetComponent<Image>().enabled = true;//일반공격 인디케이터 이미지 활성화 
        }
        //이미지 활성화 및 좌클릭시
        if (NomalAttackRangeCircle.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            //레이캐스트 실행 
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                NomalAttackRangeCircle.GetComponent<Image>().enabled = false;//일반공격 인디케이터 이미지 비활성화

                //레이캐스트 결과에서 Targetable클래스 반환시 타깃에너미 할당
                if (hit.collider.GetComponent<Targetable>() != null && hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    if (hit.collider.GetComponent<Targetable>().eTargetType == Targetable.EnemyType.minion)
                    {
                        gameObject.GetComponent<CharacterCombat>().targetedEnemy = hit.collider.GetComponent<Targetable>();
                    }
                }

                //타깃 미 반환시 레이캐스트 지점으로 이동 및 탐색
                else if (hit.collider.GetComponent<Targetable>() == null || !hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    gameObject.GetComponent<CharacterCombat>().targetedEnemy = null;
                    //hit.point 지점으로 목적지 설정,정지거리 0
                    navAgent.SetDestination(hit.point);
                    navAgent.stoppingDistance = 0.0f;

                    //이동 및 탐색 활성화
                    bMovingAndSearch = true;
                }
            }
        }
        //타깃 확인시
        if (characterCombat.targetedEnemy != null)
        {
            //이동 및 탐색 비활성화
            bMovingAndSearch = false;
        }
    }
}
