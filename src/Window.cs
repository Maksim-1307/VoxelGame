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
using System.Threading;

namespace VoxelGame
{

    public class Window : GameWindow
    {
        private readonly float[] _vertices =
        {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _EBO;
        private int _VBO;
        private int _VAO;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private double _time;

        private FPSCounter FPSCounter;

        private Generator generator;
        private VoxelStorage voxelStorage;
        private ChunkRenderer meshBuilder;
        private ChunksController chunksController;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {}

        protected override void OnLoad()
        {
            base.OnLoad();

            GameObject.GameObjectsStart();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            generator = new Generator();
            voxelStorage = new VoxelStorage(generator);
            meshBuilder = new ChunkRenderer(voxelStorage);
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            chunksController = new ChunksController(_camera, voxelStorage);
            
            CursorState = CursorState.Grabbed;

            FPSCounter = new FPSCounter();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            FPSCounter.Update();
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

            //GameObject.GameObjectsUpdate();

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

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


            var mouse = MouseState;

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