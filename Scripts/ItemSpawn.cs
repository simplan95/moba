using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//����޽� 

//������ ���� ��ũ��Ʈ(���� �ð����� �÷��̾��ֺ� ���ѻ���)
public class ItemSpawn : MonoBehaviour
{
    public GameObject healPack;//�� ������
    public Transform playerTransform;//�÷��̾��� Ʈ������

    public float fMaxDistance = 8.0f;//�÷��̾� ��ġ�κ��� �������� ��ġ�� �ִ� �ݰ�

    public float fTimeBetSpawnMax = 25.0f;//���� �ִ� �ð� ����
    public float fTimeBetSpawnMin = 20.0f;//���� �ּ� �ð� ����
    private float fTimeBetSpawn; // ���� ����

    private float fLastSpawnTime; // ������ ���� ����

    private void Start()
    {
        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        fTimeBetSpawn = Random.Range(fTimeBetSpawnMin, fTimeBetSpawnMax);
        fLastSpawnTime = 0;
    }

    // �ֱ������� ������ ���� ó�� ����
    private void Update()
    {
        //�����ֱ� �޼���
        if (Time.time >= fLastSpawnTime + fTimeBetSpawn && playerTransform != null)
        {
            // ������ ���� �ð� ����
            fLastSpawnTime = Time.time;
            // ���� �ֱ⸦ �������� ����
            fTimeBetSpawn = Random.Range(fTimeBetSpawn, fTimeBetSpawnMax);
            // ������ ���� ����
            Spawn();
        }
    }

    //������ ����
    private void Spawn()
    {
        //�÷��̾� ��ó���� ����޽� ���� ���� ��ġ ����
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, fMaxDistance);
        //�ٴڿ��� 0.5��ŭ ���� �ø�
        spawnPosition += Vector3.up * 0.5f;

        //���� ����
        GameObject item = Instantiate(healPack, spawnPosition, Quaternion.identity);

        // ������ �������� 8�� �ڿ� �ı�
        Destroy(item, 8.0f);
    }

    // ����޽� ���� ������ ��ġ�� ��ȯ
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ �ϳ��� ����
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ���, randomPos�� ���� ����� ����޽� ���� �� �� ��ȯ
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ���� ��ġ ��ȯ
        return hit.position;
    }
}
