using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    public List<WeaponSO> weapons;
    public Image weaponImage;
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI meditivnessText;
    public TextMeshProUGUI attentivnessText;
    public AudioClip scrollSound;

    public Button leftButton;
    public Button rightButton;

    private int currentIndex = 0;

    public float scrollCooldown = 0.2f;
    private float lastScrollTime;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShowWeapon(0);
        leftButton.onClick.AddListener(PrevWeapon);
        rightButton.onClick.AddListener(NextWeapon);
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Time.time - lastScrollTime >= scrollCooldown)
        {
            if (scroll > 0f)
            {
                PrevWeapon();
                lastScrollTime = Time.time;
                PlayScrollSound();
            }
            else if (scroll < 0f)
            {
                NextWeapon();
                lastScrollTime = Time.time;
                PlayScrollSound();
            }
        }
    }

    void ShowWeapon(int index)
    {
        if (weapons == null || weapons.Count == 0) return;

        currentIndex = Mathf.Clamp(index, 0, weapons.Count - 1);
        WeaponSO weapon = weapons[currentIndex];

        weaponImage.sprite = weapon.icon;
        weaponNameText.text = weapon.weaponName;
        damageText.text = weapon.damage.ToString();
        fireRateText.text = weapon.fireRate.ToString();
        rangeText.text = weapon.range.ToString();
        meditivnessText.text = weapon.meditivness.ToString();
        attentivnessText.text = weapon.attentivness.ToString();
    }

    public void PrevWeapon()
    {
        int newIndex = (currentIndex - 1 + weapons.Count) % weapons.Count;
        ShowWeapon(newIndex);
    }

    public void NextWeapon()
    {
        int newIndex = (currentIndex + 1) % weapons.Count;
        ShowWeapon(newIndex);
    }

    public void ConfirmSelection()
    {
        string sceneToLoad = $"Level{PlayerPrefs.GetInt("SelectedLevelIndex")}";
        SceneManager.LoadScene(sceneToLoad);
    }

    void PlayScrollSound()
    {
        if (audioSource != null && scrollSound != null)
        {
            audioSource.PlayOneShot(scrollSound);
        }
    }
}
