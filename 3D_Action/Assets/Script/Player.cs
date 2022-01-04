using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 속도 조절
    float hAxis; // x축
    float vAxis; // z축
    bool rDown; // run down 달리는 버튼 설정

    Vector3 moveVec; //변수 선언
    Animator animator; //애니메이션

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
        hAxis = Input.GetAxis("Horizontal"); //x축 이동
        vAxis = Input.GetAxis("Vertical"); //z축 이동
        rDown = Input.GetButton("Run"); //달리기

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //moveVec은 새로운 vector3 위치를 받음(움직여서 생긴)

        transform.position += moveVec * speed * (rDown ? 1f : 0.3f) * Time.deltaTime; //rdown이 참이면 속도가 1, 거짓이면 0.3

        animator.SetBool("isWalk", moveVec != Vector3.zero); //vector3가 0,0,0이 아닐때 걷는 모션이 나온다.
        animator.SetBool("isRun", rDown); //run down 상태일때 달리는 모션이 나온다.

        transform.LookAt(transform.position + moveVec); //가는 방향으로 바라본다.


    }
}
