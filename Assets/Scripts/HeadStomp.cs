using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStomp : MonoBehaviour
{
    public delegate void GetStomped();
    public event GetStomped getStomped;
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        
        if (rb) {
            //   Debug.Log(rb.name + " stomped!");
            rb.velocity.Set(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 400));
            getStomped?.Invoke();
        }
    }

}
