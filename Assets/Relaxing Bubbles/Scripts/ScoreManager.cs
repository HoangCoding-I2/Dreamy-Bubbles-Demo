using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Quản lý hệ thống điểm số của game
/// Xử lý tính điểm, hiển thị điểm hiện tại, và lưu trữ điểm cao nhất
/// </summary>
public class ScoreManager : MonoBehaviour {
    
    // Singleton pattern để truy cập từ bất kỳ đâu trong game
    public static ScoreManager Instance;
    
    // Điểm số hiện tại của người chơi trong game session này
    private int _score = 0;
    
    // UI Text để hiển thị điểm số hiện tại trên màn hình
    [SerializeField] private Text _currentScoreText;
    
    // UI Text để hiển thị điểm số cao nhất đã đạt được
    [SerializeField] private Text _bestScoreText;

    /// <summary>
    /// Khởi tạo Singleton và thiết lập UI điểm số ban đầu
    /// </summary>
    private void Awake()
    {
        // Thiết lập Singleton pattern
        Instance = this;
        
        // Hiển thị điểm số bắt đầu là 0
        _currentScoreText.text = "0";
        
        // Lấy và hiển thị điểm cao nhất từ PlayerPrefs (lưu trữ local)
        // Nếu chưa có điểm nào được lưu, mặc định là 0
        _bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    
    /// <summary>
    /// Tăng điểm số khi người chơi thu thập item
    /// Được gọi từ Player.cs khi va chạm với Item_ColorChange
    /// </summary>
    public void AddScore()
    {
        // Tăng điểm số hiện tại lên 1
        _score++;
        
        // Cập nhật hiển thị điểm số trên UI
        _currentScoreText.text = _score.ToString();

        // Kiểm tra xem có phá vỡ kỷ lục không
        if (_score > PlayerPrefs.GetInt("BestScore", 0))
        {
            // Lưu điểm cao nhất mới vào PlayerPrefs (persistent storage)
            PlayerPrefs.SetInt("BestScore", _score);
            
            // Cập nhật hiển thị điểm cao nhất trên UI
            _bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        }
    }
    
    /// <summary>
    /// Trả về điểm số hiện tại
    /// Được sử dụng bởi ObstacleManager để điều chỉnh độ khó
    /// </summary>
    /// <returns>Điểm số hiện tại của người chơi</returns>
    public int GetScore()
    {
        return _score;
    }
}
