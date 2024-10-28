using VoxelGame.Logic;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace VoxelGame.Graphics{
    public class Canvas : GameObject {
        private Camera _camera;
        public Canvas(Camera camera){
            _camera = camera;
        }
        public override void Update () {
            Draw();
        }
        public void Draw(){
            for (int i = 0; i < UIelement.ElementsArray.Count; i++){
                UIelement.ElementsArray.ToArray()[i].Draw(_camera);
            }
        }
        
    }
    public class UIelement {

        public static ConcurrentBag<UIelement> ElementsArray = new ConcurrentBag<UIelement>();

        // position relative to the upper left corner in px
        public (int x, int y) position = (0, 0);

        public UIelement(){
            ElementsArray.Add(this);
        }
        ~UIelement(){
        //    var itemToRemove = resultlist.SingleOrDefault(r => r.Id == 2);
        //     if (itemToRemove != null)
        //         resultList.Remove(itemToRemove);
            Console.WriteLine("destroyed");
        }

        public virtual void Draw(Camera _camera){}

    }
}


