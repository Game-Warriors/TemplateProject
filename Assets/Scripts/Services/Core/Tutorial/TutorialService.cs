using GameWarriors.StorageDomain.Abstraction;
using GameWarriors.StorageDomain.Data;
using GameWarriors.TutorialDomain.Abstraction;
using Services.Abstraction;
using Services.Data.Tutorial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core.Tutorial
{
    public class TutorialService : ITutorialService
    {
        private const string FILE_NAME = "tut.bin";
        private readonly ITutorial _tutorial;
        private TutorialDataModel _dataModel;

        public bool IsTutorialRunning => false;


        [UnityEngine.Scripting.Preserve]
        public TutorialService(ITutorial tutorial, IStorage storage)
        {
            _tutorial = tutorial;
            GetDataModel(storage);
            tutorial.OnTutorialSetup += TutorialSetup;
            tutorial.OnTutorialDone += TutorialDone;
        }


        public async void GetDataModel(IStorage storage)
        {
            _dataModel = await storage.LoadingModelAsync<TutorialDataModel>(FILE_NAME, false);
            _tutorial.SetDoneTutorials(FetchAllDoneTutorials());
            foreach(string tutorial in _dataModel.CurrnetTutorialsKey)
            {
                _tutorial.StartTutorialJourney(tutorial);
            }
        }

        private void TutorialSetup(ITutorialSession tutorialSession)
        {
            _dataModel.AddCurrentTutorial(tutorialSession.TutorialKey);
        }

        private void TutorialDone(ITutorialSession tutorialSession)
        {
            _dataModel.RemoveCurrentTutorial(tutorialSession.TutorialKey);
            _dataModel.AddDoneTutorial(tutorialSession.TutorialKey);
        }

        private IEnumerable<string> FetchAllDoneTutorials()
        {
            return _dataModel.DoneTutorialsKey;
        }
    }
}