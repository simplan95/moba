using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타깃 관련 스크립트
public class Targetable : MonoBehaviour
{
    public enum EnemyType { minion, player };//타깃 타입 enum
    public EnemyType eTargetType;
    public bool bTargetable { get; private set; }//타깃 가능여부

    public GameObject LastHitGameObject;//해당 타깃을 마지막으로 공격한 게임 오브젝트

    public void SetEnemyType(EnemyType targetType)//타입을 설정함
    {
        eTargetType = targetType;
    }

    public bool IsPlayer()//플레이어 타입 확인
    {
        return eTargetType == EnemyType.player;
    }

    public bool IsMinion()//미니언 타입 확인
    {
        return eTargetType == EnemyType.minion;
    }

    public void SetTargetable(bool set)//타깃가능 여부 세팅
    {
        bTargetable = set;
    }

    public bool IsCanTargetable()//타깃될 수 있는지 판단
    {
        return bTargetable;
    }

    public void SetLastHitGameObject(GameObject HitObject)//마지막으로 자신을 공격한 게임 오브젝트를 셋
    {
        LastHitGameObject = HitObject;
    }
}
