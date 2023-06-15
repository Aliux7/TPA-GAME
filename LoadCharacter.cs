using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter
{
    // Start is called before the first frame update
    private static LoadCharacter instance = null;
    private int charNum;
    public GameObject player;
    private float currHp;

    private LoadCharacter()
    {
        currHp = 100f;
    }

    public void setCurrHealth(float CurrHealthCount)
    {
        currHp = CurrHealthCount;
    }

    public float getCurrHealth()
    {
        return currHp;
    }

    public static LoadCharacter getInstance()
    {
        if (instance == null)
        {
            instance = new LoadCharacter();
        }
        return instance;
    }

    public void setcharNum(int num)
    {
        charNum = num;
    }

    public int getcharNum()
    {
        return charNum;
    }
}
