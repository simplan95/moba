using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI

//��ų UI ���� ��ũ��Ʈ(������, �ε������)
public class SkillUIManager : MonoBehaviour
{
    private PlayerStatManager playerStatManager;//�÷��̾� ����
    private SkillManager skillManager;//��ų�Ŵ���

    [Header("Skill1")]
    public Image SkillImage1;//��ų ������ 
    public Text coolDownText1;//��ų ��ٿ� �ð�ǥ�� �ؽ�Ʈ

    public Canvas skill1Canvas;//��ų1 ĵ����
    public Image skill1shot;//��ų1 �̹���

    [Header("Skill2")]
    public Image SkillImage2;//��ų ������
    public Text coolDownText2;//��ų ��ٿ� �ð�ǥ�� �ؽ�Ʈ

    //Skill2 �Է� ����
    public Canvas skill2Canvas;//��ų2 ĵ����
    public Image skill2TargetCircle;//Ÿ�� ��Ŭ �̹���
    public Image skill2RangeCircle;//������Ŭ �̹���


    [Header("Skill3")]
    public Image SkillImage3;//��ų ������
    public Text coolDownText3;


    [Header("Skill4")]
    public Image SkillImage4;//��ų ������


    // Start is called before the first frame update
    void Start()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
        skillManager = GetComponent<SkillManager>();

        //��ų ��ٿ� �̹��� �ʱ�ȭ
        SkillImage1.fillAmount = 0;
        SkillImage2.fillAmount = 0;
        SkillImage3.fillAmount = 0;
        SkillImage4.fillAmount = 0;

        //��ų �ε������� �̹��� �ʱ�ȭ
        skill1shot.GetComponent<Image>().enabled = false;
        skill2TargetCircle.GetComponent<Image>().enabled = false;
        skill2RangeCircle.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //��ų ������ ������Ʈ
        Skill1_IconUpdade();
        Skill2_IconUpdade();
        Skill3_IconUpdade();
        Skill4_IconUpdade();

        //��ų1 
        //��ų1 ĵ������ ȸ���� ����
        Quaternion totateTo = Quaternion.LookRotation(skillManager.skillPointer - transform.position);
        //x�� ȸ���� ������Ŵ
        totateTo.eulerAngles = new Vector3(0, totateTo.eulerAngles.y, totateTo.eulerAngles.z);
        //��ų1 ĵ������ �ش��ǥ�� ȸ��
        skill1Canvas.transform.rotation = Quaternion.Lerp(totateTo, skill1Canvas.transform.rotation, 0f);
        //��ų1 ĵ������ ��ų����������� ���� ������ ������Ʈ
        skill1Canvas.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient, 1f, 1f);


        //skill2Canvas ��ġ�� Ÿ���������� ������Ʈ
        skill2Canvas.transform.position = skillManager.skillPointerInRange;
        //��ų2 ĵ������ ��ų����������� ���� ���� ������Ʈ
        skill2Canvas.transform.localScale = new Vector3(3f * playerStatManager.fSkillGaugeCoefficient,
                                                        3f * playerStatManager.fSkillGaugeCoefficient, 3f);
    }

    void Skill1_IconUpdade()
    {
        //��ų��ư Ȱ��ȭ �� ��ٿ��� �ʱ�ȭ �Ǿ��� ��
        if (Input.GetKey(skillManager.skill1) && skillManager.bIsCoolDown1 == false && skillManager.bCanUseSkill)
        {
            skill1shot.GetComponent<Image>().enabled = true;//��ų1 �ε������� �̹��� Ȱ��ȭ 
            //������ ��ų �ε������� ��Ȱ��ȭ
            skill2TargetCircle.GetComponent<Image>().enabled = false;
            skill2RangeCircle.GetComponent<Image>().enabled = false;

        }
        //�̹��� Ȱ��ȭ �� ��Ŭ����
        if (skill1shot.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            SkillImage1.fillAmount = 1;//��ų ������ �̹��� 
        }
        //��ٿ� ���� ��
        if (skillManager.bIsCoolDown1)
        {
            skill1shot.GetComponent<Image>().enabled = false;//��ų1 �ε������� �̹��� ��Ȱ��ȭ 
            coolDownText1.GetComponent<Text>().enabled = true;//��ٿ� �ð� �ؽ�Ʈ Ȱ��ȭ

            //��ٿ� �ð� ����
            SkillImage1.fillAmount -= 1 / skillManager.fCoolDown1 * Time.deltaTime;

            //��ٿ� �ؽ�Ʈ ������Ʈ
            coolDownText1.text = "" + (int)skillManager.fCurrentCoolDown1;

            //�̹��� fillAmount�� 0������ ��
            if (SkillImage1.fillAmount <= 0)
            {
                SkillImage1.fillAmount = 0;
            }
        }
        else
        {
            coolDownText1.GetComponent<Text>().enabled = false;//��ٿ� �ð� �ؽ�Ʈ ��Ȱ��ȭ
        }
    }

    void Skill2_IconUpdade()
    {
        //��ų2
        if (Input.GetKey(skillManager.skill2) && skillManager.bIsCoolDown2 == false && skillManager.bCanUseSkill)
        {
            //��ų2 �ε������� �̹��� Ȱ��ȭ
            skill2TargetCircle.GetComponent<Image>().enabled = true;
            skill2RangeCircle.GetComponent<Image>().enabled = true;
            //������ ��ų �ε������� ��Ȱ��ȭ
            skill1shot.GetComponent<Image>().enabled = false;
        }
        //�̹��� Ȱ��ȭ �� Ŭ����
        if (skill2RangeCircle.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            SkillImage2.fillAmount = 1;
        }
        if (skillManager.bIsCoolDown2)
        {
            //��ų2 �ε������� �̹��� ��Ȱ��ȭ �� ��Ÿ�� �ؽ�Ʈ Ȱ��ȭ
            skill2TargetCircle.GetComponent<Image>().enabled = false;
            skill2RangeCircle.GetComponent<Image>().enabled = false;
            coolDownText2.GetComponent<Text>().enabled = true;

            SkillImage2.fillAmount -= 1 / skillManager.fCoolDown2 * Time.deltaTime;

            coolDownText2.text = "" + (int)skillManager.fCurrentCoolDown2;

            if (SkillImage2.fillAmount <= 0)
            {
                SkillImage2.fillAmount = 0;
            }
        }
        else
        {
            coolDownText2.GetComponent<Text>().enabled = false;
        }
    }

    void Skill3_IconUpdade()
    {
        if (skillManager.bIsCoolDown3)
        {
            coolDownText3.GetComponent<Text>().enabled = true;

            SkillImage3.fillAmount -= 1 / skillManager.fCoolDown3 * Time.deltaTime;

            coolDownText3.text = "" + (int)skillManager.fCurrentCoolDown3;

            if (SkillImage3.fillAmount <= 0)
            {
                SkillImage3.fillAmount = 0;
            }
        }
        else
        {
            coolDownText3.GetComponent<Text>().enabled = false;
        }
    }

    void Skill4_IconUpdade()
    {
        //��ų4 �������� ���� ��ų ������ Ȱ��, ��Ȱ�� ����
        if (playerStatManager.fSkillGauge >= 90.0f)
        {
            SkillImage4.fillAmount = 0.0f;
        }
        else
        {
            SkillImage4.fillAmount = 1.0f;
        }

        //��ų4�� Ȱ��ȭ �Ǹ� ������ Skill Indicator �̹��� ��Ȱ��ȭ
        if (Input.GetKey(skillManager.skill4) && skillManager.bSkill4Ready)
        {
            skill1shot.GetComponent<Image>().enabled = false;
            skill2TargetCircle.GetComponent<Image>().enabled = false;
            skill2RangeCircle.GetComponent<Image>().enabled = false;
        }
    }
}
