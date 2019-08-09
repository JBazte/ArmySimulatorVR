
using UnityEngine;
public class PersistableObject : MonoBehaviour
{

    public virtual void Save(GameDataWriter writer)
    {
        writer.Write(transform.position);
        writer.Write(transform.rotation);
        writer.Write(transform.localScale);
    }

    public virtual void Load(GameDataReader reader)
    {
        transform.position = reader.ReadVector3();
        transform.rotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
    }
}
