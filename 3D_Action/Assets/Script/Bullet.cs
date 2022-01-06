using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Floor"))
        {
            Destroy(gameObject, 3); //�ٴ��̶� ������ 3�ʵڿ� ����
        }

        else if(collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject); //���̶� ������ �ٷ� ����
        }
    }
}
