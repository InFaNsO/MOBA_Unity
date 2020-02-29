using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
    enum Command
    {
        none,
        Move,
        Attack,
        Skill
    }

    enum State
    {
        Idle,
        Move,
        Attack1,
        Death,
    }

    struct MoveCommand
    {
        public Vector3 destination;
    }
    struct AttackCommand
    {
        public Health targetHealth;
    }


    private NavMeshAgent mAgent;
    private Animator mAnimator;
    private Health mHealth;
    private Health mTargetHealth;
    private Weapon mWeapon;
    private Team mTeam;

    private State mCurrentState = State.Idle;

    [SerializeField] float mAttackRate = 1.0f;
    [SerializeField] float mAttackRange = 1.0f;
    float mNextAttackTime = 1.0f;

    MoveCommand mMoveCommand;
    AttackCommand mAttackCommand;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponentInChildren<Animator>();
        mHealth = GetComponent<Health>();
        mWeapon = GetComponent<Weapon>();
        mTeam = GetComponent<Team>();
        mNextAttackTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        switch (CheckInputCommand())
        {
            case Command.none:
                break;
            case Command.Move:
                ProcessMoveCommand();
                break;
            case Command.Attack:
                ProcessAttackCommand();
                break;
            case Command.Skill:
                break;
            default:
                break;
        }

        switch (mCurrentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Move:
                UpdateMove();
                break;
            case State.Attack1:
                UpdateAttack();
                break;
            case State.Death:
                UpdateDeath();
                break;
            default:
                break;
        }
    }

    private Command CheckInputCommand()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                var hitTeam = hit.collider.GetComponent<Team>();
                if (hitTeam && hitTeam.faction != mTeam.faction)
                {
                    mAttackCommand.targetHealth = hit.collider.GetComponent<Health>();
                    return Command.Attack;
                }
                else
                {
                    mMoveCommand.destination = hit.point;
                    mCurrentState = State.Move;
                    return Command.Move;
                }
            }
        }

        return Command.none;
    }

    private void ProcessMoveCommand()
    {
        mCurrentState = State.Move;
        mAgent.SetDestination(mMoveCommand.destination);
    }
    private void ProcessAttackCommand()
    {
        if(mAttackCommand.targetHealth)
        {
            mCurrentState = State.Attack1;
            mTargetHealth = mAttackCommand.targetHealth;
        }
    }

    private void UpdateIdle()
    { 
    }
    private void UpdateMove()
    {
        if (!mAgent.pathPending && mAgent.remainingDistance <= mAgent.stoppingDistance)
        {
            mCurrentState = State.Idle;
            mAnimator.SetInteger("AnimationState", (int)State.Idle);
        }
        else
        {
            mAnimator.SetInteger("AnimationState", (int)State.Move);
        }

    }
    private void UpdateAttack()
    {
        if(mTargetHealth == null || !mTargetHealth.IsAlive())
        {
            mCurrentState = State.Idle;
            mAnimator.SetInteger("AnimationState", (int)mCurrentState);
            return;
        }

        if(Vector3.Distance(mTargetHealth.transform.position, transform.position) > mAttackRange)
        {
            mAgent.SetDestination(mTargetHealth.transform.position);
            mAnimator.SetInteger("AnimationState", (int)State.Move);
        }
        else
        {
            mAgent.transform.forward = Vector3.Normalize(mTargetHealth.transform.position - transform.position);
            mAgent.SetDestination(transform.position);

            var animationState = mAnimator.GetInteger("AnimationState");
            if (animationState != (int)State.Idle && animationState != (int)State.Attack1)
            {
                mAnimator.SetInteger("AnimationState", (int)State.Idle);
            }
            else if (mNextAttackTime < Time.time)
            {
                mAnimator.SetInteger("AnimationState", (int)State.Attack1);
                mAnimator.SetTrigger("Attack");

                mNextAttackTime = Time.time + (1.0f / mAttackRate);
            }
        }
    }

    private void UpdateDeath()
    {
    }

    public void OnAttack()
    {
        if(mTargetHealth && mTargetHealth.IsAlive())
           mWeapon.Use(mTargetHealth);

    }
}
