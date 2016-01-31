﻿using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        //find the GameManager
        GameManager gm = GameObject.FindObjectOfType<GameManager>();

        //set the found egg = true
        gm.foundEgg = true;

        //destroy the collectable
        Destroy(this.gameObject);
    }
}
