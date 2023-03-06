using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text StatusText;
    public InputField roomInput, NickNameInput;
    public Button joinButton;

    private void Awake()
    {
        Screen.SetResolution(960, 600, false);
        PhotonNetwork.ConnectUsingSettings();
    }
    
    // Update is called once per frame
    public void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속 완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("no Random room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        print("connected room");
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("Main");
        Info();
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);

    [ContextMenu("info")]
    void Info()
    {
        if(PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
    }
}
