using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JoinGameMenu:PunBehaviour {
    public Button joinGameButton;
    public Image statusPanel;
    public Text statusText;

    public void Start() {
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        // Status
        SetStatus("Connecting...", Color.yellow);
        // Start with join game button disabled
        joinGameButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    // ******************** Network State ********************

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //Status 
        SetStatus("Connected to Lobby", Color.green);

        joinGameButton.interactable = true;
    }

    // ******************** UI Interactions ********************

    public void ClickJoin() {
        // Prevent double click
        joinGameButton.interactable = false;
        // Status
        SetStatus("Joining random room", Color.yellow);
        // Join the game
        //SceneManager.LoadScene("Game");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SetStatus("Joined room successfully", Color.green);

        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        SetStatus("Join room failed, creating new room", Color.red);
        //Couldn't join a room, create one for us instead
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
        SetStatus("Creating room failed, try again", Color.red);
        //Creating room failed, re-enable join button
        joinGameButton.interactable = true;
    }

    private void SetStatus(string status, Color color) {
        statusText.text = status;
        statusPanel.color = color;
    }
}
