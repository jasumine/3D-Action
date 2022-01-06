using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; //���� �߽�
    public float orbitSpeed; // ���� �ӵ�
    Vector3 offset; // �÷��̾���� �Ÿ�


    void Start()
    {
        offset = transform.position - target.position; //���� ����ź�� ��ġ - Ÿ���� ��ġ
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

        offset = transform.position - target.position; // ��ġ�� �� �ٲ��� update�������
    }
}
