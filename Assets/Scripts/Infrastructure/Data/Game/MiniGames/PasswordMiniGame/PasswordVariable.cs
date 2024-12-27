using System;
using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames.PasswordMiniGame
{
    [Serializable]
    public class PasswordVariable
    {
        [SerializeField] private int[] _nodesIndexes;

        public int[] NodesIndexes => _nodesIndexes;
    }
}