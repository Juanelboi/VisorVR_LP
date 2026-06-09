using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float distanceThreshold = 0.1f;
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform seatPoint;   // ← asiento dentro de la guagua
    [SerializeField] private GameObject Minimap;

    private Transform currentWaypoint;
    private float currentSpeed = 5f;
    private Quaternion rotationGoal;
    private Vector3 directionToWaypoint;
    private bool _arrived = false;

    void Start()
    {
        SeatPlayer();

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        currentSpeed = waypoints.GetSpeed(currentWaypoint);
        transform.LookAt(currentWaypoint.position);
    }

    void Update()
    {
        if (_arrived) return;

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            currentSpeed = waypoints.GetSpeed(currentWaypoint);
        }

        RotateTorwardsWaypoints();

        if (waypoints.IsStopPoint(currentWaypoint))
        {
            _arrived = true;
            DropOffPlayer();
        }
    }

    private void SeatPlayer()
    {
        player.transform.SetParent(seatPoint, worldPositionStays: false);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
        player.transform.localScale = Vector3.one;
    }

    private void DropOffPlayer()
    {
        if (Minimap != null) Minimap.SetActive(true);

        player.transform.SetParent(null, worldPositionStays: true);
        player.transform.position = new Vector3(
            currentWaypoint.position.x + 3,
            currentWaypoint.position.y,
            currentWaypoint.position.z
        );
        player.transform.localScale = Vector3.one;


        Destroy(this.gameObject);
    }


    private void RotateTorwardsWaypoints()
    {
        directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationGoal = Quaternion.LookRotation(directionToWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, rotationSpeed * Time.deltaTime);
    }
}