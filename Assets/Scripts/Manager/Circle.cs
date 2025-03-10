using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] float circleExitDuration = 3f;
    [SerializeField] UI ui;
    GameObject ship;
    PhotonView shipPhotonView;

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            ship = other.transform.parent.gameObject;
            Health playerHealth = ship.GetComponentInParent<Health>();
            if (playerHealth != null && !playerHealth.isDead) 
            {
                shipPhotonView = ship.GetComponent<PhotonView>();
                if (shipPhotonView != null && shipPhotonView.IsMine)
                {
                    Invoke("DestroyShip", circleExitDuration);
                    ui.SetPanel(4,true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            ui.SetPanel(4,false);
            CancelInvoke("DestroyShip");
        }
    }

    private void DestroyShip()
    {
        if (ship != null)
        {
            ui.SetPanel(3, true);
            ui.SetPanel(1, false);
            ui.SetPanel(2,false);
            ui.SetPanel(4,false);
            PhotonNetwork.Destroy(ship);
        }
    }
}
