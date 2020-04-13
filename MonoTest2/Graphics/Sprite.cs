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
        private BasicEffect effect;
        private VertexPositionTexture[] verts;

        public Sprite(Game1 game, int x, int y, int w, int h, string texture)
        {
            effect = new BasicEffect(game.graphics.GraphicsDevice);

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


            verts = new VertexPositionTexture[6];

            verts[0].Position = new Vector3(0, 0, 0);
            verts[1].Position = new Vector3(0, h, 0);
            verts[2].Position = new Vector3(w, 0, 0);
            verts[3].Position = verts[1].Position;
            verts[4].Position = new Vector3(h, w, 0);
            verts[5].Position = verts[2].Position;

            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(0, 1);
            verts[2].TextureCoordinate = new Vector2(1, 0);
            verts[3].TextureCoordinate = verts[1].TextureCoordinate;
            verts[4].TextureCoordinate = new Vector2(1, 1);
            verts[5].TextureCoordinate = verts[2].TextureCoordinate;

        }

        public void Draw(GraphicsDeviceManager graphics)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
            }

        }
    }
}
