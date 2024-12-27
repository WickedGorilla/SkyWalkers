using UnityEngine;

namespace Infrastructure.Data.Game.MiniGames.PasswordMiniGame
{
    [CreateAssetMenu(fileName = "PasswordMiniGameData", menuName = "ScriptableObjects/Minigames/PasswordMiniGameData", order = 0)]
    public class PasswordMiniGameData : ScriptableObject
    {
        [SerializeField] private PasswordVariable[] _passwords;

        public PasswordVariable[] Passwords => _passwords;
        
        public PasswordVariable GetRandomPassword()
        {
            int randomIndex = Random.Range(0, _passwords.Length);
            return _passwords[randomIndex];
        }
    }
}