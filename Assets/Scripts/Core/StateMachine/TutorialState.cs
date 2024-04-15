using System.Linq;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;
using Core.Tutorial;
using Core.Tutorial.UI;
using Main;
using Main.Level;
using ObjectsData;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class TutorialState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ISaveService<PlayerProgressData> _playerProgressData;
        private readonly UILoadingProvider _uiLoadingProvider;
        
        private LevelManager _levelManager;
        private UIPause _uiPause;

        public TutorialState(GameStateMachine stateMachine, 
            Game game, 
            ICoroutineRunner coroutineRunner, 
            UILoadingProvider uiLoadingProvider,
            ISaveService<PlayerProgressData> playerProgressData)
        {
            _stateMachine = stateMachine;
            _game = game;
            _coroutineRunner = coroutineRunner;
            _uiLoadingProvider = uiLoadingProvider;
            _playerProgressData = playerProgressData;
        }

        public async void Enter()
        {
            PrepareLevelManager();
            PrepareUIPause();
            DisableUIPause();

            TutorialProvider tutorialProvider = new TutorialProvider();
            await tutorialProvider.Load();

            _levelManager.BuildTutorialLevel(ResourceLoader.TrashCanDatas.First(data => data.Type == TrashType.Organic));
            var trash = GameObject.FindObjectsOfType<TrashObject>();
            var banana = trash.First(t => t.TrashData.Title == "Banana");
            
            foreach (var trashObject in trash) 
                trashObject.ToggleInteraction(false);

            var tutorialUI = GameObject.FindObjectOfType<TutorialMessages>();

            var tutorial = TutorialManager.Create()
                .AddPart(new CustomActionPart(tutorialUI.Init))
                .AddPart(new TriggerPart(tutorialUI.Messages[0].SkipButton.onClick))
                .AddPart(new CustomActionPart(tutorialUI.SwitchToNext))
                .AddPart(new TriggerPart(tutorialUI.Messages[1].SkipButton.onClick))
                .AddPart(new CustomActionPart(tutorialUI.SwitchToNext))
                .AddPart(new TriggerPart(tutorialUI.Messages[2].SkipButton.onClick))
                .AddPart(new CustomActionPart(tutorialUI.DisableCurrent))
                // highlight banana
                .AddPart(new CustomActionPart(() => banana.ToggleInteraction(true)))
                .AddPart(new TriggerPart(banana.OnDisposed))
                .AddPart(new CustomActionPart(tutorialUI.EnableNext))
                .AddPart(new TriggerPart(tutorialUI.Messages[3].SkipButton.onClick))
                .AddPart(new CustomActionPart(tutorialUI.DisableCurrent))
                .AddPart(new CustomActionPart(() =>
                {
                    foreach (var trashObject in trash)
                        trashObject.ToggleInteraction(true);
                }))
                .AddPart(new TriggerPart(_levelManager.LevelComplete))
                .AddPart(new CustomActionPart(tutorialUI.EnableNext))
                .AddPart(new TriggerPart(tutorialUI.Messages[4].SkipButton.onClick))
                .AddPart(new CustomActionPart(tutorialUI.DisableCurrent));

            tutorial.TutorialCompleted += OnTutorialCompleted;
            
            _uiLoadingProvider.TryUnload();
            _coroutineRunner.StartCoroutine(tutorial.StartExecution());
        }

        private void OnTutorialCompleted()
        {
            UpdateProgressData();
            _stateMachine.Enter<GameOverState, GameOverCondition>(GameOverCondition.TutorialCompleted);
        }

        public void Exit()
        {
            _levelManager.LevelComplete.RemoveAllListeners();
        }

        private void PrepareLevelManager()
        {
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            _levelManager.ClearLevelUI();
        }

        private void PrepareUIPause() => 
            _uiPause = GameObject.FindObjectOfType<UIPause>();

        private void DisableUIPause() => 
            _uiPause.CloseWithoutAnimation();

        private void UpdateProgressData()
        {
            _playerProgressData.SaveData.TutorialCompleted = true;
            _playerProgressData.Save();
        }
    }
}