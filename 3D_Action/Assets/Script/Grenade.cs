using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f); //3초 후 폭발
        rigidbody.velocity = Vector3.zero; //물리속도초기화
        rigidbody.angularVelocity = Vector3.zero; //초기화
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll
            (transform.position, 15, Vector3.up, 0f,LayerMask.GetMask("Enemy")); 
        //구체모양으로 전부다, 오브젝트위치에서 반지름15, 방향(중심축?), 길이(5를주면 구체가 올라가서 0을 줘야함), ?

        foreach(RaycastHit hitObj in rayHits) //수류탄 범위 적들의 피격함수를 호출
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
            //맞은 적들의 위치는 enemy스크립트의 hitbygrenade함수에 있는 위치이다.
        }

        Destroy(gameObject, 5); //이펙트가 사라지는거까지 생각해서 5초
    }

}
