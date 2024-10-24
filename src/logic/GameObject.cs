using System.Collections.Generic;
using System.Collections.Concurrent;

namespace VoxelGame.Logic{
    public class GameObject{
        private static ConcurrentBag<GameObject> ObjectsArray = new ConcurrentBag<GameObject>();

        public GameObject(){
            ObjectsArray.Add(this);
        }

        public virtual void Start(){}
        public virtual void Update(){}

        public static void GameObjectsUpdate(){
            for (int i = 0; i < ObjectsArray.Count; i++){
                ObjectsArray.ToArray()[i].Update();
            }
        }
        public static void GameObjectsStart(){
            for (int i = 0; i < ObjectsArray.Count; i++){
                ObjectsArray.ToArray()[i].Start();
            }
        }
    }
}