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
        private Texture2D texture;

        public Sprite(Game1 game, int x, int y, int w, int h, string texture)
        {
            effect = new BasicEffect(game.graphics.GraphicsDevice);

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

            this.texture = game.Content.Load<Texture2D>(texture);
        }

        public void Draw(GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);

        }
    }
}
