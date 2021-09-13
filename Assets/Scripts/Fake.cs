using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake : Advertise
{
      public int advertise(int score){
        int rand = Random.Range(0,2);
        if (rand==0) {
            return score - 1;
        }
        return score;
      }
   
}
