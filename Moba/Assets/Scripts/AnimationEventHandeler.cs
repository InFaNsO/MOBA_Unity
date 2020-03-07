using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandeler : MonoBehaviour
{
    HeroController myHero;
    EnemyController myEnemy;

    // Start is called before the first frame update
    void Start()
    {
        myHero = GetComponentInParent<HeroController>();
        myEnemy = GetComponentInParent<EnemyController>();
    }

    public void OnAttack()
    {
        if(myHero)
        {
            myHero.OnAttack();
        }
        else if(myEnemy)
        {
            myEnemy.OnAttack();
        }
    }
}
