using System.Collections.Generic;

namespace VoxelGame.Logic{
    public class GameObject{
        private static List<GameObject> ObjectsArray = new List<GameObject>();

        public GameObject(){
            ObjectsArray.Add(this);
        }

        public virtual void Start(){}
        public virtual void Update(){}

        public static void GameObjectsUpdate(){
            for (int i = 0; i < ObjectsArray.Count; i++){
                ObjectsArray[i].Update();
            }
        }
        public static void GameObjectsStart(){
            for (int i = 0; i < ObjectsArray.Count; i++){
                ObjectsArray[i].Start();
            }
        }
    }
}