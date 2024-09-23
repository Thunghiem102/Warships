using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : ShooterController
{
    public float speed = 5f; // Tốc độ di chuyển
    public float rotationSp = 50f; // Tốc độ xoay
    public float lerpSpeed = 0.1f; // Tốc độ nội suy (lerp) khi di chuyển đến vị trí chuột
    private float ZRotation; // Góc xoay hiện tại trên trục Z
    private float ZaxisRotation; // Giá trị xoay trên trục Z
    public float rotationReturnSpeed = 10f; // Tốc độ quay về góc ban đầu
    private Camera mainCamera;
    private Vector3 objectExtents;
    public GameObject gameOverUI;
    [SerializeField] private AudioSource bulletSound;
    public ShootingMode currentShootingMode = ShootingMode.Single;




    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
        objectExtents = GetComponent<Renderer>().bounds.extents;
        
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.Space) && CanShoot())
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.forward * 2);
            Quaternion bulletRotation = transform.rotation;

            switch (currentShootingMode)
            {
                case ShootingMode.Single:
                    ShootSingle(spawnPosition, bulletRotation);
                    break;
                case ShootingMode.Burst:
                    ShootBurst(spawnPosition, bulletRotation);
                    break;
                case ShootingMode.MultipleDirections:
                    ShootMultipleDirections(spawnPosition, numberOfBullets);
                    break;
            }

            PlayShootSound();
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Lấy giá trị nhập từ bàn phím (trục ngang)
        float moveVertical = Input.GetAxis("Vertical"); // Lấy giá trị nhập từ bàn phím (trục dọc)

        if (Input.GetMouseButton(0)) // Nếu nút chuột trái được nhấn
        {
            Vector3 mousePosition = Input.mousePosition; // Lấy vị trí chuột trên màn hình
            mousePosition.z = Camera.main.transform.position.y; // Đặt z của vị trí chuột bằng vị trí y của camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // Chuyển đổi vị trí chuột từ màn hình sang thế giới
            worldPosition.y = transform.position.y; // Giữ nguyên y của đối tượng

            transform.position = Vector3.Lerp(transform.position, worldPosition, lerpSpeed); // Nội suy (lerp) vị trí của đối tượng đến vị trí của chuột
            transform.position = ClampPositionToCameraBounds(transform.position);
            ZaxisRotation = (worldPosition.x - transform.position.x) * rotationSp / 10 * Time.deltaTime; // Tính toán góc xoay trên trục Z dựa trên vị trí chuột
            ZRotation -= ZaxisRotation; // Cập nhật giá trị góc xoay trên trục Z
            ZRotation = Mathf.Clamp(ZRotation, -30f, 30f); // Giới hạn góc xoay trong khoảng -30 đến 30 độ
            transform.localRotation = Quaternion.Euler(0f, 0f, ZRotation); // Cập nhật góc xoay của đối tượng
        }
        else // Nếu nút chuột trái không được nhấn
        {

            Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical); // Tạo vector chuyển động dựa trên giá trị nhập từ bàn phím
            transform.Translate(movement * speed * Time.deltaTime, Space.World); // Di chuyển đối tượng trong không gian thế giới
            transform.position = ClampPositionToCameraBounds(transform.position);
        }

        if (moveHorizontal != 0) // Nếu có nhập từ trục ngang
        {
            ZaxisRotation = Input.GetAxis("Horizontal") * rotationSp * Time.deltaTime; // Tính toán góc xoay trên trục Z
            ZRotation -= ZaxisRotation; // Cập nhật giá trị góc xoay trên trục Z
            ZRotation = Mathf.Clamp(ZRotation, -30f, 30f); // Giới hạn góc xoay trong khoảng -30 đến 30 độ
            transform.localRotation = Quaternion.Euler(0f, 0f, ZRotation); // Cập nhật góc xoay của đối tượng
        }
        else
        {
            // Tự động xoay về góc ban đầu khi không di chuyển
            ZRotation = Mathf.LerpAngle(ZRotation, 0f, rotationReturnSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0f, 0f, ZRotation);
        }
       
    }
    private Vector3 ClampPositionToCameraBounds(Vector3 position)
    {
        Vector3 minScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.y));
        Vector3 maxScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.y));

        position.x = Mathf.Clamp(position.x, minScreenBounds.x + objectExtents.x, maxScreenBounds.x - objectExtents.x);
        position.z = Mathf.Clamp(position.z, minScreenBounds.z + objectExtents.z, maxScreenBounds.z - objectExtents.z);

        return position;
    }
    private void PlayShootSound()
    {
        if (bulletSound != null)
        {
            bulletSound.Play();
        }
    }
    private void OnDestroy()
    {
       Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }

}
