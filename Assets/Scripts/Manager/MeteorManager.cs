using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MeteorManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject meteorPrefab;
    [SerializeField] private int initialMeteorCount = 10;
    [SerializeField] private int maxMeteorCount = 20; 
    [SerializeField] private float spawnInterval = 5.0f; 
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float collisionCheckRadius = 1f; 
    private List<Vector2> meteorPositions = new List<Vector2>();
    [SerializeField] List<MeteorScriptableObjects> meteors;
    private Coroutine spawnCoroutine;
    public int index;

    void Start()
    {
        // Only the Master Client should spawn meteors
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < initialMeteorCount; i++)
            {
                SpawnMeteor();
            }

            spawnCoroutine = StartCoroutine(SpawnMeteorPeriodically());
        }
    }

    // When the Master Client changes, reassign meteor management
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
            spawnCoroutine = StartCoroutine(SpawnMeteorPeriodically());
        }
    }

    void SpawnMeteor()
    {
        Vector2 randomPosition;

        for (int attempt = 0; attempt < 100; attempt++)
        {
            randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            if (IsPositionValid(randomPosition))
            {
                GameObject meteor = PhotonNetwork.Instantiate(meteorPrefab.name, randomPosition, Quaternion.identity);
                meteor.transform.SetParent(transform);
                meteorPositions.Add(randomPosition);
                photonView.RPC("SyncMeteorPositions", RpcTarget.Others, randomPosition, true);
                index = Random.Range(0,meteors.Count);
                meteor.GetComponent<PhotonView>().RPC("SetMeteor",RpcTarget.AllBuffered,index);

                if (meteorPositions.Count >= maxMeteorCount && spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }
                break;
            }
        }
    }

    bool IsPositionValid(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, collisionCheckRadius);
        return colliders.Length < 2;
    }

    IEnumerator SpawnMeteorPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnMeteor();
        }
    }

    public void OnMeteorDestroyed(Vector2 position)
    {
    if (!PhotonNetwork.IsMasterClient) return;

        if (meteorPositions.Contains(position))
        {
            meteorPositions.Remove(position);
            photonView.RPC("SyncMeteorPositions", RpcTarget.Others, position, false);

            if (meteorPositions.Count < maxMeteorCount && spawnCoroutine == null && gameObject.activeSelf == true)
            {
                spawnCoroutine = StartCoroutine(SpawnMeteorPeriodically());
            }
        }
    }

    [PunRPC]
    void SyncMeteorPositions(Vector2 position, bool isAdded)
    {
        if (isAdded)
        {
            meteorPositions.Add(position);
        }
        else
        {
            meteorPositions.Remove(position);
        }
    }
}
