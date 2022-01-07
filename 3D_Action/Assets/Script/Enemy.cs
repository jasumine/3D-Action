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
        if(other.collider.CompareTag("Melee")) //��������� �浹
        {
            Weapon weapon = other.collider.GetComponent<Weapon>();
            curHealth -= weapon.damage; //���� ������ ��ŭ ü���� ��������.

            Debug.Log("Melee : " + curHealth);
        }
        else if(other.collider.CompareTag("Range")) //���̶� �浹
        {
            Bullet bullet = other.collider.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Debug.Log("Range : " + curHealth);
        }
    }
}
