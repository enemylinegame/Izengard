using System;
using UnityEngine;

namespace StartupMenu
{
    [CreateAssetMenu(
        fileName = nameof(SettingsDataConfig), 
        menuName = "GameParametes/" + nameof(SettingsDataConfig), 
        order = 0)]
    public class SettingsDataConfig : ScriptableObject, ISettingsData
    {
        #region ISettingsData

        public int ResolutionWidth => _baseSceenWidth;
        public int ResolutionHeight => _baseSceenHeight;
        public int ShadowId => _shadowId;

        public bool IsFullScreenOn => _isFullScreenOn;
        public bool IsVSyncOn => _isFVSyncOn;

        public float MixerMaxValue => _maxMixerValue;
        public float MixerMinValue => _minMixerVBalue;

        public float MasterVolumeValue => ConvertRange(_masterVolumeValue);
        public float MusicVolumeValue => ConvertRange(_musicVolumeValue);
        public float VoiceVolumeValue => ConvertRange(_voiceVolumeValue);
        public float EffectsVolumeValue => ConvertRange(_effectsVolumeValue);

        #endregion

        [Header("Graphics Settings")]
        [SerializeField] private int _baseSceenWidth = 1920;
        [SerializeField] private int _baseSceenHeight = 1080;
        [SerializeField] private int _shadowId = 1;
        [SerializeField] private bool _isFullScreenOn = true;
        [SerializeField] private bool _isFVSyncOn = true;

        [Space(10)]
        [Header("AudioMixer Parametrs")]
        [Tooltip("Set value in dB")]
        [SerializeField] private float _maxMixerValue = 0;
        [Tooltip("Set value in dB")]
        [SerializeField] private float _minMixerVBalue = -80;

        [Space(10)]
        [Header("Sound Settings (value in %)")]

        [Range(0f, 100f)]
        [SerializeField] private float _masterVolumeValue = 100f;
        [Range(0f, 100f)]
        [SerializeField] private float _musicVolumeValue = 100f;
        [Range(0f, 100f)]
        [SerializeField] private float _voiceVolumeValue = 100f;
        [Range(0f, 100f)]
        [SerializeField] private float _effectsVolumeValue = 100f;


        private float ConvertRange(float value)
        {
            float convertedValue = ((value - 0) / (100 - 0)) * (_maxMixerValue - _minMixerVBalue) + _minMixerVBalue;
            return convertedValue;
        }
    }
}
