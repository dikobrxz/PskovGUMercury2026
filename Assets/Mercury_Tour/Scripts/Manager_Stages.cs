using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Android;
using NUnit.Framework.Internal.Commands;

public class Manager_Stages : MonoBehaviour
{
    public static Manager_Stages Instance;
    public delegate void StageChanged();
    public static event StageChanged OnStageChanged;
    public enum Stages {Start, Exploration, CraterQuest, SatelliteFound, PasswordStage, CoordinatesStage, Rescue, End}
    public Stages _currentStage;

    [SerializeField] public GameObject _introContainer;
    [SerializeField] public GameObject _mercuryContainer;
    //[Header("Script")] private MonoBehaviour _managerPanelScript;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeStage(int _stageIndex)
    {
        _currentStage = (Stages)_stageIndex;

        _introContainer.SetActive(_stageIndex == 0);
        _mercuryContainer.SetActive(_stageIndex >= 1);

        OnStageChanged?.Invoke();
    }

    public void NextStage()
    {
        _currentStage = (Stages)((int)++_currentStage);
        
        OnStageChanged?.Invoke();
    }

    public int GetIndexCurrentStage()
    {
        return (int)_currentStage;
    }
}