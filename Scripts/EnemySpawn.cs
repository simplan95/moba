using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적(미니언) 소환 스크립트
public class EnemySpawn : MonoBehaviour
{
    public GameObject[] minionType;// 미니언 타입
    public Transform[] spawnPoints;// 미니언 소환할 위치

    private List<GameObject> minions = new List<GameObject>();//생성된 미니언 저장 리스트
    public int iCurrentWave { get; private set; }//현재 웨이브

    private void Update()
    {
        // 게임 오버 상태일때 미니언 미생성
        if (GamePlayManager.instance != null && GamePlayManager.instance.isGameover)
        {
            return;
        }

        //미니언을 모두 물리칠경우 새 웨이브 시작
        if (minions.Count <= 0)
        {
            SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적 수 표시
        GameUIManager.instance.UpdateWaveText(iCurrentWave, minions.Count);
    }

    // 웨이브 레벨에 따라 미니언 생성
    private void SpawnWave()
    {
        // 웨이브 1 증가
        iCurrentWave++;

        // 현재 웨이브 * 1.5에 반올림 한 개수 만큼 미니언 생성
        int spawnCount = Mathf.RoundToInt(iCurrentWave * 1.5f);

        //spawnCount 만큼 미니언 생성
        for (int i = 0; i < spawnCount; i++)
        {
            //미니언 생성
            CreateMinion();
        }
    }

    //미니언을 생성하고 생성한 미니언에게 추적할 대상을 할당
    private void CreateMinion()
    {
        //생성될 미니언 타입 랜덤 결정
        GameObject type = minionType[Random.Range(0, minionType.Length)];

        //생성 위치 랜덤결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //미니언 생성
        GameObject minion = Instantiate(type, spawnPoint.position, spawnPoint.rotation);

        //생성된 미니언 리스트에 추가
        minions.Add(minion);

        //미니언 onDeath 이벤트에 메서드 등록
        //사망한 미니언을 리스트에서 제거
        minion.GetComponent<CharacterStatManager>().onDeath += () => minions.Remove(minion);
    }
}
