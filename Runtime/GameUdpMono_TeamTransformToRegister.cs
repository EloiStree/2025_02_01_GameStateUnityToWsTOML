using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameUdpMono_TeamTransformToRegister : MonoBehaviour
{


    public GameUdpMono_PlayerInGameToExport m_registerRef;
    //    public GameUdpMono_PlayerInGameToExport m_registerRef;
    public Transform m_rootRefrence;
    public Transform [] m_teamTransforms;

    [Header("Generated")]
    public List<Transform > m_listOfPlayers;

    public TeamTransform[] m_teamTransformsArray;
    [System.Serializable]
    public class TeamTransform
    {
        public Transform m_teamTransform;
        public List<Transform> m_players = new List<Transform>();
    }

    public List<LobbyIndexToPlayer> m_listOfLobbyIndexToPlayer = new List<LobbyIndexToPlayer>();

    public UnityEvent m_requestRefreshIndexFromTransform;
    public UnityEvent m_onWasUpdated;

    /// <summary>
    ///  This abstract tool can't know what player index is assigned to what player.
    ///  This methode allows to set the player integer index.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="index"></param>
    public void SetTransformPlayerIndex(Transform player, int index)
    {
        if (m_listOfPlayers == null)
        {
            return;
        }

        for(int i=0; i< m_listOfLobbyIndexToPlayer.Count; i++)
        {
            if(m_listOfLobbyIndexToPlayer[i].m_linkedPlayer == player)
            {
                m_listOfLobbyIndexToPlayer[i].m_playerIndex = index;
                m_listOfLobbyIndexToPlayer[i].m_player.m_playerIndex = index;
                break;
            }
        }
    }



    [ContextMenu("Refresh List Of Player")]
    public void RefreshListOfPlayer()
    {
        m_listOfLobbyIndexToPlayer.Clear();


        if (m_listOfPlayers == null)
        {
            return;
        }
        m_listOfPlayers = new List<Transform>();  
        m_teamTransformsArray = new TeamTransform[m_teamTransforms.Length];
        for (int i = 0; i < m_teamTransforms.Length; i++)
        {
            m_teamTransformsArray[i] = new TeamTransform();
            m_teamTransformsArray[i].m_teamTransform = m_teamTransforms[i];
            for (int j = 0; j < m_teamTransforms[i].childCount; j++)
            {
                m_teamTransformsArray[i].m_players.Add(m_teamTransforms[i].GetChild(j));
                m_listOfPlayers.Add(m_teamTransforms[i].GetChild(j));
            }
        }

        for (int i = 0; i < m_listOfPlayers.Count; i++)
        {
            LobbyIndexToPlayer player = new LobbyIndexToPlayer();
            player.m_playerIndex = 0;
            player.m_lobbyIndex= i;
            player.m_teamIndex = -1;
            for (int t = 0; t < m_teamTransformsArray.Length; t++)
            {
                if (m_teamTransformsArray[t].m_teamTransform == m_listOfPlayers[i].parent)
                {
                    player.m_teamIndex = t;
                    break;
                }
            }
            player.m_linkedPlayer = m_listOfPlayers[i];
            m_listOfLobbyIndexToPlayer.Add(player);
        }
        m_requestRefreshIndexFromTransform.Invoke();
        Update();        
    }

    [System.Serializable]
    public class LobbyIndexToPlayer{

        public int m_playerIndex;
        public int m_teamIndex;
        public int m_lobbyIndex;
        public Transform m_linkedPlayer;
        public PlayerInGameIndexTeamTransform m_player;
    }

    [ContextMenu("Update Register")]
    public void Update(){

        

        m_registerRef.ClearAllPlayers();
        Vector3 positionReference = m_rootRefrence.position;
        Quaternion rotationReference = m_rootRefrence.rotation;
        foreach (var item in m_listOfLobbyIndexToPlayer)
        {

            if (item.m_linkedPlayer == null)
            {
                continue;
            }
            if (item.m_linkedPlayer.gameObject.activeInHierarchy == false)
            {
                continue;
            }

            

            GetWorldToLocal_DirectionalPoint(item.m_linkedPlayer.position, item.m_linkedPlayer.rotation, positionReference, rotationReference, 
            out Vector3 localPosition, out Quaternion localRotation);
            Vector3 localRotationForward = localRotation * Vector3.forward;
            localRotationForward.y = 0;

            float floatAngleClassicMath = -Vector3.SignedAngle(Vector3.right, localRotationForward, Vector3.up);
            m_registerRef.AddIfNotExistingFromLobbyIndex( item.m_lobbyIndex, out PlayerInGameIndexTeamTransform player);
            player.m_positionMmX = (int)(localPosition.x * 1000);
            player.m_positionMmY = (int)(localPosition.y * 1000);
            player.m_positionMmZ = (int)(localPosition.z * 1000);
            player.m_rotationDegreeX = (int)(localRotation.eulerAngles.x * 1000);
            player.m_rotationDegreeY = (int)(localRotation.eulerAngles.y * 1000);
            player.m_rotationDegreeZ = (int)(localRotation.eulerAngles.z * 1000);
            
            player.m_sizeMmRadius = (int)(item.m_linkedPlayer.transform.localScale.x * 1000);
            player.m_flatXZDegreeAngle =(int)(floatAngleClassicMath*1000);
            player.m_teamIndex = (ushort)item.m_teamIndex;
            player.m_playerIndex = item.m_playerIndex;
            player.m_playerLobbyIndex = item.m_lobbyIndex;
            item.m_player = player;

            
        }   
        m_onWasUpdated.Invoke();
    }


       public static void GetWorldToLocal_DirectionalPoint(in Vector3 worldPosition, in Quaternion worldRotation, in Vector3 positionReference, in Quaternion rotationReference, out Vector3 localPosition, out Quaternion localRotation)
        {
            localRotation = Quaternion.Inverse(rotationReference)* worldRotation;
            localPosition = Quaternion.Inverse(rotationReference) * (worldPosition - positionReference);
        }       
}
