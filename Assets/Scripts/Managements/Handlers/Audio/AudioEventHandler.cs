using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using System;

namespace Managements.Handlers.Audio
{
    public class AudioEventHandler : IAudioEventHandler
    {
        private readonly IUpdateTask _updateTask;

        public AudioEventHandler(IUpdateTask updateTask)
        {
            _updateTask = updateTask;
        }

        public void RegisterUpdate(Action audioUpdate)
        {
            _updateTask.RegisterUpdateTask(audioUpdate);
        }
    }
}