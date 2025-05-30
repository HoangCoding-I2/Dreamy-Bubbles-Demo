using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý kích thước màn hình và ranh giới của game
/// Tính toán các giới hạn hiển thị dựa trên camera và resolution
/// Cung cấp thông tin về kích thước để các object khác sử dụng
/// </summary>
public class DisplayManager : MonoBehaviour
{
    // === THIẾT LẬP KÍCH THƯỚC MAP ===
    // Kích thước tổng thể của map game (virtual units)
    private float mapX = 100.0f;  // Chiều rộng map
    private float mapY = 100.0f;  // Chiều cao map

    // === CÁC GIỚI HẠN HIỂN THỊ ===
    // Ranh giới tối thiểu và tối đa mà camera có thể nhìn thấy
    private float minX;  // Giới hạn trái
    private float maxX;  // Giới hạn phải
    private float minY;  // Giới hạn dưới
    private float maxY;  // Giới hạn trên

    // === CÁC ĐIỂM GÓC CỦA VÙNG HIỂN THỊ ===
    // 4 góc của vùng game có thể chơi được
    private Vector2 LEFT_UP;     // Góc trái-trên
    private Vector2 LEFT_DOWN;   // Góc trái-dưới
    private Vector2 RIGHT_UP;    // Góc phải-trên
    private Vector2 RIGHT_DOWN;  // Góc phải-dưới

    // === KÍCH THƯỚC VÙNG CHƠI ===
    private float WIDTH;   // Chiều rộng vùng chơi
    private float HEIGHT;  // Chiều cao vùng chơi

    // === RANH GIỚI CỤ THỂ ===
    private float LEFT;    // Tọa độ X bên trái
    private float RIGHT;   // Tọa độ X bên phải
    private float DOWN;    // Tọa độ Y phía dưới
    private float UP;      // Tọa độ Y phía trên

    /// <summary>
    /// Khởi tạo hệ thống ranh giới ngay khi object được tạo
    /// </summary>
    void Awake()
    {
        InitDisplayBound();
    }

    /// <summary>
    /// Tính toán tất cả ranh giới hiển thị dựa trên camera và screen resolution
    /// Hàm này đảm bảo game hoạt động đúng trên mọi tỷ lệ màn hình
    /// </summary>
    void InitDisplayBound()
    {
        // === TÍNH TOÁN KÍCH THƯỚC CAMERA VIEWPORT ===
        // orthographicSize = nửa chiều cao mà camera có thể nhìn thấy
        float vertExtent = Camera.main.orthographicSize;
        
        // Tính nửa chiều rộng dựa trên tỷ lệ aspect ratio của màn hình
        // vertExtent * (width/height) = kích thước viewport theo trục X
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // === TÍNH TOÁN RANH GIỚI CÓ THỂ NHÌN THẤY ===
        // Ranh giới này tính từ tâm map (0,0) đến các cạnh camera có thể thấy
        minX = horzExtent - mapX / 2.0f;  // Cạnh trái: từ tâm trừ đi nửa map và nửa viewport
        maxX = mapX / 2.0f - horzExtent;  // Cạnh phải: từ nửa map trừ đi nửa viewport
        minY = vertExtent - mapY / 2.0f;  // Cạnh dưới: tương tự cho trục Y
        maxY = mapY / 2.0f - vertExtent;  // Cạnh trên: tương tự cho trục Y

        // === TÍNH TOÁN CÁC ĐIỂM GÓC CỦA VÙNG CHƠI ===
        // Thêm padding 50 units từ các cạnh để tạo safe zone
        LEFT_UP = new Vector2(maxX - 50, minY + 50);       // Góc trái-trên
        LEFT_DOWN = new Vector2(maxX - 50, maxY - 50);     // Góc trái-dưới
        RIGHT_UP = new Vector2(minX + 50, minY + 50);      // Góc phải-trên
        RIGHT_DOWN = new Vector2(minX + 50, maxY - 50);    // Góc phải-dưới

        // === TÍNH TOÁN KÍCH THƯỚC VÙNG CHƠI THỰC TẾ ===
        WIDTH = RIGHT_DOWN.x - LEFT_DOWN.x;  // Chiều rộng = phải - trái
        HEIGHT = LEFT_UP.y - LEFT_DOWN.y;    // Chiều cao = trên - dưới

        // === LƯU CÁC RANH GIỚI ĐỂ SỬ DỤNG SAU ===
        LEFT = LEFT_UP.x;     // Ranh giới trái
        RIGHT = RIGHT_UP.x;   // Ranh giới phải
        UP = LEFT_UP.y;       // Ranh giới trên
        DOWN = LEFT_DOWN.y;   // Ranh giới dưới
    }
    
    /// <summary>
    /// Trả về chiều rộng vùng chơi
    /// Được sử dụng bởi Player.cs để tính toán di chuyển wave
    /// </summary>
    /// <returns>Chiều rộng vùng chơi tính bằng Unity units</returns>
    public float GetWidth()
    {
        return WIDTH;
    }
}