
using Infrastructure.Network.Request.Base.Player;

namespace Game.Invite
{
    public class InviteSystem
    {
        private const string InviteMessage = "Join the Game! Click here to play:";
        
        public string InviteLink { get; private set;  }
        public string InviteShareLink { get; private set;  }
        public int ReferralCount { get; private set;  }
        public int Score { get; private set;  }
        public string InviteText { get; set; }
        
        public void Initialize(ReferralInfo referralInfo)
        {
            InviteLink = referralInfo.ReferralLink;
            InviteShareLink = CreateShareLink(InviteLink);
            ReferralCount = referralInfo.CountReferrals;
            Score = referralInfo.TotalCoins;
            InviteText = CreateInviteText(InviteLink);
        }

        private string CreateInviteText(string url) 
            => $"{InviteMessage}'\n''\n'{url}";

        private string CreateShareLink(string link) 
            => $"https://t.me/share/url?url={link}&text={InviteMessage}";
    }
}