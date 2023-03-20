using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour, IDestructible
{
    private int dir = 1;
    private float lifeTime = 5f;

    public void SetDir(int dir) {
        this.dir = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(dir, 0, 0) * Time.deltaTime );
        lifeTime-= Time.deltaTime;
        if (lifeTime < 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            Debug.Log("Hit the player!");
            player.takeDamage(1, (int)Mathf.Sign(transform.position.x - collision.transform.position.x));
            Destroy(this.gameObject);
        }
    }

    public void getHit(int dmg) {
        Destroy(gameObject);
    }

}
