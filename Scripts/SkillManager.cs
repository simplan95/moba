using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//AI
using UnityEngine.UI;//UI

//��ų ���� ���� ��ũ��Ʈ
public class SkillManager : MonoBehaviour
{
    private enum SkillType { Skill1, Skill2, Skill3, Skill4 };//��ų Ÿ�� enum
    public Vector3 skillPointer { get; private set; }//��ų ������
    public Vector3 skillPointerInRange { get; private set; }//������ ��ų ������(UI ������Ʈ)
    public bool bCanUseSkill { get; private set; }//��ų ��� ���ɿ���


    private SkillUIManager skillUI;//��ų UI��ũ��Ʈ
    private CharacterMovement charaMovement;//ĳ���� Movement ��ũ��Ʈ
    private Animator animator;//���ϸ�����
    private PlayerStatManager playerStatManager;//�÷��̾� ����
    private NavMeshAgent navAgent;//���� �޽�
    private Quaternion rotateTo;//��ų ����
    private SkillType eSkillSet;//���� ��ųŸ��

    public Transform projSpawnPoint;//�߻�ü ������ġ
    public LayerMask Target; // Ž�� ��� ���̾�

    [Header("Skill1")]
    public KeyCode skill1;//��ų ��ư
    public float fCoolDown1 = 5.0f;//��ٿ� �ð�
    public GameObject skill1Proj;//��ų 1�߻�ü
    public bool bIsCoolDown1 { get; private set; }//��ų ��ٿ� ����
    public float fCurrentCoolDown1 { get; private set; }//���� ��ų ��ٿ�ð�
    public bool bSkill1Ready { get; private set; }//��ų �غ�


    [Header("Skill2")]
    public KeyCode skill2;
    public float fCoolDown2 = 8.0f;//��ų 2 ��ٿ� �ð�
    public GameObject skill2Prefab;//��ų 2 ������
    public float fMaxSkill2Range;//��ų2 ����
    private Vector3 skill2Position;//��ų 2������ ��ȯ ��ġ
    public bool bIsCoolDown2 { get; private set; }//��ų ��� ���ɿ���
    public float fCurrentCoolDown2 { get; private set; }//���� ��ų ��ٿ�ð�
    public bool bSkill2Ready { get; private set; }//��ų �غ�

    [Header("Skill3")]
    public KeyCode skill3;//��ų��ư
    public float fCoolDown3 = 10.0f;//��ų 3 ��ٿ� �ð�
    public GameObject skill3Prefab;//��ų 3 ������
    public float fMaxSkill3ExistenceTime = 4.0f;//��ų 3 �ִ� ���� �ð�
    private float fCurrentSkill3ExistenceTime;//��ų 3 ���� ���� �ð�
    private bool bSkill3Activate;//��ų3 ������ Ȱ��ȭ ����
    public float fSkill3DamageInterval = 1.0f;//��ų3 ������ �ð� ����
    private float fSkill3DamageIntervalUpdate;//��ų3 ������ �ð� ���� ������Ʈ
    public bool bIsCoolDown3 { get; private set; }//��ų ��� ���ɿ���
    public float fCurrentCoolDown3 { get; private set; }//���� ��ų ��ٿ�ð�
    public bool bSkill3Ready { get; private set; }//��ų �غ�

    [Header("Skill4")]
    public KeyCode skill4;//��ų��ư
    public GameObject skill4Prefab;//��ų 4 ������
    public bool bSkill4Ready { get; private set; }//��ų �غ�

    // Start is called before the first frame update
    void Start()
    {
        skillUI = GetComponent<SkillUIManager>();
        charaMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        playerStatManager = GetComponent<PlayerStatManager>();
        navAgent = GetComponent<NavMeshAgent>();

        bCanUseSkill = true;
    }

    // Update is called once per frame
    void Update()
    {
        //��ũ�� ���� ���콺 ��ġ�� ���̺����� �Ҵ� �� Hit ���� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //��ų ������ ������Ʈ�� ���� raycast ����
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
        {
            //����ĳ��Ʈ ���� �� �ش� ������ skillPointer��ġ ������Ʈ
            skillPointer = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        //��ų�������� ���⼳��
        Vector3 hitPosDir = skillPointer - transform.position;
        //ĳ������ Ÿ������ ������ ����ȭ  
        Vector3 hitPosDirNomal = hitPosDir.normalized;
        //ĳ���Ϳ� Ÿ�������� �Ÿ�
        float fDistance = Vector3.Distance(skillPointer, transform.position);
        //ĳ���Ϳ� Ÿ�������� �Ÿ��� ���� �� ��Ÿ��� ��
        fDistance = Mathf.Min(fDistance, fMaxSkill2Range);

        //ĳ���� ��ġ���� Ÿ���������� ���� ��ġ�� ����
        Vector3 newhitPos = transform.position + (hitPosDirNomal * fDistance);
        //y�� ��ġ�� ������Ŵ
        newhitPos = new Vector3(newhitPos.x, 0.3f, newhitPos.z);
        //skill2Canvas ��ġ �� skill2 ������ ��ġ ����
        skillPointerInRange = newhitPos;

        Skill1_Update();//��ų 1 ������Ʈ
        Skill2_Update();//��ų 2 ������Ʈ
        Skill3_Update();//���i 3 ������Ʈ
        Skill4_Update();//���i 3 ������Ʈ
    }

    private IEnumerator SkillAnimation()
    {
        animator.SetBool("SkillAttack", true);//��ų �ִϸ��̼� Ȱ��ȭ
        charaMovement.bCanRotation = false;//��ų ��� �߿� ĳ���� ȸ���� ���
        navAgent.angularSpeed = 0.0f;
        bCanUseSkill = false;//��ų ��� �Ұ�

        //��ų �ִϸ��̼� �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(1.6f);

        animator.SetBool("SkillAttack", false);//��ų �ִϸ��̼� ��Ȱ��ȭ
        charaMovement.bCanRotation = true;//ĳ���� ȸ����� ����
        navAgent.angularSpeed = 720.0f;
        bCanUseSkill = true;//��ų ��� ����
    }

    public void SkillActivate()
    {
        switch (eSkillSet)
        {
            case SkillType.Skill1:
                //��ų ������ ����(�⺻ ������ * ��ų ������ ���)
                skill1Proj.GetComponent<SkillProj>().fDamage = playerStatManager.fskill1Damage * playerStatManager.fSkillGaugeCoefficient;
                //��ų �߻�ü ���� ����
                skill1Proj.GetComponent<SkillProj>().SetOwnerPlayer(gameObject);
                //��ų������ ����� ���� ��ų�� ũ�� ����
                skill1Proj.transform.localScale = new Vector3(1.5f * playerStatManager.fSkillGaugeCoefficient,
                                                              1.5f * playerStatManager.fSkillGaugeCoefficient,
                                                              1.5f * playerStatManager.fSkillGaugeCoefficient);
                //��ų �߻�ü ����
                Instantiate(skill1Proj, projSpawnPoint.transform.position, rotateTo);
                break;

            case SkillType.Skill2:
                //��ų������ ����� ���� ��ų�� ũ�� ����
                skill2Prefab.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient,
                                                                1f * playerStatManager.fSkillGaugeCoefficient,
                                                                1f * playerStatManager.fSkillGaugeCoefficient);
                //��ų2 ������ ����
                Instantiate(skill2Prefab, skill2Position, rotateTo);
                //�ش� ���� Ž�� �� ������ó��
                SphereOverlap(skill2Position, 1.5f * playerStatManager.fSkillGaugeCoefficient, playerStatManager.fskill2Damage);
                break;

            case SkillType.Skill3:
                //DoNothing
                break;

            case SkillType.Skill4:
                //��ų 4 ������ ��ȯ
                Instantiate(skill4Prefab, transform.position, rotateTo);
                //���� Ž�� �� ������ ó��
                SphereOverlap(transform.position, 5.0f, playerStatManager.fskill4Damage);

                bSkill4Ready = false;
                break;

        }

    }

    //�ش� ���� Ž���� ������ ó��
    void SphereOverlap(Vector3 pos, float range, float damage)
    {
        //���� ���� ����3 ��ŭ Ž�� ����
        Collider[] colliders =
        Physics.OverlapSphere(pos, range, Target);

        // ��� �ݶ��̴����� ��ȸ�ϸ鼭 Ÿ�� ã��
        for (int i = 0; i < colliders.Length; i++)
        {
            // �ݶ��̴��κ��� ������Ʈ Get
            Targetable targetEntity = colliders[i].GetComponent<Targetable>();

            // targetEntity ���� ��ȯ �� Ÿ�� ������ ��
            if (targetEntity.GetComponent<Targetable>() != null && targetEntity.GetComponent<Targetable>().IsCanTargetable())
            {
                //Ÿ���� ���������� ���� ���� ������Ʈ ����
                targetEntity.GetComponent<Targetable>().SetLastHitGameObject(gameObject);
                //Ÿ�� ������ ó��(�⺻ ������ * ��ų ������ ���)
                targetEntity.GetComponent<CharacterStatManager>().TakeDamage(damage * playerStatManager.fSkillGaugeCoefficient);

            }
        }
    }

    void Skill1_Update()
    {
        //��ų Ȱ��ȭ �� ��ٿ��� �ƴҶ�
        if (Input.GetKey(skill1) && bIsCoolDown1 == false && bCanUseSkill)
        {
            //��ų1 �غ�Ϸ� �� ������ ��ų �غ� ����
            eSkillSet = SkillType.Skill1;
            bSkill1Ready = true;
            bSkill2Ready = false;
            bSkill3Ready = false;
            bSkill4Ready = false;
        }
        //��Ŭ�� ���� �� ��ų �غ� �Ϸ��� ��
        if (Input.GetMouseButtonDown(0) && bSkill1Ready)
        {
            //��ų������ ����
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //ĳ���Ͱ� ��ų�� ��� ����
            rotateTo = Quaternion.LookRotation(skillPointer - transform.position);
            //ĳ���͸� �߻�ü�� ������ �������� ����
            transform.eulerAngles = new Vector3(0, rotateTo.eulerAngles.y, 0);

            //��ų �ڷ�ƾ ����
            StartCoroutine(SkillAnimation());

            fCurrentCoolDown1 = fCoolDown1;//���� ��ٿ� �ð� �ʱ�ȭ
            bIsCoolDown1 = true;//��ٿ� Ȱ��ȭ
            bSkill1Ready = false;//��ų �غ� ����
        }
        //��ų ��ٿ� ���� ��
        if (bIsCoolDown1)
        {
            //��ٿ� ����
            fCurrentCoolDown1 -= Time.deltaTime;

            //��ٿ� �Ϸ��
            if (fCurrentCoolDown1 <= 0.0f)
            {
                fCurrentCoolDown1 = 0.0f;
                bIsCoolDown1 = false;
            }
        }
    }

    void Skill2_Update()
    {
        //��ų Ȱ��ȭ �� ��ٿ��� �ƴҶ�
        if (Input.GetKey(skill2) && bIsCoolDown2 == false && bCanUseSkill)
        {
            eSkillSet = SkillType.Skill2;
            bSkill1Ready = false;
            bSkill2Ready = true;
            bSkill3Ready = false;
            bSkill4Ready = false;
        }
        //��Ŭ�� ���� �� ��ų �غ� �Ϸ��� ��
        if (Input.GetMouseButtonDown(0) && bSkill2Ready)
        {
            //��ų������ ����
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //ĳ���Ͱ� ��ų�� ��� ����
            rotateTo = Quaternion.LookRotation(skillPointer - transform.position);
            //ĳ���͸� �߻�ü�� ������ �������� ����
            transform.eulerAngles = new Vector3(0, rotateTo.eulerAngles.y, 0);

            //��ų2 ������ ��ġ ����
            skill2Position = skillPointerInRange;

            //��ų �ڷ�ƾ ����
            StartCoroutine(SkillAnimation());

            fCurrentCoolDown2 = fCoolDown2;//���� ��ٿ� �ð� �ʱ�ȭ
            bIsCoolDown2 = true;//��ٿ� Ȱ��ȭ
            bSkill2Ready = false;//��ų �غ� ����
        }
        //��ų ��ٿ� ���� ��
        if (bIsCoolDown2)
        {
            //��ٿ� ����
            fCurrentCoolDown2 -= Time.deltaTime;

            //��ٿ� �Ϸ��
            if (fCurrentCoolDown2 <= 0.0f)
            {
                fCurrentCoolDown2 = 0.0f;
                bIsCoolDown2 = false;
            }
        }
    }

    void Skill3_Update()
    {
        //��ų Ȱ��ȭ �� ��ٿ��� �ƴҶ�
        if (Input.GetKey(skill3) && bIsCoolDown3 == false && bCanUseSkill && bSkill3Ready == false)
        {
            eSkillSet = SkillType.Skill3;
            bSkill1Ready = false;
            bSkill2Ready = false;
            bSkill3Ready = true;
            bSkill4Ready = false;

            //��ų������ ����
            if (playerStatManager.fSkillGauge < 100.0f) playerStatManager.fSkillGauge += 10.0f;

            //��ų ������ �ʱ�ȭ
            skillUI.SkillImage3.fillAmount = 1;

            //���� �÷��̾� ���� �� �ı��ð� ����
            skill3Prefab.GetComponent<FollowThis>().SetOwnerPlayer(gameObject, fMaxSkill3ExistenceTime);
            //��ų������ ����� ���� ��ų�� ũ�� ����
            skill3Prefab.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient,
                                                            1f * playerStatManager.fSkillGaugeCoefficient,
                                                            1f * playerStatManager.fSkillGaugeCoefficient);
            //��ų 3 ������ ��ȯ
            Instantiate(skill3Prefab, transform.position, rotateTo);

            fCurrentCoolDown3 = fCoolDown3;//���� ��ٿ� �ð� �ʱ�ȭ
            bSkill3Activate = true;//��ų3 Ȱ��ȭ

        }
        if (bSkill3Activate)
        {
            //���� �ֱ⸶�� �ش���� Ž���� ������ ó��
            if (Time.time >= fSkill3DamageIntervalUpdate + fSkill3DamageInterval)
            {
                SphereOverlap(transform.position, 3.0f * playerStatManager.fSkillGaugeCoefficient, playerStatManager.fskill3Damage);
                fSkill3DamageIntervalUpdate = Time.time;
            }

            //��ų ���� ���ӽð� ������Ʈ
            fCurrentSkill3ExistenceTime += Time.deltaTime;

            //���� ���ӽð��� �ִ�ð��� �� ��� 
            if (fCurrentSkill3ExistenceTime >= fMaxSkill3ExistenceTime)
            {
                bSkill3Ready = false;//��ų �غ� ����
                bSkill3Activate = false;//��ų3 Ȱ��ȭ ����
                bIsCoolDown3 = true;//��ٿ� Ȱ��ȭ
                fCurrentSkill3ExistenceTime = 0.0f;//��ų3 ���� ���ӽð� 0
            }
        }
        //��ų ��ٿ� ���� ��
        if (bIsCoolDown3)
        {
            //��ٿ� ����
            fCurrentCoolDown3 -= Time.deltaTime;

            //��ٿ� �Ϸ��
            if (fCurrentCoolDown3 <= 0.0f)
            {
                fCurrentCoolDown3 = 0.0f;
                bIsCoolDown3 = false;
            }
        }
    }

    void Skill4_Update()
    {
        //��ų Ȱ��ȭ �� ��ų �������� 90 �̻��� ��
        if (Input.GetKey(skill4) && bCanUseSkill && bSkill4Ready == false && playerStatManager.fSkillGauge >= 90.0f)
        {
            eSkillSet = SkillType.Skill4;
            bSkill1Ready = false;
            bSkill2Ready = false;
            bSkill3Ready = false;
            bSkill4Ready = true;

            //R ��ų ��� �� ��ų������ �ʱ�ȭ
            if (playerStatManager.fSkillGauge >= 90.0f) playerStatManager.fSkillGauge = 0.0f;

            //��ų �ڷ�ƾ ����
            StartCoroutine(SkillAnimation());
        }
    }
}


