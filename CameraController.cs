using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace FractalViewer
{
    class CameraController
    {
        private Window window;
        private Vector3 position;
        private Vector3 rotation = new Vector3();
        private float moveSpeed = 5f;
        private float cameraSensitivity = 1 / 800f;
        private Vector2 rotationConstraints = new Vector2(-MathF.PI / 2, MathF.PI / 2);
        private Matrix4 projectionMatrix;
        private bool wKey, sKey, aKey, dKey, eKey, qKey;

        public CameraController()
        {
            window = Program.GetWindow();
            window.KeyUp += KeyUp;
            window.KeyDown += KeyDown;
            window.MouseMove += MouseMove;
            window.RenderFrame += Update;
        }

        public void Rotate(Vector3 rotation)
        {
            this.rotation -= rotation * cameraSensitivity;
            this.rotation.X = Math.Clamp(this.rotation.X, rotationConstraints.X, rotationConstraints.Y);
        }

       
        private void Update(FrameEventArgs e)
        {
            Vector3 movement = new Vector3();

            if (wKey)
                movement.Z -= 1;
            if (sKey)
                movement.Z += 1;
            if (dKey)
                movement.X += 1;
            if (aKey)
                movement.X -= 1;
            if (eKey)
                movement.Y -= 1;
            if (qKey)
                movement.Y += 1;

            Move(((Forward().Length > 0 ? Forward().Normalized() * movement.Z : new Vector3()) +
                (Right().Length > 0 ? Right().Normalized() * movement.X : new Vector3()) +
                Vector3.UnitY * movement.Y) * moveSpeed * (float)e.Time);
        }

        private void KeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    wKey = true;
                    break;
                case Keys.S:
                    sKey = true;
                    break;
                case Keys.A:
                    aKey = true;
                    break;
                case Keys.D:
                    dKey = true;
                    break;
                case Keys.E:
                    eKey = true;
                    break;
                case Keys.Q:
                    qKey = true;
                    break;
            }
        }
        private void KeyUp(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.W:
                    wKey = false;
                    break;
                case Keys.S:
                    sKey = false;
                    break;
                case Keys.A:
                    aKey = false;
                    break;
                case Keys.D:
                    dKey = false;
                    break;
                case Keys.E:
                    eKey = false;
                    break;
                case Keys.Q:
                    qKey = false;
                    break;
            }
        }
        private void MouseMove(MouseMoveEventArgs e)
        {
            Rotate(new Vector3(e.DeltaY, e.DeltaX, 0f));
        }

        public Matrix4 GetProjectionMatrix()
            => projectionMatrix;
        public void SetProjectionMatrix(Matrix4 projectionMatrix)
            => this.projectionMatrix = projectionMatrix;

        public void Move(Vector3 movement)
        {
            position += movement;
        }

        public Vector3 Position()
            => position;

        public Vector3 Forward()
            => (Vector4.UnitZ * CameraMatrix().Inverted()).Xyz;
        public Vector3 Right()
            => Vector3.Cross(Forward(), Vector3.UnitY);
        public Vector3 Left()
            => -Right();
        public Vector3 Backward()
            => -Forward();
        public Matrix4 CameraMatrix()
            => Matrix4.CreateTranslation(position) * CameraRotationMatrix();
        public Matrix4 CameraTranslationMatrix()
            => Matrix4.CreateTranslation(position);

        public Matrix4 CameraRotationMatrix()
            => Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation));
    }
}
