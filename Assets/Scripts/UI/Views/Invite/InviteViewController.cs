using System.Runtime.InteropServices;
using Game.Invite;
using SkyExtensions;
using UI.Core;

namespace UI.Views
{
    public class InviteViewController : ViewController<InviteView>
    {
        private readonly InviteSystem _inviteSystem;


        public InviteViewController(InviteView view, InviteSystem inviteSystem) : base(view)
        {
            _inviteSystem = inviteSystem;
        }

        public void Initialize(string link, int referralCount, int score)
        {
            View.Initialize(link, referralCount, score);
        }

        protected override void OnShow()
        {
            View.CopyLinkButton.AddClickAction(OnClickCopyLink);
            View.ShareLinkButton.AddClickAction(OnClickShare);
        }

        protected override void OnHide()
        {
            View.CopyLinkButton.RemoveClickAction(OnClickCopyLink);
            View.ShareLinkButton.RemoveClickAction(OnClickShare);
        }

        private void OnClickShare()
        {
            WebGLExtensions.CopyWebGLText(_inviteSystem.InviteText);
        }

        private void OnClickCopyLink()
        {
        }
    }
}