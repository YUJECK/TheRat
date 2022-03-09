using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    public int cheeseScore;
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            FindObjectOfType<GameManager>().CheeseScore(cheeseScore);
            Destroy(gameObject);
        }
    }
}
