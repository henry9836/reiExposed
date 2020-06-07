using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mythHPbar : MonoBehaviour
{
    public EnemyController EC;
    public float maxhealth;

    void Start()
    {
        EC = this.transform.root.GetComponent<EnemyController>();
        maxhealth = EC.health;
    }

    void Update()
    {
        this.GetComponent<Image>().fillAmount = EC.health / maxhealth;
    }
}
