using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ŭ���� Ÿ���Ҵ� �Ǵ� ���� ��ũ��Ʈ
public class InputTargeting : MonoBehaviour
{
    public GameObject selectedCharacter;//�÷��̾ �������� �ɸ���
    public bool bIsPlayer;//�÷��̾� �Ǵ�
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾�� �±׵� ���� ������Ʈ�� selectedCharacter�� �Ҵ�
        selectedCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //���콺 ��Ŭ����
        if (Input.GetMouseButtonDown(1))
        {
            //���콺 ��ġ�������� ����ĳ��Ʈ ����
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                //����ĳ��Ʈ ������� TargetableŬ���� ��ȯ�� Ÿ�� ���ɽ�
                if (hit.collider.GetComponent<Targetable>() != null && hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    if (hit.collider.GetComponent<Targetable>().eTargetType == Targetable.EnemyType.minion)
                    {
                        //Ÿ���� ����
                        selectedCharacter.GetComponent<CharacterCombat>().targetedEnemy = hit.collider.GetComponent<Targetable>();
                    }
                }
                //TargetableŬ���� �̹�ȯ �Ǵ� Ÿ�� �Ұ��� ��
                else if (hit.collider.GetComponent<Targetable>() == null || !hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    //Ÿ���� ����
                    selectedCharacter.GetComponent<CharacterCombat>().targetedEnemy = null;
                }
            }
        }
    }
}
