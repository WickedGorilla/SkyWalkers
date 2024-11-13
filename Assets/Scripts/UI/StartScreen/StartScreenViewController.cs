using UI.Core;

public class StartScreenViewController : ViewController<StartScreenView>
{
    private readonly ViewService _viewService;

    public StartScreenViewController(StartScreenView view, ViewService viewService) : base(view) 
        => _viewService = viewService;

    protected override void OnShow() 
        => View.OnEnd += OnEnd;

    protected override void OnHide() 
        => View.OnEnd -= OnEnd;

    private void OnEnd() 
        => _viewService.HidePermanent<StartScreenViewController>();
}
