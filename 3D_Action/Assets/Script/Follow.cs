using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    void Update()
    {
        transform.position = target.position + offset; //카메라 위치는 타겟 위치 + offset
    }
}
