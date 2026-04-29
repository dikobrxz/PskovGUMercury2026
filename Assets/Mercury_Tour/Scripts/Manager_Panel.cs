using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Manager_Panel : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _display;
    private Color _originalColor;
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
        _originalColor = _background.color;
        UpdateSettingsByStage();
        UpdateButtonsByStage();
    }

    private void UpdateSettingsByStage()
    {
        int _currentStage = Manager_Stages.Instance.GetIndexCurrentStage();
        if (_currentStage != 4 && _currentStage != 5)
        {
            return;
        }
        if (_currentStage == 4)
        {
            ConfigurePanel("427", 3);
        }
        else
        {
            ConfigurePanel((UnityEngine.Random.Range(0, 1000000)).ToString("D6"), 6);
        }
    }

    private void UpdateButtonsByStage()
    {
        int _currentStage = Manager_Stages.Instance.GetIndexCurrentStage();
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
            Debug.Log("Добавлена цифра: " + digit);
        }
    }

    public void ClearLastDigit()
    {
        if (_currentInput.Length > 0)
        {
            _currentInput = _currentInput.Remove(_currentInput.Length - 1);
            UpdateDisplay();
            Debug.Log("Очищена последняя цифра");
        }
    }

    public void SubmitAnswer()
    {
        if (_currentInput == _correctAnswer)
        {
            Flash(Color.green);
            _currentInput = "";
            UpdateDisplay();
            Manager_Stages.Instance.NextStage();
            Debug.Log("Верный код или координаты. " + Manager_Stages.Instance._currentStage);
        }
        else
        {
            Flash(Color.red);
            _currentInput = "";
            UpdateDisplay();
            Debug.Log("Неверный код или координаты");
        }
    }

    public void Flash(Color color)
    {
        StartCoroutine(FlashRoutine(color));
    }

    IEnumerator FlashRoutine(Color color)
    {
        _background.color = color;
        yield return new WaitForSeconds(0.5f);
        _background.color = _originalColor;
    }

    private void UpdateDisplay()
    {
        _display.text = _currentInput;
        Debug.Log("Текущий экран: " + _display.text);
    }
}