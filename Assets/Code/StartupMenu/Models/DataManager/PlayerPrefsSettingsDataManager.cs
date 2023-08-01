﻿using System.Collections.Generic;
using UnityEngine;

namespace StartupMenu.DataManager
{
    public class PlayerPrefsSettingsDataManager : SettingsDataManager
    {
        #region Const Names

        private const string SCREEN_RESOLUTION_WIDTH = "ScreenWidth";
        private const string SCREEN_RESOLUTION_HEIGHT = "ScreenHeight";
        private const string SHADOW_QUALITY = "ShadowQuality";
        private const string FULLSCREEN_MODE = "FullscreenMode";
        private const string VSYNC_MODE = "VSyncMode";
        
        private const string MASTER_VOLUME_LEVEL = "MasterVolumeLevel";
        private const string MUSIC_VOLUME_LEVEL = "MusicVolumeLevel";
        private const string VOICE_VOLUME_LEVEL = "VoiceVolumeLevel";
        private const string EFFECTS_VOLUME_LEVEL = "EffectsVolumeLevel";

        #endregion

        public PlayerPrefsSettingsDataManager() 
        {
            var parametesStateList = new List<bool>();

            parametesStateList.Add(PlayerPrefs.HasKey(SCREEN_RESOLUTION_WIDTH));
            parametesStateList.Add(PlayerPrefs.HasKey(SCREEN_RESOLUTION_HEIGHT));

            parametesStateList.Add(PlayerPrefs.HasKey(SHADOW_QUALITY));

            parametesStateList.Add(PlayerPrefs.HasKey(FULLSCREEN_MODE));
            parametesStateList.Add(PlayerPrefs.HasKey(VSYNC_MODE));

            parametesStateList.Add(PlayerPrefs.HasKey(MASTER_VOLUME_LEVEL));
            parametesStateList.Add(PlayerPrefs.HasKey(MUSIC_VOLUME_LEVEL));
            parametesStateList.Add(PlayerPrefs.HasKey(VOICE_VOLUME_LEVEL));
            parametesStateList.Add(PlayerPrefs.HasKey(EFFECTS_VOLUME_LEVEL));

            IsDataStored = !parametesStateList.Contains(false);
        }


        public override void SaveData(SaveLoadSettingsModel data)
        {
            PlayerPrefs.SetInt(SCREEN_RESOLUTION_WIDTH, data.ResolutionWidth);
            PlayerPrefs.SetInt(SCREEN_RESOLUTION_HEIGHT, data.ResolutionHeight);

            PlayerPrefs.SetInt(SHADOW_QUALITY, data.ShadowId);

            PlayerPrefs.SetInt(FULLSCREEN_MODE, data.IsFullScreenOn ? 1 : 0);
            PlayerPrefs.SetInt(VSYNC_MODE, data.IsVSyncOn ? 1 : 0);

            PlayerPrefs.SetFloat(MASTER_VOLUME_LEVEL, data.MasterVolumeValue);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_LEVEL, data.MusicVolumeValue);
            PlayerPrefs.SetFloat(VOICE_VOLUME_LEVEL, data.VoiceVolumeValue);
            PlayerPrefs.SetFloat(EFFECTS_VOLUME_LEVEL, data.EffectsVolumeValue);
        }

        public override SaveLoadSettingsModel LoadData()
        {
            var screenWidth = PlayerPrefs.GetInt(SCREEN_RESOLUTION_WIDTH);
            var screenHeight = PlayerPrefs.GetInt(SCREEN_RESOLUTION_HEIGHT);

            var shadowId = PlayerPrefs.GetInt(SHADOW_QUALITY);

            var isFullScreen = PlayerPrefs.GetInt(FULLSCREEN_MODE) == 1;
            var isVSync = PlayerPrefs.GetInt(VSYNC_MODE) == 1;

            var masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_LEVEL);
            var musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_LEVEL);
            var voiceVolume = PlayerPrefs.GetFloat(VOICE_VOLUME_LEVEL);
            var effectsVolume = PlayerPrefs.GetFloat(EFFECTS_VOLUME_LEVEL);

            var loadData = new SaveLoadSettingsModel
            {
                ResolutionWidth = screenWidth,
                ResolutionHeight = screenHeight,
                ShadowId = shadowId,
                IsFullScreenOn = isFullScreen,
                IsVSyncOn = isVSync,
                MasterVolumeValue = masterVolume,
                MusicVolumeValue = musicVolume,
                VoiceVolumeValue = voiceVolume,
                EffectsVolumeValue = effectsVolume
            };

            return loadData;
        }
    }
}
