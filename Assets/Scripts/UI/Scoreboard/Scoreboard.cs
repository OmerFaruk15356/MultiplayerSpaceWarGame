using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] CanvasGroup scoreBoardPanel;
    [SerializeField] GameObject scoreBoardPrefab;
    private bool state = false;

    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }
        SortScoreboardItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            state = !state;
            scoreBoardPanel.alpha = state ? 1 : 0;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
        UpdateScoreForNewPlayer(newPlayer);
    }

    private void AddScoreBoardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreBoardPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;

        if (player.CustomProperties.ContainsKey("Score"))
        {
            int score = (int)player.CustomProperties["Score"];
            item.SetScore(score);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    private void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    public void AddScore(Player player, int amount)
    {
        if (scoreboardItems.TryGetValue(player, out ScoreboardItem item))
        {
            item.AddScore(amount);

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "Score", item.GetScore() }
            };
            player.SetCustomProperties(playerProperties);

            SortScoreboardItems();
        }
    }

    private void UpdateScoreForNewPlayer(Player newPlayer)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Score"))
            {
                int score = (int)player.CustomProperties["Score"];
                scoreboardItems[player].SetScore(score);
            }
        }
    }

    private void SortScoreboardItems()
    {
        var sortedPlayers = PhotonNetwork.PlayerList
            .OrderBy(p => p.CustomProperties.TryGetValue("Score", out object score) ? (int)score : 0)
            .ToList();

        foreach (var player in sortedPlayers)
        {
            if (scoreboardItems.TryGetValue(player, out ScoreboardItem item))
            {
                item.transform.SetAsFirstSibling(); 
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Score"))
        {
            SortScoreboardItems();
        }
    }

    internal int ShowScore(Player player)
    {
        if (scoreboardItems.TryGetValue(player, out ScoreboardItem item))
        {
            return item.GetScore();
        }
        else
        {
            return 0;
        }
    }
}