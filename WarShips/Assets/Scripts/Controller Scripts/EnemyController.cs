using UnityEngine;

public class EnemyController : ShooterController
{
    public int experiencePoints = 50;
    private BoundaryChecker boundaryChecker;
    public float scoreValue = 10f;
    private ExperienceSystem experienceSystem;

    public Transform[] waypoints; // Các điểm trên quỹ đạo
    public float moveSpeed = 2f;
    private int currentWaypointIndex = 0;


    protected override void Start()
    {
        base.Start();
        experienceSystem = FindObjectOfType<ExperienceSystem>();
        boundaryChecker = gameObject.AddComponent<BoundaryChecker>();
    }
   protected override void Update()
    {
        base.Update(); // Gọi Update() của lớp ShooterController để cập nhật thời gian chờ


        // Kiểm tra xem có thể bắn hay không
        if (CanShoot())
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.forward * 2);
            Quaternion bulletRotation = transform.rotation;
            ShootBurst(spawnPosition, bulletRotation); 
        }

        // Nếu còn waypoint để đến
        if (currentWaypointIndex < waypoints.Length)
        {

            Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
            direction.Normalize(); // Chuẩn hóa vector thành độ dài 1


            // Di chuyển enemy về phía waypoint hiện tại
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

            // Xoay enemy về hướng di chuyển
            RotateTowardsDirection(direction);


            // Nếu đã đến gần waypoint hiện tại, chuyển sang waypoint tiếp theo
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
    }

    void RotateTowardsDirection(Vector3 direction)
    {
         // Tính toán góc quay và xoay đối tượng
         Quaternion rotation = Quaternion.LookRotation(direction);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed);
        
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }
    private void OnDisable()
    {
        AddScore.Instance.Scoring(scoreValue);
        if (experienceSystem != null)
        {
            experienceSystem.AddExperience(experiencePoints);
        }
    }
}
