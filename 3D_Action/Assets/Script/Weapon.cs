using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range}; // 근접, 원거리
    public Type type; //무기 종류 설정
    public int damage; //무기의 데미지
    public float rate; //공격 속도
    public int maxAmmo; //최대 총알 갯수
    public int curAmmo; //현재 총알 갯수

    public BoxCollider meleeArea; //근접공격 범위
    public TrailRenderer trailEffect; // 이펙트

    public Transform bulletPos; //총알 위치
    public GameObject bullet; //총알
    public float bulletspeed; //총알 속도
    public Transform bulletCasePos;//탄피 위치
    public GameObject bulletCase; //탄피

    public void Use()
    {
        if (type == Type.Melee) //근접공격이라면
        {
            StopCoroutine("Swing"); // 원래 실행하는 함수를 멈춤
            StartCoroutine("Swing"); //코루틴은 Swing();말고 StartCoroutine("")해줘야함 실행함수, 새로 시작하는 함수
        }

        else if(type == Type.Range && curAmmo>0)
        {
            curAmmo --;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() //열거형 함수 클래스
    {
        //1
        yield return new WaitForSeconds(0.1f); //  0.1f =0.1초 대기 , null=1프레임 대기
        meleeArea.enabled = true; //근접공격 실행
        trailEffect.enabled = true; //이펙트 실행
        //2
        yield return new WaitForSeconds(0.3f); //0.3초후
        meleeArea.enabled = false; //근접공격 멈춤
        //3
        yield return new WaitForSeconds(0.3f); //0.3초후
        trailEffect.enabled = false;//이펙트 멈춤

        //yield return null = 결과를 반환하는데 null값을 반환
        //yield는 여러개 사용해도됨
        //WaitForSeconds() 주어진 수치만큼 기다리는 함수
        // yield break; 쓰면 밑에 비활성화 됨, 코루틴 탈출
    }

    //일반함수 : Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 
    //코루틴 : Use() 메인루틴 + Swing() 코루틴(Co - Op) ( 같이 실행 ) 

    IEnumerator Shot() //열거형 함수 클래스
    {
        // #1.총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation); //instantiate 인스턴스화 (변수, 위치, 각도)
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * bulletspeed; //방향(forward =z축 * 총알 속도 적용 )

        yield return null; // 1프레임 쉬고

        // #2. 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody casetRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        //힘을 준다 forward의 반대 방향(마이너스값을 랜덤하게) + 살짝 위로 vector3.up 랜덤하게 
        casetRigid.AddForce(caseVec, ForceMode.Impulse); //즉발적
        casetRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); //회전함수 (회전축)
    }
}
