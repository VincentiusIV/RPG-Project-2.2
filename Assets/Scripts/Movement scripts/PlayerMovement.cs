using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Author: Vincent Versnel
// Script for the player to interact with the player character
public class PlayerMovement : MonoBehaviour
{
    // Public Variables
    public CombatStats playerStats;
    public float speedMultiplier;

    // Public & Hidden Variables
    public GameObject drone;

    // Serialized & Private Variables
    [SerializeField] private GameObject hotbar;
    [SerializeField] private Sprite[] hotbarSprites;
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject gunEnd;

    // Private Reference Variables
    private ButtonFunctionality userInterface;

    private MeleeScript meleeWeapon;
    [SerializeField]private WeaponScript weaponSlot;

    private Rigidbody2D rig;
    private Vector2 movement;
    private HandRotation hand;
    private Animator ani;
    private GameController gc;

    private int currentSelection;

    private bool isSlowRunning;
    IEnumerator slow;

    // Combat bools
    private float slowAmount = 100;

    void Start()
    {
        ani = GetComponent<Animator>();
        userInterface = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        //playerStats.doDamage(50, ElementType.fire);
        rig = GetComponent<Rigidbody2D>();
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        hotbar.transform.GetChild(0).GetComponent<Image>().sprite = hotbarSprites[0];

        hand = transform.GetChild(0).GetComponent<HandRotation>();
        meleeWeapon = transform.GetChild(0).FindChild("MeleeWeapon").GetComponent<MeleeScript>();
        //GetWeapon();
        aim.GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 1; i < 5; i++)
        {
            GetMagic(i, new Sprite());
        }

        slow = SlowDuration();
    }

    void Update()
    {
        if (userInterface.canPlay)
        {
            // Hotbar
            if(Input.GetButtonDown("LB_1"))
                SelectHotbar(currentSelection - 1);
            if(Input.GetButtonDown("RB_1"))
                SelectHotbar(currentSelection + 1);
            
            // ranged combo attack
            if (weaponSlot != null)
            {
                //Input.GetAxis("X360_Triggers") > 0
                if (Input.GetAxisRaw("TriggersL_1") != 0)
                {
                    //aim
                    float lStickH = Input.GetAxis("L_XAxis_1");
                    float lStickV = Input.GetAxis("L_YAxis_1");

                    ani.enabled = false;
                    hand.isAiming = true;
                    aim.GetComponent<SpriteRenderer>().enabled = true;
                    aim.transform.position = new Vector3(transform.position.x + lStickH, transform.position.y + lStickV, 0f);
                    
                    // fixing player in position to aim
                    slowAmount = 0;

                    // fire selected ammunition
                    if (Input.GetAxisRaw("TriggersR_1") != 0)
                        weaponSlot.RangedAttack(currentSelection);
                }
                else if (Input.GetAxis("TriggersR_1") == 0)
                {
                    aim.GetComponent<SpriteRenderer>().enabled = false;
                    slowAmount = 100;
                    ani.enabled = true;
                    hand.isAiming = false;
                }
                    
            }
            // melee attack
            if (Input.GetButtonDown("X_1"))
            {
                if(meleeWeapon.MeleeAttack())
                {
                    ani.SetBool("isAttacking", true);
                }
            }
                

            CheckHealth();

            speedMultiplier = slowAmount / 100;
        }
        else if (!userInterface.canPlay)
            rig.velocity = Vector3.zero;
    }
    
    void FixedUpdate()
    {
        Vector2 oldPos = new Vector2(transform.position.x, transform.position.y); 
        float xPos = 0;
        float yPos = 0;

        if(userInterface.canPlay)
        {
            // Movement
            xPos = Input.GetAxis("L_XAxis_1") * moveSpeed.x / 10;
            yPos = Input.GetAxis("L_YAxis_1") * moveSpeed.y / 10;
            Vector3 movement = new Vector3(xPos, yPos, 0f);
            rig.velocity = movement * speedMultiplier;
        }

        // Animator
        bool isWalking = true;
        if (Mathf.Abs(xPos) + Mathf.Abs(yPos) > 0)
            isWalking = true;
        else isWalking = false;

        ani.SetBool("isWalking", isWalking);
        if (isWalking && !ani.GetBool("isAttacking"))
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

    // Selects the hotbar counter based on left or right controller button
    void SelectHotbar(int nextSelection)
    {
        if (nextSelection < 0)
            nextSelection = 3;
        if (nextSelection > 3)
            nextSelection = 0;
        hotbar.transform.GetChild(currentSelection).gameObject.GetComponent<Image>().sprite = hotbarSprites[0];
        currentSelection = nextSelection;
        hotbar.transform.GetChild(currentSelection).gameObject.GetComponent<Image>().sprite = hotbarSprites[1];
        GetComponent<AudioSource>().Play();
    }

    // sets the image for an equipped magic icon
    public void GetMagic(int spot, Sprite icon)
    {
        // Show icon on hotbar
        if (icon == new Sprite())
            hotbar.transform.GetChild(spot - 1).GetChild(0).gameObject.SetActive(false);
        else
        {
            hotbar.transform.GetChild(spot - 1).GetChild(0).gameObject.SetActive(true);
            hotbar.transform.GetChild(spot - 1).GetChild(0).GetComponent<Image>().sprite = icon;
        }
    }
    // checks the health of the player
    void CheckHealth()
    {
        if(playerStats.hp <= 0)
        {
            Debug.Log("Player died");
            // death animation?
            StartCoroutine(gc.RespawnPlayer());
        }
    }
    
    // Function that slows the player based on the given amount
    public void SlowPlayer(float amount, bool resetAfterTime = false)
    {
        slowAmount = amount;

        if (isSlowRunning && resetAfterTime)
        {
            StopCoroutine(slow);
            slow = SlowDuration();
            StartCoroutine(slow);
        }
        else if (!isSlowRunning && resetAfterTime)
            StartCoroutine(slow);
    }

    IEnumerator SlowDuration()
    {
        isSlowRunning = true;
        yield return new WaitForSeconds(3f);
        slowAmount = 100;
        isSlowRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Drone"))
        {
            col.GetComponent<DroneAI>().reachedPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Drone"))
        {
            col.GetComponent<DroneAI>().reachedPlayer = false;
        }
    }
    // called on the last frame of the attacking animation
    public void EndAttackAnimation()
    {
        ani.SetBool("isAttacking", false);
    }
}



