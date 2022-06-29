using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI; // AI, ������̼� 

//�÷��̾� ���� �Ŵ���
public class PlayerStatManager : CharacterStatManager//ĳ���� ���� �Ŵ��� ���
{
    public CharacterStats scriptableStats;//��ũ���ͺ� ����
    public Targetable targetable;//Ÿ�� ��ũ��Ʈ
    private CharacterCombat characterCombat;//ĳ���� ���� ��ũ��Ʈ
    private CharacterMovement characterMovement;//ĳ���� �̵� ��ũ��Ʈ
    private Animator animator;//���ϸ�����
    private NavMeshAgent navMeshAgent;//����޽�
    private AudioSource audioPlayer;//�÷��̾� ����� �����

    public AudioClip getItemSound;//�÷��̾� ������ ȹ�� �Ҹ�
    public AudioClip levelUpSound;//�÷��̾� ������ �Ҹ�
    public Slider gameHealthSlider;//���ӳ� �÷��̾� ü�¹�
    public Slider HUDHealthSlider;//HUD ü�¹�
    public Slider skillGaugageSlider;//��ų������ �����̴�

    [Header("Custom Stats")]
    [Range(0, 100)]
    public float fSkillGauge = 0.0f;//��ų������
    public float fSkillGaugeCoefficient { get; private set; }//��ų������ ���(��ų ������ �� ���� ���� ���ʽ�)

    public float fskill1Damage = 10.0f;//��ų1 ������
    public float fskill2Damage = 10.0f;//��ų2 ������
    public float fskill3Damage = 5.0f;//��ų3 ������
    public float fskill4Damage = 100.0f;//��ų4 ������

    // Start is called before the first frame update
    void Start()
    {
        targetable = GetComponent<Targetable>();
        characterCombat = GetComponent<CharacterCombat>();
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioPlayer = GetComponent<AudioSource>();

        targetable.SetTargetable(true);//Ÿ�� ����Ȱ��ȭ

        //ondeath �̺�Ʈ �߰�
        onDeath += PlayDeadSequence;

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void OnEnable()
    {
        // CharacterStatManager OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();

        bDead = false;//�ʱ���´� ����
        fMaxHealth = scriptableStats.fMaxHealth;//�ִ�ü�� ����
        fCurrentHealth = fMaxHealth;//����ü�� ����
        fAttackDamage = scriptableStats.fAttackDamage;//�븻 ���ݴ����
        fAttackSpeed = scriptableStats.fAttackSpeed;//���� ���ǵ�
        fAttackTime = scriptableStats.fAttackTime;//���ݽð�
        fSkillGauge = 0.0f;//��ų������

        targetable.SetTargetable(true);//Ÿ�� ����Ȱ��ȭ

        //�÷��̾� ü�¹� �ִ�ü�� ����
        HUDHealthSlider.maxValue = fMaxHealth;
        gameHealthSlider.maxValue = fMaxHealth;

        //��ų������ �ִ�ġ ����
        skillGaugageSlider.maxValue = 100.0f;

    }


    // Update is called once per frame
    void Update()
    {
        //HUD ü�¹ٿ� �÷��̾� ü�¹ٸ� ���ε� �� ����ü�� ������Ʈ
        HUDHealthSlider.value = fCurrentHealth;
        gameHealthSlider.value = HUDHealthSlider.value;

        if (fSkillGauge > 100.0f)
        {
            fSkillGauge = 100.0f;
        }
        if (fSkillGauge > 0.0f)
        {
            //��ų�������� 0 �̻� �϶� �����ð����� ����
            fSkillGauge -= 1.0f * Time.deltaTime;
        }

        skillGaugageSlider.value = fSkillGauge;//��ų�������� ������Ʈ

        fSkillGaugeCoefficient = 1 + (fSkillGauge / 100);//��ų ������ ��� ������Ʈ
    }

    void PlayDeadSequence()//�÷��̾� �����
    {
        // ��� �ִϸ��̼� ���
        animator.SetTrigger("Dead");
        targetable.SetTargetable(false);//Ÿ�� ���� ��Ȱ��ȭ
        //animator.SetBool("MinionDead", true);//�ִϸ����� �̴Ͼ� ��� true

        // AI ������ �����ϰ� ����޽� ������Ʈ�� ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        characterCombat.targetedEnemy = null;//Ÿ�� null
        characterCombat.enabled = false;//ĳ���� �Ĺ� ��ũ��Ʈ ��Ȱ��ȭ
        characterMovement.enabled = false;//ĳ���� �����Ʈ ��ũ��Ʈ ��Ȱ��ȭ
        targetable.enabled = false;//Targetable ��ũ��Ʈ ��Ȱ��ȭ

        StartCoroutine(GameOver());//5�ʵ� ���ӿ��� ȭ�� ���
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public void Heal(float amount)//ĳ���� ü��ȸ��
    {
        fCurrentHealth += amount;
        if (fCurrentHealth >= fMaxHealth)
        {
            fCurrentHealth = fMaxHealth;
        }
    }

    //������ �� ���� ��
    public void StatsUp()//
    {
        //�ִ�ü�� ����
        fMaxHealth += 10.0f;
        HUDHealthSlider.maxValue = fMaxHealth;
        gameHealthSlider.maxValue = fMaxHealth;

        //�븻���ݹ� ��ų ������ ����
        fAttackDamage += 1.0f;
        fskill1Damage += 1.0f;
        fskill2Damage += 1.0f;
        fskill3Damage += 0.5f;

        //������ ���� ���
        audioPlayer.PlayOneShot(levelUpSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        //ĳ���� ������ ������ ��밡��
        if (!bDead)
        {
            //HealPack ������Ʈ ��������
            HealPack item = other.GetComponent<HealPack>();

            //�浹�� ����� HealPack��ũ��Ʈ�� ��ȯ��
            if (item != null)
            {
                //���� ���
                item.Use(gameObject);
                //������ ȹ�� �Ҹ� ���
                audioPlayer.PlayOneShot(getItemSound);
            }
        }
    }
}
