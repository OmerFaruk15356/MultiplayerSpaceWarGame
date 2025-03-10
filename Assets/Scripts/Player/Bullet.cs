using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    private int attackerPhotonViewID;
    private CapsuleCollider2D capsuleCollider;
    public float damage;
    void Start()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    [PunRPC]
    public void SetAttacker(int photonViewID)
    {
        attackerPhotonViewID = photonViewID;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;
        int otherPlayer = other.gameObject.GetComponentInParent<PhotonView>().ViewID;

        if (other.CompareTag("Player") && attackerPhotonViewID != otherPlayer)
        {
            HandlePlayerCollision(other.gameObject);
        }
        else if (other.CompareTag("Meteor"))
        {
            HandleMeteorCollision(other.gameObject);
        }

        if (!other.CompareTag("Background") && attackerPhotonViewID != otherPlayer)
        {
            photonView.RPC("Hit", RpcTarget.AllBuffered);
        }
    }
    private void HandlePlayerCollision(GameObject playerObject)
    {
        capsuleCollider.enabled = false; 
        Health health = playerObject.GetComponentInParent<Health>();
        if (health != null)
        {
            PhotonView attackerPhotonView = PhotonView.Find(attackerPhotonViewID);
            if (attackerPhotonView != null)
            {
                Player attacker = attackerPhotonView.Owner;
                health.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, attacker);
            }
        }
    }

    private void HandleMeteorCollision(GameObject meteorObject)
    {
        Meteor meteor = meteorObject.GetComponent<Meteor>();
        if (meteor != null)
        {
            PhotonView attackerPhotonView = PhotonView.Find(attackerPhotonViewID);
            if (attackerPhotonView != null)
            {
                meteor.photonView.RPC("GetDamage", RpcTarget.AllBuffered, damage, attackerPhotonViewID);
            }
        }        
    }

    [PunRPC]
    public void Hit()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}