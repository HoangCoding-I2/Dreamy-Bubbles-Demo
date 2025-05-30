using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Điều khiển camera theo dõi player một cách mượt mà
/// Tạo hiệu ứng camera smooth follow với độ trễ tự nhiên
/// Camera chỉ theo trục Y, giữ nguyên X để không bị xoay theo player
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    // Reference đến player object để lấy vị trí
    [SerializeField] private GameObject _player;
    
    // Thời gian để camera catch up với target position (càng nhỏ càng nhanh)
    [SerializeField] private float _smoothTime = 0.3F;
    
    // Offset theo trục Y so với player (camera sẽ cao hơn player bao nhiêu)
    [SerializeField] private int _yOffset;
    
    // Vector3 để lưu trữ velocity hiện tại cho smooth damping
    // Được sử dụng nội bộ bởi Unity's SmoothDamp function
    private Vector3 _velocity;

    /// <summary>
    /// Cập nhật vị trí camera theo player trong mỗi frame
    /// Sử dụng smooth damping để tạo chuyển động tự nhiên
    /// </summary>
    void Update()
    {
        // === TÍNH TOÁN VỊ TRÍ TARGET CHO CAMERA ===
        // TransformPoint: chuyển đổi tọa độ local sang world coordinates
        // Vector3(0, yOffset, -10): 
        //   - X = 0: camera luôn ở giữa, không theo X của player
        //   - Y = yOffset: camera cao hơn player một khoảng cố định
        //   - Z = -10: camera ở phía sau để có thể nhìn thấy (orthographic camera)
        Vector3 targetPosition = _player.transform.TransformPoint(new Vector3(0, _yOffset, -10));
        
        // Đảm bảo camera chỉ di chuyển theo trục Y, X luôn = 0
        // Điều này tạo ra hiệu ứng camera cố định về mặt ngang, chỉ theo dọc
        targetPosition = new Vector3(0, targetPosition.y, targetPosition.z);

        // === ÁP DỤNG SMOOTH MOVEMENT ===
        // SmoothDamp tạo chuyển động mượt mà từ vị trí hiện tại đến target
        // - transform.position: vị trí hiện tại của camera
        // - targetPosition: vị trí muốn đến
        // - velocity: được Unity tự động cập nhật để tính toán smooth transition
        // - smoothTime: thời gian để đạt 63% khoảng cách đến target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }
}