using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // �ӵ� ����
    float hAxis; // x��
    float vAxis; // z��
    bool rDown; // run down �޸��� ��ư ����

    Vector3 moveVec; //���� ����
    Animator animator; //�ִϸ��̼�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxis("Horizontal"); //x�� �̵�
        vAxis = Input.GetAxis("Vertical"); //z�� �̵�
        rDown = Input.GetButton("Run"); //�޸���

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //moveVec�� ���ο� vector3 ��ġ�� ����(�������� ����)

        transform.position += moveVec * speed * (rDown ? 1f : 0.3f) * Time.deltaTime; //rdown�� ���̸� �ӵ��� 1, �����̸� 0.3

        animator.SetBool("isWalk", moveVec != Vector3.zero); //vector3�� 0,0,0�� �ƴҶ� �ȴ� ����� ���´�.
        animator.SetBool("isRun", rDown); //run down �����϶� �޸��� ����� ���´�.

        transform.LookAt(transform.position + moveVec); //���� �������� �ٶ󺻴�.


    }
}
