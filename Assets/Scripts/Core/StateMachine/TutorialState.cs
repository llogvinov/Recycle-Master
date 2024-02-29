using System.Collections;
using System.Linq;
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
        
        private LevelManager _levelManager;
        private UIMessage _uiMessage;

        public TutorialState(GameStateMachine stateMachine, 
            Game game, 
            ICoroutineRunner coroutineRunner, 
            ISaveService<PlayerProgressData> playerProgressData)
        {
            _stateMachine = stateMachine;
            _game = game;
            _coroutineRunner = coroutineRunner;
            _playerProgressData = playerProgressData;
        }

        public void Enter()
        {
            PrepareLevelManager();
            
            _levelManager.BuildTutorialLevel(ResourceLoader.TrashCanDatas[0]);
            var trash = GameObject.FindObjectsOfType<TrashObject>();
            var glass = trash.First(t => t.TrashData.Title == "BeerBottle");
            
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
                // highlight bottle
                .AddPart(new CustomActionPart(() => glass.ToggleInteraction(true)))
                .AddPart(new TriggerPart(glass.OnDisposed))
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
            _coroutineRunner.StartCoroutine(tutorial.StartExecution());
        }

        private void OnTutorialCompleted()
        {
            UpdateProgressData();
            _stateMachine.Enter<GameOverState, GameOverCondition>(GameOverCondition.TutorialCompleted);
        }

        public void Exit()
        {
            _levelManager.LevelComplete = null;
        }

        private void PrepareLevelManager() => 
            _levelManager = GameObject.FindObjectOfType<LevelManager>();

        private void GenerateTutorialLevel(TrashCanData trashCanData) => 
            _levelManager.BuildTutorialLevel(trashCanData);

        private void UpdateProgressData()
        {
            _playerProgressData.SaveData.TutorialCompleted = true;
            _playerProgressData.Save();
        }
    }
}