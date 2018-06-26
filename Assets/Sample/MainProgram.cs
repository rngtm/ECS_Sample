    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Rendering;
    using Unity.Mathematics;
    using Unity.Collections;

    public class MainProgram : MonoBehaviour
    {
        const int k_CubeCount = 10000; // キューブの個数
        const float k_CubeRange = 200f; // キューブを配置する範囲

        [SerializeField] MeshInstanceRendererComponent m_CubeLook;　// キューブの見た目。 Prefabを指定しても動く
        NativeArray<Entity> m_EntityArray; // 作成したエンティティ

        void Start()
        {
            m_EntityArray = new NativeArray<Entity>(k_CubeCount, Allocator.Persistent);

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // アーキタイプ (エンティティの元となるもの)
            // エンティティと紐づくデータをここで指定する
            var cubeArchType = entityManager.CreateArchetype(
                    typeof(Position), // 位置を扱うデータ「Position」を登録
                    typeof(TransformMatrix) // 行列を扱うデータ「TransformMatrix」　Transformがサポートしている移動・回転・拡大縮小を扱える
                );
            
            // EntityManagerへの登録
            for (int i = 0; i < k_CubeCount; i++)
            {
                // エンティティ(実体)を作成
                // 指定したデータを要求するComponentSystemが動き始める
                var cube = entityManager.CreateEntity(cubeArchType); // Entityの作成

                // データの設定
                // このデータはComponentSystemの処理に利用される
                entityManager.SetComponentData(cube, new Position // 位置の設定
                {
                    Value = new float3(
                        Random.Range(-0.5f, 0.5f) * k_CubeRange,
                        Random.Range(-0.5f, 0.5f) * k_CubeRange,
                        Random.Range(-0.5f, 0.5f) * k_CubeRange
                    ) 
                });
                entityManager.AddSharedComponentData(cube, m_CubeLook.Value); // 見た目を登録

                m_EntityArray[i] = cube; 
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                TryDestroyEntity();
            }
        }
        
        // エンティティを削除
	    void TryDestroyEntity()
	    {
            if (!m_EntityArray.IsCreated) { return; } 

            Debug.Log("DestroyEntity");

	    	// 作ったEntityを削除
	    	if( World.Active != null)
	    	{
	    		var entityManager = World.Active.GetExistingManager<EntityManager>();
	    		entityManager.DestroyEntity(m_EntityArray);
	    	}

            m_EntityArray.Dispose();
	    }
    }