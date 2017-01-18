using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    // Public Variables
    public PlayerStats playerStats;
    public float speedMultiplier;

    // Public & Hidden Variables
    [HideInInspector]public bool canPlay;
    
    // Serialized & Private Variables
    [SerializeField] private GameObject hotbar;
    [SerializeField] private Sprite[] hotbarSprites;
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private GameObject aim;
    [SerializeField] private bool useController = true;

    // Private Reference Variables
    private WeaponScript weaponSlot;
    [SerializeField]private WeaponScript[] magicSlots;

    private Rigidbody2D rig;
    private Vector2 movement;
    private SpriteRenderer ren;

    private int currentSelection;
    private bool buttonInUse;

    void Start()
    {
        //playerStats.doDamage(50, ElementType.fire);
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        hotbar.transform.GetChild(0).GetComponent<Image>().sprite = hotbarSprites[0];

        GetWeapon();
        for (int i = 1; i < 5; i++)
        {
            GetMagic(i, new Sprite());
        }
    }

    void Update()
    {
        if (canPlay && useController)
        {
            // Hotbar
            if (Input.GetAxisRaw("X360_DpadX") == 0)
                buttonInUse = false;
           /* if (Input.GetAxisRaw("X360_DpadX") < 0 && buttonInUse == false)
            {
                buttonInUse = true;
                SelectHotbar(currentSelection - 1);
            }
                
            if (Input.GetAxisRaw("X360_DpadX") > 0 && buttonInUse == false)
            {
                buttonInUse = true;
                SelectHotbar(currentSelection + 1);
            }*/

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
        }
        if (canPlay && !useController){
            // Hotbar
            if (Input.GetKeyDown(KeyCode.Z)){
                SelectHotbar(currentSelection - 1);
            }
            if (Input.GetKeyDown(KeyCode.X)){
                SelectHotbar(currentSelection + 1);
            }
            // Weapon
            if (weaponSlot != null){
                if (Input.GetMouseButtonDown(0))
                    magicSlots[currentSelection].Attack();

                if (Input.GetMouseButtonDown(1))
                    weaponSlot.Attack();
            }
        }
        // move hp to player stats at some point
        //HP
        ren.color = Color.Lerp(Color.red, Color.green, playerStats.hp / playerStats.maxHP);

        /*if (playerStats.hp < 100 && playerStats.hp > 0)
        {
            playerStats.hp += 1 / 60f;
        }*/
        if (playerStats.hp <= 0){
            //Respawn?? End Game?? Lifes??
        }
    }
    
    void FixedUpdate(){
        if(canPlay && useController){
            // Rotation with controller
            float angleRad = Mathf.Atan2(aim.transform.position.y - transform.position.y, aim.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            // Movement
            float xPos = Input.GetAxis("X360_LStickX") * moveSpeed.x;
            float yPos = Input.GetAxis("X360_LStickY") * moveSpeed.y;
            Vector3 movement = new Vector3(xPos, yPos, 0f);
            rig.velocity = movement * speedMultiplier;
        }
        if (canPlay && !useController){
            // rotation with mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            // Movement
            float xPos = Input.GetAxis("Horizontal") * moveSpeed.x;
            float yPos = Input.GetAxis("Vertical") * moveSpeed.y;
            Vector3 movement = new Vector3(xPos, yPos, 0f).normalized;
            rig.velocity = movement * speedMultiplier;
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
    
    // Function that slows the player based on the given amount & duration
    public void SlowPlayer(float amount, float slowDuration)
    {
        Debug.Log("Slowing player for " + amount + "%");
        speedMultiplier = amount /= 100;
        StartCoroutine(Slow(slowDuration));
    }

    IEnumerator Slow(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.transform.parent != null && coll.transform.parent.name == "MeleeEnemy(Clone)"){
            playerStats.hp -= coll.transform.parent.GetComponent<EnemyScript>().dmg;
        }
        if (coll.transform.name == "Bullet(Clone)"){
            //currentHP -= coll.gameObject.GetComponent<BulletScript>().enemyScript.dmg;
        }
        if (coll.gameObject.tag == "Teleporter"){
            //reload lvl and move player to opposite side of the lvl
            GeneratingDungeon gameManagerScript = FindObjectOfType<GeneratingDungeon>();
            gameManagerScript.DestroyLevel();
            gameManagerScript.GenerateLevel();
            transform.parent.transform.position = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider col){
        if (col.transform.gameObject.tag == "AI") {
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be true)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }

    private void OnTriggerExit(Collider col){
        if (col.transform.gameObject.tag == "AI"){
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be false)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }
}



