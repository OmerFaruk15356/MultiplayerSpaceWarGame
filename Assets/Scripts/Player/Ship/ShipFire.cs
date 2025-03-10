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
            if (ammo.currentAmmo > 0)
            {
                float elapsedTime = Time.time - lastFireTime;
                fireRateSlider.maxValue = fireCooldown; 
                fireRateSlider.value = Mathf.Clamp(elapsedTime, 0, fireCooldown);
                Fire();
            }
            else
            {
                float elapsedTime = Time.time - lastFireTime;
                fireRateSlider.maxValue = setShip.reloadSpeed; 
                fireRateSlider.value = Mathf.Clamp(elapsedTime, 0, setShip.reloadSpeed);
                ammo.ReloadAmmo();
            }

            if (Input.GetKeyDown(KeyCode.R) && ammo.currentAmmo < setShip.maxAmmo)
            {
                ammo.ReloadAmmo();
            }
        }
    }

    void Fire()
    {
        if (isFiring && fireCoroutine == null && ammo.currentAmmo > 0) 
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if ((!isFiring || ammo.currentAmmo <= 0) && fireCoroutine != null) 
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (isFiring && ammo.currentAmmo > 0) 
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
