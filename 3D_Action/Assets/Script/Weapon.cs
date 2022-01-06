using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range}; // ����, ���Ÿ�
    public Type type; //���� ���� ����
    public int damage; //������ ������
    public float rate; //���� �ӵ�
    public int maxAmmo; //�ִ� �Ѿ� ����
    public int curAmmo; //���� �Ѿ� ����

    public BoxCollider meleeArea; //�������� ����
    public TrailRenderer trailEffect; // ����Ʈ

    public Transform bulletPos; //�Ѿ� ��ġ
    public GameObject bullet; //�Ѿ�
    public float bulletspeed; //�Ѿ� �ӵ�
    public Transform bulletCasePos;//ź�� ��ġ
    public GameObject bulletCase; //ź��

    public void Use()
    {
        if (type == Type.Melee) //���������̶��
        {
            StopCoroutine("Swing"); // ���� �����ϴ� �Լ��� ����
            StartCoroutine("Swing"); //�ڷ�ƾ�� Swing();���� StartCoroutine("")������� �����Լ�, ���� �����ϴ� �Լ�
        }

        else if(type == Type.Range && curAmmo>0)
        {
            curAmmo --;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() //������ �Լ� Ŭ����
    {
        //1
        yield return new WaitForSeconds(0.1f); //  0.1f =0.1�� ��� , null=1������ ���
        meleeArea.enabled = true; //�������� ����
        trailEffect.enabled = true; //����Ʈ ����
        //2
        yield return new WaitForSeconds(0.3f); //0.3����
        meleeArea.enabled = false; //�������� ����
        //3
        yield return new WaitForSeconds(0.3f); //0.3����
        trailEffect.enabled = false;//����Ʈ ����

        //yield return null = ����� ��ȯ�ϴµ� null���� ��ȯ
        //yield�� ������ ����ص���
        //WaitForSeconds() �־��� ��ġ��ŭ ��ٸ��� �Լ�
        // yield break; ���� �ؿ� ��Ȱ��ȭ ��, �ڷ�ƾ Ż��
    }

    //�Ϲ��Լ� : Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ 
    //�ڷ�ƾ : Use() ���η�ƾ + Swing() �ڷ�ƾ(Co - Op) ( ���� ���� ) 

    IEnumerator Shot() //������ �Լ� Ŭ����
    {
        // #1.�Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation); //instantiate �ν��Ͻ�ȭ (����, ��ġ, ����)
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * bulletspeed; //����(forward =z�� * �Ѿ� �ӵ� ���� )

        yield return null; // 1������ ����

        // #2. ź�� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody casetRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        //���� �ش� forward�� �ݴ� ����(���̳ʽ����� �����ϰ�) + ��¦ ���� vector3.up �����ϰ� 
        casetRigid.AddForce(caseVec, ForceMode.Impulse); //�����
        casetRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); //ȸ���Լ� (ȸ����)
    }
}
