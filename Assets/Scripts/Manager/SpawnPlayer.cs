using Photon.Pun;
using TMPro;
using UnityEngine;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] UI ui;
    [SerializeField] TextMeshProUGUI alert;
    [SerializeField] MeteorManager meteorManager;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float collisionCheckRadius = 1f; 
    private bool playerSpawned = false;
    public int shipIndex = -1;

    public void SpawnPlayers()
    {
        if (shipIndex != -1)
        {
            while(!playerSpawned)
            {
                Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                if (IsPositionValid(randomPosition))
                {
                    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
                    player.GetComponent<PhotonView>().RPC("_SetShip", RpcTarget.AllBuffered, shipIndex);
                    playerSpawned = true;
                }
            }
            playerSpawned = false;
            SetPanels();
        }
        else
        {
            alert.gameObject.SetActive(true);
            alert.text = "CHOOSE A CLASS";
            ui.Invoke("StopAlert", 1f);
        }
    }

    private void SetPanels()
    {
        ui.SetPanel(0, false);
        ui.SetPanel(1, true);
        ui.SetPanel(2, true);
        ui.SetPanel(3, false);
        ui.SetPanel(5, true);
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