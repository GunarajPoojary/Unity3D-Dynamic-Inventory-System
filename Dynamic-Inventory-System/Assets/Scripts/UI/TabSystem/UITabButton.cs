using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _bg;
    [SerializeField] private Color _normal = Color.white;
    [SerializeField] private Color _selected = Color.cyan;

    private UITabGroup _group;
    private int _index;

    public void SetIndex(int index, UITabGroup group)
    {
        _index = index;
        _group = group;
    }

    public virtual void OnPointerClick(PointerEventData e)
    {
        _group.SelectTab(_index);
        SetState(true);
    }

    public void SetState(bool active)
    {
        if (_bg != null)
            _bg.color = active ? _selected : _normal;
    }
}