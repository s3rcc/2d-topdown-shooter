using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    public delegate void ExpChangeHandler(int amount);
    public event ExpChangeHandler OnExpChange;

    private void Awake()
    {
        if( Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    public void AddExp(int amount)
    {
        OnExpChange?.Invoke(amount);
    }
}
