using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    SpawnerManager spawner;
    BoardManager board;

    private ShapeManager activeObject;

    [Header("Counters")]
    [Range(0.02f,1f)]
    [SerializeField] private float fallTime = .5f;
    private float fallCount;
    private float fallDownLevelCount;
    [Range(0.02f,1f)]
    [SerializeField] private float keyPressTime = .25f;
    private float keyPressCount;
    [Range(0.02f,1f)]
    [SerializeField] private float turnTime = .25f;
    private float turnCount;
    [Range(0.02f,1f)]
    [SerializeField] private float downKeyTime = .25f;
    private float downKeyCount;

    public bool gameOver = false;

    public bool isClockwise = true;
    
    public IconOnOffManager rotateIcon;

    public GameObject gameOverPanel;

    private ScoreManager scoreManager;

    private FollowShapeManager followShape;

    private ShapeManager holdBlock;
    public GameObject holdObject;   

    public Image holdBlockImg;
    
    private bool isHoldChangeable = true;

    private bool shouldMove = true;

    public ParticleManager[] levelUpEffects = new ParticleManager[5];
    public ParticleManager[] gameOverEffects = new ParticleManager[5];
    
    enum Direction{none,left,right,up,down}

    private Direction swipeDirection = Direction.none;
    private Direction swipeEndDirection = Direction.none;

    private float nextTouchTime;
    private float nextSwipeTime;

    [Range(0.05f, 1f)] public float minTouchTime = 0.15f;
    [Range(0.05f, 1f)] public float minSwipeTime = 0.3f;

    private bool isTouched = false;

    private void Awake()
    {
        board = GameObject.FindObjectOfType<BoardManager>();
        spawner = GameObject.FindObjectOfType<SpawnerManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        followShape = GameObject.FindObjectOfType<FollowShapeManager>();
    }

    private void OnEnable()
    {
        TouchManager.DragEvent += SwipeFNC;
        TouchManager.SwipeEvent += SwipeEndFnc;
        TouchManager.TapEvent += TapFNC;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= SwipeFNC;
        TouchManager.SwipeEvent -= SwipeEndFnc;
        TouchManager.TapEvent += TapFNC;
    }

    public void StartGameFNC()
    {
        if (spawner)
        {
            spawner.ChangeAllToNullFNC();
            
            if (activeObject == null)
            {
                activeObject = spawner.CreateObjectFNC();
                activeObject.transform.position = VectortoIntFNC(activeObject.transform.position);
            }

            if (activeObject)
            {
                activeObject.transform.localScale = Vector3.zero;
                shouldMove = false;
                activeObject.transform.DOScale(1, .3f).SetEase(Ease.OutBack).OnComplete(()=>shouldMove=true);
            }

            if (holdBlock == null)
            {
                holdBlock = spawner.CreateHoldShapeFNC();

                if (holdBlock.name == activeObject.name)
                {
                    Destroy(holdBlock.gameObject);
                    holdBlock = spawner.CreateHoldShapeFNC();
                    
                    holdBlockImg.sprite = holdBlock.shapeBlock;
                    holdBlock.gameObject.SetActive(false);
                    
                }
                else
                {
                    holdBlockImg.sprite = holdBlock.shapeBlock;
                    holdBlock.gameObject.SetActive(false);
                }
            }
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        fallDownLevelCount = fallTime;
    }

    private void Update()
    {
        if (!board || !spawner || !activeObject || gameOver || !scoreManager || !shouldMove)
        {
            return;
        }
        
        InputControlFNC();
        
    }

    private void LateUpdate()
    {
        if (!board || !spawner || !activeObject || gameOver || !scoreManager || !followShape || !shouldMove)
        {
            return;
        }
        
        if (followShape)
        {
            followShape.CreateFollowShapeFNC(activeObject,board);
        }
    }

    void InputControlFNC()
    {
        //PC Inputs
        if (Input.GetKey("right") && Time.time>keyPressCount || Input.GetKeyDown("right"))
        {
            RightMovementFNC();
        }
        else if (Input.GetKey("left") && Time.time>keyPressCount || Input.GetKeyDown("left"))
        {
            LeftMovementFNC();
        }
        else if (Input.GetKeyDown("up") && Time.time > turnCount)
        {
            RotateFNC();
        }
        else if (((Input.GetKey("down") && Time.time > downKeyCount)) || Time.time>fallCount)
        {
            DownFallMovementFNC();
        }
        //Mobile Inputs
        else if ((swipeEndDirection==Direction.right && Time.time>nextSwipeTime) || 
                 (swipeDirection==Direction.right && Time.time>nextTouchTime))
        {
            RightMovementFNC();
            nextSwipeTime = Time.time + minSwipeTime;
            nextTouchTime = Time.time + minTouchTime;
        }
        else if ((swipeEndDirection==Direction.left && Time.time>nextSwipeTime) || 
                 (swipeDirection==Direction.left && Time.time>nextTouchTime))
        {
            LeftMovementFNC();
            nextSwipeTime = Time.time + minSwipeTime;
            nextTouchTime = Time.time + minTouchTime;
        }
        else if ((swipeEndDirection == Direction.up && Time.time>nextSwipeTime) || (isTouched))
        {
            RotateFNC();
            nextSwipeTime = Time.time + minSwipeTime;
        }
        else if (swipeDirection == Direction.down && Time.time > nextTouchTime)
        {
            DownFallMovementFNC();
        }
        swipeDirection = Direction.none;
        swipeEndDirection = Direction.none;
        isTouched = false;
    }
    
    private void DownFallMovementFNC()
    {
        fallCount = Time.time + fallDownLevelCount;
        downKeyCount = Time.time + downKeyTime;

        if (activeObject)
        {
            activeObject.DownMoveFNC();

            if (!board.IsInPosition(activeObject))
            {
                if (board.IsOverflowFNC(activeObject))
                {
                    activeObject.UpMoveFNC();
                    gameOver = true;
                    SoundManager.instance.PlaySoundEffect(6);

                    if (gameOverPanel)
                    {
                        StartCoroutine(GameOverRoutineFNC());
                    }
                }
                else
                {
                    PlacedFNC();
                }
            }
        }
    }

    private void RotateFNC()
    {
        activeObject.LeftTurnFNC();
        turnCount = Time.time + turnTime;

        if (!board.IsInPosition(activeObject))
        {
            SoundManager.instance.PlaySoundEffect(1);
            activeObject.RightTurnFNC();
        }
        else
        {
            isClockwise = !isClockwise;

            if (rotateIcon)
            {
                rotateIcon.IconOnOffFNC(isClockwise);
            }

            SoundManager.instance.PlaySoundEffect(3);
        }
    }

    private void LeftMovementFNC()
    {
        activeObject.LeftMoveFNC();
        keyPressCount = Time.time + keyPressTime;

        if (!board.IsInPosition(activeObject))
        {
            SoundManager.instance.PlaySoundEffect(1);
            activeObject.RightMoveFNC();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(3);
        }
    }

    private void RightMovementFNC()
    {
        activeObject.RightMoveFNC();
        keyPressCount = Time.time + keyPressTime;

        if (!board.IsInPosition(activeObject))
        {
            SoundManager.instance.PlaySoundEffect(1);
            activeObject.LeftMoveFNC();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(3);
        }
    }

    private void PlacedFNC()
    {
        if (activeObject)
        {
            keyPressCount = Time.time;
            downKeyCount = Time.time;
            turnCount = Time.time;
        
            holdObject.gameObject.SetActive(true);
        
            activeObject.UpMoveFNC();

            activeObject.CreatePlacedEffectFNC();
        
            board.PushObjectToGridFNC(activeObject);
            SoundManager.instance.PlaySoundEffect(5);

            isHoldChangeable = true;
        
            if (spawner)
            {
                activeObject = spawner.CreateObjectFNC();
                
                if (activeObject)
                {
                    activeObject.transform.localScale = Vector3.zero;
                    shouldMove = false;
                    activeObject.transform.DOScale(1, .3f).SetEase(Ease.OutBack).OnComplete(()=>shouldMove=true);
                }
                
                holdBlock = spawner.CreateHoldShapeFNC();

                if (holdBlock.name == activeObject.name)
                {
                    Destroy(holdBlock.gameObject);
                    holdBlock = spawner.CreateHoldShapeFNC();
                    
                    holdBlockImg.sprite = holdBlock.shapeBlock;
                    holdBlock.gameObject.SetActive(false);
                }
                else
                {
                    holdBlockImg.sprite = holdBlock.shapeBlock;
                    holdBlock.gameObject.SetActive(false);
                }
            }

            if (followShape)
            {
                followShape.ResetFNC();
            }

            StartCoroutine(board.ClearAllRowsFNC());

            if (board.clearedLines > 0)
            {
                scoreManager.RowScore(board.clearedLines);

                if (scoreManager.isNextLevel)
                {
                    SoundManager.instance.PlaySoundEffect(2);
                    fallDownLevelCount = fallTime - Mathf.Clamp(((float)scoreManager.level - 1)*.1f,0.05f,1f);

                    StartCoroutine(NextLevelRoutineFNC());
                }
                else
                {
                    if (board.clearedLines > 1)
                    {
                        SoundManager.instance.PlayVocalEffect();
                    }
                }
                SoundManager.instance.PlaySoundEffect(4);
            }
        }
        
    }

    Vector2 VectortoIntFNC(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    public void RotationIconDirectionFNC()
    {
        isClockwise = !isClockwise;
        
        activeObject.clockwiseTurnFNC(isClockwise);

        if (!board.IsInPosition(activeObject))
        {
            activeObject.clockwiseTurnFNC(!isClockwise);
            SoundManager.instance.PlaySoundEffect(3);
        }
        else
        {
            if (rotateIcon)
            {
                rotateIcon.IconOnOffFNC(isClockwise);
            }
            SoundManager.instance.PlaySoundEffect(1);
        }
    }

    public void ChangeHoldObjectFNC()
    {
        if (isHoldChangeable)
        {
            isHoldChangeable = false;
            
            activeObject.gameObject.SetActive(false);
            holdBlock.gameObject.SetActive(true);
            holdObject.gameObject.SetActive(false);

            holdBlock.transform.position = activeObject.transform.position;

            activeObject = holdBlock;
        }

        if (followShape)
        {
            followShape.ResetFNC();
        }
    }

    IEnumerator NextLevelRoutineFNC()
    {
        yield return new WaitForSeconds(.2f);

        int count = 0;

        while (count < levelUpEffects.Length)
        {
            levelUpEffects[count].EffectPlayFNC();
            yield return new WaitForSeconds(.1f);

            count++;
        }
    }

    IEnumerator GameOverRoutineFNC()
    {
        yield return new WaitForSeconds(.2f);
        int count = 0;

        while (count < gameOverEffects.Length)
        {
            gameOverEffects[count].EffectPlayFNC();
            yield return new WaitForSeconds(.1f);

            count++;
        }

        yield return new WaitForSeconds(1f);
        if (gameOverPanel)
        {
            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverPanel.SetActive(true);

            gameOverPanel.transform.DOScale(1, .5f).SetEase(Ease.OutBack);
        }
    }

    void SwipeFNC(Vector2 swipeMovement)
    {
        swipeDirection = DetermineDirectionFNC(swipeMovement);
    }

    void SwipeEndFnc(Vector2 swipeMovement)
    {
        swipeEndDirection = DetermineDirectionFNC(swipeMovement);

    }

    void TapFNC(Vector2 swipeMovement)
    {
        isTouched = true;
    }

    Direction DetermineDirectionFNC(Vector2 swipeMovement)
    {
        Direction swipeDirection = Direction.none;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDirection = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        else
        {
            swipeDirection = (swipeMovement.y >= 0) ? Direction.up : Direction.down;

        }

        return swipeDirection;
    }
}
