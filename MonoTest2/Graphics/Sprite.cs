using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTest2.Graphics
{
    public class Sprite
    {
        private Game1 game;
        private BasicEffect effect;
        private VertexPositionTexture[] verts;
        private Vector3 positionVec;
        private Matrix positionMatrix, scale, rotation;

        public Sprite(Game1 _game, float x, float y, int w, int h, string texture)
        {
            game = _game;

            positionVec = new Vector3(x, y, 0);

            effect = new BasicEffect(game.graphics.GraphicsDevice);

            positionMatrix = Matrix.CreateTranslation(x, y, 0);
            scale = Matrix.CreateScale(1);
            rotation = Matrix.CreateFromYawPitchRoll(0, 0, 0);

            float aspectRatio = game.graphics.PreferredBackBufferWidth / (float)game.graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            effect.TextureEnabled = true;
            effect.Texture = game.Content.Load<Texture2D>(texture);

            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitY;
            effect.View = Matrix.CreateLookAt(game.cameraPosition, cameraLookAtVector, cameraUpVector);

            verts = new VertexPositionTexture[4];

            verts[0].Position = new Vector3(0, 0, 0);
            verts[1].Position = new Vector3(0, h, 0);
            verts[2].Position = new Vector3(w, 0, 0);
            verts[3].Position = new Vector3(w, h, 0);


            //---
            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(0, 1);
            verts[2].TextureCoordinate = new Vector2(1, 0);
            verts[3].TextureCoordinate = new Vector2(1, 1);
        }


        public void MoveTo(float x, float y, float z)
        {
            this.positionVec.X = x;
            this.positionVec.Y = y;
            this.positionVec.Z = z;
            positionMatrix = Matrix.CreateTranslation(this.positionVec);
        }


        public void MoveBy(float x, float y, float z)
        {
            this.positionVec.X += x;
            this.positionVec.Y += y;
            this.positionVec.Z += z;
            positionMatrix = Matrix.CreateTranslation(this.positionVec);
        }


        public void Draw(GraphicsDeviceManager graphics)
        {
            effect.World = this.positionMatrix * scale * rotation;
            //effect.World = Matrix.CreateConstrainedBillboard(this.positionVec, game.cameraPosition, 
             //   Vector3.Zero, Vector3.UnitY, Vector3.UnitY);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, verts, 0, 2);
            }

        }
    }
}
