using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetChecker : MonoBehaviour
{

    public static InternetChecker Instance;
    private const bool allowCarrierDataNetwork = false;
    private const string pingAddress = "8.8.8.8"; // Google Public DNS server
    private const float waitingTime = 5.0f;
    public bool internetConnectBool;
    public string Ping;
    private Ping ping;
    private float pingStartTime;
    void Awake()
    {
        Instance = this;

    }
    public IEnumerator Start()
    {
        bool internetPossiblyAvailable;
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                internetPossiblyAvailable = true;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                //internetPossiblyAvailable = allowCarrierDataNetwork;
                internetPossiblyAvailable = true;
                break;
            default:
                internetPossiblyAvailable = false;
                break;
        }
        if (!internetPossiblyAvailable)
        {
            InternetIsNotAvailable();
        }
        if (!internetConnectBool)
        {
            ping = new Ping(pingAddress);
            pingStartTime = Time.time;
        }
        print("InternetCheck");
        yield return new WaitForSeconds(3f);
        StartCoroutine(Start());
    }

    public void Update()
    {
        if (ping != null)
        {
            bool stopCheck = true;
            if (ping.isDone)
                InternetAvailable();
            else if (Time.time - pingStartTime < waitingTime)
                stopCheck = false;
            else
                InternetIsNotAvailable();
            if (stopCheck)
                ping = null;
        }
    }

    public void InternetIsNotAvailable()
    {
        //Debug.Log("No Internet");

        internetConnectBool = false;
    }

    public void InternetAvailable()
    {
        //Debug.Log("Internet is available;)");

        internetConnectBool = true;
        Ping = ping.time.ToString();

    }
}
