using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IPlayerAction
    {
        void gameStart();
        int getScore();

        bool getOver();
    }
    public interface ISceneController
    {
        void loadResources();
    }
}
