using Game.Invite;
using SkyExtensions;
using UI.Core;
using UnityEngine;

namespace UI.Views
{
    public class InviteViewController : ViewController<InviteView>
    {
        private readonly InviteSystem _inviteSystem;

        public InviteViewController(InviteView view, InviteSystem inviteSystem) : base(view)
        {
            _inviteSystem = inviteSystem;
        }

        protected override void OnShow()
        {
            View.CopyLinkButton.AddClickAction(OnClickCopyLink);
            View.ShareLinkButton.AddClickAction(OnClickShare);
            
            View.Initialize(_inviteSystem.InviteLink, _inviteSystem.ReferralCount, _inviteSystem.Score);
        }

        protected override void OnHide()
        {
            View.CopyLinkButton.RemoveClickAction(OnClickCopyLink);
            View.ShareLinkButton.RemoveClickAction(OnClickShare);
        }

        private void OnClickShare() 
            => Application.OpenURL(_inviteSystem.InviteShareLink);

        private void OnClickCopyLink() 
            => WebGLExtensions.CopyWebGLText(_inviteSystem.InviteText);
    }
}