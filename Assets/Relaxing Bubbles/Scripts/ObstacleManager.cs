using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý việc tạo ra các chướng ngại vật trong game
/// Tự động sinh obstacles dựa trên vị trí player và điều chỉnh độ khó theo điểm số
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    // Reference đến player để theo dõi vị trí và tự động tạo obstacles
    [SerializeField] private GameObject player;
    
    // Mảng chứa các prefab obstacles dễ (điểm < 20)
    [SerializeField] private GameObject[] easyObstacleArray;
    
    // Mảng chứa các prefab obstacles khó (điểm >= 20)
    [SerializeField] private GameObject[] normalObstacleArray;
    
    // Chỉ số obstacle tiếp theo sẽ được tạo (để tính toán vị trí spawn)
    private int obstacleIndex = 0;
    
    // Khoảng cách giữa các obstacles (tính bằng Unity units)
    private int distanceToNextObstacle = 50;

    // Vị trí Y cuối cùng của player (dùng để phát hiện khi cần tạo obstacle mới)
    private int playerPositionIndex = -1;

    /// <summary>
    /// Tạo obstacle đầu tiên khi game bắt đầu
    /// </summary>
    private void Start()
    {
        InstantiateObstacle();
    }

    /// <summary>
    /// Kiểm tra vị trí player và tạo obstacles mới khi cần thiết
    /// </summary>
    private void Update()
    {
        // Kiểm tra xem player đã di chuyển đủ xa chưa (mỗi 25 units)
        // Khi playerPositionIndex thay đổi = player đã đi đủ xa để cần obstacle mới
        if (playerPositionIndex != (int)player.transform.position.y / 25)
        {
            // Tạo obstacle mới
            InstantiateObstacle();
            
            // Cập nhật vị trí cuối cùng đã xử lý
            playerPositionIndex = (int)player.transform.position.y / 25;
        }
    }

    /// <summary>
    /// Tạo ra một obstacle mới dựa trên điểm số hiện tại
    /// Điểm thấp = obstacles dễ, điểm cao = obstacles khó
    /// </summary>
    private void InstantiateObstacle()
    {
        // Lấy điểm số hiện tại từ ScoreManager
        int score = ScoreManager.Instance.GetScore();
        
        // Chọn loại obstacle dựa trên điểm số
        if (score < 20)
        {
            // === CHẾ ĐỘ DỄ (ĐIỂM < 20) ===
            // Chọn ngẫu nhiên từ mảng obstacles dễ
            int randomObstacle = Random.Range(0, easyObstacleArray.Length);
            
            // Tính vị trí spawn: Y = obstacleIndex * khoảng cách
            Vector3 spawnPosition = new Vector3(0, obstacleIndex * distanceToNextObstacle);
            
            // Tạo obstacle và đặt làm con của ObstacleManager (để dễ quản lý)
            GameObject newObstacle = Instantiate(easyObstacleArray[randomObstacle], spawnPosition, Quaternion.identity);
            newObstacle.transform.SetParent(transform);
        }
        else
        {
            // === CHẾ ĐỘ KHÓ (ĐIỂM >= 20) ===
            // Chọn ngẫu nhiên từ mảng obstacles khó
            int randomObstacle = Random.Range(0, normalObstacleArray.Length);
            
            // Tính vị trí spawn tương tự như trên
            Vector3 spawnPosition = new Vector3(0, obstacleIndex * distanceToNextObstacle);
            
            // Tạo obstacle khó hơn
            GameObject newObstacle = Instantiate(normalObstacleArray[randomObstacle], spawnPosition, Quaternion.identity);
            newObstacle.transform.SetParent(transform);
        }

        // Tăng chỉ số để obstacle tiếp theo spawn xa hơn
        obstacleIndex++;
    }
}