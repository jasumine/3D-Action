using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            Destroy(gameObject, 3); //바닥이랑 닿으면 3초뒤에 삭제
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject); //벽이랑 닿으면 바로 삭제
        }
    }
}
