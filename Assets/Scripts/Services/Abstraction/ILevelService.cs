using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Abstraction
{
    public interface ILevel
    {
        string LevelName { get; }
        bool IsOnlineLevel { get; }
        bool IsCloseByLoading { get; }

        void Setup(Action<ILevel> onSetupDone);
        void Close();

        void LevelUpdate();
    }

    public interface ILevelFactory
    {
        void CreateLevel(string gameType, Action<ILevel> onDone);
    }

    public interface ILevelService
    {
        void LoadLevel(string levelName,int stageCount, bool isShowLoading = true);
        void CloseCurrentLevel();
    }
}