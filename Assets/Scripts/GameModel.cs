using System.Collections;
using System.Collections.Generic;
using System;


public class GameModel
{
    public float MaxDistance=5;
    private bool canContinue;
    private float timer;
    private int score;
    private int level_player;
    private float RelativeDistance;
    private bool isPlay = false;


    public float get_MaxDistance()
    {
        return MaxDistance;
    }

    public void set_MaxDistance(float n)
    {
        MaxDistance = n;
    }

    public bool get_canContinue()
    {
        return canContinue;
    }

    public void set_canContinue(bool n)
    {
        canContinue = n;
    }



    public float get_timer()
    {
        return timer;
    }

    public void set_timer(float n)
    {
        timer = n;
    }

    public int get_score()
    {
        return score;
    }

    public void set_score(int n)
    {
        score = n;
    }


    public int get_level_player()
    {
        return level_player;
    }

    public void set_level_player(int n)
    {
        level_player = n;
    }



    public float get_RelativeDistance()
    {
        return RelativeDistance;
    }

    public void set_RelativeDistance(float n)
    {
        RelativeDistance = n;
    }

    public bool get_isPlay()
    {
        return isPlay;
    }

    public void set_isPlay(bool n)
    {
        isPlay = n;
    }

    public bool checkWin(float x,float y, int lvl)
    {
        if(x>100 && x<110 && y > 150 && y < 160 && lvl == 2)
        {
            return true;

        }
        else
        {
            return false;
        }

    }

    public bool checklvl(float x, int lvl)
    {
        if (x > 260 && x < 270  && lvl == 1)
        {
            return true;

        }
        else
        {
            return false;
        }

    }
   

}
