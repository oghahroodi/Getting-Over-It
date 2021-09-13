using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    //public IPurchaseManager purchaseManager;
    //public IAdvertisementPlayer advertisemetPlayer;
    public Advertise advertise;
    public Pay pay;
    private static ServiceLocator instance;
    public static ServiceLocator Instance;

    private void Awake()
    {
        instance = this;
        
        if (Instance == null)
            Instance = instance;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SetupInterfaces();
    }

    private void SetupInterfaces()
    {
        advertise = new Fake();
        pay = new FakePay();
        //advertisementPlayer = new FakeAdvertisementPlayer();
        //purchaseManager = new FakePurchaseManaer();
    }
}
