using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnNewLevelStarted;

    [SerializeField]
    private List<GameObject> m_levelList = null;

    [SerializeField]
    private GameObject m_playerReference = null;

    [SerializeField]
    private Vector3 m_levelPosition = Vector3.zero;

    [SerializeField]
    private Vector3 m_playerStartPosition = Vector3.zero;

    private GameObject m_currentLevelObjectReference;
    private int m_currentIndexLevel;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Start()
    {
        if (m_levelList == null || m_levelList.Count == 0)
            Debug.LogError("Level list is empty or null", gameObject);
        if (m_playerReference == null)
            Debug.LogError("Plyaer object is not referenced in the game manager!", gameObject);

        InitializeGame();
    }

    private void InitializeGame()
    {
        m_playerReference.transform.position = m_playerStartPosition;
        m_currentIndexLevel = 0;
        m_currentLevelObjectReference = Instantiate(m_levelList[m_currentIndexLevel]);
    }

    private void FinishCurrentLevel()
    {
        StartCoroutine(PreparingNextLevel());
    }

    private void InitializeNextLevel()
    {
        if (m_currentLevelObjectReference != null)
            Destroy(m_currentLevelObjectReference);


        if (m_currentIndexLevel < m_levelList.Count - 1)
            m_currentIndexLevel++;
        else
            m_currentIndexLevel = 0;


        m_currentLevelObjectReference = Instantiate(m_levelList[m_currentIndexLevel]);
        m_playerReference.transform.position = m_playerStartPosition;
        BroadcastOnNewLevelStarted();
    }

    private IEnumerator PreparingNextLevel()
    {
        yield return new WaitForSeconds(4f);
        InitializeNextLevel();
    }

    private void BroadcastOnNewLevelStarted()
    {
        if (OnNewLevelStarted != null)
            OnNewLevelStarted();
    }
}
