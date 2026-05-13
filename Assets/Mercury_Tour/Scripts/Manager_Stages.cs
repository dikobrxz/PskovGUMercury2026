using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Manager_Stages : MonoBehaviour
{
    [SerializeField] public GameObject _introContainer;
    [SerializeField] public GameObject _mercuryContainer;
    [SerializeField] public GameObject _quiz;
    [SerializeField] public GameObject _screen;
    [SerializeField] public XRBaseInteractable _button;

    [Header("Триггеры этапов")]
    [SerializeField] private GameObject[] stageTriggers;

    public static Manager_Stages Instance;
    public delegate void StageChanged();
    public static event StageChanged OnStageChanged;
    public enum Stages {Start, Exploration, CraterQuest, SatelliteFound, PasswordStage, CoordinatesStage, Rescue, Quiz, End}
    public Stages _currentStage;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < stageTriggers.Length; i++)
        {
            int index = i;
            var proxy = stageTriggers[i].GetComponent<StageTriggerProxy>();

            proxy.OnPlayerEnter = () => {OnTriggerReached(index);};
        }

        _quiz.SetActive(false);
        _button.enabled = false;
        _screen.SetActive(false);
    }

    public void ChangeStage(int _stageIndex)
    {
        if (_stageIndex < 9)
        {
            _currentStage = (Stages)_stageIndex;

            _introContainer.SetActive(_stageIndex == 0);
            _mercuryContainer.SetActive(_stageIndex >= 1);

            OnStageChanged?.Invoke();
        }
    }

    public void NextStage()
    {
        switch ((int)_currentStage)
        {
            case 0: Player_Teleport.Instance.Teleport(); break;
            case 4: _screen.SetActive(true); break;
            case 5: _button.enabled = true; break;
            case 6: Player_Teleport.Instance.Teleport(); break;
        }

        if ((int)_currentStage < 8)
        {
            _currentStage = (Stages)((int)++_currentStage);
        
            Manager_Audio.Instance.PlayStageVoice();

            OnStageChanged?.Invoke();
        }
    }

    public int GetIndexCurrentStage()
    {
        return (int)_currentStage;
    }

    private void OnTriggerReached(int index)
    {
        NextStage();
        stageTriggers[index].SetActive(false);
        Debug.Log($"Триггер {index} сработал и отключен.");
    }
}