using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{   

    public enum Type { Ammo, Coin, Grenade, Heart, Weapon}; //���� x, �ϳ��� Ÿ��
    public Type type;
    public int value;

    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 25 * Time.deltaTime);
    }
}
