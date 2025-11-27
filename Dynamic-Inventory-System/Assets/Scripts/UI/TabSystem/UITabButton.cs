using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.cyan;

    private UITabGroup _group;
    private int _index;

    public void SetIndex(int index, UITabGroup group)
    {
        _index = index;
        _group = group;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _group.SelectTab(_index);
        SetState(true);
    }

    public void SetState(bool selected)
    {
        if (_background != null)
            _background.color = selected ? _selectedColor : _normalColor;
    }
}