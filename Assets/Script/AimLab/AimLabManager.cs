using UnityEngine;
using System.Collections;

public class AimLabManager : MonoBehaviour
{
    public static AimLabManager Instance;

    [Header("하이어라키 씬에 꺼내둔 '실물 과녁 오브젝트'를 드래그해서 넣으세요")]
    public GameObject targetPrefab; 
    public Transform[] spawnPoints; 
    
    private float spawnTime;
    private Coroutine timeoutRoutine; 

    void Awake() => Instance = this;

    void Start() => SpawnNewTarget();

    public void SpawnNewTarget()
    {
        // 1. 기존에 돌고 있던 3초 타이머가 있다면 즉시 가동 중지 (타이머 꼬임 방지)
        if (timeoutRoutine != null) StopCoroutine(timeoutRoutine);

        // 2. 무작위 벽면 위치 선정
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        // 3. 단 하나의 과녁 오브젝트를 새 좌표로 순간이동 시킨 후 강제 활성화
        if (targetPrefab != null)
        {
            targetPrefab.SetActive(false); // 순간이동 중 트리거 오작동 방지용 선 비활성화
            targetPrefab.transform.position = selectedPoint.position;
            targetPrefab.transform.rotation = selectedPoint.rotation;
            targetPrefab.SetActive(true);  // 재가동
        }
        
        spawnTime = Time.time;

        // 4. 새 과녁을 위한 독립 3초 수명 타이머 작동
        timeoutRoutine = StartCoroutine(TargetTimeoutTimer());
    }

    private IEnumerator TargetTimeoutTimer()
    {
        yield return new WaitForSeconds(3f); // 3초 대기
        
        // 3초 동안 못 맞췄다면 비활성화 후 실패 정산
        if (targetPrefab != null) targetPrefab.SetActive(false);
        OnTargetDismissed(false);
    }

    // 과녁이 꺼졌을 때(적중 완료 또는 3초 초과) 정산 처리 플랫폼
    public void OnTargetDismissed(bool isHit)
    {
        // 적중 성공 시에만 코루틴 타이머를 수동 강제 종료
        if (isHit && timeoutRoutine != null) StopCoroutine(timeoutRoutine);

        if (isHit)
        {
            float scoreTime = Time.time - spawnTime;
            Debug.Log($"<color=green>[VR 에임랩]</color> 적중 완료! 반응 속도: {scoreTime:F3}초");
        }
        else
        {
            Debug.Log("<color=red>[VR 에임랩]</color> 3초 시간 초과! 패널티 발생");
        }

        // 즉시 위치를 옮겨서 다음 라운드 가동
        SpawnNewTarget();
    }
}