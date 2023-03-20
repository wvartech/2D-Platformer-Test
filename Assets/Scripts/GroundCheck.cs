using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool grounded = true;
    public delegate void DetectGround(bool ground);
    public event DetectGround groundCheck;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ground") {
            groundCheck?.Invoke(true);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "ground") {
            groundCheck?.Invoke(false);            
        }
    }

}
