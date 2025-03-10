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

    public void PlayerKilled(Player victim, Player attacker)
    {
        if (attacker != null && attacker != victim)
        {
            scoreboard.AddScore(attacker, 10);
        }
    }

    public void MeteorKilled(int attackerViewID,int xp)
    {
        PhotonView attackerPhotonView = PhotonView.Find(attackerViewID);
        if (attackerPhotonView != null)
        {
            scoreboard.AddScore(attackerPhotonView.Owner, 10);
            attackerPhotonView.gameObject.GetComponent<Level>().SetXp(xp);
        }
    }

}