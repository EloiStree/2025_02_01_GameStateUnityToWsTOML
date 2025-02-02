using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class GameUdpMono_ExportListOfPlayerIndexTeamTransform : MonoBehaviour
{
    public GameUdpMono_PlayerInGameToExport m_registerRef;

    public UnityEvent<string> m_onTextUdpPlayerPositionChanged;
    public UnityEvent<byte[]> m_onTextUtf8ByteUdpPlayerPositionChanged;
    public UnityEvent<byte[]> m_onBytesUdpPlayerPositionChanged;

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

    [Range(0, 10)]
    public float m_byteToTextRatio;

    [ContextMenu("Generate And Export")]
    public void GeneratAndExport(){

        StringBuilder sb=  new StringBuilder();
        sb.Append("ID:NetID:TEAM:POSX:POSY:POSZ:ROTX:ROTY:ROTZ:SIZE:FLATANGLEXZ\n");
        m_registerRef.GetPlayers(out List<PlayerInGameIndexTeamTransform> players);
        int lineSize = 4*11 ;
        int byteArraySize = players.Count * lineSize;

        if(m_playerCSVAsBytes.Length!=byteArraySize)
            m_playerCSVAsBytes = new byte[byteArraySize];
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                continue;
            }

            sb.Append(players[i].m_playerIndex + ":");
            sb.Append(players[i].m_playerLobbyIndex + ":");
            sb.Append(players[i].m_teamIndex + ":");
            sb.Append(players[i].m_positionMmX + ":");
            sb.Append(players[i].m_positionMmY + ":");
            sb.Append(players[i].m_positionMmZ + ":");
            sb.Append(players[i].m_rotationDegreeX + ":");
            sb.Append(players[i].m_rotationDegreeY + ":");
            sb.Append(players[i].m_rotationDegreeZ + ":");
            sb.Append(players[i].m_sizeMmRadius + ":");
            sb.Append(players[i].m_flatXZDegreeAngle + "\n");
            BitConverter.GetBytes(players[i].m_playerIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 0);
            BitConverter.GetBytes(players[i].m_playerLobbyIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 4);
            BitConverter.GetBytes(players[i].m_teamIndex).CopyTo(m_playerCSVAsBytes, i * lineSize + 8);
            BitConverter.GetBytes(players[i].m_positionMmX).CopyTo(m_playerCSVAsBytes, i * lineSize + 12);
            BitConverter.GetBytes(players[i].m_positionMmY).CopyTo(m_playerCSVAsBytes, i * lineSize + 16);
            BitConverter.GetBytes(players[i].m_positionMmZ).CopyTo(m_playerCSVAsBytes, i * lineSize + 20);
            BitConverter.GetBytes(players[i].m_rotationDegreeX).CopyTo(m_playerCSVAsBytes, i * lineSize + 24);
            BitConverter.GetBytes(players[i].m_rotationDegreeY).CopyTo(m_playerCSVAsBytes, i * lineSize + 28);
            BitConverter.GetBytes(players[i].m_rotationDegreeZ).CopyTo(m_playerCSVAsBytes, i * lineSize + 32);
            BitConverter.GetBytes(players[i].m_sizeMmRadius).CopyTo(m_playerCSVAsBytes, i * lineSize + 36);
            BitConverter.GetBytes(players[i].m_flatXZDegreeAngle).CopyTo(m_playerCSVAsBytes, i * lineSize + 40);
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
        m_udpSizeBytes = byteArraySize;
        m_udpPercentText = (float)m_udpSizeText / 65535;
        m_udpPercentBytes = (float)m_udpSizeBytes / 65535;
        m_byteToTextRatio = (float)m_udpSizeText/m_udpSizeBytes ;

        m_onTextUdpPlayerPositionChanged.Invoke(m_playerCSVAsLines);
        m_onTextUtf8ByteUdpPlayerPositionChanged.Invoke(Encoding.UTF8.GetBytes(m_playerCSVAsLines));
        m_onBytesUdpPlayerPositionChanged.Invoke(m_playerCSVAsBytes);

    }
}
