using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameLogic : MonoBehaviourPunCallbacks
{
    [SerializeField] private Scoreboard scoreboard;
    public static GameLogic Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerKilled(Player attacker,int score)
    {
        if (attacker != null)
        {
            scoreboard.AddScore(attacker, score);
        }
    }

    public void MeteorKilled(int attackerViewID,int xp,int score)
    {
        PhotonView attackerPhotonView = PhotonView.Find(attackerViewID);
        if (attackerPhotonView != null)
        {
            scoreboard.AddScore(attackerPhotonView.Owner, score);
            attackerPhotonView.gameObject.GetComponent<Level>().SetXp(xp);
        }
    }

}