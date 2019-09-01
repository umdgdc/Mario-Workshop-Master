using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float followSpeed = 5f;

    private float currDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (transform.position.x < player.transform.position.x) {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }


    }
}
