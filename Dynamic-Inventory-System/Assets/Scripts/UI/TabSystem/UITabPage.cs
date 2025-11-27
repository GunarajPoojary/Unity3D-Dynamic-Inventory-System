using UnityEngine;

public class UITabPage : MonoBehaviour
{
    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}