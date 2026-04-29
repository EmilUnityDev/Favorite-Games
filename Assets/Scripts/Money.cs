using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.Instance.AddCoin();
            SoundController.Instance.STartEffectMoney();
            Destroy(gameObject);
        }
    }
}
