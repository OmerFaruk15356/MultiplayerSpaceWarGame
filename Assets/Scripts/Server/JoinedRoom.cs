using Photon.Pun;
using TMPro;
using UnityEngine;

public class JoinedRoom : MonoBehaviourPunCallbacks
{
  [SerializeField] private TMP_InputField createInput;
  [SerializeField] private TMP_InputField joinInput;

  public void CreateRoom() => PhotonNetwork.CreateRoom(this.createInput.text);

  public void JoinRoom()
  {
    if (!string.IsNullOrEmpty(joinInput.text))
    {
      PhotonNetwork.JoinRoom(joinInput.text);
    }
    else
    {
      Debug.LogWarning("Room name is required to join.");     
    }
  }

  public override void OnJoinedRoom() => PhotonNetwork.LoadLevel("Game");
}