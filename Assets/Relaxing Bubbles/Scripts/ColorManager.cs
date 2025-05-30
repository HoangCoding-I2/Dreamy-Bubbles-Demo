using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Quản lý việc thay đổi màu sắc background của game
/// Sử dụng hệ màu HSV để tạo hiệu ứng chuyển màu mượt mà
/// </summary>
public class ColorManager : MonoBehaviour {
    
    // Singleton pattern để có thể truy cập từ bất kỳ đâu trong game
    public static ColorManager Instance;
    
    // Giá trị Hue (sắc thái) trong hệ màu HSV, dao động từ 0 đến 1
    // Hue = 0: Đỏ, Hue = 0.33: Xanh lá, Hue = 0.66: Xanh dương
    private float _hueValue;


    /// <summary>
    /// Khởi tạo Singleton pattern khi GameObject được tạo
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Thiết lập màu nền ban đầu khi game bắt đầu
    /// </summary>
    private void Start()
    {
        // Tạo giá trị Hue ngẫu nhiên từ 0.0 đến 1.0 để màu nền khởi đầu đa dạng
        _hueValue = Random.Range(0, 10) / 10.0f;
        
        // Áp dụng màu nền đầu tiên
        ChangeBackgroundColor();
    }
    
    /// <summary>
    /// Thay đổi màu nền của camera chính
    /// Được gọi khi player thu thập item hoặc cần đổi màu
    /// </summary>
    public void ChangeBackgroundColor()
    {
        // Tăng giá trị Hue lên 0.1 để chuyển sang màu tiếp theo
        // Việc tăng từ từ tạo hiệu ứng chuyển màu mượt mà
        _hueValue += 0.1f;
        
        // Nếu Hue vượt quá 1.0 thì reset về 0 (quay lại chu kỳ màu)
        // Điều này đảm bảo màu sắc luôn trong phạm vi hợp lệ
        if (_hueValue >= 1)
        {
            _hueValue = 0;
        }
        
        // Chuyển đổi từ HSV sang RGB và áp dụng cho camera chính
        // HSV(hueValue, 0.6, 0.8) = Saturation 60%, Value 80% tạo màu sống động nhưng không chói
        Camera.main.backgroundColor = Color.HSVToRGB(_hueValue, 0.6f, 0.8f);
    }
}
