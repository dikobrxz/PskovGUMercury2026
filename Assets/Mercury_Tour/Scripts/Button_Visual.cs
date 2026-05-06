using UnityEngine;
using UnityEngine.UI;

public class Button_Visual : MonoBehaviour
{
    private Image img;
    void Start() => img = GetComponent<Image>();

    public void SetHoverColor(bool isHover)
    {
        img.color = isHover ? Color.gray : Color.white;
    }
}
