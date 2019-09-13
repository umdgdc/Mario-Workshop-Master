using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * 5, rb.velocity.y, 0);
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 400));
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        print("sdad");
        if (collision.gameObject.tag == "Die")
        {

            print("dddd");
            SceneManager.LoadScene("SampleScene");
        } else if (collision.gameObject.tag == "Win")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }



}
