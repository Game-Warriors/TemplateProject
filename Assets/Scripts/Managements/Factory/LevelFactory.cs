using Services.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managements.Factory
{
    public class LevelFactory : ILevelFactory
    {
        private Action<ILevel> _onDone;

        public LevelFactory()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void CreateLevel(string levelName, Action<ILevel> onDone)
        {
            switch (levelName)
            {
                case "level1": /*_levelManager = new LevelManager(); onDone?.Invoke(_levelManager); _onDone = null;*/ break;
                case "level2": /*_onDone = onDone; SceneManager.LoadSceneAsync(MENCH_SCENE_NAME, LoadSceneMode.Additive);*/ break;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            GameObject[] objects = scene.GetRootGameObjects();
            int length = objects.Length;
            for (int i = 0; i < length; ++i)
            {
                ILevel level = objects[i].GetComponent<ILevel>();
                if (level != null)
                {
                    //if (level is MenschManager)
                    //{
                    //    ((MenschManager)level).Initialization(ServiceLocator.Resolve<IEvent>(), ServiceLocator.Resolve<IArenaService>()
                    //        , ServiceLocator.Resolve<IGameTableService>(), ServiceLocator.Resolve<IPool>(), ServiceLocator.Resolve<IParticleService>()
                    //        , ServiceLocator.Resolve<ILevelService>(), ServiceLocator.Resolve<IScreenHandler>(), ServiceLocator.Resolve<ILocalizeHandler>());
                    //}

                    //_onDone?.Invoke(level);
                    //_onDone = null;
                    break;
                }
            }
        }

    }
}