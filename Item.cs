using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string name = "";
    private int amount = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public String getName()
    {
        return name;
    }

    public void setName(string newName)
    {
        name = newName;
    }

    public int getAmount()
    {
        return amount;
    }

    public void setAmount(int newAmount)
    {
        amount = newAmount;
    }
}
