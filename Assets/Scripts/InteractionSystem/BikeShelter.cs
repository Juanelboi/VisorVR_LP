using rayzngames;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class BikeShelter : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    [SerializeField] private string _promptTake = "Coger bici";
    [SerializeField] private string _promptLeave = "Dejar bici";
    [SerializeField] private GameObject _bikePrefab;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float spawnHeight = 3f;

    public string InteractionPrompt => _prompt;
    private string _prompt;

    private void Awake()
    {
        _prompt = _promptTake;
    }

    public void Interact(Interactor interactor)
    {
        if (interactor.GetComponent<XROrigin>() != null)
        {
            TakeBike(interactor);
        }
        else if (interactor.GetComponent<BicycleVehicle>() != null)
        {
            LeaveBike(interactor);
        }
    }

    private void TakeBike(Interactor interactor)
    {
        Vector3 spawnPos = GetSpawnPos(interactor.transform);
        Quaternion spawnRot = GetSpawnRot(interactor.transform);

        GameObject bikeGO = Instantiate(_bikePrefab, spawnPos, spawnRot);

        BikeData bikeData = bikeGO.GetComponent<BikeData>();
        if (bikeData == null)
            bikeData = bikeGO.AddComponent<BikeData>();

        bikeData.player = interactor.gameObject;

        Transform anchor = GetSeatAnchor(bikeGO);

        Transform playerT = interactor.transform;
        playerT.SetParent(anchor, worldPositionStays: false);
        playerT.localPosition = Vector3.zero;
        playerT.localRotation = Quaternion.identity;

        SetWalkingControlsEnabled(interactor, false);

        _prompt = _promptLeave;
    }

    private void LeaveBike(Interactor interactor)
    {
        BikeData bikeData = interactor.GetComponent<BikeData>();

        if (bikeData == null || bikeData.player == null)
        {
            Debug.LogError("BikeData o player es null");
            return;
        }

        GameObject player = bikeData.player;

        Vector3 playerPos = interactor.transform.position;
        Quaternion playerRot = GetSpawnRot(interactor.transform);

        player.transform.SetParent(null, worldPositionStays: true);
        player.transform.SetPositionAndRotation(playerPos, playerRot);
        player.transform.localScale = Vector3.one;   // ← forzar escala 1


        Interactor rigInteractor = player.GetComponentInChildren<Interactor>(true);
        if (rigInteractor != null)
            SetWalkingControlsEnabled(rigInteractor, true);

        Destroy(interactor.gameObject);

        rigInteractor.RefreshInputAction();

        _prompt = _promptTake;
    }

    // ── Helpers ────────────────────────────────────────────────────
    private Transform GetSeatAnchor(GameObject bikeGO)
    {
        Transform anchor = bikeGO.transform.Find("SeatAnchor");
        if (anchor == null)
        {            GameObject anchorGO = new GameObject("SeatAnchor");
            anchor = anchorGO.transform;
            anchor.SetParent(bikeGO.transform, false);
        }
        return anchor;
    }

    private void SetWalkingControlsEnabled(Interactor interactor, bool enabled)
    {
        var bodyTransformer = interactor.GetComponentInChildren<XRBodyTransformer>(true);
        if (bodyTransformer != null)
            bodyTransformer.enabled = enabled;
        else
            Debug.LogWarning("No se encontró XRBodyTransformer en el rig.");
    }

    private Vector3 GetSpawnPos(Transform origin)
    {
        Vector3 flat = origin.forward;
        flat.y = 0f;
        flat.Normalize();
        return origin.position + flat * spawnDistance + Vector3.up * spawnHeight;
    }

    private Quaternion GetSpawnRot(Transform origin)
    {
        Vector3 flat = origin.forward;
        flat.y = 0f;
        return Quaternion.LookRotation(flat);
    }
}