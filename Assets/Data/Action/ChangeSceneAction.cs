using Cysharp.Threading.Tasks;

public class ChangeSceneAction : BaseAction
{
    public override ActionType ActionType => ActionType.ChangeSceneAction;

    public ChangeSceneCmp _changeSceneCmp;
    private string _sceneToChange;

    public ChangeSceneAction(ChangeSceneCmp changeSceneCmp, string sceneToChange)
    {
        _changeSceneCmp = changeSceneCmp;
        _sceneToChange = sceneToChange;
    }
    
    public override async UniTask Excuse()
    {
        await _changeSceneCmp.StartFade(_sceneToChange);
    }
}
