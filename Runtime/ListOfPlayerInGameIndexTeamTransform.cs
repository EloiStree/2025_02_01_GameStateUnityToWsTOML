using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListOfPlayerInGameIndexTeamTransform{

    public List<PlayerInGameIndexTeamTransform> m_player = new List<PlayerInGameIndexTeamTransform>();
    public Dictionary<int, PlayerInGameIndexTeamTransform> m_playerDict = new Dictionary<int, PlayerInGameIndexTeamTransform>();

    public void ClearAllPlayers()
    {
        m_player.Clear();
        m_playerDict.Clear();
    }

    public void AddIfNotExistingFromPlayerIndex(int playerIndex, out PlayerInGameIndexTeamTransform player){

        for (int i = 0; i < m_player.Count; i++)
        {
            if (m_player[i].m_playerIndex == playerIndex)
            {
                player = m_player[i];
                return;
            }
        }
        PlayerInGameIndexTeamTransform newPlayer = new PlayerInGameIndexTeamTransform();
        newPlayer.m_playerIndex = playerIndex;
        newPlayer.m_playerLobbyIndex = -1;
        m_player.Add(newPlayer);
        player = newPlayer;
        m_playerDict.Add(playerIndex, newPlayer);
    }

    public void GetPlayerByGivenIndex(int playerIndex, out PlayerInGameIndexTeamTransform player)
    {
        for (int i = 0; i < m_player.Count; i++)
        {
            if (m_player[i].m_playerIndex == playerIndex)
            {
                player = m_player[i];
                return;
            }
        }
        player = null;
    }

    public void SetTeamIndexFromPlayerIndex(int playerIndex, ushort teamIndex)
    {
        GetPlayerByGivenIndex(playerIndex, out PlayerInGameIndexTeamTransform player);
        player.m_teamIndex = teamIndex;
    }
    public void SetLobbyIndexFromPlayerIndex(int playerIndex, int lobbyIndex)
    {
        GetPlayerByGivenIndex(playerIndex, out PlayerInGameIndexTeamTransform player);
        player.m_playerLobbyIndex = lobbyIndex;
    }

    public void SetPositionRotationFromPlayerIndex(int playerIndex, Transform givenTransform){

        SetPositionRotationFromPlayerIndex(playerIndex, givenTransform.position, givenTransform.eulerAngles);
    }

    public void SetScaleFromPlayerIndex(int playerIndex, float scale)
    {
        GetPlayerByGivenIndex(playerIndex, out PlayerInGameIndexTeamTransform player);
        player.m_sizeMmRadius = (int)(scale * 1000);
    }

    public void SetPositionRotationFromPlayerIndex(int playerIndex, Vector3 position, Vector3 rotation)
    {
        GetPlayerByGivenIndex(playerIndex, out PlayerInGameIndexTeamTransform player);
        player.m_positionMmX = (int)(position.x * 1000);
        player.m_positionMmY = (int)(position.y * 1000);
        player.m_positionMmZ = (int)(position.z * 1000);

        player.m_rotationDegreeX = (int)(rotation.x * 1000);
        player.m_rotationDegreeY = (int)(rotation.y * 1000);
        player.m_rotationDegreeZ = (int)(rotation.z * 1000);

    }

    public void GetPlayers(out List<PlayerInGameIndexTeamTransform> players)
    {
        players = m_player;
    }

    public void RandomizedForTesting()
    {
        for (int i = 0; i < m_player.Count; i++)
        {
            m_player[i].RandomizedForTesting();
            m_player[i].m_playerLobbyIndex=i;
        }
    }
}
