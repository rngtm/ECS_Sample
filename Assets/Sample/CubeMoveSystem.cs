using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// キューブの移動を行うシステム
public class CubeMoveSystem : ComponentSystem
{
    [Inject] CubeGroup m_CubeGroup;

	// ComponentSystemが要求するデータ
    public struct CubeGroup 
    {
        public ComponentDataArray<Position> Positions; // CubeMoveSystemは位置を利用するので「Position」を要求。 
        public int Length; 
    }
    
	// OnUpdateはメインスレッドで毎フレーム実行される
    protected override void OnUpdate()
    {
        for (int i = 0; i < m_CubeGroup.Length; i++) // すべてのキューブに対して処理を行う
        {
            var position = m_CubeGroup.Positions[i];
            position.Value += new float3(0f, Mathf.Sin(Time.time * 2f), 0f); // とりあえずサイン波で動かす
            m_CubeGroup.Positions[i] = position; 
        }
    }
}
