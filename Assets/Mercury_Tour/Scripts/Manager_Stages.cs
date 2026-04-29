using UnityEngine;
using System.Collections;

public class Manager_Stages : MonoBehaviour
{
    public static Manager_Stages Instance;
    public delegate void StageChanged();
    public static event StageChanged OnStageChanged;
    public enum Stages {Start, Exploration, CraterQuest, SatelliteFound, PasswordStage, CoordinatesStage, Rescue, End}
    public Stages _currentStage;

    [SerializeField] public GameObject _introContainer;
    [SerializeField] public GameObject _mercuryContainer;
    [SerializeField] public GameObject _quiz;
    //[Header("Script")] private MonoBehaviour _managerPanelScript;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //_mercuryContainer.SetActive(false);
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
}