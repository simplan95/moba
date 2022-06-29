using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ���� ���� ���� ��ũ��Ʈ
public class CharacterCombat : MonoBehaviour
{
    public Targetable targetedEnemy;//Ÿ��� ��
    private CharacterMovement charaMovement;//ĳ���� �����Ʈ ��ũ��Ʈ
    private PlayerStatManager playerStatManager;//�÷��̾� ���� ��ũ��Ʈ
    private PlayerInputManager playerInputManaget;//�÷��̾� ��ǲ ��ũ��Ʈ
    private Animator animator;//���ϸ�����

    public bool bCanNomalAttack = true;//�Ϲ� ���� ���ɽ�
    public float fAttackRange = 6.0f;//���ݹ���
    public float fDetectRange = 6.0f;//Ž������
    public GameObject projPrefab;//�븻���� �߻�ü
    public Transform projSpawnPoint;//�븻���� �߻�ü ������ġ

    public LayerMask Target; // Ž�� ��� ���̾�

    // Start is called before the first frame update
    void Start()
    {
        charaMovement = GetComponent<CharacterMovement>();
        playerStatManager = GetComponent<PlayerStatManager>();
        animator = GetComponent<Animator>();
        playerInputManaget = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ÿ���� null�϶�  �븻���� �غ� ����
        if (targetedEnemy == null)
        {
            ReleaseAttackReady();
        }

        //Ÿ���� null�̸鼭 ĳ���� �̵�,Ž�� Ȱ��ȭ�� ���� Ž���ǽ� 
        if (targetedEnemy == null && charaMovement.bMovingFirst || targetedEnemy == null && playerInputManaget.bMovingAndSearch)
        {
            TargetDetect();
        }

        //Ÿ���� null�� �ƴ� �� ���ݽ���
        else if (targetedEnemy != null)
        {
            if (!targetedEnemy.IsCanTargetable())//Ÿ���� Ÿ�� �Ұ������� �� null�� return
            {
                targetedEnemy = null;
                return;
            }

            //Ÿ���� ���� ĳ���Ͱ� ȸ��
            charaMovement.CharacterRotateToTarget(targetedEnemy.transform.position);

            //�÷��̾� ĳ������ �������� Ÿ������ ���� �� fAttackRange�� �����Ÿ��� ����
            charaMovement.navAgent.SetDestination(targetedEnemy.transform.position);
            charaMovement.navAgent.stoppingDistance = fAttackRange;

            //Ÿ���� �����Ÿ� �ȿ� ���� �� �Ϲݰ��ݽ���
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) <= fAttackRange)
            {
                if (bCanNomalAttack)
                {
                    StartCoroutine(NomalAttackInterval());//�Ϲݰ��� �ڷ�ƾ ����
                }
            }
        }
    }

    private IEnumerator NomalAttackInterval()
    {
        ReadyToAttack();//�븻���� �غ� Ȱ��ȭ

        //���ݽð� ��ŭ ��ٸ�
        yield return new WaitForSeconds(playerStatManager.fAttackTime);

        if (targetedEnemy == null)//Ÿ���� null�϶� �븻���� �غ� ��Ȱ��ȭ
        {
            ReleaseAttackReady();
        }
    }

    //���ϸ��̼� �븻���� �̺�Ʈ �߻��� ������ �޼��� ����
    public void NonmalAttack()
    {
        if (targetedEnemy != null)//Ÿ���� null�� �ƴ� ��
        {
            if (targetedEnemy.GetComponent<Targetable>() != null)//Targetable ��ũ��Ʈ�� ��ȯ�� ��
            {
                playerStatManager.fSkillGauge += 5.0f;//��ų������ ���
                SpawnRangedProj(targetedEnemy);//�߻�ü ��ȯ            
            }
        }
    }

    void SpawnRangedProj(Targetable targetEnemy)
    {
        //�븻 �߻�ü ����(������, Ÿ��, ������ �ǽ��� �÷��̾�)
        projPrefab.GetComponent<NomalAttackProj>().fDamage = playerStatManager.fAttackDamage;
        projPrefab.GetComponent<NomalAttackProj>().target = targetEnemy;
        projPrefab.GetComponent<NomalAttackProj>().SetOwnerPlayer(gameObject);

        //�߻�ü ��ȯ
        Instantiate(projPrefab, projSpawnPoint.transform.position, Quaternion.identity);
    }


    public void TargetDetect()
    {
        //ĳ���� �ֺ� �������� ��ŭ Ž�� ����
        Collider[] colliders =
        Physics.OverlapSphere(transform.position, fDetectRange, Target);

        // ��� �ݶ��̴����� ��ȸ�ϸ鼭 Ÿ�� ã��
        for (int i = 0; i < colliders.Length; i++)
        {
            // �ݶ��̴��κ��� ������Ʈ Get
            Targetable targetEntity = colliders[i].GetComponent<Targetable>();

            // targetEntity�� �̴Ͼ��� ���
            if (targetEntity.GetComponent<Targetable>() != null
                && targetEntity.GetComponent<Targetable>().IsCanTargetable()
                && targetEntity.eTargetType == Targetable.EnemyType.minion)
            {
                // ���� ��� ����
                targetedEnemy = targetEntity.GetComponent<Targetable>();
                break;
            }
        }
    }

    public void ReadyToAttack()//�븻 ���� �غ�
    {
        bCanNomalAttack = false;//�븻���� ���� false
        animator.SetBool("NomalAttack", true);//�ִϸ����� NomalAttack true
    }

    public void ReleaseAttackReady()//�븻���� �غ� ��Ȱ��ȭ
    {
        bCanNomalAttack = true;//�븻���� ���� true
        animator.SetBool("NomalAttack", false);//�ִϸ����� NomalAttack false
    }
}
