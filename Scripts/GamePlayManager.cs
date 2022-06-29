using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//신관련

//게임 플레이관련 매니저 스크립트
public class GamePlayManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static GamePlayManager instance
    {
        get
        {
            // 싱글톤 변수가 null 일때
            if (m_instance == null)
            {
                //GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GamePlayManager>();
            }

            // 싱글톤 오브젝트 반환
            return m_instance;
        }
    }

    private static GamePlayManager m_instance; // 싱글톤 할당 static 변수
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerStatManager>().onDeath += EndGame;
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        GameUIManager.instance.SetActiveGameoverUI(true);
    }
}
