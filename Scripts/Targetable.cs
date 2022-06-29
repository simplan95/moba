using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ÿ�� ���� ��ũ��Ʈ
public class Targetable : MonoBehaviour
{
    public enum EnemyType { minion, player };//Ÿ�� Ÿ�� enum
    public EnemyType eTargetType;
    public bool bTargetable { get; private set; }//Ÿ�� ���ɿ���

    public GameObject LastHitGameObject;//�ش� Ÿ���� ���������� ������ ���� ������Ʈ

    public void SetEnemyType(EnemyType targetType)//Ÿ���� ������
    {
        eTargetType = targetType;
    }

    public bool IsPlayer()//�÷��̾� Ÿ�� Ȯ��
    {
        return eTargetType == EnemyType.player;
    }

    public bool IsMinion()//�̴Ͼ� Ÿ�� Ȯ��
    {
        return eTargetType == EnemyType.minion;
    }

    public void SetTargetable(bool set)//Ÿ�갡�� ���� ����
    {
        bTargetable = set;
    }

    public bool IsCanTargetable()//Ÿ��� �� �ִ��� �Ǵ�
    {
        return bTargetable;
    }

    public void SetLastHitGameObject(GameObject HitObject)//���������� �ڽ��� ������ ���� ������Ʈ�� ��
    {
        LastHitGameObject = HitObject;
    }
}
