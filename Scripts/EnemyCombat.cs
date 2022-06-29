using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� 

//��(�̴Ͼ�) ���� ���� ��ũ��Ʈ
public class EnemyCombat : MonoBehaviour
{
    public Targetable targetedEnemy;//Ÿ��� ��
    private Animator animator;//���ϸ�����
    private NavMeshAgent navMeshAgent; //����޽�
    private EnemyStatManager enemyStatManager;//�� ���� �Ŵ���
    private AudioSource audioPlayer; // ����� �ҽ� ������Ʈ

    private bool bCanNomalAttack = true;//�Ϲ� ���� ���ɿ���
    public GameObject projPrefab;//�߻�ü
    public Transform projSpawnPoint;//�߻�ü ������ġ
    public float fAttackRange;//���ݹ���
    public float fDetectRange = 10.0f;//Ž������
    public AudioClip meleeAttackSound;//���� ���� ����
    public enum AttackType { melee, range };//����Ÿ�� enum
    public AttackType eAttackType;//����Ÿ��

    public LayerMask Target; // Ž�� ��� ���̾�

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyStatManager = GetComponent<EnemyStatManager>();
        audioPlayer = GetComponent<AudioSource>();

        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(DetectTarget());
    }

    // Update is called once per frame
    void Update()
    {
        //Ÿ���� null�� ��
        if (targetedEnemy == null)
        {
            bCanNomalAttack = true;//�븻���� ���� true
            animator.SetBool("NomalAttack", false);//�ִϸ����� NomalAttack false
        }
        //Ÿ���� null�� �ƴ� ��
        else if (targetedEnemy != null)
        {
            if (!targetedEnemy.IsCanTargetable())//Ÿ���� Ÿ�� ���� ���°� �ƴ� �� Ÿ�� null �� return
            {
                targetedEnemy = null;
                return;
            }

            //Ÿ�� �������� ȸ��
            Quaternion rotationToLookat = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
            float rotationY = rotationToLookat.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, rotationY, 0);

            //Ÿ���� �����Ÿ� �ȿ� ���� �� ���� ����
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) <= fAttackRange)
            {
                if (bCanNomalAttack)
                {
                    StartCoroutine(NomalAttackInterval());//�Ϲݰ��� �ڷ�ƾ ����
                }
            }
        }

        //NavMeshAgent�� magnitude�� Speed�� ������ ���ؼ� �ش� ĳ������ �ӵ��� (0.0~1.0) ���� 
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;

        //�ִϸ������� Speed ���� ����
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    //���� ����(���� ���ϸ��̼ǿ��� �̺�Ʈ�� �����)
    public void IguanaAttack()
    {
        if (targetedEnemy != null)//Ÿ���� null�� �ƴ� ��
        {
            if (eAttackType == AttackType.melee)//����Ÿ���� ������ ��
            {
                {
                    audioPlayer.PlayOneShot(meleeAttackSound);//�и����� ���� ���
                    targetedEnemy.GetComponent<CharacterStatManager>().TakeDamage(enemyStatManager.fAttackDamage);//�÷��̾� ����ũ����������
                }
            }
            if (eAttackType == AttackType.range)//����Ÿ���� ���Ÿ� �� ��
            {
                {
                    //�߻�ü ���� ���� �� ������ ����
                    projPrefab.GetComponent<SkillProj>().SetOwnerPlayer(gameObject);
                    projPrefab.GetComponent<SkillProj>().fDamage = enemyStatManager.fAttackDamage;

                    //�߻�ü ����
                    Quaternion rotateTo = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                    //�߻�ü ��ȯ
                    Instantiate(projPrefab, projSpawnPoint.transform.position, rotateTo);
                }
            }
        }
    }

    private IEnumerator DetectTarget()
    {
        // ��� �ִ� ���� ���� ����
        while (!enemyStatManager.bDead)
        {
            if (targetedEnemy != null)//Ÿ�� Ȱ��ȭ��
            {
                //Ÿ�� ��ġ�� �̵�
                navMeshAgent.SetDestination(targetedEnemy.transform.position);
                navMeshAgent.stoppingDistance = fAttackRange;
            }
            else if (targetedEnemy == null)//Ÿ�� ��Ȱ��ȭ��
            {
                //Ÿ�� ��Ž���� ĳ���� �ֺ� ���� ���� ���� �̵���ġ ����
                Vector3 moveTo = GetRandomPointOnNavMesh(gameObject.transform.position, fDetectRange);

                //��ġ�� �̵�
                navMeshAgent.SetDestination(moveTo);
                navMeshAgent.stoppingDistance = 0.0f;

                // ��� �ݶ��̴����� ��ȸ�ϸ鼭 Ÿ�� ã��
                Collider[] colliders =
                    Physics.OverlapSphere(transform.position, fDetectRange, Target);

                // ��� �ݶ��̴����� ��ȸ�ϸ鼭 Ÿ��Ž��
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� LivingEntity ������Ʈ ��������
                    Targetable targetEntity = colliders[i].GetComponent<Targetable>();

                    // Ÿ���� �÷��̾� Ÿ�� ��ȯ��
                    if (targetEntity.GetComponent<Targetable>() != null && targetEntity.GetComponent<Targetable>().IsCanTargetable() && targetEntity.eTargetType == Targetable.EnemyType.player)
                    {
                        // ���� ��� ����
                        targetedEnemy = targetEntity;
                        break;
                    }
                }
            }

            // 0.1�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Ÿ�� ��Ž���� ĳ���� �ֺ� ���� ���� ���� �̵���ġ ����
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center�� �߽����� �������� maxDistance�� �� �ȿ����� ���� ��ġ ����
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ���, randomPos�� ���� ����� ����޽� ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ã�� �� ��ȯ
        return hit.position;
    }

    private IEnumerator NomalAttackInterval()
    {
        bCanNomalAttack = false;//�븻���� ���� false
        animator.SetBool("NomalAttack", true);//�ִϸ����� NomalAttack true

        // ���ݽð���ŭ ��ٸ�
        yield return new WaitForSeconds(enemyStatManager.fAttackTime + 0.1f);

        if (targetedEnemy == null)//Ÿ���� null�϶� �븻���� �غ� ��Ȱ��ȭ
        {
            bCanNomalAttack = true;//�븻���� ���� true
            animator.SetBool("NomalAttack", false);//�ִϸ����� NomalAttack false
        }
    }
}
