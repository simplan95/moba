using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StatData", fileName = "Stat Data")]
public class CharacterStats : ScriptableObject
{
    public float fMaxHealth;//�ִ�ü��
    public float fAttackDamage;//�븻 ���ݴ����
    public float fAttackSpeed;//���� ���ǵ�
    public float fAttackTime;//���ݽð�
}
