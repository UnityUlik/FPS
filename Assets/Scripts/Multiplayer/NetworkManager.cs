using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private byte maxPlayers = 20;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private GameObject _roomListItemPrefab;
    [SerializeField] private GameObject _playerListItemPrefab;
    [SerializeField] private Transform _roomListContent;
    [SerializeField] private Transform _playerListContent;
    [SerializeField] private GameObject _startGameButton;

    public Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings();
    }

    #region Pun
    public override void OnConnectedToMaster()
    {
        Debug.Log("We connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("We joined lobby");
        PhotonNetwork.NickName = $"Player_" + Guid.NewGuid();
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform childObject in _playerListContent)
        {
            Destroy(childObject.gameObject);
        }

        foreach (var player in players)
        {
            Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = $"Room Creation Failed:\n{message}";
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);

        foreach (Transform _trans in _roomListContent)
        {
            Destroy(_trans.gameObject);
        }

        foreach (var roomInfo in _cachedRoomList)
        {
            if (_cachedRoomList[roomInfo.Key].RemovedFromList) continue;
            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomInfo.Value);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnLeftLobby()
    {
        _cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _cachedRoomList.Clear();
    }
    #endregion

    #region PublicMethods

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = maxPlayers;
        
        if (string.IsNullOrEmpty(_roomNameInputField.text)) return;
        PhotonNetwork.CreateRoom(_roomNameInputField.text, roomOptions, TypedLobby.Default);

        MenuManager.Instance.OpenMenu("loading");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            if (info.RemovedFromList)
            {
                _cachedRoomList.Remove(info.Name);
            }
            else
            {
                _cachedRoomList[info.Name] = info;
                Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(info);
            }
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    #endregion
}
