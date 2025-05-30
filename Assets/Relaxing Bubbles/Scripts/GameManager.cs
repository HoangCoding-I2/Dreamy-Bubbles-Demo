using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using TMPro;

/// <summary>
/// Quản lý trạng thái tổng thể của game: khởi động, game over, restart
/// Điều khiển UI và hiệu ứng chuyển cảnh
/// </summary>
public class GameManager : MonoBehaviour
{
    // Các UI Panel được hiển thị khi game over
    [SerializeField] private GameObject _gameOverPanel;        // Panel hiển thị kết quả và nút restart
    [SerializeField] private GameObject _gameOverEffectPanel;  // Panel hiệu ứng khi game over
    [SerializeField] private GameObject _touchToMoveTextObj;   // Text hướng dẫn "Touch to move"
    [SerializeField] private GameObject _StartFadeInObj;       // Panel fade-in khi bắt đầu game
    
    /// <summary>
    /// Thiết lập cấu hình game khi khởi động
    /// </summary>
    void Awake()
    {
        // Giới hạn FPS ở 60 để đảm bảo hiệu suất ổn định trên mọi thiết bị
        Application.targetFrameRate = 60;
        
        // Đặt tốc độ thời gian về bình thường (có thể đã bị thay đổi từ game trước)
        Time.timeScale = 1.0f;
        
        // Bắt đầu hiệu ứng fade-in khi vào game
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Kiểm tra input của người chơi trong mỗi frame
    /// </summary>
    void Update()
    {
        // Chỉ kiểm tra input khi text hướng dẫn đang hiển thị
        if (_touchToMoveTextObj.activeSelf == false) return;
        
        // Khi người chơi nhấn chuột/touch, ẩn text hướng dẫn và bắt đầu game
        if (Input.GetMouseButton(0))
        {
            _touchToMoveTextObj.SetActive(false);
        }
    }

    /// <summary>
    /// Hiệu ứng fade-in mượt mà khi bắt đầu game
    /// Tạo cảm giác chuyển tiếp tự nhiên
    /// </summary>
    IEnumerator FadeIn()
    {
        // Hiển thị panel fade-in (màn hình đen/trắng)
        _StartFadeInObj.SetActive(true);
        
        // Chờ 0.5 giây để người chơi có thời gian chuẩn bị
        yield return new WaitForSeconds(0.5f);
        
        // Ẩn panel fade-in, game chính thức bắt đầu
        _StartFadeInObj.SetActive(false);
    }

    /// <summary>
    /// Được gọi từ Player.cs khi player va chạm với obstacle
    /// Khởi động quá trình game over
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    /// <summary>
    /// Xử lý hiệu ứng game over với slow motion và UI
    /// Tạo cảm giác dramatic khi người chơi thua
    /// </summary>
    private IEnumerator GameOverCoroutine()
    {
        // Hiển thị hiệu ứng game over (có thể là particle, flash effect, etc.)
        _gameOverEffectPanel.SetActive(true);
        
        // Chậm thời gian xuống 10% để tạo hiệu ứng slow motion dramatic
        Time.timeScale = 0.1f;
        
        // Chờ 0.5 giây thời gian thực (không bị ảnh hưởng bởi timeScale)
        yield return new WaitForSecondsRealtime(0.5f);
        
        // Hiển thị panel game over với điểm số và nút restart
        _gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Restart lại game từ đầu
    /// Được gọi khi người chơi nhấn nút Restart trong GameOverPanel
    /// </summary>
    public void Restart()
    {
        // Load lại scene hiện tại để reset toàn bộ game về trạng thái ban đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
