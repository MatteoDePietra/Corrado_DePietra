using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.CompareTag("Coin"))
        {
            Destroy(gameObject);
            CoinCounter.AddCoin(1);
        }
        else if (this.gameObject.CompareTag("Star"))
        {
            Destroy(gameObject);
            CoinCounter.AddCoin(3);
        }
        else if (this.gameObject.CompareTag("Diamond"))
        {
            Destroy(gameObject);
            DiamondCounter.AddCoin();
        }
        else if (this.gameObject.CompareTag("Plus"))
        {
            Destroy(gameObject);
            other.GetComponent<PlayerHealth>().Heal(1);
        }
        else if (this.gameObject.CompareTag("Heart"))
        {
            Destroy(gameObject);
            other.GetComponent<PlayerHealth>().Heal(5);
        }
    }
}