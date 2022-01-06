using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; //공전 중심
    public float orbitSpeed; // 공전 속도
    Vector3 offset; // 플레이어와의 거리


    void Start()
    {
        offset = transform.position - target.position; //현재 수류탄의 위치 - 타겟의 위치
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

        offset = transform.position - target.position; // 위치가 또 바껴서 update해줘야함
    }
}
