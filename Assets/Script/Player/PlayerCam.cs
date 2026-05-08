using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [Header("회전 설정")]
public float mouseSensitivity = 100f;
public GameObject player; // 플레이어 몸체 참조

public Transform camHolder;

private float xRotation = 0f; // 상하 회전 누적값

public void PlayerLook(InputAction.CallbackContext context)
{
    if (!context.performed) return;
    
    Vector2 lookInput = context.ReadValue<Vector2>();
    
    // 1. 입력값 계산
    float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
    float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

    // 2. 상하 회전 (카메라 자체 변수)
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    // 3. 플레이어 몸체 회전 (좌우 회전은 플레이어가 주도)
    player.transform.Rotate(Vector3.up * mouseX);

    // 4. 카메라 위치와 회전 업데이트
    // 카메라가 플레이어를 따라다녀야 하므로 위치를 동기화 (필요시 Offset 추가)
    transform.position = camHolder.position;    

    // [중요] 카메라의 회전은: 
    // 자신의 상하 회전(xRotation) + 플레이어의 현재 좌우 회전(player.y)
    transform.rotation = Quaternion.Euler(xRotation, player.transform.eulerAngles.y, 0f);
}
}
