using Photon.Pun;
using TMPro;
using UnityEngine;

public class UsernameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nickName;

    public void OnUserNameInputChanged()
    {
        PhotonNetwork.NickName = nickName.text;
    }
}