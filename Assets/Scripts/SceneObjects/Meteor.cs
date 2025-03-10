using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Meteor : MonoBehaviourPun
{
    [SerializeField] List<MeteorScriptableObjects> meteors;
    [SerializeField] SpriteRenderer meteorSprite;
    private float meteorHp;
    private int xp;
    private int score;
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
        score = meteors[index].GetScore();
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
                GameLogic.Instance.MeteorKilled(attackerViewID, xp,score);

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