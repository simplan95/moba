using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StatData", fileName = "Stat Data")]
public class CharacterStats : ScriptableObject
{
    public float fMaxHealth;//최대체력
    public float fAttackDamage;//노말 공격대미지
    public float fAttackSpeed;//공격 스피드
    public float fAttackTime;//공격시간
}
