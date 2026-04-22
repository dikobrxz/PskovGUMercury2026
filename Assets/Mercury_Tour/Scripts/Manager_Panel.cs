using UnityEngine;
using TMPro;

public class Manager_Panel : MonoBehaviour
{
    [SerializeField] TextMeshPro _display;
    public static string _correctAnswer;
    private int _maxLength;
    private string _currentInput = "";

    private void OnEnable()
    {
        Manager_Stages.OnStageChanged += UpdateSettingsByStage;
        Manager_Stages.OnStageChanged += UpdateButtonsByStage;
    }

    private void OnDisable()
    {
        Manager_Stages.OnStageChanged -= UpdateSettingsByStage;
        Manager_Stages.OnStageChanged -= UpdateButtonsByStage;
    }

    public void Start()
    {
        UpdateSettingsByStage();
        UpdateButtonsByStage();
    }

    private void UpdateSettingsByStage()
    {
        int stageIndex = Manager_Stages.Instance.GetIndexCurrentStage();

        if (stageIndex == 5)
        {
            ConfigurePanel((UnityEngine.Random.Range(0, 1000000)).ToString("D6"), 6);
        }
        else
        {
            ConfigurePanel("427", 3);
        }
    }

    private void UpdateButtonsByStage()
    {
        int _currentStage = (int)Manager_Stages.Instance._currentStage;
        bool _isActive = (_currentStage == 4 || _currentStage == 5);

        Button_Action[] allButtons = GetComponentsInChildren<Button_Action>();
        foreach(var btn in allButtons)
        {
            btn.ButtonState(_isActive);
        }

    }

    public void ConfigurePanel(string text, int length)
    {
        _correctAnswer = text;
        _maxLength = length;
        Debug.Log("Код или координаты: " + text + "\nДлина:" + length);
    }

    public void AddDigit(int digit)
    {
        if (_currentInput.Length < _maxLength)
        {
            _currentInput += digit.ToString();
            UpdateDisplay();
        }
    }

    public void ClearLastDigit()
    {
        if (_currentInput.Length > 0)
        {
            _currentInput = _currentInput.Remove(_currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    public void SubmitAnswer()
    {
        if (_currentInput == _correctAnswer && (int)Manager_Stages.Instance._currentStage > 3 && (int)Manager_Stages.Instance._currentStage < 6)
        {
            _display.color = Color.green;
            _currentInput = "";
            UpdateDisplay();
            Manager_Stages.Instance.NextStage();
            Debug.Log(Manager_Stages.Instance._currentStage);
        }
        else
        {
            _display.color = Color.red;
            _currentInput = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        _display.color = Color.white;
        _display.text = _currentInput;

    }
}