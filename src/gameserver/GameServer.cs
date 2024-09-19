using VoxelGame.Voxels;

namespace VoxelGame.GameServer{
    public class GameServer {

        private VoxelStorage _voxelStorage;
        private Generator _generator;

        public GameServer(){
            _generator = new Generator();
            _voxelStorage = new VoxelStorage(_generator);
        }
    }
}