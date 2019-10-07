using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawnZone : SpawnZone
{
    [SerializeField]
    bool surfaceOnly;
    [SerializeField]
    bool bidimensional;
    public override Vector3 SpawnPoint
    {
        get
        {
            Vector3 p;

            p.x = Random.Range(-.5f, .5f);
            p.y = Random.Range(-.5f, .5f);
            p.z = Random.Range(-.5f, .5f);

            if (surfaceOnly)
            {
                int axis = Random.Range(0, 3);
                p[axis] = p[axis] < 0f ? -.5f : .5f;
            }
            if (bidimensional)
            {
                p.y = -0.5f;
            }
            return transform.TransformPoint(p);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
    }

    public override Enemy SpawnEnemy()
    {
        return base.SpawnEnemy();
    }

}
