using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.AI;//AI

//�÷��̾� ��ǲ �Ŵ��� ��ũ��Ʈ(ī�޶� ����,���� �� ���ö� ����)
public class PlayerInputManager : MonoBehaviour
{
    public KeyCode CameraViewModeKey;//ī�޶� ���庯ȯ Ű
    public CameraUnlock cameraUnlock;//ī�޶� ���� ���� ��ũ��Ʈ
    public CameraFollow cameraFollow;//ī�޶� ���� ��ũ��Ʈ

    public KeyCode NomalAttackKey;//�Ϲݰ��� Ű
    public Image NomalAttackRangeCircle;//������Ŭ �̹���
    private NavMeshAgent navAgent;//���� �޽�
    private CharacterCombat characterCombat;//ĳ���� �Ĺ�
    private RaycastHit hit;//����ĳ��Ʈ ���
    public bool bMovingAndSearch { get; private set; }//�̵� �� Ž�� Ȱ��ȭ ����

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterCombat = GetComponent<CharacterCombat>();

        cameraUnlock.enabled = false;//CameraUnlock ��Ȱ��ȭ
        cameraFollow.enabled = true;//CameraFollow Ȱ��ȭ

        NomalAttackRangeCircle.GetComponent<Image>().enabled = false;//�Ϲݰ��� ���� �̹��� ��Ȱ��ȭ
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(CameraViewModeKey))//ī�޶� ���� ü����
        {
            cameraUnlock.enabled = !cameraUnlock.enabled;//CameraUnlock Ȱ��ȭ ü����
            cameraFollow.enabled = !cameraFollow.enabled;//CameraFollow Ȱ��ȭ ü����
        }

        //�̵� �� Ž��(���ö�)
        NomalAttackUpdate();
    }

    void NomalAttackUpdate()
    {
        // �̵� �� Ž��Ű Ȱ��ȭ
        if (Input.GetKey(NomalAttackKey))
        {
            NomalAttackRangeCircle.GetComponent<Image>().enabled = true;//�Ϲݰ��� �ε������� �̹��� Ȱ��ȭ 
        }
        //�̹��� Ȱ��ȭ �� ��Ŭ����
        if (NomalAttackRangeCircle.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            //����ĳ��Ʈ ���� 
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                NomalAttackRangeCircle.GetComponent<Image>().enabled = false;//�Ϲݰ��� �ε������� �̹��� ��Ȱ��ȭ

                //����ĳ��Ʈ ������� TargetableŬ���� ��ȯ�� Ÿ�꿡�ʹ� �Ҵ�
                if (hit.collider.GetComponent<Targetable>() != null && hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    if (hit.collider.GetComponent<Targetable>().eTargetType == Targetable.EnemyType.minion)
                    {
                        gameObject.GetComponent<CharacterCombat>().targetedEnemy = hit.collider.GetComponent<Targetable>();
                    }
                }

                //Ÿ�� �� ��ȯ�� ����ĳ��Ʈ �������� �̵� �� Ž��
                else if (hit.collider.GetComponent<Targetable>() == null || !hit.collider.GetComponent<Targetable>().IsCanTargetable())
                {
                    gameObject.GetComponent<CharacterCombat>().targetedEnemy = null;
                    //hit.point �������� ������ ����,�����Ÿ� 0
                    navAgent.SetDestination(hit.point);
                    navAgent.stoppingDistance = 0.0f;

                    //�̵� �� Ž�� Ȱ��ȭ
                    bMovingAndSearch = true;
                }
            }
        }
        //Ÿ�� Ȯ�ν�
        if (characterCombat.targetedEnemy != null)
        {
            //�̵� �� Ž�� ��Ȱ��ȭ
            bMovingAndSearch = false;
        }
    }
}
