using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.SceneManagement; // 씬 관리자 관련 

//UI 관리, 업데이트 스크립트
public class GameUIManager : MonoBehaviour
{
    // UI 매니저 싱글톤 접근용 프로퍼티
    public static GameUIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameUIManager>();
            }

            return m_instance;
        }
    }

    private static GameUIManager m_instance; // 싱글톤 할당 static 변수

    public PlayerStatManager playerStatManager;//플레이어 스탯매니저
    public Text waveText; // 적 웨이브 표시용 텍스트
    public Text gameOverText; // 게임오버 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public EnemySpawn enemySpawn; //에너미 스폰 스크립트

    // Start is called before the first frame update
    void Start()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameUIManager가 있을 경우
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameOverText.text = "YOU DIED" + "\nWave Record : " + enemySpawn.iCurrentWave;

        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
