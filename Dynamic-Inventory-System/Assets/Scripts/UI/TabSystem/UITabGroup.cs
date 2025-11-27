using UnityEngine;

public class UITabGroup : MonoBehaviour
{
    [SerializeField] private UITabButton[] _tabButtons;
    [SerializeField] private UITabPage[] _tabPages;

    private int _currentIndex = 0;

    private void Start()
    {
        for (int i = 0; i < _tabButtons.Length; i++)
        {
            _tabButtons[i].SetIndex(i, this);
        }

        // Auto-bind pages if count matches buttons
        if (_tabPages.Length == _tabButtons.Length)
            SelectTab(_currentIndex);
    }

    /// <summary>Select tab by index.</summary>
    public void SelectTab(int index)
    {
        if (index < 0 || index >= _tabButtons.Length)
            return;

        DeselectPreviousTab();

        _tabPages[index].SetVisible(true);
        _tabButtons[index].SetState(true);

        _currentIndex = index;
    }

    private void DeselectPreviousTab()
    {
        _tabPages[_currentIndex].SetVisible(false);
        _tabButtons[_currentIndex].SetState(false);
    }
}
