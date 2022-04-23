using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Manager.Graphics
{
    public sealed class ShapeManager : IDisposable, IServiceProvider
    {
        private bool isDisposed;
        private Game game;
        private BasicEffect effect;

        private VertexPositionColor[] vertices;
        private int[] indices;

        private static readonly int maxVertexCount = 1024;
        private static readonly int maxIndexCount = 1024;

        private int vertexCount;
        private int indexCount;
        private int shapeCount;

        private bool isStarted;

        public ShapeManager(Game game)
        {
            this.game = game ?? throw new ArgumentNullException("game");

            this.isDisposed = false;
            this.isStarted = false;

            this.effect = new BasicEffect(this.game.GraphicsDevice);
            this.effect.TextureEnabled = false;
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = false;
            this.effect.FogEnabled = false;
            this.effect.World = Matrix.Identity;
            this.effect.Projection = Matrix.Identity;
            this.effect.View = Matrix.Identity;

            this.vertices = new VertexPositionColor[maxVertexCount];
            this.indices = new int[maxIndexCount * 3];

            this.vertexCount = 0;
            this.indexCount = 0;
            this.shapeCount = 0;
        }

        public void Dispose()
        {
            if(this.isDisposed)
            {
                return;
            }

            this.effect?.Dispose();
            this.isDisposed = true;
        }

        private void EnsureStarted()
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException("Batching is not started.");
            }
        }

        private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if(shapeVertexCount > ShapeManager.maxVertexCount)
            {
                throw new InvalidOperationException("Maximum shape vertex count is: " + ShapeManager.maxVertexCount);
            }

            if (shapeIndexCount > ShapeManager.maxIndexCount)
            {
                throw new InvalidOperationException("Maximum shape index count is: " + ShapeManager.maxIndexCount);
            }

            if(this.vertexCount + shapeVertexCount > ShapeManager.maxVertexCount ||
               this.indexCount + shapeIndexCount > ShapeManager.maxIndexCount)
            {
                Flush();
            }
        }
#nullable enable
        public void Begin(CameraManager? camera)
        {
            if(this.isStarted)
            {
                throw new InvalidOperationException("Batching is already started.");
            }

            if (camera is null)
            {
                Viewport viewport = game.GraphicsDevice.Viewport;
                //this.effect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, 0f, viewport.Height, 0, 1);
                this.effect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, viewport.Height, 0f, 0f, 1f);
            }
            else
            {
                camera.UpdateMatrices();
                this.effect.Projection = camera.Projection;
                this.effect.View = camera.View;
            }

            this.isStarted = true;
        }
#nullable disable

        public void DrawPlayer(float x, float y, float width, float height, Color color)
        {
            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureStarted();
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            // Vertex
            this.vertices[vertexCount + 0] = new VertexPositionColor(new Vector3(x - width / 2, y - height / 2, 0), color);
            this.vertices[vertexCount + 1] = new VertexPositionColor(new Vector3(x + width / 2, y - height / 2, 0), color);
            this.vertices[vertexCount + 2] = new VertexPositionColor(new Vector3(x + width / 2, y + height / 2, 0), color);
            this.vertices[vertexCount + 3] = new VertexPositionColor(new Vector3(x - width / 2, y + height / 2, 0), color);

            this.indices[indexCount + 0] = this.vertexCount + 0;
            this.indices[indexCount + 1] = this.vertexCount + 1;
            this.indices[indexCount + 2] = this.vertexCount + 2;
            this.indices[indexCount + 3] = this.vertexCount + 0;
            this.indices[indexCount + 4] = this.vertexCount + 2;
            this.indices[indexCount + 5] = this.vertexCount + 3;

            vertexCount += shapeVertexCount;
            indexCount += shapeIndexCount;
            shapeCount++;
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureStarted();
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            this.vertices[vertexCount + 0] = new VertexPositionColor(new Vector3(x, y + height, 0f), color);
            this.vertices[vertexCount + 1] = new VertexPositionColor(new Vector3(x + width, y + height, 0f), color);
            this.vertices[vertexCount + 2] = new VertexPositionColor(new Vector3(x + width, y, 0f), color);
            this.vertices[vertexCount + 3] = new VertexPositionColor(new Vector3(x, y, 0f), color);

            this.indices[indexCount + 0] = this.vertexCount + 0;
            this.indices[indexCount + 1] = this.vertexCount + 1;
            this.indices[indexCount + 2] = this.vertexCount + 2;
            this.indices[indexCount + 3] = this.vertexCount + 0;
            this.indices[indexCount + 4] = this.vertexCount + 2;
            this.indices[indexCount + 5] = this.vertexCount + 3;

            vertexCount += shapeVertexCount;
            indexCount += shapeIndexCount;
            shapeCount++;
        }

        public void DrawLine(Vector2 p1, Vector2 p2, float thickness, Color color)
        {
            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureStarted();
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float halfThickness = thickness / 2f;

            Vector2 v1 = p2 - p1;
            v1.Normalize();
            v1 *= halfThickness;

            Vector2 v2 = -v1;

            Vector2 n1 = new Vector2(-v1.Y, v1.X);
            Vector2 n2 = -n1;

            Vector2 a = p1 + v2 + n1;
            Vector2 b = p2 + v1 + n1;
            Vector2 c = p2 + v1 + n2;
            Vector2 d = p1 + v2 + n2;

            this.vertices[this.vertexCount + 0] = new VertexPositionColor(new Vector3(a, 0f), color);
            this.vertices[this.vertexCount + 1] = new VertexPositionColor(new Vector3(b, 0f), color);
            this.vertices[this.vertexCount + 2] = new VertexPositionColor(new Vector3(c, 0f), color);
            this.vertices[this.vertexCount + 3] = new VertexPositionColor(new Vector3(d, 0f), color);

            this.indices[this.indexCount + 0] = vertexCount + 0;
            this.indices[this.indexCount + 1] = vertexCount + 1;
            this.indices[this.indexCount + 2] = vertexCount + 2;
            this.indices[this.indexCount + 3] = vertexCount + 0;
            this.indices[this.indexCount + 4] = vertexCount + 2;
            this.indices[this.indexCount + 5] = vertexCount + 3;

            this.vertexCount += shapeVertexCount;
            this.indexCount += shapeIndexCount;
            this.shapeCount++;
        }

        public void End()
        {
            EnsureStarted();

            this.Flush();
            this.isStarted = false;
        }

        public void Flush()
        {
            if(this.shapeCount == 0)
            {
                return;
            }
            
            foreach(EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, this.vertices, 0, this.vertexCount, this.indices, 0, this.indexCount / 3);
            }

            this.vertexCount = 0;
            this.indexCount = 0;
            this.shapeCount = 0;
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
