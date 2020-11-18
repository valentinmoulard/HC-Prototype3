using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBuilder : MonoBehaviour
{
    public static Action<GameObject> OnBlockInitialize;

    [Header("Block Spawning Area Parameters")]
    [SerializeField]
    private float m_minX = -12f;
    [SerializeField]
    private float m_maxX = 12f;
    [SerializeField]
    private float m_minZ = 0f;
    [SerializeField]
    private float m_maxZ = 30f;

    [Header("Spawing Parameters")]
    [SerializeField]
    private float m_spawnRate = 3f;

    [SerializeField]
    private ParticleSystem m_victoryParticleSystem = null;

    [Header("BlockReferences")]
    [SerializeField]
    private List<GameObject> m_blockList = null;

    private GameObject m_spawnedBlockBuffer;
    private Coroutine m_spawnBlocksCoroutine;
    private Vector3 m_randomSpawnPositionBuffer;
    private float m_xPositionBuffer;
    private float m_zPositionBuffer;
    private int m_randomValueBuffer;
    private int m_levelBlocksCount;
    private int m_blockDepositCount;


    private void OnEnable()
    {
        PlayerBag.OnPickUpDeposited += UpdateBlockDepositCount;
    }

    private void OnDisable()
    {
        PlayerBag.OnPickUpDeposited -= UpdateBlockDepositCount;
    }

    void Start()
    {
        if (m_blockList.Count == 0 || m_blockList == null)
            Debug.LogError("The list is null or empty!", gameObject);

        if (m_victoryParticleSystem == null)
            Debug.LogError("The particle system reference is missing!", gameObject);

        InitializeImageBlocks();

        m_spawnBlocksCoroutine = StartCoroutine(SpawnBlocks());
    }

    private void InitializeImageBlocks()
    {
        for (int i = 0; i < m_blockList.Count; i++)
        {
            BroadcastOnBlockInitialize(m_blockList[i]);
        }
    }

    private IEnumerator SpawnBlocks()
    {
        m_levelBlocksCount = m_blockList.Count;
        m_blockDepositCount = 0;


        while (m_blockList.Count > 0)
        {
            m_randomValueBuffer = UnityEngine.Random.Range(0, m_blockList.Count - 1);

            m_spawnedBlockBuffer = m_blockList[m_randomValueBuffer];
            m_spawnedBlockBuffer.SetActive(true);
            m_spawnedBlockBuffer.transform.position = GetRandomSpawnPosition();

            m_blockList.Remove(m_spawnedBlockBuffer);

            yield return new WaitForSeconds(1f / m_spawnRate);
        }


        //need to change this in detecting when all the blocks are collected and deposited.
        yield return new WaitForSeconds(3f);
        TriggerVictory();
    }

    /// <summary>
    /// WIP function
    /// </summary>
    /// <param name="pickupReference"></param>
    private void UpdateBlockDepositCount(GameObject pickupReference)
    {
        m_blockDepositCount++;
        /*
        if (m_blockDepositCount >= m_levelBlocksCount)
            TriggerVictory();
            */
    }

    private void TriggerVictory()
    {
        m_victoryParticleSystem.Play();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        m_xPositionBuffer = UnityEngine.Random.Range(m_minX, m_maxX);
        m_zPositionBuffer = UnityEngine.Random.Range(m_minZ, m_maxZ);

        m_randomSpawnPositionBuffer.x = m_xPositionBuffer;
        m_randomSpawnPositionBuffer.y = 0.0f;
        m_randomSpawnPositionBuffer.z = m_zPositionBuffer;

        return m_randomSpawnPositionBuffer;
    }

    private void BroadcastOnBlockInitialize(GameObject blockReference)
    {
        if (OnBlockInitialize != null)
            OnBlockInitialize(blockReference);
    }
}
