using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float MAX_DISTANCE_RAYCAST = 200.0f;

    [SerializeField]
    LayerMask m_layerToDetect = 0;

    private Ray m_rayBuffer;
    private RaycastHit m_hitBuffer;
    private Vector3 m_desiredPosition;

    private float m_playerHeightOffset;

    private void Start()
    {
        m_playerHeightOffset = transform.localScale.y / 2f;
    }

    private void OnEnable()
    {
        Controller.OnHold += MovePlayer;
    }

    private void OnDisable()
    {
        Controller.OnHold -= MovePlayer;
    }

    private void MovePlayer(Vector3 cursorPosition)
    {
        m_rayBuffer = Camera.main.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(m_rayBuffer, out m_hitBuffer, MAX_DISTANCE_RAYCAST, m_layerToDetect))
        {
            m_desiredPosition = m_hitBuffer.point;
            m_desiredPosition.y = m_playerHeightOffset;
            transform.position = m_desiredPosition;
        }
    }
}
