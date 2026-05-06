using UnityEngine;
using TMPro;

public class Manager_UI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _stage;
    [SerializeField] private TextMeshProUGUI _coordinates;

    private void OnEnable()
    {
        Manager_Stages.OnStageChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Manager_Stages.OnStageChanged -= UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int _currentStage = (int)Manager_Stages.Instance._currentStage;
        _stage.text = "Этап: " + (Manager_Stages.Stages)_currentStage;

        if (_currentStage == 5)
        {
            _coordinates.text += "Координаты: " + Manager_Panel._correctAnswer;
        }

        if (_currentStage == 6)
        {
            _coordinates.text = "";
        }
    }
}
