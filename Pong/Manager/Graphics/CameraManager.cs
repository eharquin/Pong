using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Manager.Graphics
{
    public class CameraManager
    {
        private Vector2 position;
        private float z;
        private float baseZ;

        private float aspectRatio;
        private float fieldOfView;

        private Matrix view;
        private Matrix projection;

        private static readonly float minZ = 1f;
        private static readonly float maxZ = 2048f;

        public Vector2 Position
        {
            get { return this.position; }
        }

        public float Z
        {
            get { return this.z; }
        }

        public Matrix View
        {
            get { return this.view; }
        }

        public Matrix Projection
        {
            get { return this.projection; }
        }

        public CameraManager(ScreenManager screen)
        {
            this.position = new Vector2(0, 0);
            this.aspectRatio = (float)screen.Width / screen.Height;
            this.fieldOfView = MathHelper.PiOver2;
            this.baseZ = getBaseZFromHeight(screen.Height);
            this.z = baseZ;

            UpdateMatrices();
        }

        public void UpdateMatrices()
        {
            this.view = Matrix.CreateLookAt(new Vector3(0, 0, z), Vector3.Zero, Vector3.Up);
            this.projection = Matrix.CreatePerspectiveFieldOfView(this.fieldOfView, this.aspectRatio, minZ, maxZ); // replace by maxZ and minZ 
        }

        private float getBaseZFromHeight(float height)
        {
            //CAH SOH **TOA

            float halfHeight = height / 2f;

            // TOA O = halfHeight A = ??  TAN = tan(fieldOfView/2f)

            float z = halfHeight / MathF.Tan(this.fieldOfView / 2f);

            return z;
        }

        public void MoveZ(float amount)
        {
            this.z += amount;
            this.z = MathHelper.Clamp(this.z, CameraManager.minZ, CameraManager.maxZ);
        }

        public void ResetZ()
        {
            this.z = this.baseZ;
        }
    }
}
