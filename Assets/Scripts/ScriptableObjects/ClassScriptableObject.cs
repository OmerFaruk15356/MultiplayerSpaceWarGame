using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/PlayerShipClasses")]
public class ClassScriptableObject : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] Sprite shipSprite;
    [SerializeField] Sprite bulletSprite;
    [SerializeField] float bulletDamage;
    [SerializeField] float collisionDamage;
    [SerializeField] float fireRate;
    [SerializeField] float maxHp;
    [SerializeField] float rotateSpeed;
    [SerializeField] float shipSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float reloadSpeed;
    [SerializeField] int ammo;

    public Sprite GetShipSprite()
    {
        return this.shipSprite;
    }
    public Sprite GetBulletSprite()
    {
        return this.bulletSprite;
    }
    public float GetDamage()
    {
        return this.bulletDamage;
    }
    public float GetCollisionDamage()
    {
        return this.collisionDamage;
    }
    public float GetFireRate()
    {
        return this.fireRate;
    }
    public float GetMaxHp()
    {
        return this.maxHp;
    }
    public float GetRotateSpeed()
    {
        return this.rotateSpeed;
    }
    public float GetSpeed()
    {
        return this.shipSpeed;
    }
    public float GetAcceleration()
    {
        return this.acceleration;
    }
    public float GetDeceleration()
    {
        return this.deceleration;
    }
    public float GetReloadSpeed()
    {
        return this.reloadSpeed;
    }
    public int GetAmmo()
    {
        return this.ammo;
    }

}
