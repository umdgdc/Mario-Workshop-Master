using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float jumpFudgeFactor = 0.2f;
    bool dead = false;
    float inputX;
    bool inputJumpUp, inputJumpDown;

    public float downGravityScaleMax = 2, downGravityScaleIncrement = 0.005f, currDownGravityScale = 0;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity() {
        GetInputs();

        Vector2 move = Vector2.zero;

        move.x = inputX;
      

        //Handle Jumping
        if (inputJumpDown && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (inputJumpUp) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }


        //Apply GravityScaleMovement
        if (!grounded && velocity.y < 0) {
            if (currDownGravityScale < downGravityScaleMax)
            currDownGravityScale += downGravityScaleIncrement;
            velocity.y -= currDownGravityScale;
        } else if (grounded) {
            currDownGravityScale = 0;
        }

        //Flip the sprite if needed
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if ( inputX != 0 && flipSprite) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        //Set Animator variables
        animator.SetBool("Running", inputX != 0);
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



    public void GetInputs() { 
        inputX = Input.GetAxis("Horizontal");
        inputJumpDown = Input.GetButtonDown("Jump");
        inputJumpUp = Input.GetButtonUp("Jump");
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
                collision.gameObject.GetComponent<Enemy>().Die();
                velocity.y = jumpTakeOffSpeed;

            } else {
                DieAnimation();
            }
            }
    }

}
