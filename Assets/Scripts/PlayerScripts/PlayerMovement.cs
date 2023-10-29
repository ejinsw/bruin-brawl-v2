using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    private Animator anim;
    private StaminaBar stamina;
    private Coroutine coroutine;
    private SpriteRenderer sprite;
    private bool isShifting = false;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        stamina = StaminaBar.instance;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        float rollingSpeedX = moveDelta.x * 2;
        float rollingSpeedY = moveDelta.y * 2;

        // Reset MoveDelta
        moveDelta = new Vector3(x, y, 1);

        // Sprite condition look left or right
        if (moveDelta.x > 0)
        {
            sprite.flipX = false;
        }
        else if (moveDelta.x < 0)
        {
            sprite.flipX = true;
        }

        // Check and move Y
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null && !isShifting)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        else if (hit.collider == null && isShifting)
        {
            transform.Translate(0, rollingSpeedY * Time.deltaTime, 0);
        }

        // Check and move X
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null && !isShifting)
        {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        else if (hit.collider == null && isShifting)
        {
            transform.Translate(rollingSpeedX * Time.deltaTime, 0, 0);
        }

        // Check if shifting & drain stamina
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isShifting)
            {
                isShifting = true;
            }
        }
        else
        {
            isShifting = false;
        }

        if (isShifting && stamina.GetCurrentStamina() - 10 >= 0)
        {
            if (moveDelta.x != 0 || moveDelta.y != 0)
                coroutine = StartCoroutine(UseStamina(0.5f));
        }
        else if (isShifting && stamina.GetCurrentStamina() - 10 < 0 || !isShifting)
        {
            isShifting = false;
            coroutine = null;
        }

        AnimationState();
    }


    private IEnumerator UseStamina(float amount)
    {
        stamina.UseStamina(amount);
        yield return new WaitForSeconds(0.1f * Time.deltaTime);
    }


    private enum MovementState { idleDown, idleSide, idleUp, walkDown, walkSide, walkUp, rollDown, rollSide, rollUp }
    private MovementState state;


    private const string IDLE_DOWN = "IdleDown";
    private const string IDLE_SIDE = "IdleSide";
    private const string IDLE_UP = "IdleUp";
    private const string WALK_DOWN = "WalkDown";
    private const string WALK_SIDE = "WalkSide";
    private const string WALK_UP = "WalkUp";
    private const string ROLL_DOWN = "RollDown";
    private const string ROLL_SIDE = "RollSide";
    private const string ROLL_UP = "RollUp";
    private string currentState;

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState, 0);
        currentState = newState;
    }

    private void AnimationState()
    {
        if (!isShifting)
        {
            if (moveDelta.x != 0 || moveDelta.y != 0)
            {
                if (moveDelta.x != 0)
                {
                    state = MovementState.walkSide;
                    ChangeAnimationState(WALK_SIDE);
                }
                else if (moveDelta.y != 0)
                {
                    if (moveDelta.y > 0)
                    {
                        state = MovementState.walkUp;
                        ChangeAnimationState(WALK_UP);
                    }
                    else if (moveDelta.y < 0)
                    {
                        state = MovementState.walkDown;
                        ChangeAnimationState(WALK_DOWN);
                    }
                }
            }
            else
            {
                if (state == MovementState.walkSide)
                {
                    state = MovementState.idleSide;
                    ChangeAnimationState(IDLE_SIDE);
                }
                else if (state == MovementState.walkUp)
                {
                    state = MovementState.idleUp;
                    ChangeAnimationState(IDLE_UP);
                }
                else if (state == MovementState.walkDown)
                {
                    state = MovementState.idleDown;
                    ChangeAnimationState(IDLE_DOWN);
                }
            }
        }
        else
        {
            if (moveDelta.x != 0 || moveDelta.y != 0)
            {
                if (moveDelta.x != 0)
                {
                    state = MovementState.rollSide;
                    ChangeAnimationState(ROLL_SIDE);
                }
                else if (moveDelta.y != 0)
                {
                    if (moveDelta.y > 0)
                    {
                        state = MovementState.rollUp;
                        ChangeAnimationState(ROLL_UP);
                    }
                    else if (moveDelta.y < 0)
                    {
                        state = MovementState.rollDown;
                        ChangeAnimationState(ROLL_DOWN);
                    }
                }
            }
            else
            {
                if (state == MovementState.rollSide)
                {
                    state = MovementState.idleSide;
                    ChangeAnimationState(IDLE_SIDE);
                }
                else if (state == MovementState.rollUp)
                {
                    state = MovementState.idleUp;
                    ChangeAnimationState(IDLE_UP);
                }
                else if (state == MovementState.rollDown)
                {
                    state = MovementState.idleDown;
                    ChangeAnimationState(IDLE_DOWN);
                }
            }
        }
    }
}