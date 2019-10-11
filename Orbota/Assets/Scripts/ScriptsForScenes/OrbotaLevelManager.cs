using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System;
using System.Linq;

public class OrbotaLevelManager : LevelManager
{
    protected override IEnumerator GotoLevelCo(string levelName)
    {
        if (Players != null && Players.Count > 0)
        {
            foreach (Character player in Players)
            {
                player.Disable();
            }
        }

        if (Time.timeScale > 0.0f)
        {
            yield return new WaitForSeconds(OutroFadeDuration);
        }
        // we trigger an unPause event for the GameManager (and potentially other classes)
        MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.UnPause));

        if (string.IsNullOrEmpty(levelName))
        {
            LoadingSceneManager.LoadScene("StartScreen");
        }
        else
        {
            LoadingSceneManager.LoadScene(levelName);
        }
    }

    protected override IEnumerator SoloModeRestart()
    {
        if (PlayerPrefabs.Count() <= 0)
        {
            yield break;
        }

        // if we've setup our game manager to use lives (meaning our max lives is more than zero)
        if (GameManager.Instance.MaximumLives > 0)
        {
            // we lose a life
            GameManager.Instance.LoseLife();
            // if we're out of lives, we check if we have an exit scene, and move there
            if (GameManager.Instance.CurrentLives <= 0)
            {
                MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.GameOver));
                if ((GameManager.Instance.GameOverScene != null) && (GameManager.Instance.GameOverScene != ""))
                {
                    LoadingSceneManager.LoadScene(GameManager.Instance.GameOverScene);
                }
            }
        }

        if (LevelCameraController != null)
        {
            LevelCameraController.FollowsPlayer = false;
        }

        yield return new WaitForSeconds(RespawnDelay);

        if (LevelCameraController != null)
        {
            LevelCameraController.FollowsPlayer = true;
        }

        if (CurrentCheckPoint != null)
        {
            CurrentCheckPoint.SpawnPlayer(Players[0]);
        }
        _started = DateTime.UtcNow;
        // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
        MMEventManager.TriggerEvent(new CorgiEnginePointsEvent(PointsMethods.Set, 0));
        // we trigger a respawn event
        MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.Respawn));
    }

}
