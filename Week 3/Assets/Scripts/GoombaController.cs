﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : PhysicsObject
{

    public float moveSpeed = 5;
    bool goingLeft = true;
    public float playerDistance = 8;
    public float castDistance = 1.1f;
    public bool activated = false;
    Transform player;
    Vector2 castDirection;
    Vector2 castPoint;

    // Start is called before the first frame update

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetCastDirection();
        castPoint = GetComponent<BoxCollider2D>().bounds.center;
    }


    public void SetCastDirection() {
        if (goingLeft) castDirection = new Vector2(-castDistance, 0);
        else castDirection = new Vector2(castDistance, 0);
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

        if (Physics2D.Raycast(castPoint , castDirection, castDistance, 1 << 9)) {
            print("FlippingDirection");
            goingLeft = !goingLeft;
            SetCastDirection();
        }

    }


}
