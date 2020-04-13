#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace MonoGame3D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        VertexPositionTexture[] floorVerts;
        BasicEffect effect;

        Texture2D checkerboardTexture;

        VertexPositionTexture[] newSprite;

        //Vector3 cameraPosition = new Vector3(15, 10, 10);
        Vector3 cameraPosition = new Vector3(0, 0, 100);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            newSprite = this.CreateSprite(10, 10);

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
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            effect.TextureEnabled = true;
            effect.Texture = checkerboardTexture;

            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitY;

            effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

            base.Initialize();
        }


        private VertexPositionTexture[] CreateSprite(float x, float y)
        {
            VertexPositionTexture[] verts = new VertexPositionTexture[6];

            verts[0].Position = new Vector3(x-20, y-20, 0);
            verts[1].Position = new Vector3(x-20, y+20, 0);
            verts[2].Position = new Vector3(x+20, y-20, 0);

            verts[3].Position = verts[1].Position;
            verts[4].Position = new Vector3(x+20, y+20, 0);
            verts[5].Position = verts[2].Position;

            int repetitions = 1;

            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(0, repetitions);
            verts[2].TextureCoordinate = new Vector2(repetitions, 0);

            verts[3].TextureCoordinate = verts[1].TextureCoordinate;
            verts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            verts[5].TextureCoordinate = verts[2].TextureCoordinate;

            return verts;
        }

        protected override void LoadContent()
        {
            checkerboardTexture = Content.Load<Texture2D>("ericbomb");
            //checkerboardTexture.
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend; // Need for transparent bits
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            //GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GraphicsDevice.Clear(Color.Red);

            DrawGround();

            base.Draw(gameTime);

        }

        void DrawGround()
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                //this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two triangles
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    floorVerts,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);

                graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two triangles
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    this.newSprite,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }

    }
}

