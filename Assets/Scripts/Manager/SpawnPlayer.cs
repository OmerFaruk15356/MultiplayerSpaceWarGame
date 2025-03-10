using Photon.Pun;
using TMPro;
using UnityEngine;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] UI ui;
    [SerializeField] TextMeshProUGUI alert;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] MeteorManager meteorManager;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float collisionCheckRadius = 0.5f; 
    private bool playerSpawned = false;
    public int shipIndex = -1;

    public void SpawnPlayers()
    {
        if (shipIndex != -1)
        {
            ui.SetPanel(0, false);
            ui.SetPanel(1, true);
            ui.SetPanel(2, true);
            ui.SetPanel(3, false);
            meteorManager.enabled = true;
            while(!playerSpawned)
            {
                Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                if (IsPositionValid(randomPosition))
                {
                    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
                    player.GetComponent<PhotonView>().RPC("_SetShip", RpcTarget.AllBuffered, shipIndex);
                    player.transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
                    playerSpawned = true;
                }
            }
            playerSpawned = false;
        }
        else
        {
            alert.gameObject.SetActive(true);
            alert.text = "CHOOSE A CLASS";
            ui.Invoke("StopAlert", 1f);
        }
    }

    bool IsPositionValid(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, collisionCheckRadius);
        return colliders.Length < 2;
    }

    public void SetShipIndex(int index)
    {
        shipIndex = index;
    }
}