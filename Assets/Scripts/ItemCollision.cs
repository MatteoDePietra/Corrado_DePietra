using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name=="Coin1")
        {
            gameObject.SetActive(false);
        }
    }
}