using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Điều khiển các hiệu ứng chuyển động của obstacles
/// Bao gồm: thay đổi kích thước, xoay, di chuyển ngang, di chuyển dọc
/// Tạo ra các patterns chuyển động đa dạng và hấp dẫn
/// </summary>
public class ObstacleMovement : MonoBehaviour
{
    // === THIẾT LẬP THAY ĐỔI KÍCH THƯỚC ===
    [SerializeField] private float scaleSizeValue;      // Độ lớn tối đa của hiệu ứng scale
    [SerializeField] private float scaleChangeSpeedX;   // Tốc độ thay đổi scale theo trục X
    [SerializeField] private float scaleChangeSpeedY;   // Tốc độ thay đổi scale theo trục Y

    // === THIẾT LẬP XOAY ===
    [SerializeField] private float rotationSpeed;       // Tốc độ xoay (độ/giây)

    [Space(10)]
    // === THIẾT LẬP DI CHUYỂN NGANG (QUA LẠI) ===
    [SerializeField] private float angleForSideMove = 0;    // Góc hiện tại cho sine wave ngang
    [SerializeField] private float sideMoveDistance;        // Khoảng cách tối đa di chuyển ngang
    [SerializeField] private float sideMoveSpeed;           // Tốc độ di chuyển ngang

    [Space(10)]
    // === THIẾT LẬP DI CHUYỂN DỌC (LÊN XUỐNG) ===
    [SerializeField] private float angleForUpDownMove = 0;  // Góc hiện tại cho sine wave dọc
    [SerializeField] private float upDownMoveDistance;      // Khoảng cách tối đa di chuyển dọc
    [SerializeField] private float upDownMoveSpeed;         // Tốc độ di chuyển dọc
    
    // Góc cho hiệu ứng scale (riêng biệt cho X và Y)
    private float angleX = 0;
    private float angleY = 0;
    
    // Lưu trữ kích thước và vị trí ban đầu để tính toán relative movement
    private float originalScaleX;
    private float originalScaleY;
    private Vector2 originalPosition;

    /// <summary>
    /// Lưu trữ các giá trị ban đầu khi obstacle được tạo
    /// </summary>
    void Start()
    {
        // Lưu kích thước ban đầu để scale relative
        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;
        
        // Lưu vị trí ban đầu để di chuyển relative
        originalPosition = transform.position;
    }

    /// <summary>
    /// Cập nhật tất cả hiệu ứng chuyển động trong mỗi frame
    /// </summary>
    void Update()
    {
        // Kiểm tra và áp dụng hiệu ứng scale nếu được bật
        if (scaleChangeSpeedX != 0 || scaleChangeSpeedY != 0)
        {
            ScaleChange();
        }

        // Kiểm tra và áp dụng hiệu ứng xoay nếu được bật
        if (rotationSpeed != 0)
        {
            AngleChange();
        }

        // Kiểm tra và áp dụng di chuyển ngang nếu được bật
        if (sideMoveSpeed != 0)
        {
            SideToSidePositionChange();
        }

        // Kiểm tra và áp dụng di chuyển dọc nếu được bật
        if (upDownMoveDistance != 0)
        {
            UpDownPositionChange();
        }
    }

    /// <summary>
    /// Tạo hiệu ứng thay đổi kích thước theo pattern sine wave
    /// Obstacle sẽ "thở" hoặc "đập" một cách mượt mà
    /// </summary>
    void ScaleChange()
    {
        // Tính toán scale mới dựa trên sine wave
        // originalScale + sin(angle) * scaleSize = hiệu ứng dao động quanh kích thước gốc
        float newScaleX = originalScaleX + Mathf.Sin(angleX) * scaleSizeValue;
        float newScaleY = originalScaleY + Mathf.Sin(angleY) * scaleSizeValue;
        
        // Áp dụng scale mới
        transform.localScale = new Vector2(newScaleX, newScaleY);
        
        // Cập nhật góc cho frame tiếp theo
        angleX += Time.deltaTime * scaleChangeSpeedX;
        angleY += Time.deltaTime * scaleChangeSpeedY;
    }

    /// <summary>
    /// Tạo hiệu ứng xoay liên tục
    /// Obstacle sẽ quay với tốc độ không đổi
    /// </summary>
    void AngleChange()
    {
        // Xoay quanh trục Z với tốc độ cố định
        // Space.World = xoay theo hệ tọa độ thế giới, không bị ảnh hưởng bởi parent
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
    }

    /// <summary>
    /// Tạo hiệu ứng di chuyển qua lại theo trục X
    /// Obstacle sẽ lắc lư hoặc dao động ngang
    /// </summary>
    void SideToSidePositionChange()
    {
        Vector2 pos = transform.position;
        
        // Tính toán vị trí X mới dựa trên sine wave
        // originalX + sin(angle) * distance = dao động quanh vị trí gốc
        pos.x = originalPosition.x + Mathf.Sin(angleForSideMove) * sideMoveDistance;
        
        // Áp dụng vị trí mới (chỉ thay đổi X, giữ nguyên Y)
        transform.position = pos;

        // Cập nhật góc cho chuyển động mượt mà
        angleForSideMove += Time.deltaTime * sideMoveSpeed;
    }

    /// <summary>
    /// Tạo hiệu ứng di chuyển lên xuống theo trục Y
    /// Obstacle sẽ bay lên bay xuống hoặc nhảy nhót
    /// </summary>
    void UpDownPositionChange()
    {
        Vector2 pos = transform.position;
        
        // Tính toán vị trí Y mới dựa trên sine wave
        // originalY + sin(angle) * distance = dao động lên xuống quanh vị trí gốc
        pos.y = originalPosition.y + Mathf.Sin(angleForUpDownMove) * upDownMoveDistance;
        
        // Áp dụng vị trí mới (chỉ thay đổi Y, giữ nguyên X)
        transform.position = pos;

        // Cập nhật góc cho chuyển động mượt mà
        angleForUpDownMove += Time.deltaTime * upDownMoveSpeed;
    }
}
