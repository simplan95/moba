using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI; // AI, ������̼� 

//��(�̴Ͼ�) ���� �Ŵ��� 
public class EnemyStatManager : CharacterStatManager//ĳ���� ���� �Ŵ��� ���
{
    public CharacterStats scriptableEnemyStats;//��ũ���ͺ� ����
    private Targetable targetable;//Ÿ�� ��ũ��Ʈ
    private Animator animator;//���ϸ�����
    private NavMeshAgent navMeshAgent; //����޽�
    private EnemyCombat enemyCombat;//EnemyCombat ��ũ��Ʈ
    private CapsuleCollider capsuleCollider;//ĸ�� �ݶ��̴�


    public Slider enemyHealthSlider;//���ӳ� �� ü�¹�

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        targetable = GetComponent<Targetable>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<EnemyCombat>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        targetable.SetTargetable(true);//Ÿ�� ����Ȱ��ȭ

        bDead = false;//�ʱ���´� ����
        fMaxHealth = scriptableEnemyStats.fMaxHealth;//�ִ�ü�� ����
        fCurrentHealth = fMaxHealth;//����ü�� ����
        fAttackDamage = scriptableEnemyStats.fAttackDamage;//�븻 ���ݴ����
        fAttackSpeed = scriptableEnemyStats.fAttackSpeed;//���� ���ǵ�
        fAttackTime = scriptableEnemyStats.fAttackTime;//���ݽð�

        //onDeath �̺�Ʈ �߰�
        onDeath += GiveExpToPlayer;
        onDeath += PlayDeadSequence;

        //�� ü�¹� ����
        enemyHealthSlider.maxValue = fMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //�� ü�� ������Ʈ
        enemyHealthSlider.value = fCurrentHealth;
    }

    //�÷��̾�� ����ġ �߰�
    void GiveExpToPlayer()
    {
        //LastHitGameObject Ȯ�� �� �÷��̾� ���� Ȯ��
        if (targetable.LastHitGameObject != null && targetable.LastHitGameObject.GetComponent<Targetable>().IsPlayer())
        {
            //�ڽ��� óġ�� �÷��̾�� ����ġ �߰�
            targetable.LastHitGameObject.GetComponent<CharacterLevelManager>().SetExp(Random.Range(10, 15));
        }
    }

    //ĳ���� ���� �� �����ð��� Destroy
    void PlayDeadSequence()
    {

        // ��� �ִϸ��̼� ���
        animator.SetTrigger("Dead");
        targetable.SetTargetable(false);//Ÿ�� ���� ��Ȱ��ȭ

        // AI ������ �����ϰ� ����޽� ������Ʈ�� ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        enemyCombat.enabled = false;//ĳ���� �Ĺ� ��ũ��Ʈ ��Ȱ��ȭ
        targetable.enabled = false;//Targetable ��ũ��Ʈ ��Ȱ��ȭ
        capsuleCollider.isTrigger = true;

        Destroy(gameObject, 5.0f);//5�ʵ� ������Ʈ Destroy
    }
}
