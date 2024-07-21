using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;


//      METHOD EXTENSIONS:


public static class StringExtensions
{

    public static string ReformattedAudioClipName(this string soundName)
    {
        if (soundName == null)
            throw new NullReferenceException("soundName was null");
        if (soundName.Length == 0)
            throw new ArgumentException("soundName was empty");

        return soundName.Substring(0, soundName.IndexOf(' '));
    }
}

public static class ArrayExtensions
{

    public static bool Contains<T>(this T[] array, T wantedValue)
    {
        if (array == null)
            throw new ArgumentNullException("array was null");
        if (array.Length == 0)
            return false;

        foreach (T element in array)
        {
            if (EqualityComparer<T>.Default.Equals(element, wantedValue))
                return true;
        }

        return false;
    }

    
    public static void UpdateArrayQueue<T>(this T[] array, T newElementValue)
    {
        if (array == null)
            throw new ArgumentNullException("array was null");
        if (array.Length == 0)
            throw new IndexOutOfRangeException("array has a length of 0");

        if (array.Count() == 1)
        {
            array[0] = newElementValue;
            return;
        }

        for (int i = 1; i < array.Length; i++)
            array[i - 1] = array[i];

        array[array.Length - 1] = newElementValue;
    }
}

public static class ListExtensions
{
    public static void AddElements<T>(this List<T> list, params T[] parameters)
    {
        if (list == null)
            throw new ArgumentNullException("list was null");

        list.AddRange(parameters);
    }
} 

public static class TransformExtensions
{

    public static bool IsGrounded(this Transform transform, LayerMask groundLayer, float circleRadius)
    {
        if (transform == null)
            throw new NullReferenceException("Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (circleRadius <= 0)
            throw new ArithmeticException("circleRadius must be greater than 0");

        return Physics2D.OverlapCircle(transform.position, circleRadius, groundLayer);
    }
    
    public static void Flip(this Transform transform, ref bool isFacingRight)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");

        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.transform.localScale;
        localScale.x *= -1f;
        transform.transform.localScale = localScale;
    }

    public static float HorizontalDistance(this Transform transform1, Transform transform2)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");

        return Mathf.Abs(transform1.position.x - transform2.position.x);
    }

    public static float VerticalDistance(this Transform transform1, Transform transform2)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");

        return Mathf.Abs(transform1.position.y - transform2.position.y);
    }

    public static float Distance(this Transform transform1, Transform transform2)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");

        return Vector2.Distance(transform1.position, transform2.position);
    }

    public static bool WallIsNearOnTheRightSide(this Transform transform, LayerMask groundLayer, float wallSearchDistance)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (wallSearchDistance <= 0)
            throw new ArithmeticException("wallSearchDistance must be greater than 0");

        return Physics2D.OverlapBox(new Vector2(wallSearchDistance / 2  + transform.position.x, transform.position.y),
                                    new Vector2(wallSearchDistance * 1.1f, 0.5f), 0f, groundLayer);
    }

    public static bool WallIsNearOnTheLeftSide(this Transform transform, LayerMask groundLayer, float wallSearchDistance)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (wallSearchDistance <= 0)
            throw new ArithmeticException("wallSearchDistance must be greater than 0");

        return Physics2D.OverlapBox(new Vector2(-wallSearchDistance / 2 + transform.position.x, transform.position.y),
                                    new Vector2(wallSearchDistance * 1.1f, 0.5f), 0f, groundLayer);
    }

    public static bool GroundIsNearBelow(this Transform transform, LayerMask groundLayer, float groundSearchDistance)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (groundSearchDistance <= 0)
            throw new ArithmeticException("groundSearchDistance must be greater than 0");

        return Physics2D.OverlapBox(new Vector2(transform.position.x, -groundSearchDistance / 2 + transform.position.y),
                                    new Vector2(0.5f, groundSearchDistance * 1.1f), 0f, groundLayer);
    }

    public static bool GroundIsNearAbove(this Transform transform, LayerMask groundLayer, float groundSearchDistance)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (groundSearchDistance <= 0)
            throw new ArithmeticException("groundSearchDistance must be greater than 0");

        return Physics2D.OverlapBox(new Vector2(transform.position.x, groundSearchDistance / 2 + transform.position.y),
                                    new Vector2(0.5f, groundSearchDistance * 1.1f), 0f, groundLayer);
    }

    public static bool CloseHorizontally(this Transform transform1, Transform transform2, float maximumDistance)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");
        if (maximumDistance <= 0)
            throw new ArithmeticException("groundSearchDistance must be greater than 0");

        return transform1.HorizontalDistance(transform2) <= maximumDistance;
    }

    public static bool CloseVertically(this Transform transform1, Transform transform2, float maximumDistance)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");
        if (maximumDistance <= 0)
            throw new ArithmeticException("groundSearchDistance must be greater than 0");

        return transform1.VerticalDistance(transform2) <= maximumDistance;
    }

    public static bool Close(this Transform transform1, Transform transform2, float maximumDistance)
    {
        if (transform1 == null)
            throw new NullReferenceException("Transform was null");
        if (transform2 == null)
            throw new NullReferenceException("passed Transform was null");
        if (maximumDistance <= 0)
            throw new ArithmeticException("groundSearchDistance must be greater than 0");

        return transform1.Distance(transform2) <= maximumDistance;
    }

    public static Vector2 ClosestDirectionToGround(this Transform transform, LayerMask groundLayer, float circleRadius)
    {
        if (transform == null)
            throw new NullReferenceException("passed Transform was null");
        if (groundLayer < 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");
        if (circleRadius <= 0)
            throw new ArithmeticException("circleRadius must be greater than 0");

        bool isAbove = Physics2D.Raycast(transform.position, Vector2.up, circleRadius, groundLayer);
        bool isBelow = Physics2D.Raycast(transform.position, Vector2.down, circleRadius, groundLayer);
        bool isToLeft = Physics2D.Raycast(transform.position, Vector2.left, circleRadius, groundLayer);
        bool isToRight = Physics2D.Raycast(transform.position, Vector2.right, circleRadius, groundLayer);

        if (isAbove) return Vector2.up;
        if (isBelow) return Vector2.down;
        if (isToLeft) return Vector2.left;
        if (isToRight) return Vector2.right;

        return Vector2.zero;
    }
}

public static class RigidBody2DExtensions
{
    public static void Stun(this Rigidbody2D rigidBody2D)
    {
        if (rigidBody2D == null)
            throw new NullReferenceException("RigidBody2D was null");

        rigidBody2D.velocity = new Vector2(0, rigidBody2D.velocity.y);
    }
    
    public static bool StopEntityIfTimerDidntPass(this Rigidbody2D rigidBody2D, int currentTime, int totalTime)
    {
        if (rigidBody2D == null)
            throw new NullReferenceException("RigidBody2D was null");

        if (currentTime < totalTime)
        {
            rigidBody2D.velocity = new Vector2(0f, rigidBody2D.velocity.y);
            return true;
        }
        return false;
    }

    public static void Jump(this Rigidbody2D rigidBody2D, float jumpingPower) 
    {
        if (rigidBody2D == null)
            throw new NullReferenceException("RigidBody2D was null");
        if (jumpingPower <= 0)
            throw new ArithmeticException("passed jumpingPower must be greater than 0");

        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpingPower);
    }
}

public static class BoxCollider2DExtensions
{

    public static float SearchDistance(this BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            throw new NullReferenceException("boxCollider2D was null");
        if (boxCollider2D.size == null || boxCollider2D.size.x <= 0 || boxCollider2D.size.y <= 0)
            throw new ArithmeticException("passed LayerMask can't be a negative integer");

        return (boxCollider2D.size.x + boxCollider2D.size.y) / 2;
    }
}

public static class GameObjectExtensions
{

    public static void SummonGameObject(this GameObject gameObject, in GameObject gameObjectType, Vector2 position, bool enableSpriteRenderer, double? duration = null)
    {
        gameObject = MonoBehaviour.Instantiate(gameObjectType, position, Quaternion.identity);

        gameObject.GetComponent<SpriteRenderer>().enabled = enableSpriteRenderer;

        if (duration != null)
            MonoBehaviour.Destroy(gameObject, (float)duration);
    }
}


public class Game : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private Canvas[] canvasArray;

    public Canvas loadingScreenCanvas;
    public Canvas controlsExplanationsClassicCanvas;
    public Canvas controlsExplanationsUltimateCanvas;
    public Canvas pressSToShowControlsExplanationsCanvas;

    public GameObject testWithADummyEnemyAreaPosition;
    public GameObject boss1AreaPosition;
    public GameObject boss2AreaPosition;
    public GameObject boss3AreaPosition;


    //      GENERAL:


    private static  int pauseDuration = 0;
    private static bool gameModeIsClassic = false;

    public  static bool mainGameStarted = false;

    private static Color fullColor = new Color(1, 1, 1, 1);
    private static Color fadedAwayColor = new Color(1, 1, 1, 0);



    //      HELPER VARIABLES:


    private static bool playingTestWithADummyEnemy = false;
    private static bool currentlyShowingControlsExplanationCanvas = false;


    //      SETUP:


    private static Game instance;

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("Game");
                    instance = managerObject.AddComponent<Game>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }


    private void Start()
    {
        canvasArray = FindObjectsOfType<Canvas>();
    }


    //      CODE:


    private void Update()
    {
        PauseCheck();
        DisplayControlsExplanationsCanvasCheck();
    }


    //      METHODS:


    private static void PauseCheck()
    {
        if (Game.pauseDuration > 0)
        {
            PauseTheGame();
            pauseDuration--;
        }
        else if (pauseDuration == 0)
            UnpauseTheGame();
    }

    private void DisplayControlsExplanationsCanvasCheck()
    {
        if (!Game.playingTestWithADummyEnemy) 
            return;

        EnableCanvas(pressSToShowControlsExplanationsCanvas, true);

        if (!Input.GetKeyDown(KeyCode.S))
            return;

        if (currentlyShowingControlsExplanationCanvas)
        {
            EnableCanvas(gameModeIsClassic ? controlsExplanationsClassicCanvas : controlsExplanationsUltimateCanvas, false);
            currentlyShowingControlsExplanationCanvas = false;
        }
        else
        {
            EnableCanvas(gameModeIsClassic ? controlsExplanationsClassicCanvas : controlsExplanationsUltimateCanvas, true);
            currentlyShowingControlsExplanationCanvas = true;
        }
    }

    public void EnableOnlyThisCanvas(Canvas canvasItem)
    {
        foreach (Canvas canvas in canvasArray)
        {
            if (canvas == canvasItem)
            {
                EnableCanvas(canvas, true);
                return;
            }
        }
    }

    private static void EnableCanvas(Canvas canvasItem, bool enableCanvas) => canvasItem.gameObject.SetActive(enableCanvas);

    public void SetUpAllCanvases() => Array.ForEach(canvasArray, canvas => canvas.renderMode = RenderMode.ScreenSpaceOverlay);

    public void DisableAllCanvases() => canvasArray.ToList().ForEach(canvas => canvas.gameObject.SetActive(false));

    public static bool Paused() => Time.timeScale == 0;

    public static void PauseTheGameTemporarily(int duration) => pauseDuration = duration;
    
    public static void PauseTheGame() => Time.timeScale = 0;

    public static void UnpauseTheGame() => Time.timeScale = 1;

    public static void ReduceCooldownIfOnCooldownCheck(ref int cooldown) => cooldown = Math.Max(0, cooldown - 1);

    public static void ChangeAnimationState(Animator animator, string currentState, string newState)
    {
        if (newState == currentState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public static void SetGameModeToClassic(bool gameModeIsClassic) => Game.gameModeIsClassic = gameModeIsClassic;

    public static void SetPlayingTestWithDummyEnemy(bool playingTestWithADummyEnemy) => Game.playingTestWithADummyEnemy = playingTestWithADummyEnemy;
    
    public static bool CurrentGameModeIsClassic() => gameModeIsClassic;

    public static void CalculateHorizontalSpritePositionChangePerFrame(Sprite[] spriteSlices, ref float positionChangePerFrame, bool horizontal)
    {
        float oneSpriteSliceLength = 0;

        if (horizontal)
        {
            foreach (Sprite sprite in spriteSlices)
                oneSpriteSliceLength += sprite.bounds.size.x;
        }
        else
        {
            foreach (Sprite sprite in spriteSlices)
                oneSpriteSliceLength += sprite.bounds.size.y;
        }

        positionChangePerFrame = oneSpriteSliceLength / spriteSlices.Length;
    }

    public static IEnumerator CountdownUntilZero(float duration)
    {
        while (duration > 0)
        {
            yield return new WaitForFixedUpdate();
            Promeni(ref duration, Time.fixedDeltaTime);
        }
    }

    public static void Promeni(ref float brojka, float razlika)
    {
        brojka -= razlika;

        if (brojka < 0)
            brojka = 0;
    }

    public static IEnumerator AttackFadeOverTime(GameObject gameObject, float colorTransitionDuration, float colorTransitionProgressToDisableColliders)
    {
        float currentTime = 0f;

        if (gameObject == null)
            yield break;

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        while (currentTime < colorTransitionDuration)
        {
            currentTime += Time.deltaTime;

            float currentLerpProgress = Mathf.Clamp01(currentTime / colorTransitionDuration);

            if (gameObject != null && currentLerpProgress > colorTransitionProgressToDisableColliders)
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (spriteRenderer != null)
                spriteRenderer.color = Color.Lerp(fullColor, fadedAwayColor, currentLerpProgress);

            yield return null;
        }
    }

    public void StartFadeOverTimeCoroutine(List<GameObject> gameObjects, ref int nextGameObjectToFadeAway,
                                           float colorTransitionDuration, float colorTransitionProgressToDisableColliders)
    {
        StartCoroutine(AttackFadeOverTime(gameObjects[nextGameObjectToFadeAway++], colorTransitionDuration, colorTransitionProgressToDisableColliders));
        
        if (nextGameObjectToFadeAway > gameObjects.Count - 1)
        {
            gameObjects.Clear();
            nextGameObjectToFadeAway = 0;
        }    
    }
}
