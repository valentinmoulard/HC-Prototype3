using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_targetToFollow = null;

    [SerializeField]
    private float m_smoothSpeed = 0.0f;

    [SerializeField]
    private Vector3 m_cameraPositionOffset = Vector3.zero;


    private Vector3 m_desiredCameraPositionBuffer;
    private Vector3 m_cameraSmoothedPosition;

    private void Start()
    {
        //check state
    }


    private void FixedUpdate()
    {
        m_desiredCameraPositionBuffer = m_targetToFollow.position + m_cameraPositionOffset;
        m_cameraSmoothedPosition = Vector3.Lerp(transform.position, m_desiredCameraPositionBuffer, m_smoothSpeed);
        transform.position = m_cameraSmoothedPosition;

        transform.LookAt(m_targetToFollow);
    }

}
