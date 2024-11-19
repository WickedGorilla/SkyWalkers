
using Infrastructure.Network.Request.Base.Player;

namespace Game.Invite
{
    public class InviteSystem
    {
        public string InviteLink { get; private set;  }
        public int ReferralCount { get; private set;  }
        public int Score { get; private set;  }
        public string InviteText { get; set; }

        public void Initialize(ReferralInfo referralInfo)
        {
            InviteLink = referralInfo.ReferralLink;
            ReferralCount = referralInfo.CountReferrals;
            Score = referralInfo.TotalCoins;
            InviteText = CreateInviteText(InviteLink);
        }

        private string CreateInviteText(string url) 
            => $"Join the SkyWalkers community! Click here to play: {url}";
    }
}