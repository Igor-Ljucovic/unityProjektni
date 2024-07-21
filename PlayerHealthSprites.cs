using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSprites : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private PlayerHealth playerHealth;


    //      IMAGES AND SPRITES:


    [SerializeField] private Image playerHealthMask1;
    [SerializeField] private Image playerHealthMask2;
    [SerializeField] private Image playerHealthMask3;
    [SerializeField] private Image playerHealthMask4;
    [SerializeField] private Image playerHealthMask5;
    [SerializeField] private Image playerHealthMask6;
    [SerializeField] private Image playerHealthMask7;
    [SerializeField] private Image playerHealthMask8;
    [SerializeField] private Image playerHealthMask9;

    [SerializeField] private Sprite playerEmptyHealthSprite;
    [SerializeField] private Sprite playerFullHealthSprite;

    private List<Image> playerHealthMasks = new List<Image>();


    //      SETUP:


    private void Start()
    {
        playerHealthMasks.AddElements(playerHealthMask1, playerHealthMask2, playerHealthMask3, playerHealthMask4,
            playerHealthMask5, playerHealthMask6, playerHealthMask7, playerHealthMask8, playerHealthMask9);
    }


    //      METHODS


    public void ChangeHealthSpritesCheck()
    {
        for (int i = 0; i < playerHealthMasks.Count; i++)
            SwitchHealthMaskSprite(playerHealthMasks[i], i);
    }

    public void SwitchHealthMaskSprite(Image healthMaskSprite, int currentHealthThreshold) =>
        healthMaskSprite.sprite = currentHealthThreshold >= playerHealth.currentPlayerHealth ? playerEmptyHealthSprite : playerFullHealthSprite;
}
