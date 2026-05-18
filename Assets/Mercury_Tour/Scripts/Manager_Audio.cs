using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Audio : MonoBehaviour
{
    public static Manager_Audio Instance;
    public static Action<int> OnAudioFinished;
    public AudioSource _voiceSource;

    [Header("Настройка пауз")]
    public float _defaultDelay = 2.0f;

    [System.Serializable]
    public class VoiceGroup
    {
        public AudioClip[] clips;
    }

    [Header("Список фраз по стадиям (массивы)")]
    public VoiceGroup[] _stageVoiceGroups;
    
    public AudioClip _ojectSpeech;

    private Queue<int> _stagesQueue = new Queue<int>();
    private bool _isProcessingQueue = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayStageVoice(); 
    }

    public void PlayStageVoice()
    {
        int _currentStage = Manager_Stages.Instance.GetIndexCurrentStage();
        if (_currentStage >= 0 && _currentStage < _stageVoiceGroups.Length)
        {
            _stagesQueue.Enqueue(_currentStage);

            if (!_isProcessingQueue)
            {
                StartCoroutine(ProcessAudioQueueCoroutine());
            }
        }
    }

    private IEnumerator ProcessAudioQueueCoroutine()
    {
        _isProcessingQueue = true;

        while (_stagesQueue.Count > 0)
        {
            int nextStage = _stagesQueue.Dequeue();
            
            yield return StartCoroutine(PlayGroupCoroutine(_stageVoiceGroups[nextStage]));

            OnAudioFinished?.Invoke(nextStage);
        }

        _isProcessingQueue = false;
    }

    private IEnumerator PlayGroupCoroutine(VoiceGroup group)
    {
        foreach (AudioClip clip in group.clips)
        {
            if (clip == null) continue;

            yield return new WaitForSeconds(_defaultDelay);

            _voiceSource.clip = clip;
            _voiceSource.Play();

            yield return new WaitWhile(() => _voiceSource.isPlaying);
        }
    }

    public void PlayObjecVoice(AudioClip clip)
    {
        _voiceSource.PlayOneShot(clip);
    }

    // public void OnObject()
    // {
    //     Manager_Audio.Instance.PlayObjecVoice(_ojectSpeech);
    // }

    public void PlayDirect(AudioClip clip)
    {
        if (clip == null) return;
        
        _voiceSource.PlayOneShot(clip);
    }
}