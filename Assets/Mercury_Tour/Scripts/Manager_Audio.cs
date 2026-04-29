using System.Collections;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class Manager_Audio : MonoBehaviour
{
    public static Manager_Audio Instance;
    public AudioSource _voiceSource;

    [Header("Настройка пауз")]
    public float _defaultDelay = 2.0f;
    [Header("список фраз по стадиям")]
    public AudioClip[] _stageVoices;
    public AudioClip _ojectSpeech;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(DelayedPlay(_stageVoices[0], 2.0f));
    }

    public void PlayStageVoice()
    {
        int _currentStage = Manager_Stages.Instance.GetIndexCurrentStage();
        if (_currentStage >= 0 && _currentStage < _stageVoices.Length)
        {
            StartCoroutine(DelayedPlay(_stageVoices[_currentStage], _defaultDelay));
        }
    }

    public void PlayObjecVoice(AudioClip clip)
    {
        _voiceSource.PlayOneShot(clip);
    }

    private IEnumerator DelayedPlay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        _voiceSource.clip = clip;
        _voiceSource.Play();
    }

    public void OnObject()
    {
        Manager_Audio.Instance.PlayObjecVoice(_ojectSpeech);
    }
}
