using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon };//enum열거형 0 1 2 3 4 5 6 
    public Type type; //아이템 종류
    public int value; //아이템 갯수

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); //회전
    }
}
