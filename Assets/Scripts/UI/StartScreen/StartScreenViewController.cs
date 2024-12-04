using UI.Core;

public class StartScreenViewController : ViewController<StartScreenView>
{

    public StartScreenViewController(StartScreenView view) : base(view)
    {
        
    }

    protected override void OnShow() 
        => View.OnEnd += Hide;

    protected override void OnHide() 
        => View.OnEnd -= Hide;
}
