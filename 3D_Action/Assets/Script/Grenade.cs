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
        yield return new WaitForSeconds(3f); //3�� �� ����
        rigidbody.velocity = Vector3.zero; //�����ӵ��ʱ�ȭ
        rigidbody.angularVelocity = Vector3.zero; //�ʱ�ȭ
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll
            (transform.position, 15, Vector3.up, 0f,LayerMask.GetMask("Enemy")); 
        //��ü������� ���δ�, ������Ʈ��ġ���� ������15, ����(�߽���?), ����(5���ָ� ��ü�� �ö󰡼� 0�� �����), ?

        foreach(RaycastHit hitObj in rayHits) //����ź ���� ������ �ǰ��Լ��� ȣ��
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
            //���� ������ ��ġ�� enemy��ũ��Ʈ�� hitbygrenade�Լ��� �ִ� ��ġ�̴�.
        }

        Destroy(gameObject, 5); //����Ʈ�� ������°ű��� �����ؼ� 5��
    }

}
