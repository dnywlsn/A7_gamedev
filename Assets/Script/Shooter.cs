using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float cooldown = 0.3f;
    public AudioClip shootClip;
    float nextShot;
    Vector3 lastDir = Vector3.right;
    AudioSource src;

    void Start() { src = GetComponent<AudioSource>(); }

    // вызывается из Player3D.OnMove — запоминаем направление стрельбы
    public void SetDirection(Vector2 m)
    {
        if (m != Vector2.zero)
            lastDir = new Vector3(m.x, 0, m.y).normalized;
    }

    // вызывается из Player3D.OnFire
    public void Shoot()
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;
        Bullet b = Instantiate(bulletPrefab, transform.position + lastDir, Quaternion.identity);
        b.Launch(lastDir);
        if (shootClip && src) src.PlayOneShot(shootClip);
    }
}




