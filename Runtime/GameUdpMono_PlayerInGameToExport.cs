using System;
using System.Collections.Generic;
using UnityEngine;


public class GameUdpMono_PlayerInGameToExport : MonoBehaviour
{
    public ListOfPlayerInGameIndexTeamTransform m_register;

    public void GetPlayers(out List<PlayerInGameIndexTeamTransform> players)
    {
        m_register.GetPlayers(out players);
    }



    [ContextMenu("Clear All Players")]
    public void ClearAllPlayers()
    {
        m_register.ClearAllPlayers();
    }

    public void AddIfNotExistingFromPlayerIndex(int playerIndex, out PlayerInGameIndexTeamTransform player)
    {
        m_register.AddIfNotExistingFromPlayerIndex(playerIndex, out player);
    }
    public void GetPlayerByGivenIndex(int playerIndex, out PlayerInGameIndexTeamTransform player)
    {
        m_register.GetPlayerByGivenIndex(playerIndex, out player);
    }

    public void AddIfNotExistingFromLobbyIndex(int lobbyIndex, out PlayerInGameIndexTeamTransform player)
    {
        m_register.AddIfNotExistingFromLobbyIndex(lobbyIndex, out player);
    }
    public void GetPlayerByGivenLobbyIndex(int lobbyIndex, out PlayerInGameIndexTeamTransform player)
    {
        m_register.GetPlayerByGivenLobbyIndex(lobbyIndex, out player);
    }

    public void SetTeamIndexFromPlayerIndex(int playerIndex, ushort teamIndex)
    {
        m_register.SetTeamIndexFromPlayerIndex(playerIndex, teamIndex);
    }
    public void SetLobbyIndexFromPlayerIndex(int playerIndex, int lobbyIndex)
    {
        m_register.SetLobbyIndexFromPlayerIndex(playerIndex, lobbyIndex);
    }
    public void SetPositionRotationFromPlayerIndex(int playerIndex, Transform givenTransform)
    {
        m_register.SetPositionRotationFromPlayerIndex(playerIndex, givenTransform);
    }
    public void SetSizeFlatAngleFromPlayerIndex(int playerIndex, int size)
    {
        m_register.SetScaleFromPlayerIndex(playerIndex, size);
    }

    public void SetSizeFlatAngleFromPlayerIndex(int playerIndex, int size, int flatAngle)
    {
        m_register.SetFlatDegreeAngleXZFromPlayerIndex(playerIndex, flatAngle);
    }

    [ContextMenu("Randomized For Testing")]
    public void RandomizedForTesting()
    {
        m_register.RandomizedForTesting();
    }

    public void GetPlayerByLobbyIndex(int index, out PlayerInGameIndexTeamTransform player)
    {
        m_register.GetPlayerByLobbyIndex(index, out player);
    }
}

