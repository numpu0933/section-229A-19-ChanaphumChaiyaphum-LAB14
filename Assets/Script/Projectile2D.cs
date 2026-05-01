using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile2D : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bulletPrefab;

    void Update()
    {
        // เช็คก่อนว่ามีระบบเมาส์หรือไม่
        if (Mouse.current == null)
        {
            Debug.Log("หาระบบเมาส์ไม่เจอ! ลืมเปิด Input System Package แน่ๆ");
            return;
        }

        // เมื่อคลิกเมาส์ซ้าย
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("1. ตรวจจับการคลิกเมาส์ได้แล้ว!");

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            // วาดเส้นสีแดงให้ดูในหน้า Scene ตอนยิง
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            // ถ้าเส้น Ray วิ่งไปชน Collider
            if (hit.collider != null)
            {
                Debug.Log("2. ชนฉากหลัง/วัตถุที่มี Collider! ชื่อ: " + hit.collider.name);

                // ย้ายเป้าหมายไปตรงจุดที่คลิก
                target.transform.position = new Vector2(hit.point.x, hit.point.y);

                // คำนวณความเร็ววิถีโค้ง
                Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, hit.point, 1f);

                // สร้างกระสุนและใส่ความเร็ว
                GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                Rigidbody2D shootBullet = newBullet.GetComponent<Rigidbody2D>();
                shootBullet.linearVelocity = projectileVelocity;
            }
            else
            {
                Debug.Log("x คลิกโดนอากาศ (ไม่ได้ใส่ Collider ให้ฉากหลัง หรือลืมติ๊ก Is Trigger)");
            }
        }
    }

    // ฟังก์ชันคำนวณวิถีโค้ง (ห้ามลบ)
    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float velocityX = distance.x / time;
        float velocityY = (distance.y / time) + (0.5f * Mathf.Abs(Physics2D.gravity.y) * time);
        return new Vector2(velocityX, velocityY);
    }
}