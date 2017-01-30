using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    // Public Variables
    public CombatStats playerStats;
    public float speedMultiplier;

    // Public & Hidden Variables
    
    // Serialized & Private Variables
    [SerializeField] private GameObject hotbar;
    [SerializeField] private Sprite[] hotbarSprites;
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private GameObject aim;

    // Private Reference Variables
    private ButtonFunctionality userInterface;
    private WeaponScript weaponSlot;
    [SerializeField]private WeaponScript[] magicSlots;

    private Rigidbody2D rig;
    private Vector2 movement;
    private SpriteRenderer ren;
    private Animator ani;
    private GameController gc;

    private int currentSelection;

    // Combat bools
    private float slowAmount = 100;

    void Start()
    {
        ani = GetComponent<Animator>();
        userInterface = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        //playerStats.doDamage(50, ElementType.fire);
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        hotbar.transform.GetChild(0).GetComponent<Image>().sprite = hotbarSprites[0];

        GetWeapon();
        for (int i = 1; i < 5; i++)
        {
            GetMagic(i, new Sprite());
        }
    }

    void Update()
    {
        if (userInterface.canPlay)
        {
            // Hotbar
            if(Input.GetButtonDown("X360_LeftButton"))
                SelectHotbar(currentSelection - 1);
            if(Input.GetButtonDown("X360_RightButton"))
                SelectHotbar(currentSelection + 1);
            // Aiming
            float rStickH = Input.GetAxis("X360_RStickX");
            float rStickV = Input.GetAxis("X360_RStickY");

            aim.transform.position = new Vector3(transform.position.x + rStickH, transform.position.y + rStickV, 0f);
            
            // Weapon
            if (weaponSlot != null)
            {
                if (Input.GetAxis("X360_Triggers") < 0)
                    magicSlots[currentSelection].Attack();

                if (Input.GetAxis("X360_Triggers") > 0)
                    weaponSlot.Attack();
            }

            CheckHealth();
        }
        else if (!userInterface.canPlay)
            rig.velocity = Vector3.zero;

        // Slows player
        speedMultiplier = slowAmount / 100;
    }
    
    void FixedUpdate()
    {
        Vector2 oldPos = new Vector2(transform.position.x, transform.position.y); 
        float xPos = 0;
        float yPos = 0;

        if(userInterface.canPlay)
        {
            // Rotation with controller
            float angleRad = Mathf.Atan2(aim.transform.position.y - transform.position.y, aim.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            // Movement
            xPos = Input.GetAxis("X360_LStickX") * moveSpeed.x / 10;
            yPos = Input.GetAxis("X360_LStickY") * moveSpeed.y / 10;
            Vector3 movement = new Vector3(xPos, yPos, 0f);
            rig.velocity = movement * speedMultiplier;
        }

        // Animator
        bool isWalking = true;
        if (Mathf.Abs(xPos) + Mathf.Abs(yPos) > 0)
            isWalking = true;
        else isWalking = false;

        ani.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            ani.SetFloat("X", xPos);
            ani.SetFloat("Y", yPos);

            if (xPos < 0)
                xPos *= -1;
            if (yPos < 0)
                yPos *= -1;

            if (xPos > yPos)
                ani.speed = xPos;
            else ani.speed = yPos * .45f;
        }
    }

    void SelectHotbar(int nextSelection)
    {
        if (nextSelection < 0)
            nextSelection = 3;
        if (nextSelection > 3)
            nextSelection = 0;
        hotbar.transform.GetChild(currentSelection).gameObject.GetComponent<Image>().sprite = hotbarSprites[0];
        currentSelection = nextSelection;
        hotbar.transform.GetChild(currentSelection).gameObject.GetComponent<Image>().sprite = hotbarSprites[1];
    }

    public void GetWeapon()
    {
        weaponSlot = transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>();
    }

    public void GetMagic(int spot, Sprite icon)
    {
        magicSlots[spot - 1] = transform.GetChild(0).GetChild(spot).GetComponent<WeaponScript>();
        // Show icon on hotbar
        if (icon == new Sprite())
            hotbar.transform.GetChild(spot - 1).GetChild(0).gameObject.SetActive(false);
        else
        {
            hotbar.transform.GetChild(spot - 1).GetChild(0).gameObject.SetActive(true);
            hotbar.transform.GetChild(spot - 1).GetChild(0).GetComponent<Image>().sprite = icon;
        }
            
    }

    void CheckHealth()
    {
        if(playerStats.hp <= 0)
        {
            Debug.Log("Player died");
            // death animation?
            // fade to black
            StartCoroutine(gc.RespawnPlayer());
            // reset health
            // respawn
            
        
        }
    }
    
    // Function that slows the player based on the given amount
    public void SlowPlayer(float amount)
    {
        slowAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Drone"))
        {
            col.GetComponent<DroneAI>().reachedPlayer = true;
        }
        if (col.transform.gameObject.tag == "AI") {
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be true)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Drone"))
        {
            col.GetComponent<DroneAI>().reachedPlayer = false;
        }

        if (col.CompareTag("AI")){
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be false)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }
}



