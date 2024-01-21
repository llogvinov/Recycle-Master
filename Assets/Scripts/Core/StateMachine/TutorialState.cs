using System.Collections;
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
            //_coroutineRunner.StartCoroutine(ProceedTutorial());

            var tutorial = TutorialManager.Create()
                .AddPart(new MessagePart(_uiMessage, "hello recycle master! you need to dispose all trash..."))
                .AddPart(new MessagePart(_uiMessage, "let's start!!!"))
                .AddPart(new LevelPart(_levelManager, ResourceLoader.TrashCanDatas[0]))
                .AddPart(new MessagePart(_uiMessage, "let's continue"))
                .AddPart(new LevelPart(_levelManager, ResourceLoader.TrashCanDatas[1]))
                .AddPart(new MessagePart(_uiMessage, "well done!"));

            tutorial.TutorialCompleted += OnTutorialCompleted;
            _coroutineRunner.StartCoroutine(tutorial.StartExecution());

            /*_game.GameOver = null;
            _game.GameOver += (condition) =>  _stateMachine.Enter<GameOverState, GameOverCondition>(condition);*/
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

        /*private IEnumerator ProceedTutorial()
        {
            const float checkDelay = 0.5f;
            
            foreach (var trashCanData in ResourceLoader.TrashCanDatas)
            {
                var tutorialPartCompleted = false;
                GenerateTutorialLevel(trashCanData);
                _uiMessage.SetMessage("tutorial", $"dispose all {trashCanData.Type} trash");

                // message
                var messageRead = false;
                _uiMessage.MessageRead = null;
                _uiMessage.MessageRead += () => messageRead = true;
                
                _uiMessage.MessageSkiped = null;
                _uiMessage.MessageSkiped += () =>
                {
                    messageRead = true;
                    tutorialPartCompleted = true;
                };
                
                _uiMessage.Open();
                while (!messageRead)
                {
                    yield return new WaitForSeconds(checkDelay);
                }

                // level
                _levelManager.LevelComplete = null;
                _levelManager.LevelComplete += () => tutorialPartCompleted = true;
                while (!tutorialPartCompleted)
                {
                    yield return new WaitForSeconds(checkDelay);
                }
            }
            UpdateProgressData();
            _game.GameOver(GameOverCondition.Won);
        }*/

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