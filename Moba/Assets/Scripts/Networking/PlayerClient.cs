using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerClient : NetworkBehaviour
{

                      public GameObject heroPrefab;
    [HideInInspector] public GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
            Cmd_SpawnHero();
    }

    [Command] void Cmd_SpawnHero()
    {
        hero = Instantiate(heroPrefab);
        NetworkServer.SpawnWithClientAuthority(hero, connectionToClient);
    }
}
