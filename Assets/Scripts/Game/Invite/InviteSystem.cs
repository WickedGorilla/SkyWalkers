
namespace Game.Invite
{
    public class InviteSystem
    {
        public string InviteLink { get; private set;  }
        public int ReferralCount { get; private set;  }
        public int Score { get; private set;  }
        public string InviteText { get; set; }

        public void Initialize(string link, int referralCount, int score)
        {
            InviteLink = link;
            ReferralCount = referralCount;
            Score = score;
            InviteText = CreateInviteText(InviteLink);
        }

        private string CreateInviteText(string url) 
            => $"Join the SkyWalkers community! Click here to play: {url}";
    }
}