using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.SceneManagement; // �� ������ ���� 

//UI ����, ������Ʈ ��ũ��Ʈ
public class GameUIManager : MonoBehaviour
{
    // UI �Ŵ��� �̱��� ���ٿ� ������Ƽ
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

    private static GameUIManager m_instance; // �̱��� �Ҵ� static ����

    public PlayerStatManager playerStatManager;//�÷��̾� ���ȸŴ���
    public Text waveText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public Text gameOverText; // ���ӿ��� �ؽ�Ʈ
    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 
    public EnemySpawn enemySpawn; //���ʹ� ���� ��ũ��Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameUIManager�� ���� ���
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameOverText.text = "YOU DIED" + "\nWave Record : " + enemySpawn.iCurrentWave;

        gameoverUI.SetActive(active);
    }

    // ���� �����
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
