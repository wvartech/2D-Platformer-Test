using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player) {
            Debug.Log("Player entered!");
            GameManager.Instance.AddScore(coinValue);
            Destroy(this.gameObject);
        }
    }

}
