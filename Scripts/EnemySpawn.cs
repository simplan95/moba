using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��(�̴Ͼ�) ��ȯ ��ũ��Ʈ
public class EnemySpawn : MonoBehaviour
{
    public GameObject[] minionType;// �̴Ͼ� Ÿ��
    public Transform[] spawnPoints;// �̴Ͼ� ��ȯ�� ��ġ

    private List<GameObject> minions = new List<GameObject>();//������ �̴Ͼ� ���� ����Ʈ
    public int iCurrentWave { get; private set; }//���� ���̺�

    private void Update()
    {
        // ���� ���� �����϶� �̴Ͼ� �̻���
        if (GamePlayManager.instance != null && GamePlayManager.instance.isGameover)
        {
            return;
        }

        //�̴Ͼ��� ��� ����ĥ��� �� ���̺� ����
        if (minions.Count <= 0)
        {
            SpawnWave();
        }

        // UI ����
        UpdateUI();
    }

    // ���̺� ������ UI�� ǥ��
    private void UpdateUI()
    {
        // ���� ���̺�� ���� �� �� ǥ��
        GameUIManager.instance.UpdateWaveText(iCurrentWave, minions.Count);
    }

    // ���̺� ������ ���� �̴Ͼ� ����
    private void SpawnWave()
    {
        // ���̺� 1 ����
        iCurrentWave++;

        // ���� ���̺� * 1.5�� �ݿø� �� ���� ��ŭ �̴Ͼ� ����
        int spawnCount = Mathf.RoundToInt(iCurrentWave * 1.5f);

        //spawnCount ��ŭ �̴Ͼ� ����
        for (int i = 0; i < spawnCount; i++)
        {
            //�̴Ͼ� ����
            CreateMinion();
        }
    }

    //�̴Ͼ��� �����ϰ� ������ �̴Ͼ𿡰� ������ ����� �Ҵ�
    private void CreateMinion()
    {
        //������ �̴Ͼ� Ÿ�� ���� ����
        GameObject type = minionType[Random.Range(0, minionType.Length)];

        //���� ��ġ ��������
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //�̴Ͼ� ����
        GameObject minion = Instantiate(type, spawnPoint.position, spawnPoint.rotation);

        //������ �̴Ͼ� ����Ʈ�� �߰�
        minions.Add(minion);

        //�̴Ͼ� onDeath �̺�Ʈ�� �޼��� ���
        //����� �̴Ͼ��� ����Ʈ���� ����
        minion.GetComponent<CharacterStatManager>().onDeath += () => minions.Remove(minion);
    }
}
