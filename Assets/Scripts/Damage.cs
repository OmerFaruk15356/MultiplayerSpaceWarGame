using Photon.Pun;
using UnityEngine;

public class Damage : MonoBehaviourPun
{
    [SerializeField] float damage = 0;

    public float GetDamage()
    {
        return damage;
    }
}