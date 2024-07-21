using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //      OBJECTS:


    [SerializeField] private TextMeshProUGUI currentHoverText;

    [SerializeField] private TextMeshProUGUI appropriateHoverText;


    //      SETUP:


    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.Instance;
    }


    //      METHODS:


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        audioManager.PlayAudioClip(audioManager.mainMenuSounds, "ButtonHovered");

        if (currentHoverText != null)
            currentHoverText.text = appropriateHoverText.text ?? "";
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (currentHoverText != null)
            currentHoverText.text = "";
    }
}
