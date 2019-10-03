using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerUpType { LIFEUP, MUSHROOM, FIREFLOWER, NONE };

public class PowerUp : MonoBehaviour
{

    public int points;
    public PowerUpType powerUpType;
    
    public virtual PowerUpType Activate() {

        PlayerPlatformerController.AddPoints(points);


        Destroy(gameObject);
        return powerUpType;
    }


}
