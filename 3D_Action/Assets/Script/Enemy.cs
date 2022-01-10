using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth; //�ִ� hp
    public int curHealth; //���� hp
    public Transform target; //�Ѿư��� Ÿ��
    public bool isChase; //�߰�

    Rigidbody rigidbody;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav; //navmesh = ���Ͱ� ����ٴ� ��(bake����, static ������Ʈ�� ����)
    Animator animator;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

   void Update()
    {
        if (isChase)
        { nav.SetDestination(target.position); }//target�� ��ġ�� ���󰣴�
    }

    void FixedUpdate()
    {
        FreezeVeloticy();
    }

    void FreezeVeloticy()
    {
        if (isChase) //�߰� ���� ��
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero; //��ü�� �浹�� �ڵ�ȸ���ϴ°� ����
        }
    }
    void ChaseStart()
    {
        isChase = true;
        animator.SetBool("isWalk", true);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee") //��������� �浹
        {
            Weapon weapon = other.GetComponent<Weapon>(); //weapon��ũ��Ʈ
            curHealth -= weapon.damage; //���� ������ ��ŭ ü���� ��������.
            Vector3 reactVec = transform.position - other.transform.position; //������ �и�����

            StartCoroutine(OnDamage(reactVec, false));

            

        }
        else if(other.tag =="Bullet") //�Ѿ��̶� �浹
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject); //�浹�ϸ� �Ѿ��� ���������

            StartCoroutine(OnDamage(reactVec, false));
        }
    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos; //����ź�� ���� ��ġ�� ����
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade) // (reactVec , true or false)
    {
        mat.color = Color.yellow; //������ ��������� ����
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0) //���� ü���� 0���� Ŭ��� = �������
        {
            mat.color = Color.white; //������� ����
        }

        else //0���ϰ� �ɰ�� = ����
        {
            mat.color = Color.black; //���������� ����
            gameObject.layer = 13; //�������̵Ǹ鼭 13�����̾� Enemy Dead�� �ǰ� �������Ժ���
            isChase = false; //�� �̻� �Ѿư��� ����
            nav.enabled = false; //�� �̻� �Ѿư��� ����
            animator.SetTrigger("doDie");

            //�˹���
            if (isGrenade)
            {
                reactVec = reactVec.normalized; //1�� �� ����
                reactVec += Vector3.up * 3; //���� �ö󰡵���

                rigidbody.freezeRotation = false; //������ x ,z ���� Ǯ��
                rigidbody.AddForce(reactVec * 5, ForceMode.Impulse); //���� ��������
                rigidbody.AddTorque(reactVec * 15, ForceMode.Impulse); //ȸ���Ѵ�
            }

            else
            {
                reactVec = reactVec.normalized; //1�� �� ����
                reactVec += Vector3.up; //���� �ö󰡵���
                rigidbody.AddForce(reactVec * 5, ForceMode.Impulse); //���� ��������
            }
            Destroy(gameObject, 4); //4�ʵڿ� ����
        }

    }
}
