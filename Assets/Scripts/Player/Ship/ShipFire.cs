using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShipFire : MonoBehaviourPun
{
    [SerializeField] Slider fireRateSlider;
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform bulletContainer;
    [SerializeField] float ammoSpeed = 10;
    [SerializeField] float ammoDuration = 2;
    [SerializeField] Ammo ammo;
    [SerializeField] SetShip setShip;
    public bool isFiring;
    private Coroutine fireCoroutine;
    private float lastFireTime = 0f;
    private float fireCooldown;

    void Start()
    {
        var BulletObject = GameObject.Find("BulletContainer");
        bulletContainer = BulletObject.transform;

        fireCooldown = 1 / setShip.fireRate; 
        fireRateSlider.value = 0; 
    }

    void LateUpdate()
    {
        if (photonView.IsMine)
        {
            float elapsedTime = Time.time - lastFireTime;
            if (Input.GetKeyDown(KeyCode.R) && ammo.currentAmmo < setShip.maxAmmo && !isFiring)
            {
                ammo.hasAmmo = false;
                lastFireTime = Time.time;
                ammo.ReloadAmmo();
                SetSlider(0,setShip.reloadSpeed);
            }
            else if (ammo.hasAmmo)
            {
                Fire();
                SetSlider(elapsedTime,fireCooldown);
            }
            else
            {
                SetSlider(elapsedTime,setShip.reloadSpeed);
            }

        }
    }
    void SetSlider(float min,float max)
    {
        fireRateSlider.maxValue = max; 
        fireRateSlider.value = Mathf.Clamp(min, 0, max);
    }
    void Fire()
    {
        if (isFiring && fireCoroutine == null) 
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && fireCoroutine != null) 
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (isFiring && ammo.hasAmmo) 
        {
            float timeSinceLastFire = Time.time - lastFireTime;
            if (timeSinceLastFire >= fireCooldown) 
            {
                FireBullet();
                ammo.UseAmmo(); 
                lastFireTime = Time.time;
                fireRateSlider.value = 0; 
            }
            yield return null;
        }

        fireCoroutine = null;
    }

    private void FireBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(ammoPrefab.name, transform.position + transform.up / 1.25f, Quaternion.identity);
        bullet.transform.SetParent(bulletContainer);
        bullet.GetComponentInChildren<SpriteRenderer>().sprite = setShip.bulletSprite;
        bullet.GetComponentInChildren<Bullet>().damage = setShip.bullletdamage;
        PhotonView bulletPhotonView = bullet.GetComponent<PhotonView>();

        photonView.RPC("SetBulletRotation", RpcTarget.AllBuffered, bulletPhotonView.ViewID, transform.rotation);
        bullet.GetComponent<Bullet>().photonView.RPC("SetAttacker", RpcTarget.AllBuffered, photonView.ViewID);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * ammoSpeed;
        }

        StartCoroutine(DestroyBulletAfterDuration(bullet, ammoDuration));
    }

    [PunRPC]
    private void SetBulletRotation(int bulletViewID, Quaternion rotation)
    {
        PhotonView bulletPhotonView = PhotonView.Find(bulletViewID);
        if (bulletPhotonView != null)
        {
            bulletPhotonView.transform.rotation = rotation;
        }
    }

    IEnumerator DestroyBulletAfterDuration(GameObject bullet, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (bullet != null && bullet.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(bullet);
        }
    }
}
