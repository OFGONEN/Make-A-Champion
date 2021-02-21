﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
	[CreateAssetMenu( fileName = "CurrentLevelData", menuName = "FF/Data/CurrentLevelData" )]
    public class CurrentLevelData : ScriptableObject
    {
        public int currentLevel;
        public GameSettings gameSettings;
        public LevelData levelData;
        public void LoadCurrentLevelData()
        {
            if (currentLevel > gameSettings.maxLevelCount)
            {
                currentLevel = Random.Range(1, gameSettings.maxLevelCount);
            }

            levelData = Resources.Load<LevelData>("LevelData_" + currentLevel);
        }
    }
}