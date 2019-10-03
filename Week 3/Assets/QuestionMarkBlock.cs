using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkBlock : Block
{

    public GameObject contents;
    public Sprite unTickedSprite;
    public bool hitYet;
    


    public override void HitBlock() {
        
        if (!hitYet) {
            GetComponent<SpriteRenderer>().sprite = unTickedSprite;
            Instantiate(contents, transform.position + Vector3.up, Quaternion.identity);
            hitYet = true;
            Destroy(GetComponent<Animator>());
        }

    }





}
