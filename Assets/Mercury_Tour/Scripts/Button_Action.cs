using UnityEngine;

public class Button_Action : MonoBehaviour
{
    [SerializeField] int _digit;
    [SerializeField] bool _isDigit = true;
    /*[SerializeField]*/ private Manager_Panel _panel;

    void Start()
    {
        _panel = GameObject.FindAnyObjectByType<Manager_Panel>();

        if (_panel == null)
        {
            Debug.LogError("Панели нет!");
        }
    }

    public void ButtonState(bool _isActive)
    {
        this.enabled = _isActive;
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().enabled = _isActive;
    }

    public void AddDigit()
    {
        if (_isDigit)
        {
            _panel.AddDigit(_digit);
        }
    }

    public void ClearLastDigit()
    {
        _panel.ClearLastDigit();
    }

    public void SubmitAnswer()
    {
        _panel.SubmitAnswer();
    }
}
