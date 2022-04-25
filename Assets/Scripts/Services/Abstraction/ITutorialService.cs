using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Abstraction
{
    public interface ITutorialService
    {
        bool IsTutorialRunning { get; }
    }
}