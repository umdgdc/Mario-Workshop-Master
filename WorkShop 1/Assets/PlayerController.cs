using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    //Inspector Variables
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform feetSpot;
    [SerializeField] private float feetCheckDistance = 0.1f;


    //Local Variables
    private bool grounded;
    private float inputHorizontal, inputVertical;
    private bool inputJump;
    private Vector3 moveDirection;
    private int groundLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = 1 << GameData.groundLayer;
    }
    
    // Update is called once per frame
    void Update()
    {

        GetInputs();
        HandleMovement();
        
    }


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Death") {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
         if (collider.transform.tag == "Goal") {
            WinLevel();
        }
    }


    void Die() {
        SceneManager.LoadScene(0);
    }

    void WinLevel() {
        print("Win");
    }

    void HandleMovement() {
        //Perform a groundcheck
        grounded = Physics2D.Raycast(feetSpot.position, Vector2.down, feetCheckDistance, groundLayerMask);


        //Check if jumping
        if (grounded && inputJump) {
            grounded = false;
            rb.AddForce(new Vector3(0, jumpSpeed, 0));
        }

        //Apply movement
        moveDirection = new Vector3(inputHorizontal * runSpeed, rb.velocity.y, 0);
        rb.velocity = moveDirection;

    }

    void GetInputs() {

        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");

    }


}
