using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI

//ĳ���� �������� �Ŵ���
public class CharacterLevelManager : MonoBehaviour
{
    private const int MaxLevel = 10;//�ִ뷹��

    public int iLevel = 1;//ĳ���� ����
    private float fExp;//����ġ
    private float fExpNeeded;//�������� �ʿ��� ����ġ
    private float fPreviousExp;//���緹��-1 ����ġ 

    private PlayerStatManager playerStatManager;//�÷��̾� ���� ��ũ��Ʈ

    public Slider expSlider;//����ġ�� �����̴�
    public Text levelText;//���� �ؽ�Ʈ

    //�������� �ʿ��� ����ġ ����
    public static int ExpNeedToLevelUp(int CurrentLevel)
    {
        if (CurrentLevel == 0) return 0;

        //�������� �ʿ��� ����ġ�� ���緹��*50 ���� ����
        return (CurrentLevel * 50);
    }

    //Exp ����
    public void SetExp(float exp)
    {
        //����ġ �߰�
        fExp += exp;

        fExpNeeded = ExpNeedToLevelUp(iLevel);
        fPreviousExp = ExpNeedToLevelUp(iLevel - 1);

        //���� ����ġ�� �ش� �������� �ʿ��� ����ġ�� ������ ������
        if (fExp >= fExpNeeded)
        {
            LevelUp();
            fExpNeeded = ExpNeedToLevelUp(iLevel);
            fPreviousExp = ExpNeedToLevelUp(iLevel - 1);
        }

        //���� ����ġ�� �������� �ʿ��� ����ġ�� ���� ����ġ �����̵忡 ǥ��
        expSlider.maxValue = fExpNeeded - fPreviousExp;
        expSlider.value = fExp - fPreviousExp;
    }

    public void LevelUp()
    {
        if (iLevel < MaxLevel)
        {
            iLevel++;//������
            levelText.text = "LV" + iLevel;//���� �ؽ�Ʈ ������Ʈ
            playerStatManager.StatsUp();//���Ⱦ�
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
    }
}
