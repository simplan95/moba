using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI

//캐릭터 레벨관리 매니저
public class CharacterLevelManager : MonoBehaviour
{
    private const int MaxLevel = 10;//최대레벨

    public int iLevel = 1;//캐릭터 레벨
    private float fExp;//경험치
    private float fExpNeeded;//레벨업에 필요한 경험치
    private float fPreviousExp;//현재레벨-1 경험치 

    private PlayerStatManager playerStatManager;//플레이어 스탯 스크립트

    public Slider expSlider;//경험치바 슬라이더
    public Text levelText;//레벨 텍스트

    //레벨업에 필요한 경험치 설정
    public static int ExpNeedToLevelUp(int CurrentLevel)
    {
        if (CurrentLevel == 0) return 0;

        //레벨업에 필요한 경험치는 현재레벨*50 으로 설정
        return (CurrentLevel * 50);
    }

    //Exp 설정
    public void SetExp(float exp)
    {
        //경험치 추가
        fExp += exp;

        fExpNeeded = ExpNeedToLevelUp(iLevel);
        fPreviousExp = ExpNeedToLevelUp(iLevel - 1);

        //현재 경험치가 해당 레벨업에 필요한 경험치를 넘으면 레벨업
        if (fExp >= fExpNeeded)
        {
            LevelUp();
            fExpNeeded = ExpNeedToLevelUp(iLevel);
            fPreviousExp = ExpNeedToLevelUp(iLevel - 1);
        }

        //현재 경험치와 레벨업에 필요한 경험치를 구해 경험치 슬라이드에 표시
        expSlider.maxValue = fExpNeeded - fPreviousExp;
        expSlider.value = fExp - fPreviousExp;
    }

    public void LevelUp()
    {
        if (iLevel < MaxLevel)
        {
            iLevel++;//레벨업
            levelText.text = "LV" + iLevel;//레벨 텍스트 업데이트
            playerStatManager.StatsUp();//스탯업
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
    }
}
