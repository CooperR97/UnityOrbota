using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class OrbotaFinishLevel : FinishLevel
{
    public override void GoToNextLevel()
    {
        if (OrbotaLevelManager.Instance != null)
        {
            OrbotaLevelManager.Instance.GotoLevel(LevelName);
        }
        else
        {
            OrbotaLoadingManager.LoadScene(LevelName);
        }

    }
}
