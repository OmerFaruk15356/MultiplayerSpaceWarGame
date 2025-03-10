using Photon.Pun;
using TMPro;
using UnityEngine;

public class Username : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] TextMeshProUGUI nicknameText;

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void UpdateNickname(string newNickname)
    {
        if(photonView.IsMine)
        nicknameText.gameObject.SetActive(false);

        nicknameText.text = newNickname;
    }
}