using UnityEngine;

public class BusStopController : MonoBehaviour
{
    [Header("Datos de la parada")]
    public string stopId;
    public string displayName;
    public double longitude;
    public double latitude;
    public double height;

    public void Initialize(ObjectData data)
    {
        stopId = data.id;
        displayName = !string.IsNullOrEmpty(data.nombre) ? data.nombre : data.id;
        longitude = data.longitude;
        latitude = data.latitude;
        height = data.height;

        if (!string.IsNullOrEmpty(data.tag))
            gameObject.tag = data.tag;

    }

}