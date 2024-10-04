using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    PlayerController playerController;

    // spell lockout timer display
    [SerializeField] private GameObject lockoutDisplay;
    private Image sliderBar;
    private int currentTime = 0;
    private int lockoutTime = 0;
    private float scaleFactor = 100f;
    private float previousIncrement = 0f;
    private int oldTime = 0;

    // health system
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Sprite fullHeartImage;
    [SerializeField] private Sprite damagedHeartImage;
    private bool[] healthStatus = { true, true, true };
    private int maxHP;
    private int currentHP;
    private int lastHP;

    void Start()
    {
        SetupSlider();
        SetupHealth();
    }

    void Update()
    {
        RunSlider();
        RunHealth();
    }

    private void SetupSlider()
    {
        sliderBar = GameObject.Find("SliderBar").GetComponent<Image>();
        playerController = player.GetComponent<PlayerController>();
        lockoutTime = Mathf.FloorToInt(playerController.GetLockoutTime() * scaleFactor);
        sliderBar.fillAmount = 0f;
    }

    private void RunSlider()
    {
        currentTime = Mathf.FloorToInt(playerController.GetCurrentTime() * scaleFactor);
        if (currentTime == lockoutTime)
        {
            sliderBar.fillAmount = 1f;
            previousIncrement = 0f;
            return;
        }

        float percentage = currentTime % lockoutTime;
        if (percentage != 0 && percentage % (lockoutTime / scaleFactor) == 0 && oldTime != currentTime)
        {
            oldTime = currentTime;
            previousIncrement += 0.01f;
            sliderBar.fillAmount = previousIncrement;
        }
    }

    private void SetupHealth()
    {
        lastHP = healthStatus.Length;
        currentHP = lastHP;
        maxHP = lastHP - 1;
        foreach (GameObject heart in hearts) heart.GetComponent<Image>().sprite = fullHeartImage;
    }

    private void RunHealth()
    {
        bool dirty = false;
        currentHP = playerController.GetHP();

        // lose health
        if (currentHP < lastHP)
        {
            lastHP = currentHP;
            healthStatus[maxHP - lastHP] = false;
            dirty = true;
        }

        // gain health
        if (currentHP > lastHP)
        {
            lastHP = currentHP;
            healthStatus[maxHP - lastHP] = true;
            dirty = true;
        }

        // update display
        if (dirty)
        {
            for(int i = 0; i < healthStatus.Length; i++)
            {
                GameObject heart = hearts[i];
                heart.GetComponent<Image>().sprite = healthStatus[i] ? fullHeartImage : damagedHeartImage;
            }
        }

    }
}
