using System.Collections.Generic;
using UnityEngine;


public class GameUdpMono_PlayerInGameToExport : MonoBehaviour
{
    public ListOfPlayerInGameIndexTeamTransform m_register;

    public void GetPlayers(out List<PlayerInGameIndexTeamTransform> players)
    {
        m_register.GetPlayers(out players);
    }

    [ContextMenu("Randomized For Testing")]
    public void RandomizedForTesting()
    {
        m_register.RandomizedForTesting();
    }
}

