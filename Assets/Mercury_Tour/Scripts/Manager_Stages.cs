using UnityEngine;
using System.Collections;
using System.Net;

public class Manager_Stages : MonoBehaviour
{
    [SerializeField] public GameObject _introContainer;
    [SerializeField] public GameObject _mercuryContainer;
    [SerializeField] public GameObject _quiz;

    [Header("Триггеры этапов")]
    [SerializeField] private GameObject[] stageTriggers;

    public static Manager_Stages Instance;
    public delegate void StageChanged();
    public static event StageChanged OnStageChanged;
    public enum Stages {Start, Exploration, CraterQuest, SatelliteFound, PasswordStage, CoordinatesStage, Rescue, End}
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
    }

    public void ChangeStage(int _stageIndex)
    {
        if (_stageIndex < 8)
        {
            _currentStage = (Stages)_stageIndex;

            _introContainer.SetActive(_stageIndex == 0);
            _mercuryContainer.SetActive(_stageIndex >= 1);

            OnStageChanged?.Invoke();
        }
    }

    public void NextStage()
    {
        if ((int)_currentStage == 0)
        {
            //_mercuryContainer.SetActive(true);
            Player_Teleport.Instance.Teleport();
            //_introContainer.SetActive(false);
        }

        if ((int)_currentStage == 6)
        {
            //_introContainer.SetActive(true);
            Player_Teleport.Instance.Teleport();
            //_mercuryContainer.SetActive(false);
        }

        if ((int)_currentStage < 7)
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