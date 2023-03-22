using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour , IDamageable
{
    private Rigidbody2D rb;
    private float jumpForce = 350f;
    private PlayerInputActions playerInput;
    private InputAction movement;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform spriteContainer;
    const string MOVING = "moving";
    const string VERTICAL_SPEED = "verticalSpeed";
    const string STUNNED = "stunned";
    const string GROUNDED = "grounded";
    const string ATTACKING = "attacking";

    private float Xmovement;
    private float moveSpeed = 2f;
    private float groundDistance;
    private bool grounded = true;
    private bool stunned = false;
    private bool attacking = false;
    [SerializeField] GroundCheck groundCheck;

    public int HEALTH {get;set;} = 3;

    public delegate void PlayerDamaged();
    public event PlayerDamaged OnPlayerDamaged;

    public static Player instance;

    private void Awake() {

        if (instance) { Destroy(gameObject); }else { instance = this; }

        rb = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInputActions();
        groundDistance = this.GetComponent<Collider2D>().bounds.extents.y;
        Debug.Log(groundDistance);
        if (groundCheck) { groundCheck.groundCheck += updateGroundDetection; }
    }

    private void OnEnable() {
        movement = playerInput.Player.movement;
        movement.Enable();

        playerInput.Player.jump.performed += OnJump;
        playerInput.Player.jump.Enable();
        playerInput.Player.attack.performed += StartAttack;
        playerInput.Player.attack.Enable();

    }

    private void OnJump(InputAction.CallbackContext obj) {        
        if (grounded && !stunned) {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpForce)); }        
    }

    private void OnDisable() {
        movement.Disable();
        playerInput.Player.jump.Disable();
        playerInput.Player.attack.Disable();
    }


    void Update()
    {
        Xmovement = stunned? 0:movement.ReadValue<float>();
        if (Xmovement != 0) { 
            animator.SetBool(MOVING, true); 
            if (Xmovement == 1) { spriteRenderer.flipX = true; spriteContainer.localPosition = new Vector3(0.2f,0,0); }else 
                { spriteRenderer.flipX = false;
                spriteContainer.localPosition = Vector3.zero;
            }
        } else { animator.SetBool(MOVING, false); }   
        int Ymovement = (int)Mathf.Sign(rb.velocity.y);
        animator.SetInteger(VERTICAL_SPEED, Ymovement);

    }

    private void FixedUpdate() {
        
        if (Xmovement != 0) {
            rb.velocity = new Vector2(Xmovement * moveSpeed, rb.velocity.y);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == null) return;
    }

    private void updateGroundDetection(bool ground) {
        grounded = ground;
        animator.SetBool(GROUNDED, ground);
    }

    public void takeDamage(int dmg, int dir) {
        //Debug.Log($"Player took {dmg} from dir {dir}");
        if (HEALTH > 0) {
            getHit(dmg);
            OnPlayerDamaged?.Invoke();
        }else {
            getHit(dmg);
        }
        
        rb.AddForce(new Vector2(-dir * 200, 100));
        
    }

    private void StartAttack(InputAction.CallbackContext obj) {
        if (!attacking && !stunned) {
            StartCoroutine(Attack());
        }
        
        Debug.Log("Attack pressed");
    }

    private IEnumerator Attack() {
        attacking = true; animator.SetBool(ATTACKING, true);
        AudioManager.Instance.PlaySound("slash");
        yield return new WaitForSeconds(0.25f);
        HitCheck(1);
        attacking = false; animator.SetBool(ATTACKING,false);
    }

    private void HitCheck(int dir) {

        Debug.Log("Hitcheck");
        //     List<Collider2D> hitColliders = new List<Collider2D>();
        Collider2D[] hitColliders;
                //    Physics2D.OverlapCircle(new Vector2(transform.position.x + dir, transform.position.y), 1,ContactFilter2D.NoFilter(),hitColliders);

        hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.6f,1),0);
        

        foreach (Collider2D collider in hitColliders) {
            IDestructible target = collider.GetComponent<IDestructible>();
            if (target != null) {
                Debug.Log($"Hit {collider.gameObject.name}");
                target.getHit(100);
                AudioManager.Instance.PlaySound("slash_hit");
            }
        }
        
    }

    private IEnumerator GetStunned(float duration) {
        stunned = true; 
        animator.SetBool(STUNNED, true);

        yield return new WaitForSeconds(duration);

        stunned = false;
        animator.SetBool(STUNNED, false);
    }

    public void getHit(int dmg) {
        AudioManager.Instance.PlaySound("flinch");
        HEALTH -= dmg;
        if (HEALTH < 0) { HEALTH = 0; }
        if(HEALTH <= 0) { StartCoroutine(die()); } else { StartCoroutine(GetStunned(0.6f)); }
    }

    public IEnumerator die() {
        Debug.Log("I am die thank you forever.");
        stunned = true; animator.SetBool(STUNNED, true);
        yield return new WaitForSeconds(1.2f);
        AudioManager.Instance.PlaySound("player_die");

        Destroy(this.gameObject);
    }
}
