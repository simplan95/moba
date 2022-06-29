using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//�Ű���

//���� �÷��̰��� �Ŵ��� ��ũ��Ʈ
public class GamePlayManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
    public static GamePlayManager instance
    {
        get
        {
            // �̱��� ������ null �϶�
            if (m_instance == null)
            {
                //GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GamePlayManager>();
            }

            // �̱��� ������Ʈ ��ȯ
            return m_instance;
        }
    }

    private static GamePlayManager m_instance; // �̱��� �Ҵ� static ����
    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerStatManager>().onDeath += EndGame;
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        GameUIManager.instance.SetActiveGameoverUI(true);
    }
}
