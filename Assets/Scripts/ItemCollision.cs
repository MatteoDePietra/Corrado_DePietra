using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.CompareTag("Coin"))
        {
            Destroy(gameObject);
            //CoinCounter.coinCount += 1;
            CoinCounter.AddCoin();
        }
        
        //Destroy(gameObject);
    }

}