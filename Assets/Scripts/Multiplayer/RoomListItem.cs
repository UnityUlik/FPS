using Photon.Realtime;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private RoomInfo _info;

    public void SetUp(RoomInfo roomInfo)
    {
        _info = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void OnClick()
    {
        NetworkManager.Instance.JoinRoom(_info);
    }
}
