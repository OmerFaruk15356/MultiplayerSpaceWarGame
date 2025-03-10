using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ShipFire : MonoBehaviourPun
{
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] float ammoSpeed = 10;
    [SerializeField] float ammoDuration = 2;
    [SerializeField] Transform bulletContainer;
    
    [SerializeField] SetShip setShip;
    public bool isFiring;
    private Coroutine fireCoroutine;
    private float lastFireTime = 0f;

    void Start()
    {
        var BulletObject = GameObject.Find("BulletContainer");
        bulletContainer = BulletObject.transform;
    }
    void LateUpdate()
    {
        if (photonView.IsMine)
        {
            Fire();
        }
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
        while (isFiring)
        {
            float timeSinceLastFire = Time.time - lastFireTime;
            if (timeSinceLastFire >= 1 / setShip.fireRate)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
            yield return null;
        }
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