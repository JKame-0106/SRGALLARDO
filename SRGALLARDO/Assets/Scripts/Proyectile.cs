using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float fSpeed = 12f;
    [SerializeField] private float fLifeTime = 3f;

    [Header("Combate")] //Por ahora esta vaina es generica, luego hay que ligarlo con el tipo de municion...
    [SerializeField] private float fDmage = 10f;

    void Start()
    {
        Destroy(gameObject, fLifeTime);
    }

    void Update()
    {
        Debug.Log("Projectile Update corriendo, Y actual: " + transform.position.y);
        transform.position += Vector3.up * fSpeed * Time.deltaTime;
    }

    //Por ahora ignoren esto, era para el enemigo pero eso lo veo ahora con calma
    /*void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(fDmage);
            Destroy(gameObject);
        }
    }*/
}
