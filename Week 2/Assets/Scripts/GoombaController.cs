using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : PhysicsObject
{

    public float moveSpeed = 5;
    bool goingLeft = true;
    public float playerDistance = 8;
    public bool activated = false;
    Transform player;

    // Start is called before the first frame update

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    protected override void ComputeVelocity() {

        if (!activated) {
            if (Mathf.Abs(player.position.x - transform.position.x) < playerDistance)
                activated = true;
            else return;
        }


        if (goingLeft)
            targetVelocity = new Vector2(-moveSpeed, 0);
        else
            targetVelocity = new Vector2(moveSpeed, 0);

    }


}
