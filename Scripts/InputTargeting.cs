using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//우클릭시 타겟할당 또는 해제 스크립트
public class InputTargeting : MonoBehaviour
{
    public GameObject selectedCharacter;//플레이어가 조종중인 케릭터
    public bool bIsPlayer;//플레이어 판단
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        //플레이어로 태그된 게임 오브젝트를 selectedCharacter에 할당
        selectedCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 우클릭시
        if (Input.GetMouseButtonDown(1))
        {
            //마우스 위치지점으로 레이캐스트 실행
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                //레이캐스트 결과에서 Targetable클래스 반환및 타깃 가능시
                if (hit.collider.GetComponent<Targetable>() != null && hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    if (hit.collider.GetComponent<Targetable>().eTargetType == Targetable.EnemyType.minion)
                    {
                        //타깃을 설정
                        selectedCharacter.GetComponent<CharacterCombat>().targetedEnemy = hit.collider.GetComponent<Targetable>();
                    }
                }
                //Targetable클래스 미반환 또는 타깃 불가능 시
                else if (hit.collider.GetComponent<Targetable>() == null || !hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    //타깃을 해제
                    selectedCharacter.GetComponent<CharacterCombat>().targetedEnemy = null;
                }
            }
        }
    }
}
