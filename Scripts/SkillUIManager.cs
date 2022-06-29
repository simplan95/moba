using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI

//스킬 UI 관리 스크립트(아이콘, 인디게이터)
public class SkillUIManager : MonoBehaviour
{
    private PlayerStatManager playerStatManager;//플레이어 스탯
    private SkillManager skillManager;//스킬매니저

    [Header("Skill1")]
    public Image SkillImage1;//스킬 아이콘 
    public Text coolDownText1;//스킬 쿨다운 시간표시 텍스트

    public Canvas skill1Canvas;//스킬1 캔버스
    public Image skill1shot;//스킬1 이미지

    [Header("Skill2")]
    public Image SkillImage2;//스킬 아이콘
    public Text coolDownText2;//스킬 쿨다운 시간표시 텍스트

    //Skill2 입력 변수
    public Canvas skill2Canvas;//스킬2 캔버스
    public Image skill2TargetCircle;//타깃 서클 이미지
    public Image skill2RangeCircle;//범위서클 이미지


    [Header("Skill3")]
    public Image SkillImage3;//스킬 아이콘
    public Text coolDownText3;


    [Header("Skill4")]
    public Image SkillImage4;//스킬 아이콘


    // Start is called before the first frame update
    void Start()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
        skillManager = GetComponent<SkillManager>();

        //스킬 쿨다운 이미지 초기화
        SkillImage1.fillAmount = 0;
        SkillImage2.fillAmount = 0;
        SkillImage3.fillAmount = 0;
        SkillImage4.fillAmount = 0;

        //스킬 인디케이터 이미지 초기화
        skill1shot.GetComponent<Image>().enabled = false;
        skill2TargetCircle.GetComponent<Image>().enabled = false;
        skill2RangeCircle.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //스킬 아이콘 업데이트
        Skill1_IconUpdade();
        Skill2_IconUpdade();
        Skill3_IconUpdade();
        Skill4_IconUpdade();

        //스킬1 
        //스킬1 캔버스가 회전할 방향
        Quaternion totateTo = Quaternion.LookRotation(skillManager.skillPointer - transform.position);
        //x축 회전은 고정시킴
        totateTo.eulerAngles = new Vector3(0, totateTo.eulerAngles.y, totateTo.eulerAngles.z);
        //스킬1 캔버스를 해당목표로 회전
        skill1Canvas.transform.rotation = Quaternion.Lerp(totateTo, skill1Canvas.transform.rotation, 0f);
        //스킬1 캔버스를 스킬게이지계수에 따라 범위가 업데이트
        skill1Canvas.transform.localScale = new Vector3(1f * playerStatManager.fSkillGaugeCoefficient, 1f, 1f);


        //skill2Canvas 위치를 타깃지점으로 업데이트
        skill2Canvas.transform.position = skillManager.skillPointerInRange;
        //스킬2 캔버스를 스킬게이지계수에 따라 범위 업데이트
        skill2Canvas.transform.localScale = new Vector3(3f * playerStatManager.fSkillGaugeCoefficient,
                                                        3f * playerStatManager.fSkillGaugeCoefficient, 3f);
    }

    void Skill1_IconUpdade()
    {
        //스킬버튼 활성화 및 쿨다운이 초기화 되었을 때
        if (Input.GetKey(skillManager.skill1) && skillManager.bIsCoolDown1 == false && skillManager.bCanUseSkill)
        {
            skill1shot.GetComponent<Image>().enabled = true;//스킬1 인디케이터 이미지 활성화 
            //나머지 스킬 인디케이터 비활성화
            skill2TargetCircle.GetComponent<Image>().enabled = false;
            skill2RangeCircle.GetComponent<Image>().enabled = false;

        }
        //이미지 활성화 및 좌클릭시
        if (skill1shot.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            SkillImage1.fillAmount = 1;//스킬 아이콘 이미지 
        }
        //쿨다운 중일 때
        if (skillManager.bIsCoolDown1)
        {
            skill1shot.GetComponent<Image>().enabled = false;//스킬1 인디케이터 이미지 비활성화 
            coolDownText1.GetComponent<Text>().enabled = true;//쿨다운 시간 텍스트 활성화

            //쿨다운 시간 감소
            SkillImage1.fillAmount -= 1 / skillManager.fCoolDown1 * Time.deltaTime;

            //쿨다운 텍스트 업데이트
            coolDownText1.text = "" + (int)skillManager.fCurrentCoolDown1;

            //이미지 fillAmount가 0이하일 때
            if (SkillImage1.fillAmount <= 0)
            {
                SkillImage1.fillAmount = 0;
            }
        }
        else
        {
            coolDownText1.GetComponent<Text>().enabled = false;//쿨다운 시간 텍스트 비활성화
        }
    }

    void Skill2_IconUpdade()
    {
        //스킬2
        if (Input.GetKey(skillManager.skill2) && skillManager.bIsCoolDown2 == false && skillManager.bCanUseSkill)
        {
            //스킬2 인디케이터 이미지 활성화
            skill2TargetCircle.GetComponent<Image>().enabled = true;
            skill2RangeCircle.GetComponent<Image>().enabled = true;
            //나머지 스킬 인디케이터 비활성화
            skill1shot.GetComponent<Image>().enabled = false;
        }
        //이미지 활성화 및 클릭시
        if (skill2RangeCircle.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            SkillImage2.fillAmount = 1;
        }
        if (skillManager.bIsCoolDown2)
        {
            //스킬2 인디케이터 이미지 비활성화 및 쿨타임 텍스트 활성화
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
        //스킬4 게이지에 따라 스킬 아이콘 활성, 비활성 결정
        if (playerStatManager.fSkillGauge >= 90.0f)
        {
            SkillImage4.fillAmount = 0.0f;
        }
        else
        {
            SkillImage4.fillAmount = 1.0f;
        }

        //스킬4가 활성화 되면 나머지 Skill Indicator 이미지 비활성화
        if (Input.GetKey(skillManager.skill4) && skillManager.bSkill4Ready)
        {
            skill1shot.GetComponent<Image>().enabled = false;
            skill2TargetCircle.GetComponent<Image>().enabled = false;
            skill2RangeCircle.GetComponent<Image>().enabled = false;
        }
    }
}
