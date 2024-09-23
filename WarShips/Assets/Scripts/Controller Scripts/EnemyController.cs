using UnityEngine;

public class EnemyController : ShooterController
{
    public int experiencePoints = 50;
    public float speed = 2.0f; // Tốc độ di chuyển của enemy
    private BoundaryChecker boundaryChecker;
    public float scoreValue = 10f;
    private ExperienceSystem experienceSystem;


    protected override void Start()
    {
        base.Start();
        experienceSystem = FindObjectOfType<ExperienceSystem>();
        boundaryChecker = gameObject.AddComponent<BoundaryChecker>();
    }
   protected override void Update()
    {
        base.Update(); // Gọi Update() của lớp ShooterController để cập nhật thời gian chờ

        // Di chuyển enemy
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Kiểm tra xem có thể bắn hay không
        if (CanShoot())
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.forward * 2);
            Quaternion bulletRotation = transform.rotation;
            ShootBurst(spawnPosition, bulletRotation); 
        }
    }
    private void OnDestroy()
    {
        AddScore.Instance.Scoring(scoreValue);
        if (experienceSystem != null)
        {
            experienceSystem.AddExperience(experiencePoints);
        }
    }
}
