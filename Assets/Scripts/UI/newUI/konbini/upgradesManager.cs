using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradesManager : MonoBehaviour
{
    public Text followersCurr;
    public Text oniCurr;
    public Text tenguCurr;
    public Text kappaCurr;

    void Update()
    {
        oniCurr.text = currency.oni.ToString();
        tenguCurr.text = currency.tengu.ToString();
        kappaCurr.text = currency.kappa.ToString();
        followersCurr.text = currency.Followers.ToString();
    }
}
