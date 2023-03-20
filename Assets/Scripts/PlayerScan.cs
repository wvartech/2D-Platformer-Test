using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScan : MonoBehaviour
{
    [SerializeField] private Enemy parent;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player) {
            Debug.Log("found player!");
            parent.SetTarget(player.gameObject.transform);
            parent.agro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player) {
            Debug.Log("player left scan area!");
            parent.agro= false;
        }
    }

}
