using UnityEngine;

public class GuaguaDoble : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt = "Pille la guagua turista";

    [SerializeField] private GameObject _guaguaTurista;

    public string InteractionPrompt => _interactionPrompt;

    public void Interact(Interactor interactor)
    {
        _guaguaTurista.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
