using System;
using VoxelGame.Graphics;
using VoxelGame.Time;
using VoxelGame.Logic;
using VoxelGame.Voxels;
using VoxelGame.World;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Input;
using System.Threading;
using System.Diagnostics;
using VoxelGame.Lighting;

namespace VoxelGame
{
    public class Window : GameWindow
    {
        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private double _time;

        private FPSCounter FPSCounter;

        private Generator generator;
        private VoxelStorage voxelStorage;
        private LightMap lightMap = new LightMap();
        private ChunkRenderer meshBuilder;
        private ChunksController chunksController;
        private Canvas canvas;

        private Text _text;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {}

        protected override void OnLoad()
        {
            base.OnLoad();

            GameObject.GameObjectsStart();

            GL.ClearColor(0.67f, 0.84f, 0.9f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            loadBlocks();
            generator = new Generator();
            voxelStorage = new VoxelStorage(generator);
            lightMap = new LightMap();
            meshBuilder = new ChunkRenderer(voxelStorage, lightMap);
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            chunksController = new ChunksController(_camera, voxelStorage, lightMap);
            canvas = new Canvas(_camera);
            new AxisLines();
            _text = new Text("");

            CursorState = CursorState.Grabbed;

            FPSCounter = new FPSCounter();
        }

        protected void loadBlocks(){
            new Block(0, BlockModel.Air, [(0, 0)]);
            new Block(1, BlockModel.Cube, [(1, 0)]);
            new Block(2, BlockModel.Cube, [(1, 1), (0, 1), (0, 0)]);
            new Block(3, BlockModel.Foliage, [(2, 2)]);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GameObject.GameObjectsUpdate();

            SwapBuffers();

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            Mesh mesh = new Mesh([],[]);
            mesh.loadBuffers();

            // cleaning buffers of deleted objects
            BuffersCleaner.Clean();

            var input = KeyboardState;
            var mouse = MouseState;

            if (mouse.IsButtonDown(MouseButton.Right))
            {
                if (chunksController.RayCast(_camera.Position, _camera.Front, 10.0f) != null){
                    chunksController.RayCast(_camera.Position, _camera.Front, 10.0f);
                    ///_text.Update("the voxel id is " + vox.Id);
                }
            }


            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // if (chunksController.IsObstableAt(new Vector3(_camera.Position.X, _camera.Position.Y, _camera.Position.Z))) {
            //     _text.Update("Obstable");
            // } else {
            //     _text.Update("Free");
            // }

            const float cameraSpeed = 15.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            if (_firstMove) 
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; 
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X * 2, Size.Y * 2);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}