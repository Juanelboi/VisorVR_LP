using System.Collections;
using System.Collections.Generic;
using System.IO;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

public class GeoObjectSpawner : MonoBehaviour
{
    [Header("Configuración de archivos")]
    [Tooltip("Nombre del archivo JSON dentro de StreamingAssets/")]
    public string jsonFileName = "objects_data.json";

    [Tooltip("Subcarpeta dentro de Resources/ donde están los prefabs")]
    public string prefabsFolder = "Prefabs";

    [Header("Cesium")]
    [Tooltip("Arrastra aquí el CesiumGeoreference de la escena")]
    public CesiumGeoreference georeference;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();


    private IEnumerator Start()
    {
        yield return null;

        georeference ??= FindObjectOfType<CesiumGeoreference>();

        if (georeference == null)
        {
            Debug.LogError("[GeoObjectSpawner] No se encontró CesiumGeoreference en la escena.");
            yield break;
        }

        LoadAndSpawn();
    }


    public void LoadAndSpawn()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"[GeoObjectSpawner] Archivo no encontrado: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        ObjectDataList dataList = JsonUtility.FromJson<ObjectDataList>(json);

        if (dataList == null || dataList.objects == null)
        {
            Debug.LogError("[GeoObjectSpawner] El archivo JSON está vacío o tiene formato incorrecto.");
            return;
        }

        Debug.Log($"[GeoObjectSpawner] Cargando {dataList.objects.Count} objetos...");

        foreach (ObjectData data in dataList.objects)
            SpawnObject(data);

        Debug.Log($"[GeoObjectSpawner] {spawnedObjects.Count} objetos colocados.");
    }


    private void SpawnObject(ObjectData data)
    {
        GameObject prefab = Resources.Load<GameObject>($"{prefabsFolder}/{data.prefabName}");
        if (prefab == null)
        {
            Debug.LogWarning($"[GeoObjectSpawner] Prefab '{data.prefabName}' no encontrado " +
                             $"en Resources/{prefabsFolder}/");
            return;
        }

        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        go.name = !string.IsNullOrEmpty(data.nombre) ? data.nombre : data.id;
        go.transform.localScale = data.scale.ToVector3();

        CesiumGlobeAnchor anchor = go.GetComponent<CesiumGlobeAnchor>()
                                ?? go.AddComponent<CesiumGlobeAnchor>();

        anchor.longitudeLatitudeHeight = new double3(
            data.longitude,
            data.latitude,
            data.height
        );

        if (data.localOffset != null)
        {
            Vector3 offset = data.localOffset.ToVector3();
            if (offset != Vector3.zero)
                go.transform.position += go.transform.TransformDirection(offset);
        }

        BusStopController controller = go.GetComponent<BusStopController>()
                                    ?? go.AddComponent<BusStopController>();
        controller.Initialize(data);

        spawnedObjects[data.id] = go;
    }

    public GameObject GetBusStop(string id) =>
        spawnedObjects.TryGetValue(id, out var go) ? go : null;

    public IEnumerable<GameObject> GetAllBusStops() => spawnedObjects.Values;
}
