using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("프리팹 및 위치")]
    public GameObject platePrefab;       // 날릴 접시 프리팹
    public List<Transform> spawnPoints;  // 발사 위치 리스트

    [Header("풀링 설정")]
    public int poolSize = 10;            // 미리 생성해둘 개수
    private Queue<GameObject> platePool = new Queue<GameObject>();

    [Header("발사 설정")]
    public float spawnInterval = 2f;
    public float launchForce = 15f;

    void Awake()
    {
        // 1. 풀 초기화: 미리 생성해서 비활성화 상태로 큐에 보관
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platePrefab);
            obj.SetActive(false);
            platePool.Enqueue(obj);
        }
    }

    void Start()
    {
        // 2. 발사 루틴 시작
        InvokeRepeating(nameof(LaunchFromPool), 1f, spawnInterval);
    }

    void LaunchFromPool()
    {
        if (spawnPoints.Count == 0 || platePool.Count == 0) return;

        // 3. 풀에서 하나 꺼내기 (비활성 상태인 것을 찾음)
        GameObject plate = GetObjectFromPool();

        if (plate != null)
        {
            // 4. 랜덤 위치 선정 및 배치
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform sp = spawnPoints[randomIndex];

            plate.transform.position = sp.position;
            plate.transform.rotation = sp.rotation;
            plate.SetActive(true);

            // 5. 물리 발사
            Rigidbody rb = plate.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                
                // 위쪽으로 살짝 각도를 주어 포물선 유도
                Vector3 launchDir = sp.forward + Vector3.up * 0.5f;
                rb.AddForce(launchDir.normalized * launchForce, ForceMode.Impulse);
            }
        }
    }

    // 풀에서 객체를 꺼내는 메서드
    GameObject GetObjectFromPool()
    {
        // 큐의 맨 앞 객체를 확인
        GameObject obj = platePool.Dequeue();
        
        // 사용을 위해 다시 큐의 맨 뒤로 보냄 (순환 구조)
        platePool.Enqueue(obj);

        // 만약 꺼낸 객체가 이미 활성화 상태라면(아직 날아가고 있다면) 
        // 새로 생성하거나 처리를 해줘야 하지만, 여기서는 순환 구조를 기본으로 합니다.
        return obj;
    }
}