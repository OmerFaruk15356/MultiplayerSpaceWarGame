using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Meteors")]
public class MeteorScriptableObjects : ScriptableObject
{
    [SerializeField] Sprite meteorSprite;
    [SerializeField] float collisionDamage;
    [SerializeField] float hp;
    [SerializeField] int xp;

    public Sprite GetMeteorSprite()
    {
        return this.meteorSprite;
    }
    public float GetCollisionDamage()
    {
        return this.collisionDamage;
    }
    public float GetHp()
    {
        return this.hp;
    }
    public int GetXp()
    {
        return xp;
    }
}
