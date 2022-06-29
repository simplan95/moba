using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//내비메시 

//아이템 스폰 스크립트(일정 시간마다 플레이어주변 힐팩생성)
public class ItemSpawn : MonoBehaviour
{
    public GameObject healPack;//힐 아이템
    public Transform playerTransform;//플레이어의 트랜스폼

    public float fMaxDistance = 8.0f;//플레이어 위치로부터 아이템이 배치될 최대 반경

    public float fTimeBetSpawnMax = 25.0f;//스폰 최대 시간 간격
    public float fTimeBetSpawnMin = 20.0f;//스폰 최소 시간 간격
    private float fTimeBetSpawn; // 생성 간격

    private float fLastSpawnTime; // 마지막 생성 시점

    private void Start()
    {
        // 생성 간격과 마지막 생성 시점 초기화
        fTimeBetSpawn = Random.Range(fTimeBetSpawnMin, fTimeBetSpawnMax);
        fLastSpawnTime = 0;
    }

    // 주기적으로 아이템 생성 처리 실행
    private void Update()
    {
        //생성주기 달성시
        if (Time.time >= fLastSpawnTime + fTimeBetSpawn && playerTransform != null)
        {
            // 마지막 생성 시간 갱신
            fLastSpawnTime = Time.time;
            // 생성 주기를 랜덤으로 변경
            fTimeBetSpawn = Random.Range(fTimeBetSpawn, fTimeBetSpawnMax);
            // 아이템 생성 실행
            Spawn();
        }
    }

    //아이템 생성
    private void Spawn()
    {
        //플레이어 근처에서 내비메시 위의 랜덤 위치 설정
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, fMaxDistance);
        //바닥에서 0.5만큼 위로 올림
        spawnPosition += Vector3.up * 0.5f;

        //힐팩 생성
        GameObject item = Instantiate(healPack, spawnPosition, Quaternion.identity);

        // 생성된 아이템을 8초 뒤에 파괴
        Destroy(item, 8.0f);
    }

    // 내비메시 위의 랜덤한 위치를 반환
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // 내비메시 샘플링의 결과 정보
        NavMeshHit hit;

        // maxDistance 반경 안에서, randomPos에 가장 가까운 내비메시 위의 한 점 반환
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // 생성 위치 반환
        return hit.position;
    }
}
