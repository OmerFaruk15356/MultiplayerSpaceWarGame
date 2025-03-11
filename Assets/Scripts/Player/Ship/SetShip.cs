using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SetShip : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    [SerializeField] List<ClassScriptableObject> shipScripts;
    public Sprite shipSprite;
    public Sprite bulletSprite;
    public float maxHp;
    public float speed;
    public float bullletdamage;
    public float collisionDamage;
    public float fireRate;
    public float rotationSpeed;
    public float acceleration;
    public float deceleration;
    public float reloadSpeed;
    public int maxAmmo;
    private int shipIndex;

    [PunRPC]
    public void _SetShip(int index)
    {
        shipIndex = index;
        SetShipAttributes(shipIndex);
    }

    public void SetShipAttributes(int index)
    {
        shipSprite = shipScripts[index].GetShipSprite();
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = shipSprite;
        bulletSprite = shipScripts[index].GetBulletSprite();
        maxHp = shipScripts[index].GetMaxHp();
        speed = shipScripts[index].GetSpeed();
        bullletdamage = shipScripts[index].GetDamage();
        collisionDamage = shipScripts[index].GetCollisionDamage();
        fireRate = shipScripts[index].GetFireRate();
        rotationSpeed = shipScripts[index].GetRotateSpeed();
        acceleration = shipScripts[index].GetAcceleration();
        deceleration = shipScripts[index].GetDeceleration();
        maxAmmo = shipScripts[index].GetAmmo();
        reloadSpeed = shipScripts[index].GetReloadSpeed();
        transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
    }

    [PunRPC]
    public void SetLevelUp(int index) 
    {
        if (index == 0) 
        {
            maxHp *= 2f;  
        } 
        else if (index == 1) 
        {
            bullletdamage *= 2f;  
        } 
        else if (index == 2) 
        {
            speed *= 2f;  
        } 
        else if (index == 3) 
        {
            fireRate *= 2f;  
        }
    }

}