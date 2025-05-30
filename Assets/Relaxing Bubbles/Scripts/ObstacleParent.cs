using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý vòng đời của obstacles
/// Tự động xóa obstacles khi chúng quá xa player để tối ưu hiệu suất
/// Tránh việc tích tụ quá nhiều objects không cần thiết trong scene
/// </summary>
public class ObstacleParent : MonoBehaviour
{
    /// <summary>
    /// Bắt đầu kiểm tra khoảng cách ngay khi obstacle được tạo
    /// </summary>
    void Start()
    {
        // Khởi động coroutine để kiểm tra khoảng cách liên tục
        StartCoroutine(CalculateDistance());
    }
    
    /// <summary>
    /// Coroutine chạy liên tục để kiểm tra khoảng cách giữa obstacle và player
    /// Nếu obstacle quá xa phía sau player, tự động xóa để tiết kiệm memory
    /// </summary>
    /// <returns>IEnumerator để Unity quản lý coroutine</returns>
    private IEnumerator CalculateDistance()
    {
        // Vòng lặp vô hạn để kiểm tra liên tục
        while (true)
        {
            // === KIỂM TRA KHOẢNG CÁCH VỚI PLAYER ===
            // Tìm player trong scene (GameObject có tên "player")
            GameObject playerObject = GameObject.Find("player");
            
            // Nếu player tồn tại, kiểm tra khoảng cách
            if (playerObject != null)
            {
                // Tính khoảng cách theo trục Y (player.Y - obstacle.Y)
                float distanceY = playerObject.transform.position.y - transform.position.y;
                
                // Nếu player đã vượt qua obstacle 60 units thì xóa obstacle
                // 60 units = khoảng cách đủ xa để đảm bảo obstacle không còn ảnh hưởng
                if (distanceY > 60)
                {
                    // Xóa toàn bộ GameObject này (bao gồm tất cả children)
                    Destroy(gameObject);
                    
                    // Thoát khỏi coroutine vì object đã bị xóa
                    yield break;
                }
            }
            
            // Chờ 1 giây trước khi kiểm tra lại
            // Không cần kiểm tra quá thường xuyên để tiết kiệm CPU
            yield return new WaitForSeconds(1.0f);
        }
    }
}