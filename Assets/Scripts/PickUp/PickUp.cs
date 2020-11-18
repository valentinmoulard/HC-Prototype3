using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private float m_pickUpSpeed = 20f;

    [SerializeField]
    private float m_playerDistanceOffset = 20f;

    [SerializeField]
    private float m_imageBlockDistanceOffset = 0.02f;

    private GameObject m_playerReference;
    private Vector3 m_imageBlockOriginalPosition;
    private bool m_isDeposited;
    private bool m_isFollowingPlayer;

    private void OnEnable()
    {
        PlayerBag.OnPickUpCollected += ActivateMovement;
        PlayerBag.OnPickUpDeposited += GetDeposited;
        ImageBuilder.OnBlockInitialize += Initialize;
    }

    private void OnDisable()
    {
        PlayerBag.OnPickUpCollected -= ActivateMovement;
        PlayerBag.OnPickUpDeposited -= GetDeposited;
        ImageBuilder.OnBlockInitialize -= Initialize;
    }

    private void Initialize(GameObject pickUpReference)
    {
        if (pickUpReference == this.gameObject)
        {
            m_isFollowingPlayer = false;
            m_isDeposited = false;
            m_imageBlockOriginalPosition = transform.position;
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (m_isFollowingPlayer && !m_isDeposited && m_playerReference != null && Vector3.Distance(transform.position, m_playerReference.transform.position) > m_playerDistanceOffset)
            transform.position = Vector3.MoveTowards(transform.position, m_playerReference.transform.position, m_pickUpSpeed * Time.deltaTime);

        if (m_isDeposited && Vector3.Distance(transform.position, m_imageBlockOriginalPosition) > m_imageBlockDistanceOffset)
            transform.position = Vector3.MoveTowards(transform.position, m_imageBlockOriginalPosition, m_pickUpSpeed * Time.deltaTime);
    }

    private void ActivateMovement(GameObject playerReference, GameObject pickUpReference)
    {
        if (pickUpReference == this.gameObject)
        {
            m_playerReference = playerReference;
            m_isFollowingPlayer = true;
        }
    }

    private void GetDeposited(GameObject pickUpReference)
    {
        if (pickUpReference == this.gameObject)
        {
            m_isFollowingPlayer = false;
            m_isDeposited = true;
        }
    }
}
