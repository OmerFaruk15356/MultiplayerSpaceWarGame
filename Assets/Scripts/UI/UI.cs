using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] List<GameObject> panels;
    [SerializeField] List<TextMeshProUGUI> statTexts;
    [SerializeField] TextMeshProUGUI statText;
    [SerializeField] TextMeshProUGUI alert;
    [SerializeField] public TextMeshProUGUI ammoText;
    [SerializeField] public CanvasGroup statsPanel;
    public void SetPanel(int index, bool set)
    {
        panels[index].SetActive(set);
    }

    public void SetStats(int statPoint, PhotonView player)
    {
        statText.text = "Remaining stat points : " + statPoint;
        statTexts[0].text = player.gameObject.GetComponent<SetShip>().maxHp.ToString();
        statTexts[1].text = player.gameObject.GetComponent<SetShip>().bullletdamage.ToString();
        statTexts[2].text = player.gameObject.GetComponent<SetShip>().speed.ToString();
        statTexts[3].text = player.gameObject.GetComponent<SetShip>().fireRate.ToString();
    }
}
