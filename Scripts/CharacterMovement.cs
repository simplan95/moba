using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//AI

//ĳ���� �̵����� ��ũ��Ʈ
public class CharacterMovement : MonoBehaviour
{
    public float fRotateSpeedMovement = 0.1f;//ĳ���� ȸ�� �ӵ�
    public float fRotateVelocity;

    public NavMeshAgent navAgent;//���� �޽�
    private CharacterCombat characterCombat;//�ɸ��� ����
    private Animator animator;//���ϸ�����
    private RaycastHit hit;//����ĳ��Ʈ 
    public bool bMovingFirst { get; private set; }//�̵����� Ȱ��ȭ ����(�̵� �ǽ�,Ž�� �̽ǽ�) 
    public bool bCanRotation { get; set; }//��ų ��� �� ĳ���� �����̼� ���


    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterCombat = GetComponent<CharacterCombat>();
        animator = GetComponent<Animator>();

        bCanRotation = true;
    }

    private void Update()
    {
        //NavMeshAgent�� magnitude�� Speed�� ������ ���� �ش� ĳ������ �ӵ��� (0.0~1.0) ���� 
        float speed = navAgent.velocity.magnitude / navAgent.speed;

        //�ִϸ������� Speed ���� ����
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        if (Input.GetMouseButtonDown(1))//���콺 ��Ŭ����
        {
            //���콺 ��Ŭ����ġ �������� ����ĳ��Ʈ����
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                    //bMovingFirst�� false(Ž�� ��Ȱ��ȭ)
                    bMovingFirst = false;

                    //hit.point �������� ������ ����,�����Ÿ� 0
                    navAgent.SetDestination(hit.point);
                    navAgent.stoppingDistance = 0.0f;
            }
        }

        //��ǥ ���� ������ bMovingFirst true(Ž�� Ȱ��ȭ)
        if (Vector3.Distance(transform.position, hit.point) <= 0.1)
        {
            bMovingFirst = true;
        }

    }

    public void CharacterRotateToTarget(Vector3 targetVec)
    {
        if (bCanRotation)//ĳ���� ȸ������� false�� ���� ȸ��
        {
            //ĳ���� ȸ������ ���� �� �ε巴�� ȸ��
            Quaternion rotationToLookat = Quaternion.LookRotation(targetVec - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                                     rotationToLookat.eulerAngles.y,
                                                     ref fRotateVelocity,
                                                     fRotateSpeedMovement * Time.deltaTime);

            //ĳ���� rotationY ������ ȸ��
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }

    }
}
