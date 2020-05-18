using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            other.gameObject.SetActive(false);
        else if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerHealth>().Damage(5);
    }
}