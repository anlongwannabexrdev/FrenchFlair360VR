using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneQuestionManager : BaseSceneActionManager
{
    public VideoPlayer videoPlayer;
    public ChatManager chatManager;
    public SceneData sceneData;
    public string currentVideoId;
   
    public Image pdfFile;
    public GameObject exitButton;
    public CoolDownCycleTimeCmp coolDownCycleTimeCmp;

    public Image grayImage;
    public float fadeFactor;
    
    [Button]
    public override void GenerationAction()
    {
        if (sceneData.TryGetVideoData(currentVideoId, out VideoData videoData))
        {
            // play video
            PlayVideoAction playVideoAction = new PlayVideoAction(videoPlayer,videoData.videoClip);
            actions.Add(playVideoAction);

            CommonAction commonAction1 = new CommonAction((() =>
            {
                grayImage.DOFade(fadeFactor, 1f).OnComplete((() =>
                {
                    grayImage.gameObject.SetActive(true);

                    grayImage.DOFade(fadeFactor, 2f);
                }));
            }));
            actions.Add(commonAction1);

            // Delay a bit
            DelayTimeAction delayTime = new DelayTimeAction(1.5f);
            actions.Add(delayTime);
            
            // show PDF file
            CommonAction commonAction2 = new CommonAction((() =>
            {
                pdfFile.gameObject.SetActive(true);
            }));
            actions.Add(commonAction2);
            
            CommonActionV2 cmmonAction3 = new CommonActionV2(async () => { await coolDownCycleTimeCmp.StartCountdown(); });
            actions.Add(cmmonAction3);
            
            CommonAction commonAction4 = new CommonAction((() =>
            {
                grayImage.DOFade(0, 1f).OnComplete((() =>
                {
                    grayImage.gameObject.SetActive(false);
                    
                    pdfFile.gameObject.SetActive(false);
                    
                    coolDownCycleTimeCmp.gameObject.SetActive(false);
                }));
            }));
            
            actions.Add(commonAction4);
            
            // NPC ask question for User
            NPCAskUserAction npcAskUserAction = new NPCAskUserAction(chatManager,videoData.question.question);
            actions.Add(npcAskUserAction);
        
            // User Answer NPC
            UserAnswerNPCAction userAnswerNpcAction = new UserAnswerNPCAction(chatManager);
            actions.Add(userAnswerNpcAction);
        
            // NPC response to User
            NPCResponesUserAction npcResponesUserAction =
                new NPCResponesUserAction(chatManager, userAnswerNpcAction, videoData.question.promptData,videoData);
        
            actions.Add(npcResponesUserAction);
        }
    }
    
    [Button]
    public override async UniTask ExcuseAction()
    {
        await base.ExcuseAction();
    }
}
