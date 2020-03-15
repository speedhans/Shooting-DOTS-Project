using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

public struct SpriteData : IBufferElementData
{
    //public UnityEngine.Sprite Sprite;
    public int Value;
}

public struct SpriteAnimationData : IComponentData
{
    //public UnityEngine.Sprite[] Sprite;
}

[DisableAutoCreation]
public class SpriteAnimationSystem : ComponentSystem
{
    NativeHashMap<int, SpriteData> m_AnimationDic;

    protected override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
