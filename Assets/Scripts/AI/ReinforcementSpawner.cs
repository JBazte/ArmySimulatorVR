using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementSpawner : MonoBehaviour {

    public Spawn[] allies;

    public float timeLeft;

    [SerializeField]
    private GameObject spawnPos;

    void Start () {
        timeLeft = 20f;
    }

    // Update is called once per frame
    void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f) {
            Spawner ();
            timeLeft = 10f;
        }
    }

    public void Spawner () {
        for (int n = 0; n < 4; n++) {
            int r = Random.Range (0, 100);
            for (int i = 0; i < allies.Length; i++) {
                if (r >= allies[i].minProb && r <= allies[i].maxProb) {
                    Instantiate (allies[i].soldier, spawnPos.transform.position, transform.rotation);
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class Spawn {
    public GameObject soldier;
    public int minProb = 0;
    public int maxProb = 0;
}