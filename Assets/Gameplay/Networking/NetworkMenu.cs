using Bolt;
using UdpKit;
using UnityEngine;
using System;

namespace SuperShooter
{

    public class NetworkMenu : GlobalEventListener
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));

            if (GUILayout.Button("Single Player", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                BoltLauncher.StartSinglePlayer();
            }

            if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                // START SERVER. Configures this peer as the host of the game.
                // When Bolt is ready, we utilise its callback to create a game room
                // somewhere in Photons cloud for client peers to join.
                BoltLauncher.StartServer();

            }

            if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                // START CLIENT. Configures this peer as a client that can join games.
                // When Bolt is ready, it will automatically connect to Photons servers and
                // pull information about any visible room that has been registered on the cloud.
                BoltLauncher.StartClient();
            }

            GUILayout.EndArea();
        }

        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            {

                string roomName = Guid.NewGuid().ToString();

                // Set up the Photon Cloud Room on Photons servers, expecting
                // new peers connect to this room and join the game.
                BoltNetwork.SetServerInfo(roomName, null);

                // Load the main scene.
                // We only want to load this on the server right now (i.e. running the game).
                BoltNetwork.LoadScene("Main");
            }

            if (BoltNetwork.IsSinglePlayer)
            {

                // Load the main scene.
                BoltNetwork.LoadScene("Main");

            }

        }

        /// <summary>Invoked when new room info arrives internally.</summary>
        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);

            foreach (var session in sessionList)
            {
                UdpSession photonSession = session.Value as UdpSession;

                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    // On connect, Bolt will automatically call LoadScene()
                    // with the scene that is loaded on the server.
                    BoltNetwork.Connect(photonSession);
                }
            }
        }
    } 
}