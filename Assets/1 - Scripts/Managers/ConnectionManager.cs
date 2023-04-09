using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DependencyInjection;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ConnectionManager : IMatchmakingCallbacks, IConnectionCallbacks, IInRoomCallbacks
    {
        private Logger logger;

        public delegate void ErrorEventHandler(string message);
        public delegate void SuccessEventHandler();
        public delegate void LeftEventHandler();

        public event ErrorEventHandler Error;
        public event SuccessEventHandler JoinedRoom;
        public event LeftEventHandler LeftRoom;

        public void InitConnection()
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 9999);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

            logger = DI.Get<Logger>();

            DI.Add(this);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void CreateRoom(string roomName)
        {
            var roomOptions = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 5
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions: roomOptions);
        }

        #region Interface Implementations

        public void OnConnected()
        {
            logger.Log($"Player {PhotonNetwork.LocalPlayer.NickName} is connected");
        }

        public void OnConnectedToMaster()
        {
            logger.Log($"Player {PhotonNetwork.LocalPlayer.NickName} is connected to master");
        }

        public void OnCreatedRoom()
        {
            logger.Log($"Room {PhotonNetwork.CurrentRoom.Name} was created, {PhotonNetwork.LocalPlayer.NickName} is master client");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new System.NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            throw new System.NotImplementedException();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            logger.Log($"Player {PhotonNetwork.LocalPlayer.NickName} Disconnected. Cause: {cause}");
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            throw new System.NotImplementedException();
        }

        public void OnJoinedRoom()
        {
            logger.Log($"Player {PhotonNetwork.LocalPlayer.NickName} joined {PhotonNetwork.CurrentRoom.Name} room");
            logger.Log($"{PhotonNetwork.MasterClient.NickName} is master client");

            JoinedRoom?.Invoke();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            logger.Log($"Failed joining room: code: {returnCode}, message: {message}");

            Error?.Invoke($"Failed joining room: {message}");
        }

        public void OnLeftRoom()
        {
            logger.Log($"Player {PhotonNetwork.LocalPlayer.NickName} left room");

            LeftRoom?.Invoke();
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            logger.Log($"Region List Received");
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            logger.Log($"Player {newPlayer} entered the room");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            logger.Log($"Player {otherPlayer} left the room");
        }

        public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            logger.Log($"Room properties update {propertiesThatChanged}");
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            logger.Log($"Player properties update {targetPlayer.NickName}");
            foreach(var prop in changedProps)
            {
                logger.Log($"Property {prop.Key}, value {prop.Value}");
            }
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            logger.Log($"{newMasterClient.NickName} is master client now");
        }

        #endregion
    }
}

