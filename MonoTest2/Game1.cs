#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTest2.Graphics;

#endregion

namespace MonoGame3D
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;

        VertexPositionTexture[] floorVerts;
        BasicEffect effect;

        Texture2D checkerboardTexture;

        Sprite newSprite1;
        Sprite newSprite2;

        public Vector3 cameraPosition = new Vector3(0, 0, 100);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            checkerboardTexture = Content.Load<Texture2D>("ericbomb");

            newSprite1 = new Sprite(this, -40, -40, 20, 20, "lunarrover");
            newSprite2 = new Sprite(this, -60, -50, 20, 40, "junglesign");

            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            int repetitions = 1;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(graphics.GraphicsDevice);

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            effect.TextureEnabled = true;
            effect.Texture = checkerboardTexture;

            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitY;
            effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

            base.Initialize();
        }


        protected override void LoadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            //this.newSprite2.MoveBy(1, 0, 0);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend; // Need for transparent bits
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            GraphicsDevice.Clear(Color.Red);

            /*foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, floorVerts, 0, 2);
            }*/

            //this.newSprite1.Draw(graphics);
            this.newSprite2.Draw(graphics);

            base.Draw(gameTime);
        }

    }
}

