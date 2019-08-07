using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Color itsuki = new Color(1, 255/255f, 0);
    private Color low = new Color(75/255f, 0f, 0f);
    private Color flashOn = new Color(161/255f, 0f, 0f, 1f);
    private Color flashOff = new Color(161 / 255f, 0f, 0f, 0f);
    private Color cameraBG = new Color(10 / 255f, 10 / 255f, 10 / 255f);
    private float tColor;
    private float fadeTime = 5f;

    Slider hpSlider;
    Text hpText;

    GameManager manager;

    public Transform effect;

    public float startingHP = 100.0f;
    public float limitHP = 100.0f;
    float maxHP;
    float hp;
    bool isDamaged = false;
    bool isDead = false;
    bool isExploding = false;

    //PlayerInput playerInput;

	void Awake ()
    {
        hp = startingHP;
        maxHP = limitHP;
        //Debug.Log("current HP = " + hp);
        //Debug.Log("max HP = " + maxHP);
        tColor = 0f;

        manager = FindObjectOfType<GameManager>();

        hpSlider = FindObjectOfType<Slider>();
        hpText = hpSlider.GetComponentInChildren<Text>();

        manager.GetDamageFlash().color = flashOff;

        UpdateHPDisplay();
    }
	
	// Update is called once per frame
	void Update () {
        //TODO find why this isn't working
        if (isDead && !isExploding)
        {
            //only once
            isExploding = true;

            //explode
            StartCoroutine(Explode());
        }
        else if (!isDamaged)
        {
            manager.GetHPBarFill().color = Color.Lerp(low, itsuki, hp / 100);
            FlashOff();
            //manager.GetDamageFlash().gameObject.GetComponent<Animator>().SetTrigger("flashoff");
        }
	}

    public float GetHP() {
        return hp;
    }

    public void SetHP(float hp) {
        this.hp = hp;
    }
    
    /// <summary>
    /// Reduces Player's HP by DMG every second if the Player isn't dead yet.
    /// </summary>
    public void DrainHP(int dmg) {
        if (!isDead) {
            manager.GetHPBarFill().color = Color.red;
            FlashOn();
            tColor = 0;

            hp = Mathf.Round((hp - dmg * Time.deltaTime) * 100f) / 100f;
            UpdateHPDisplay();
            //Debug.Log("Player.DrainHP(): " + hp);

            if (Mathf.Approximately(hp, 0f) || hp <= 0f)
            {
                hp = 0;
                isDead = true;
            }
        }

        UpdateHPDisplay();
    }

    /// <summary>
    /// Instantaneously reduces Player's HP by DMG if the Player isn't dead yet.
    /// </summary>
    /// <param name="dmg"></param>
    public void CutHP(float dmg) {
        if (!isDead)
        {
            manager.GetHPBarFill().color = Color.red;
            FlashOn();
            tColor = 0;

            hp = Mathf.Round((hp - dmg) * 100f) / 100f;
            UpdateHPDisplay();
            //Debug.Log("Player.DrainHP(): " + hp);

            if (Mathf.Approximately(hp, 0f) || hp <= 0f)
            {
                hp = 0;
                isDead = true;
            }
        }
        UpdateHPDisplay();
    }

    /// <summary>
    /// Instantaneously restores Player's HP by float Heal. 
    /// Heal amount cannot cause Player HP to exceed Player maxHP.
    /// </summary>
    public void HealHP(float heal) {
        hp = Mathf.Clamp(hp + heal, 0, 100);
        UpdateHPDisplay();
    }

    /// <summary>
    /// Fades the Damage Flash effect to transparent over fadeTime.
    /// </summary>
    void FlashOff() {
        
        if (tColor <= 1f)
        {
            tColor += Time.deltaTime / fadeTime;
            manager.GetDamageFlash().color = Color.Lerp(manager.GetDamageFlash().color, flashOff, tColor);
            //manager.GetMainCamera().backgroundColor = Color.Lerp(manager.GetMainCamera().backgroundColor, 
                //cameraBG, tColor);
        }
        //manager.GetDamageFlash().transform.GetComponent<Animator>().SetTrigger("flashoff");
    }

    void FlashOn() {
        Debug.Log("Damage flash on");
        manager.GetDamageFlash().color = flashOn;
        //manager.GetDamageFlash().transform.GetComponent<Animator>().SetTrigger("flashon");
    }

    /// <summary>
    /// Updates the HPSlider UI with current HP value as a percentage.
    /// </summary>
    void UpdateHPDisplay() {
        //round to 2 decimals
        hpText.text = (hp / maxHP) * 100.00f + "%";
        hpSlider.value = hp / maxHP;
    }

    public bool GetIsDamaged() {
        return isDamaged;
    }

    public void SetIsDamaged(bool b) {
        //if (b) print("Player.SetIsDamaged(): " + b);
        isDamaged = b;
    }

    public bool GetIsDead() { return isDead; }

    IEnumerator Explode() {
        //disable player input
        this.GetComponent<PlayerInput>().enabled = false;

        //make player model invisble
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<Light>().enabled = false;
        gameObject.GetComponent<LineRenderer>().enabled = false;

        //explosion effect, 5s
        Instantiate(effect, 
            new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            Quaternion.Euler(new Vector3(-90,0,0)));
        yield return new WaitForSeconds(5);
        manager.Restart();
    }
}
