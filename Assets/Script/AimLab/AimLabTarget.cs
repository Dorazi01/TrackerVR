using UnityEngine;

public class AimLabTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // 날아온 총알 프리팹은 소멸

            // 1. 내 몸을 임시 비활성화 상태로 전환
            this.gameObject.SetActive(false);

            // 2. 매니저에게 내가 적중당했음을 알림 -> 매니저가 나를 다른 곳으로 순간이동 시킴
            if (AimLabManager.Instance != null)
            {
                AimLabManager.Instance.OnTargetDismissed(true);
            }
        }
    }
}