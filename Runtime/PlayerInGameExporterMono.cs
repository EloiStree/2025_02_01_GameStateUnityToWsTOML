using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public class ExportListOfPlayerIndexTeamTransform : MonoBehaviour
{
    public PlayerInGameExporterMono m_registerRef;

    [TextArea]
    public string m_debugText;
    string m_playerCSVAsLines;
    byte[] m_playerCSVAsBytes;


    [Range(0, 65535)]
    public int m_udpSizeText;

    [Range(0, 65535)]
    public int m_udpSizeBytes;
    [Range(0, 1)]
    public float m_udpPercentText;
    [Range(0, 1)]
    public float m_udpPercentBytes;

    public void GenerateExport(){

        StringBuilder sb=  new StringBuilder();
        sb.Append("ID:NetID:TEAM:POSX:POSY:POSZ:ROTX:ROTY:ROTZ:SIZE:FLATANGLEXZ\n");
        m_registerRef.GetPlayers(out List<PlayerInGameIndexTeamTransform> players);
        int lineSize = 4*11 ;
        int byteArrSize = players.Count * lineSize;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                continue;
            }

            sb.Append(players[i].m_playerIndex + ":");
            sb.Append(players[i].m_playerLobbyIndex + ":");
            sb.Append(players[i].m_teamIndex + ":");
            sb.Append(players[i].m_positionX_MM + ":");
            sb.Append(players[i].m_positionY_MM + ":");
            sb.Append(players[i].m_positionZ_MM + ":");
            sb.Append(players[i].m_rotationX_Degree + ":");
            sb.Append(players[i].m_rotationY_Degree + ":");
            sb.Append(players[i].m_rotationZ_Degree + ":");
            sb.Append(players[i].m_sizeRadius_MM + ":");
            sb.Append(players[i].m_flatXZAngle_Degree + "\n");
            BitConverter.GetBytes(players[i].m_playerIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 0);
            BitConverter.GetBytes(players[i].m_playerLobbyIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 4);
            BitConverter.GetBytes(players[i].m_teamIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 8);
            BitConverter.GetBytes(players[i].m_positionX_MM).CopyTo(m_playerCSVAsBytes, i * lineSize + 12);
            BitConverter.GetBytes(players[i].m_positionY_MM).CopyTo(m_playerCSVAsBytes, i * lineSize + 16);
            BitConverter.GetBytes(players[i].m_positionZ_MM).CopyTo(m_playerCSVAsBytes, i * lineSize + 20);
            BitConverter.GetBytes(players[i].m_rotationX_Degree).CopyTo(m_playerCSVAsBytes, i * lineSize + 24);
            BitConverter.GetBytes(players[i].m_rotationY_Degree).CopyTo(m_playerCSVAsBytes, i * lineSize + 28);
            BitConverter.GetBytes(players[i].m_rotationZ_Degree).CopyTo(m_playerCSVAsBytes, i * lineSize + 32);
            BitConverter.GetBytes(players[i].m_sizeRadius_MM).CopyTo(m_playerCSVAsBytes, i * lineSize + 36);
            BitConverter.GetBytes(players[i].m_flatXZAngle_Degree).CopyTo(m_playerCSVAsBytes, i * lineSize + 40);
        }

        m_playerCSVAsLines = sb.ToString();

        if (m_playerCSVAsLines.Length<5000)
        {
            m_debugText = m_playerCSVAsLines;
        }
        else
        {
            m_debugText = m_playerCSVAsLines.Substring(0, 5000);
        }
        
        m_udpSizeText = m_playerCSVAsLines.Length;
        m_udpSizeBytes = byteArrSize;
        m_udpPercentText = (float)m_udpSizeText / (float)byteArrSize;
        m_udpPercentBytes = (float)byteArrSize / (float)m_udpSizeText;

    }
}


public class PlayerInGameExporterMono : MonoBehaviour
{
    public ListOfPlayerInGameIndexTeamTransform m_register;

    public void GetPlayers(out List<PlayerInGameIndexTeamTransform> players)
    {
        m_register.GetPlayers(out players);
    }
}


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
        player.m_sizeRadius_MM = (int)(scale * 1000);
    }

    public void SetPositionRotationFromPlayerIndex(int playerIndex, Vector3 position, Vector3 rotation)
    {
        GetPlayerByGivenIndex(playerIndex, out PlayerInGameIndexTeamTransform player);
        player.m_positionX_MM = (int)(position.x * 1000);
        player.m_positionY_MM = (int)(position.y * 1000);
        player.m_positionZ_MM = (int)(position.z * 1000);

        player.m_rotationX_Degree = (int)(rotation.x * 1000);
        player.m_rotationY_Degree = (int)(rotation.y * 1000);
        player.m_rotationZ_Degree = (int)(rotation.z * 1000);

    }

    public void GetPlayers(out List<PlayerInGameIndexTeamTransform> players)
    {
        players = m_player;
    }
}


/// <summary>
/// Represents a abstract layer of a IID player in a game.
/// </summary>

[System.Serializable]
public class PlayerInGameIndexTeamTransform
{
    /// <summary>
    /// Player Given Index in the IID
    /// </summary>
    public int m_playerIndex;
    /// <summary>
    /// Player Index in the lobby list
    /// </summary>
    public int m_playerLobbyIndex;
    /// <summary>
    /// Team index in the IID
    /// </summary>
    public int m_teamIndex;

    [Header("In Millimeters")]   
    public int m_positionX_MM;
    public int m_positionY_MM;
    public int m_positionZ_MM;
    
    [Header("In Degrees * 1000")]
    public int m_rotationX_Degree;
    public int m_rotationY_Degree;
    public int m_rotationZ_Degree;

    [Header("In Millimeters")]
    public int m_sizeRadius_MM;
    public int m_flatXZAngle_Degree;
    
}