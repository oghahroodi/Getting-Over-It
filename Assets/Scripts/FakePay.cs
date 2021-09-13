using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePay : Pay
{
      public int pay(int score){
        int rand = Random.Range(0,2);
        if (rand==0) {
            return score + 1;
        }
        return score;
      }
   
}