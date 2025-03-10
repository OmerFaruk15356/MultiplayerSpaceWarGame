using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPun
{
    [SerializeField] Slider healthBar;
    private UI ui;
    public bool isDead = false;
    private float maxHealth;
    private float currentHealth;
    private SetShip setShip = null;

    private void Start()
    {
        ui = FindObjectOfType<UI>();
        if (ui == null)
        {
            Debug.LogError("UI component not found!");
        }

        setShip = gameObject.GetComponent<SetShip>();
        if (setShip == null)
        {
            Debug.LogError("SetShip component not found!");
        }
        else
        {
            maxHealth = setShip.maxHp;
            currentHealth = maxHealth;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Meteor"))
        {
            float damage = other.gameObject.GetComponent<Meteor>().collisionDamage;
            HandleMeteorCollision(other.gameObject, damage);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            float damage = other.gameObject.GetComponent<SetShip>().collisionDamage;
            HandlePlayerCollision(other.gameObject, damage);
        }
    }

    private void HandleMeteorCollision(GameObject meteorObject, float damage)
    {
        Meteor meteor = meteorObject.GetComponent<Meteor>();
        if (meteor != null)
        {
            gameObject.GetComponent<ShipControl>().currentSpeed = 0;
            photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, null);
            meteor.photonView.RPC("GetDamage", RpcTarget.AllBuffered, damage, photonView.ViewID);
        }
    }

    private void HandlePlayerCollision(GameObject playerObject, float damage)
    {
        Health playerHp = playerObject.GetComponent<Health>();
        if (playerHp != null)
        {
            playerHp.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, playerHp.photonView.Owner);
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, Player attacker)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (photonView.IsMine)
        {
            ui.SetPanel(3, true);
            ui.SetPanel(1, false);
            ui.SetPanel(2, false);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}