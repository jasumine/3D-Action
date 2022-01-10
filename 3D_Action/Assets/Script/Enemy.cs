using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth; //최대 hp
    public int curHealth; //현재 hp
    public Transform target; //쫓아가는 타겟
    public bool isChase; //추격

    Rigidbody rigidbody;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav; //navmesh = 몬스터가 따라다닐 길(bake설정, static 오브젝트만 가능)
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
        { nav.SetDestination(target.position); }//target의 위치를 따라간다
    }

    void FixedUpdate()
    {
        FreezeVeloticy();
    }

    void FreezeVeloticy()
    {
        if (isChase) //추격 중일 때
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero; //물체와 충돌시 자동회전하는거 막음
        }
    }
    void ChaseStart()
    {
        isChase = true;
        animator.SetBool("isWalk", true);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee") //근접무기랑 충돌
        {
            Weapon weapon = other.GetComponent<Weapon>(); //weapon스크립트
            curHealth -= weapon.damage; //무기 데미지 만큼 체력이 없어진다.
            Vector3 reactVec = transform.position - other.transform.position; //맞으면 밀리도록

            StartCoroutine(OnDamage(reactVec, false));

            

        }
        else if(other.tag =="Bullet") //총알이랑 충돌
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject); //충돌하면 총알이 사라지도록

            StartCoroutine(OnDamage(reactVec, false));
        }
    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos; //수류탄이 터진 위치를 빼줌
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade) // (reactVec , true or false)
    {
        mat.color = Color.yellow; //맞으면 노란색으로 변경
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0) //현재 체력이 0보다 클경우 = 살아있음
        {
            mat.color = Color.white; //원래대로 변경
        }

        else //0이하가 될경우 = 죽음
        {
            mat.color = Color.black; //검은색으로 변경
            gameObject.layer = 13; //검은색이되면서 13번레이어 Enemy Dead가 되고 못때리게변경
            isChase = false; //더 이상 쫓아가지 않음
            nav.enabled = false; //더 이상 쫓아가지 않음
            animator.SetTrigger("doDie");

            //넉백모션
            if (isGrenade)
            {
                reactVec = reactVec.normalized; //1로 값 통일
                reactVec += Vector3.up * 3; //위로 올라가도록

                rigidbody.freezeRotation = false; //고정된 x ,z 축이 풀림
                rigidbody.AddForce(reactVec * 5, ForceMode.Impulse); //힘이 가해진다
                rigidbody.AddTorque(reactVec * 15, ForceMode.Impulse); //회전한다
            }

            else
            {
                reactVec = reactVec.normalized; //1로 값 통일
                reactVec += Vector3.up; //위로 올라가도록
                rigidbody.AddForce(reactVec * 5, ForceMode.Impulse); //힘이 가해진다
            }
            Destroy(gameObject, 4); //4초뒤에 삭제
        }

    }
}
