using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using Services.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Services.Core.Level
{
    public class LevelService : ILevelService
    {
        private readonly Dictionary<string, ILevel> _levelsTable;
        private readonly ILevelFactory _levelFactory;
        private readonly IEvent _event;
        private ILevel _currentLevel;

        public LevelService(ILevelFactory levelFactory, IEvent @event)
        {
            _levelFactory = levelFactory;
            _levelsTable = new Dictionary<string, ILevel>();
            _event = @event;
        }

        public void CloseCurrentLevel()
        {
            //Debug.Log("CloseCurrentLevel");
            //if (_currentLevel.IsCloseByLoading)
            //    _screen.ShowScreen<LoadingScreen>(LoadingScreen.SCREEN_NAME, ECanvasType.MainCanvas, true);

            _currentLevel.Close();
            _currentLevel = null;
            _event.BroadcastEvent(EEventType.OnCloseLevel);
        }

        public void LoadLevel(string levelName, int stageCount, bool isShowLoading = true)
        {
            if (isShowLoading)
            {
                //LoadingScreen screen = _screen.ShowScreen<LoadingScreen>(LoadingScreen.SCREEN_NAME, ECanvasType.MainCanvas, false, true);
                //screen.SetData(LocalizationKey.GetLevelTitleKey(levelName), stageCount);
            }
            //_screen.CloseScreen(MainMenuScreen.SCREEN_NAME);
            
            ILevel level = null;
            if (_levelsTable.TryGetValue(levelName, out level))
            {
                if (level != null)
                {
                    _event.BroadcastEvent(EEventType.OnStartLoadingLevel, levelName);
                    level.Setup(OnLoadDone);
                }
            }
            else
            {
                _levelsTable.Add(levelName, null);
                _levelFactory.CreateLevel(levelName, OnCreateLevelDone);
            }
        }

        public void ServiceUpdate()
        {
            _currentLevel?.LevelUpdate();
        }

        private void OnCreateLevelDone(ILevel level)
        {
            _levelsTable[level.LevelName] = level;
            _event.BroadcastEvent(EEventType.OnStartLoadingLevel, level.LevelName);
            level.Setup(OnLoadDone);
        }

        private void OnLoadDone(ILevel level)
        {
            _currentLevel = level;
            //_screen.CloseScreen(LoadingScreen.SCREEN_NAME);
        }
    }
}