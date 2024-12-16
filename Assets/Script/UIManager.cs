using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject dicussionPanel; // UI Panel for Discussion

    public void ShowDiscussionPanel()
    {
        dicussionPanel.SetActive(true);

        // Optionally, add animations or effects to the panels
        Debug.Log("Tibiscuit and Nana UI displayed.");
    }
    public void HideDiscussionPanel()
    {
        dicussionPanel.SetActive(false);

        // Optionally, add animations or effects to the panels
        Debug.Log("Tibiscuit and Nana UI hidden.");
    }
}