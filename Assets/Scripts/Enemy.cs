using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDestructible {
    [SerializeField] private HeadStomp head;
    [SerializeField] private GameObject proyectile;

    public int HEALTH { get { return health; } }
    private int health = 100;

    private Transform target;

    public bool agro = false;

    private float fireInterval = 2f;
    private float shootDelay = 0;
    private bool shooting = false;

    private void Awake() {
        head.getStomped += gotStomped;
    }

    void Update() {
        if (shootDelay > 0 && !shooting) { shootDelay -= Time.deltaTime; }

        if (agro && !shooting && shootDelay <= 0) {
            StartCoroutine(Shoot());
        }


    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

    private IEnumerator Shoot() {
        shooting = true;
        yield return new WaitForSeconds(0.5f);
        if (target) {
            float dir = Mathf.Sign(target.position.x - transform.position.x);
            Debug.Log(dir);
            Debug.Log(new Vector2(dir * 0.5f, transform.position.y));
            GameObject bullet = Instantiate(proyectile, new Vector2(dir*0.2f + transform.position.x, transform.position.y), Quaternion.identity);
            bullet.GetComponent<Proyectile>().SetDir(((int)dir));
        }
        shootDelay = fireInterval;
        shooting = false;

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null) {


            float dir = collision.transform.position.x - transform.position.x > 0 ? 1 : -1;

            rb.GetComponent<Player>().takeDamage(2, (int)Mathf.Sign(transform.position.x - collision.transform.position.x));
//            rb.AddForce(new Vector2(dir * 200, 100));
        }

    }

    private void gotStomped() {
        Debug.Log("got stomped on head!");
        getHit(100);
    }

    public void getHit(int dmg) {
        health -= dmg;
        if (health <= 0) { StartCoroutine(die()); }
    }

    public IEnumerator die() {
        Debug.Log("I am die.");
        yield return new WaitForSeconds(0);
        Destroy(this.gameObject);

    }
}
