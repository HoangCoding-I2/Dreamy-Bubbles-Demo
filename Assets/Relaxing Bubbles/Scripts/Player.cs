using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Điều khiển nhân vật chính của game
/// Xử lý di chuyển theo pattern wave, physics bay lên/xuống, và collision
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    
    // Component Rigidbody2D để xử lý physics (bay lên/xuống)
    private Rigidbody2D rb;
    
    // Góc hiện tại trong chu kỳ sine wave (cho di chuyển ngang)
    private float angle = 0;

    // Chiều rộng của map game (được lấy từ DisplayManager)
    private float mapWidth;
    
    // Tốc độ di chuyển ngang (tốc độ thay đổi góc trong sine wave)
    [SerializeField] private float xSpeed;
    
    // Tốc độ di chuyển lên phía trước (tự động)
    [SerializeField] private float ySpeed;
    
    // Tốc độ tối đa khi bay lên (giới hạn velocity.y)
    [SerializeField] private int ySpeedMax;
    
    // Lực đẩy khi nhấn giữ chuột/touch (bay lên)
    [SerializeField] private int yAccelerationForce;
    
    // Lực kéo xuống khi thả chuột/touch (rơi xuống)
    [SerializeField] private int yDecelerationForce;

    // Cờ kiểm tra player đã chết chưa (tránh xử lý collision nhiều lần)
    private bool isDead = false;

    // Prefab hiệu ứng khi chết và thu thập item
    [SerializeField] private GameObject fx_Dead;
    [SerializeField] private GameObject fx_ColorChange;

    // Component AudioSource và các âm thanh
    private AudioSource source;
    [SerializeField] private AudioClip DeadClip;    // Âm thanh khi chết
    [SerializeField] private AudioClip ItemClip;    // Âm thanh khi thu thập item
    
    /// <summary>
    /// Khởi tạo các component và lấy thông tin map
    /// </summary>
    private void Start()
    {
        // Lấy component Rigidbody2D để điều khiển physics
        rb = GetComponent<Rigidbody2D>();
        
        // Lấy component AudioSource để phát âm thanh
        source = GetComponent<AudioSource>();
        
        // Lấy chiều rộng map từ DisplayManager để tính toán di chuyển
        mapWidth = gameManager.GetComponent<DisplayManager>().GetWidth();
    }

    /// <summary>
    /// Xử lý di chuyển player trong mỗi frame
    /// </summary>
    private void Update()
    {
        // Nếu player đã chết thì dừng mọi xử lý
        if (isDead) return;
        
        // Tiếp tục di chuyển player
        MovePlayer();
    }

    /// <summary>
    /// Xử lý chuyển động của player: wave movement + physics bay lên/xuống
    /// </summary>
    private void MovePlayer()
    {
        Vector2 position = transform.position;
        
        // === DI CHUYỂN NGANG THEO PATTERN SINE WAVE ===
        // Sử dụng hàm Cos để tạo chuyển động qua lại mượt mà
        // mapWidth * 0.45f = giới hạn di chuyển trong 90% chiều rộng map
        position.x = Mathf.Cos(angle) * (mapWidth * 0.45f);
        
        // === DI CHUYỂN TIẾN LÊN PHÍA TRƯỚC ===
        // Tự động di chuyển lên với tốc độ cố định
        position.y += ySpeed * Time.deltaTime;
        
        // Áp dụng vị trí mới
        transform.position = position;
        
        // === CẬP NHẬT GÓC CHO SINE WAVE ===
        // Tăng góc theo thời gian để tạo chuyển động liên tục
        angle += Time.deltaTime * xSpeed;

        // === XỬ LÝ INPUT BAY LÊN/XUỐNG ===
        if (Input.GetMouseButton(0))  // Đang nhấn giữ chuột/touch
        {
            // Chỉ bay lên nếu chưa đạt tốc độ tối đa
            if (rb.velocity.y < ySpeedMax)
            {
                // Thêm lực đẩy lên trên
                rb.AddForce(new Vector2(0, yAccelerationForce));
            }
        }
        else  // Đã thả chuột/touch
        {
            // Nếu đang bay lên, áp dụng lực kéo xuống
            if (rb.velocity.y > 0)
            {
                rb.AddForce(new Vector2(0, -yDecelerationForce));
            }
            else
            {
                // Nếu đã rơi xuống, dừng velocity để không rơi quá nhanh
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }

    /// <summary>
    /// Xử lý va chạm với các đối tượng trong game
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // === VA CHẠM VỚI ITEM ĐỔI MÀU ===
        if (other.gameObject.CompareTag("Item_ColorChange"))
        {
            // Tạo hiệu ứng thu thập item tại vị trí item
            GameObject itemFxGameObject = Instantiate(fx_ColorChange, other.gameObject.transform.position, Quaternion.identity); 
            // Tự động xóa hiệu ứng sau 0.5 giây
            Destroy(itemFxGameObject, 0.5f);
            
            // Xóa toàn bộ obstacle chứa item này (parent object)
            Destroy(other.gameObject.transform.parent.gameObject);
            
            // Kích hoạt thay đổi màu nền
            ColorManager.Instance.ChangeBackgroundColor();

            // Tăng điểm số
            ScoreManager.Instance.AddScore();

            // Phát âm thanh thu thập item
            source.PlayOneShot(ItemClip, 1);
        }

        // === VA CHẠM VỚI CHƯỚNG NGẠI VẬT ===
        if (other.gameObject.CompareTag("Obstacle") && isDead == false)
        {
            // Đánh dấu player đã chết (tránh xử lý collision nhiều lần)
            isDead = true;
            
            // Tạo hiệu ứng chết tại vị trí player
            GameObject deadFx = Instantiate(fx_Dead, transform.position, Quaternion.identity); 
            // Tự động xóa hiệu ứng sau 0.5 giây
            Destroy(deadFx, 0.5f);
            
            // Dừng hoàn toàn player
            rb.velocity = new Vector2(0, 0);  // Xóa velocity hiện tại
            rb.isKinematic = true;            // Vô hiệu hóa physics
            
            // Kích hoạt game over
            gameManager.GetComponent<GameManager>().GameOver();
            
            
            // Phát âm thanh chết
            source.PlayOneShot(DeadClip, 1);
        }
    }
}
