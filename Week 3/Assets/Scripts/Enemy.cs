using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int pointValue = 100;

    public virtual void Die() {
        PlayerPlatformerController.AddPoints(pointValue);
        Destroy(gameObject);
    }



}
