using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float jumpFudgeFactor = 0.2f;
    bool dead = false;

    public float downGravityScaleMax = 2, downGravityScaleIncrement = 0.005f, currDownGravityScale = 0;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity() {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        animator.SetBool("Running", Input.GetAxis("Horizontal") != 0);

        if (Input.GetButtonDown("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp("Jump")) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (!grounded && velocity.y < 0) {
            if (currDownGravityScale < downGravityScaleMax)
            currDownGravityScale += downGravityScaleIncrement;

            velocity.y -= currDownGravityScale;

        }

        if (grounded) {
            currDownGravityScale = 0;
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if ( Input.GetAxis("Horizontal") != 0 && flipSprite) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }


        

        animator.SetBool("Grounded", grounded);
        
        targetVelocity = move * maxSpeed;
    }

    public void Die() {
        SceneManager.LoadScene("SampleScene");
    }

    public void DieAnimation() {
        if (!dead)
        animator.SetTrigger("DieTrigger");
        enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());
        dead = true;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Die") {
            DieAnimation();
        }

        if (collision.gameObject.tag == "Win") {
            print("Win");
            SceneManager.LoadScene("WinScreen");
        }

        if (collision.gameObject.tag == "Enemy") {
            print("My Y = " + transform.position.y + " Enemy Y= " + collision.transform.position.y);

            if (transform.position.y >= collision.transform.position.y) {
                Destroy(collision.gameObject);
                velocity.y = jumpTakeOffSpeed;

            } else {
                //collision.gameObject.SetActive(false);
                DieAnimation();
            }
            }
    }

}
