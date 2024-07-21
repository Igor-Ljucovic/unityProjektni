using UnityEngine;

public class Boss1Animations : MonoBehaviour
{

    //      OBJECTS:

    [SerializeField] private Boss1Behavior boss1Behavior;

    [SerializeField] private SpriteRenderer boss1Sprite;
    [SerializeField] private Sprite boss1ParryingSprite;
    [SerializeField] private Sprite boss1IdleSprite;


    private void Update()
    {
        boss1Sprite.transform.GetComponent<SpriteRenderer>().sprite = boss1Behavior.isPerformingAParryableAttack ? boss1ParryingSprite : boss1IdleSprite;
    }
}
