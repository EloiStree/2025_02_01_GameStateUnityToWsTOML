using System;
using UnityEngine;
/// <summary>
/// Represents a abstract layer of a IID player in a game.
/// Player with int lead to 44 bytes of data per player as bytes and 193 bytes as text.
/// Making 1400  players as bytes and around 320 player max as text.
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
    /// (Could be a IID player index in future so I used int)
    /// </summary>
    public int m_teamIndex;

    [Header("In Millimeters")]   
    public int m_positionMmX;
    public int m_positionMmY;
    public int m_positionMmZ;
    
    [Header("In Degrees * 1000")]
    public int m_rotationDegreeX;
    public int m_rotationDegreeY;
    public int m_rotationDegreeZ;

    [Header("In Millimeters")]
    public int m_sizeMmRadius;
    public int m_flatXZDegreeAngle;

    public void RandomizedForTesting()
    {
        m_playerIndex = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        m_playerLobbyIndex = UnityEngine.Random.Range(0,16500);
        m_teamIndex = UnityEngine.Random.Range(0, 65000);
        m_positionMmX = UnityEngine.Random.Range(0, 100000);
        m_positionMmY = UnityEngine.Random.Range(0, 100000);
        m_positionMmZ = UnityEngine.Random.Range(0, 100000);
        m_rotationDegreeX = UnityEngine.Random.Range(-400000, 400000);
        m_rotationDegreeY = UnityEngine.Random.Range(-400000, 400000);
        m_rotationDegreeZ = UnityEngine.Random.Range(-400000, 400000);
        m_sizeMmRadius = UnityEngine.Random.Range(0, 65000);
        m_flatXZDegreeAngle = UnityEngine.Random.Range(-180000, 180000);
    }
}