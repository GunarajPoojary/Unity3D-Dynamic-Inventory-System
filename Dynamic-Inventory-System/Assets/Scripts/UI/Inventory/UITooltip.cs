using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITooltip : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private RectTransform backgroundRect;

    private Canvas _parentCanvas;

    private void Awake()
    {
        if (root != null)
            root.SetActive(false);

        _parentCanvas = GetComponentInParent<Canvas>();
    }

    public void Show(string title, string description, RectTransform sourceRect = null)
    {
        if (root == null) return;

        root.SetActive(true);

        if (titleText != null) titleText.text = title;
        if (descriptionText != null) descriptionText.text = description;

        // Basic positioning: place near sourceRect if available
        if (sourceRect != null)
        {
            Vector3[] corners = new Vector3[4];
            sourceRect.GetWorldCorners(corners);
            Vector3 topRight = corners[2];
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform, 
                topRight, 
                _parentCanvas.worldCamera, 
                out anchoredPos);
            (root.transform as RectTransform).anchoredPosition = anchoredPos + new Vector2(8f, 8f);
        }
    }

    public void Hide()
    {
        if (root == null) return;
        root.SetActive(false);
    }
}