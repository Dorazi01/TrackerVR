using UnityEngine;
using UnityEngine.InputSystem; // 🛑 반드시 상단에 뉴 인풋 시스템 네임스페이스가 있어야 합니다.

public class PlayerShooting : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform GunTip;

    void Update()
    {
        // 1. 현재 연결된 오른손 XR 컨트롤러 장치를 안전하게 가져옵니다.
        var rightHandController = InputSystem.GetDevice<UnityEngine.InputSystem.XR.XRController>(CommonUsages.RightHand);

        // 2. 컨트롤러가 물리적으로 존재하고, 검지 트리거 버튼이 '이번 프레임에 막 눌렸다면' 사격합니다.
        if (rightHandController != null)
        {
            var trigger = rightHandController.GetChildControl<UnityEngine.InputSystem.Controls.ButtonControl>("triggerButton");
            
            if (trigger != null && trigger.wasPressedThisFrame)
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        // 3. 기존 프로토타입의 물리 기반 사격 매커니즘을 그대로 수행합니다.
        if (BulletPrefab != null && GunTip != null)
        {
            GameObject bullet = Instantiate(BulletPrefab, GunTip.position, GunTip.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(GunTip.forward * 8000f);
            }
        }
    }
}