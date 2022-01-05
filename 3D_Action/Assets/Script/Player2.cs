﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 속도 조절
    public float jumppower; //점프 높이 조절
    public GameObject[] weapons;
    public bool[] hasWeapons;

    public int ammo; //탄환
    public int coin; //돈
    public int health; //하트
    public int hasGrenades; //폭탄

    public int maxammo; //탄환
    public int maxcoin; //돈
    public int maxhealth; //하트
    public int maxhasGrenades; //폭탄

    float hAxis; // x축
    float vAxis; // z축

    bool rDown; // run down 달리
    bool jDwon; //jump dwon 점프
    bool gDown; //g down 아이템 입수
    bool sDown1; // swap down 1번장비
    bool sDown2; // swap down 2번장비
    bool sDown3; // swap down 3번장비

    bool isJump; //구분변수, 지금 점프 중
    bool isDodge; //구분변수, 회피
    bool isSwap; //구분변수, 무기 교체

    Vector3 moveVec; //변수 선언
    Vector3 dodgeVec; //닷지 벡터

    Rigidbody rigidbody;
    Animator animator; //애니메이션

    GameObject nearObject; //근처에있는 오브젝트
    GameObject equipWeapon; //장착중인 무기
    int equipWeaponIndex = -1; // 무기 value

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Interaction();
        Swap();
    }

    void GetInput() //변수이름 = Input.누르는방식("Input매니저이름");
    {
        hAxis = Input.GetAxis("Horizontal"); //x축 이동
        vAxis = Input.GetAxis("Vertical"); //z축 이동
        rDown = Input.GetButton("Run"); //달리기 누르는 동안
        jDwon = Input.GetButtonDown("Jump");//점프 누르는 즉시
        gDown = Input.GetButtonDown("Interaction"); //아이템 획득키 누르자마자 바로
        sDown1 = Input.GetButtonDown("Swap1"); // 1번 무기
        sDown2 = Input.GetButtonDown("Swap2"); // 2번 무기
        sDown3 = Input.GetButtonDown("Swap3"); // 3번 무기
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //moveVec은 새로운 vector3 위치를 받음(움직여서 생긴)

        if (isDodge)
            moveVec = dodgeVec; //회피 할 때 움직이는 방향 =  회피하는 방향

        if (isSwap)
            moveVec = Vector3.zero; // 교체할 때 움직임은 0

        transform.position += moveVec * speed * (rDown ? 1f : 0.3f) * Time.deltaTime; //rdown이 참이면 속도가 1, 거짓이면 0.3

        animator.SetBool("isWalk", moveVec != Vector3.zero); //vector3가 0,0,0이 아닐때 걷는 모션이 나온다.
        animator.SetBool("isRun", rDown); //run down 상태일때 달리는 모션이 나온다.
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); //가는 방향으로 바라본다.
    }

    void Jump()
    {
        if (jDwon && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap) //점프키를 누르고, 움직이지 않고, isJump가 false고, isdodge가 아니고 isSwap이 아닐 때(bool형태만 가능한 표시) 
        {
            rigidbody.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
            //rigidbody 안에 AddForce(힘이필요) (위로 올라가는up * jumppower(숫자),ForceMode 안에 Impulse(즉발적인힘)
            animator.SetBool("isJump", true); //isjump가 ture일 때 애니메이션 실행
            animator.SetTrigger("doJump"); //dojump일 때 애니메이션 실행

            isJump = true;
        }//무한 점프를 막기 위해서 isjump변수를 추가해 Jump함수에서 점프키를 누르고, isjump가 아닐때(점프중이 아님) 힘을 받아 위로 올라감 이는 isjump가 ture임
    }

    void OnCollisionEnter(Collision collision) //isjump가 false인 경우를 추가해야 Jump함수가 실행되기에 생성 (충돌함수)
    {
        if (collision.gameObject.tag == "Floor") //collision의 gameobject의 태그가 floor 일 경우(tag추가해서 생성)
        {
            animator.SetBool("isJump", false); //is jump가 false일 때 애니메이션 실행
            isJump = false;
        }
    } //gameObject=Plaer캐릭터 가 Floor태그랑 충돌하면 false임

    void Dodge()
    {
        if (jDwon && moveVec !=Vector3.zero && !isJump && !isDodge && !isSwap) //점프키를 누르고, 움직이고, isJump가 false고, isdodge가 아니고, isSwap도 아닐 때(bool형태만 가능한 표시) 
        {
            dodgeVec = moveVec;
            speed *= 2; //2배의 속도로 움직임
            animator.SetTrigger("doDodge"); //dodge 애니메이션 실행
            isDodge = true; 

            Invoke("DodgeOut", 0.4f); // 시간차 함수 invoke("함수이름(문자열)",시간차)
        }
    }

    void DodgeOut() //isDodge가
    {
        speed *= 0.5f; //2배로 움직이다가 0.5를 곱해서 다시 1(원래속도)가 됨
        isDodge = false; 
    }

    void Interaction() //아이템 줍기
    {
        if(gDown && nearObject != null && !isJump && !isDodge) //g키를 누르지만 근처에 오브젝트가 비어있지 않고, 점프키랑, 회피키를 쓰지 않을 때
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value; //item value의 값을 대입
                hasWeapons[weaponIndex] = true; //hasweapons[value값]

                Destroy(nearObject);
            }
        }
    }

    void OnTriggerStay(Collider other) // 닿다
    {
        if (other.tag == "Weapon") //닿은 태그가 weapon 일경우
            nearObject = other.gameObject; //nearobject에 그 object를 저장

    }

    void OnTriggerExit(Collider other) //벗어나다
    {
        if (other.tag == "Weapon") //벗어난 태그가 weapon일 경우
            nearObject = null; //nearobject에 null을 저장
    }

    void Swap() //무기 교체
    {
        //키를 누르고, 값이 같으면 실행이 안되도록 ( 획득하지 않았는데 무기가 나올 경우에 대한 제약)
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if(sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;


        int weaponIndex = -1; //기본 값
        if (sDown1) weaponIndex = 0; // 해머
        if (sDown2) weaponIndex = 1; // 총
        if (sDown3) weaponIndex = 2; // 머신건

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge) //1 2 3중에서 하나를 누르고, 점프가아니고, 회피가 아닐 때
        {
            if(equipWeapon !=null) //빈 손이 아닐때 실행 (무기를 획득 했을때)
            equipWeapon.SetActive(false); //기본 상태에서

            equipWeaponIndex = weaponIndex; //무기의 값을 맞춰줌
            equipWeapon = weapons[weaponIndex]; // 맞는 숫자를 넣고
            equipWeapon.SetActive(true); // weponindex값에 맞게 행동

            animator.SetTrigger("doSwap"); // 조건에 맞게 실행되면 애니메이션이 나오고
            isSwap = true; //isswap이 참임
            Invoke("SwapOut", 0.5f); //swap할때 속도가 감소
        }
    }

    void SwapOut()
    {
       isSwap = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value; // 정해준 value 만큼 더한다
                    if (ammo > maxammo) //max 수치보다 클경우
                        ammo = maxammo; // max와 같게 해준다
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxcoin)
                        coin = maxcoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxhealth)
                        health = maxhealth;
                    break;
                case Item.Type.Grenade:
                    hasGrenades += item.value;
                    if (hasGrenades > maxhasGrenades)
                        hasGrenades = maxhasGrenades;
                    break;
            }

            Destroy(other.gameObject);
        }
    }
}