using UnityEngine;

public class BookTutorial : InteractableBase
{
    [Header("UI del Tutorial")]
    [Tooltip("Arrastrá acá el Panel del Canvas que contiene el texto del tutorial")]
    public GameObject tutorialPanel;

    private bool isPanelOpen = false;

    protected override void Start()
    {
        base.Start();
        tutorialPanel.SetActive(false);
    }

    public override void OnPressE(PlayerInteraction player)
    {
      
        if (tutorialPanel == null)
        {
           
            return; 
        }

        isPanelOpen = !isPanelOpen;
        tutorialPanel.SetActive(isPanelOpen);
    }
}