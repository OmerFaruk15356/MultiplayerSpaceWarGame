using System;
using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] SetShip setShip;
    [SerializeField] UI uI;
    public int currentAmmo;
    private Coroutine reloadCoroutine;
    public bool hasAmmo = true;

    void Start()
    {
        uI = FindObjectOfType<UI>();
        currentAmmo = setShip.maxAmmo;
        SetAmmoState();
    }

    private void SetAmmoState()
    {
        uI.ammoText.text = currentAmmo + " / " + setShip.maxAmmo;
    }

    public void UseAmmo()
    {
        currentAmmo -= 1;
        SetAmmoState();
    }

    public void ReloadAmmo()
    {
        uI.ammoText.text = "Reloading...";
        reloadCoroutine = StartCoroutine(StartReload());
    }

    IEnumerator StartReload()
    {
        yield return new WaitForSeconds(setShip.reloadSpeed);
        currentAmmo = setShip.maxAmmo;
        hasAmmo = true;
        SetAmmoState();
    }
}
