using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandeler : MonoBehaviour
{
    HeroController myHero;

    // Start is called before the first frame update
    void Start()
    {
        myHero = GetComponentInParent<HeroController>();
    }

    public void OnAttack()
    {
        if(myHero)
        {
            myHero.OnAttack();
        }
    }
}
