using UnityEngine;

public class PlateContoller : MonoBehaviour
{
    float currentLifeTime = 0f;


    // Update is called once per frame
    void FixedUpdate()
    {  
        Vector3 currentEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentEuler.x, 0f, currentEuler.z);

        if (!this.gameObject.activeSelf) return;

        currentLifeTime += Time.fixedDeltaTime;
        if (currentLifeTime > 3f) // 3초 후에 비활성화
        {
            this.gameObject.SetActive(false);
            currentLifeTime = 0f; // 생명주기 초기화
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            this.gameObject.SetActive(false);
        }
    }
}
