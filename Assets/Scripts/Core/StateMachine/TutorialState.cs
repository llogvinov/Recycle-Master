using System.Collections;
using System.Linq;
using Core.Data;
using Core.SaveService;
using Core.Tutorial;
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
            PrepareUIMessage();
            
            _levelManager.BuildTutorialLevel(ResourceLoader.TrashCanDatas[0]);
            var trash = GameObject.FindObjectsOfType<TrashObject>();
            var glass = trash.First(t => t.TrashData.Title == "Glass");
            
            foreach (var trashObject in trash) 
                trashObject.ToggleInteraction(false);

            var tutorial = TutorialManager.Create()
                .AddPart(new MessagePart(_uiMessage, "hello recycle master! you need to dispose all the trash here..."))
                .AddPart(new MessagePart(_uiMessage, "let's start with the glass."))
                .AddPart(new MessagePart(_uiMessage, "tap on the glass to dispose it"))
                // highlight bottle
                .AddPart(new CustomActionPart(() => glass.ToggleInteraction(true)))
                .AddPart(new TriggerPart(glass.OnDisposed))
                .AddPart(new MessagePart(_uiMessage, "good, now dispose all the rest."))
                .AddPart(new CustomActionPart(() =>
                {
                    foreach (var trashObject in trash) 
                        trashObject.ToggleInteraction(true);
                }))
                .AddPart(new TriggerPart(_levelManager.LevelComplete))
                .AddPart(new MessagePart(_uiMessage, "well done! tutorial is complete"));

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
        
        private void PrepareUIMessage() => 
            _uiMessage = GameObject.FindObjectOfType<UIMessage>();

        private void GenerateTutorialLevel(TrashCanData trashCanData) => 
            _levelManager.BuildTutorialLevel(trashCanData);

        private void UpdateProgressData()
        {
            _playerProgressData.SaveData.TutorialCompleted = true;
            _playerProgressData.Save();
        }
    }
}