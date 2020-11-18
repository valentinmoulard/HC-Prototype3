using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    //Gameobject : player reference ; Gameobject : Pickup reference
    public static Action<GameObject, GameObject> OnPickUpCollected;
    public static Action<GameObject> OnPickUpDeposited;

    [SerializeField]
    private int m_maxBagCapacity = 10;

    private List<GameObject> m_collectedPickUpList = null;

    private void Start()
    {
        m_collectedPickUpList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            if (m_collectedPickUpList.Count < m_maxBagCapacity)
            {
                m_collectedPickUpList.Add(other.gameObject);
                BroadcastOnPickUpCollected(other.gameObject);
            }
        }


        if (other.CompareTag("DepositZone"))
        {
            DepositPickUps();
        }

    }

    private void DepositPickUps()
    {
        for (int i = 0; i < m_collectedPickUpList.Count; i++)
        {
            BroadcastOnPickUpDeposited(m_collectedPickUpList[i]);
        }

        m_collectedPickUpList.Clear();
    }

    private void BroadcastOnPickUpDeposited(GameObject pickUpReference)
    {
        if (OnPickUpDeposited != null)
            OnPickUpDeposited(pickUpReference);
    }

    private void BroadcastOnPickUpCollected(GameObject collectedPickUp)
    {
        if (OnPickUpCollected != null)
            OnPickUpCollected(this.gameObject, collectedPickUp);
    }
}
