using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Meteor : MonoBehaviourPun
{
    [SerializeField] List<MeteorScriptableObjects> meteors;
    [SerializeField] SpriteRenderer meteorSprite;
    private float meteorHp;
    private int xp;
    public float collisionDamage;

    private MeteorManager meteorManager;

    void Start()
    {
        meteorManager = FindObjectOfType<MeteorManager>();
    }

    [PunRPC]
    public void SetMeteor(int index)
    {
        meteorSprite.sprite = meteors[index].GetMeteorSprite();
        meteorHp = meteors[index].GetHp();
        collisionDamage = meteors[index].GetCollisionDamage();
        xp = meteors[index].GetXp();
        gameObject.AddComponent<PolygonCollider2D>();
        if(index == 1)
        gameObject.transform.localScale *= 0.75f;
    }

    [PunRPC]
    public void GetDamage(float damage, int attackerViewID)
    {
        meteorHp -= damage;
        if (meteorHp <= 0)
        {
            if (GameLogic.Instance != null)
                GameLogic.Instance.MeteorKilled(attackerViewID, xp);

            if (photonView.IsMine) 
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        if (meteorManager != null)
        {
            meteorManager.OnMeteorDestroyed(transform.position);
        }
    }
}