using UnityEngine;

public class UITabGroup : MonoBehaviour
{
    [SerializeField] private UITabButton[] _buttons;
    [SerializeField] private UITabPage[] _pages;

    private int _current = 0;

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].SetIndex(i, this);

        if (_pages.Length == _buttons.Length)
            SelectTab(_current);
    }

    public void SelectTab(int index)
    {
        if (index < 0 || index >= _buttons.Length)
            return;

        _pages[_current].SetVisible(false);
        _buttons[_current].SetState(false);

        _pages[index].SetVisible(true);
        _buttons[index].SetState(true);

        _current = index;
    }
}