using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TestECS1 : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //DynamicBuffer<SpriteData> dynamicbuffer = dstManager.AddBuffer<SpriteData>(entity);
        //dynamicbuffer.Add(new SpriteData() { Value = 1 });
    }
}
