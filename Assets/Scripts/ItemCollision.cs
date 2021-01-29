using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.CompareTag("Coin"))
        {
            Destroy(gameObject);
            //CoinCounter.coinCount += 1;
            CoinCounter.AddCoin();
        }
    }
}