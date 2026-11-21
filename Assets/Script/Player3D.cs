using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour
{
    public float force = 14f;
    public float jumpForce = 6f;
    public Shooter shooter;        // сюда перетащить объект Shooter из Hierarchy

    private Rigidbody rb;
    private int coins;
    private int coinsTotal;
    private Animator gateAnimator;
    private Vector2 move;          // сюда приходит Move с клавиатуры, стика или геймпада

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        Debug.Log("Coins in this level: " + coinsTotal);

        // находим ворота по тегу Exit и берём их Animator
        GameObject gate = GameObject.FindGameObjectWithTag("Exit");
        if (gate != null)
            gateAnimator = gate.GetComponent<Animator>();
    }

    // PlayerInput (Send Messages) сам вызывает эти методы по имени действия
    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        if (shooter != null) shooter.SetDirection(move);
    }

    public void OnJump()
    {
        if (IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void OnFire()
    {
        if (shooter != null) shooter.Shoot();
    }

    bool IsGrounded()
    {
        // луч вниз от центра шарика (радиус 0.5 + небольшой запас)
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    void Update()
    {
        // упал с арены — рестарт
        if (transform.position.y < -10f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(move.x, 0f, move.y) * force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            Debug.Log("Coins: " + coins + " / " + coinsTotal);

            // все монеты собраны — открываем ворота
            if (coins >= coinsTotal && gateAnimator != null)
                gateAnimator.SetBool("IsOpen", true);
        }
        if (other.CompareTag("Exit"))
        {
            if (coins >= coinsTotal)
                Debug.Log("You win!");
            else
                Debug.Log("Collect all coins first! " + coins + " / " + coinsTotal);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Game over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}





