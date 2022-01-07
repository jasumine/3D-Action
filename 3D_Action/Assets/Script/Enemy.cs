using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigidbody;
    BoxCollider boxCollider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Melee")) //근접무기랑 충돌
        {
            Weapon weapon = other.collider.GetComponent<Weapon>();
            curHealth -= weapon.damage; //무기 데미지 만큼 체력이 없어진다.

            Debug.Log("Melee : " + curHealth);
        }
        else if(other.collider.CompareTag("Range")) //총이랑 충돌
        {
            Bullet bullet = other.collider.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Debug.Log("Range : " + curHealth);
        }
    }
}
