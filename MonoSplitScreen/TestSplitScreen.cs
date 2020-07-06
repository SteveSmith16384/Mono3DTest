using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Test3D
{

    public class TestSplitScreen : Game
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        private Model model;
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));// * Matrix.CreateScale(0.05f);
        //private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0.001f, 4), new Vector3(0, 0, 0), Vector3.UnitZ);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

        BasicEffect basicEffect;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        VertexPositionColor[] vertices;

        private Vector3 topCam = new Vector3(0, 0, 4);
        private Vector3 frontCam = new Vector3(0, 4, 0);
        private Vector3 sideCam = new Vector3(4, 0, 0);
        private Vector3 perspCam = new Vector3(4, 4, 4);

        private Matrix topView;// = Matrix.CreateLookAt(topCam, new Vector3(0, 0, 0), new Vector3(0, 0.001f, 1f));
        private Matrix frontView;// = Matrix.CreateLookAt(new Vector3(0, 4, 0), new Vector3(0, 0, 0), Vector3.UnitZ);
        private Matrix sideView;// = Matrix.CreateLookAt(new Vector3(4, 0, 0), new Vector3(0, 0, 0), Vector3.UnitZ);
        private Matrix perspectiveView;// = Matrix.CreateLookAt(new Vector3(4, 4, 4), new Vector3(0, 0, 0), Vector3.UnitZ);

        private Viewport topViewport;
        private Viewport frontViewport;
        private Viewport sideViewport;
        private Viewport perspectiveViewport;


        public TestSplitScreen()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            base.Initialize();

            topView = Matrix.CreateLookAt(topCam, new Vector3(0, 0, 0), new Vector3(0, 0.001f, 1f));
            frontView = Matrix.CreateLookAt(frontCam, new Vector3(0, 0, 0), Vector3.UnitZ);
            sideView = Matrix.CreateLookAt(sideCam, new Vector3(0, 0, 0), Vector3.UnitZ);
            perspectiveView = Matrix.CreateLookAt(perspCam, new Vector3(0, 0, 0), Vector3.UnitZ);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;
            //basicEffect.Texture = Content.Load<Texture2D>("ericbomb");
            //basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            topViewport = new Viewport();
            topViewport.X = 0;
            topViewport.Y = 0;
            topViewport.Width = 400;
            topViewport.Height = 240;
            topViewport.MinDepth = 0;
            topViewport.MaxDepth = 1;

            sideViewport = new Viewport();
            sideViewport.X = 400;
            sideViewport.Y = 0;
            sideViewport.Width = 400;
            sideViewport.Height = 240;
            sideViewport.MinDepth = 0;
            sideViewport.MaxDepth = 1;

            frontViewport = new Viewport();
            frontViewport.X = 0;
            frontViewport.Y = 240;
            frontViewport.Width = 400;
            frontViewport.Height = 240;
            frontViewport.MinDepth = 0;
            frontViewport.MaxDepth = 1;

            perspectiveViewport = new Viewport();
            perspectiveViewport.X = 400;
            perspectiveViewport.Y = 240;
            perspectiveViewport.Width = 400;
            perspectiveViewport.Height = 240;
            perspectiveViewport.MinDepth = 0;
            perspectiveViewport.MaxDepth = 1;

        }

        protected override void LoadContent()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("SimpleShip/Ship");
            //model = Content.Load<Model>("Alien");
            //model = Content.Load<Model>("KnightCharacter");
            //model = Content.Load<Model>("Smooth_Male_Casual");

            LoadCube();
        }

        protected override void UnloadContent()
        {
        }

        float angle = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            //angle += 0.01f;
            //world = Matrix.CreateRotationZ(angle);

            ReadControllers();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //DrawModel(model, world, view, projection);

            Viewport original = graphics.GraphicsDevice.Viewport;

            graphics.GraphicsDevice.Viewport = topViewport;
            DrawViewport(world, topView, projection);

            graphics.GraphicsDevice.Viewport = sideViewport;
            DrawViewport(world, sideView, projection);

            graphics.GraphicsDevice.Viewport = frontViewport;
            DrawViewport(world, frontView, projection);

            graphics.GraphicsDevice.Viewport = perspectiveViewport;
            DrawViewport(world, perspectiveView, projection);

            GraphicsDevice.Viewport = original;
            
            base.Draw(gameTime);
        }


        private void DrawViewport(Matrix world, Matrix view, Matrix projection)
        {
            // Draw model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }

            // Draw cube
            basicEffect.Projection = projection;
            basicEffect.View = view;
            basicEffect.World = world;

            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertices.Length - 1);
            }
        }


        private void ReadControllers()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.X == ButtonState.Pressed)
                {
                    //Console.WriteLine("Pressed!");
                    topCam.X += 0.2f;
                    topView = Matrix.CreateLookAt(topCam, new Vector3(0, 0, 0), new Vector3(0, 0.001f, 1f));
                }
                if (gamePadState.Buttons.Y == ButtonState.Pressed)
                {
                    //Console.WriteLine("Pressed!");
                    topCam.X -= 0.2f;
                    topView = Matrix.CreateLookAt(topCam, new Vector3(0, 0, 0), new Vector3(0, 0.001f, 1f));
                }
            }
        }


        protected void LoadCube()
        {
            basicEffect = new BasicEffect(GraphicsDevice);
            vertices = new VertexPositionColor[8];
            short[] indices = new short[36];
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(graphics.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            Color Color1 = Color.DarkRed;
            //Color Color2 = Color.Pink;
            float width = 2;
            float height = 2;
            float depth = 2;// 0.5f;

            vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color1);
            vertices[1] = new VertexPositionColor(new Vector3(width, 0, 0), Color1);
            vertices[2] = new VertexPositionColor(new Vector3(width, -height, 0), Color1);
            vertices[3] = new VertexPositionColor(new Vector3(0, -height, 0), Color1);
            vertices[4] = new VertexPositionColor(new Vector3(0, 0, depth), Color1);
            vertices[5] = new VertexPositionColor(new Vector3(width, 0, depth), Color1);
            vertices[6] = new VertexPositionColor(new Vector3(width, -height, depth), Color1);
            vertices[7] = new VertexPositionColor(new Vector3(0, -height, depth), Color1);

            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 0; indices[4] = 3; indices[5] = 2;
            indices[6] = 4; indices[7] = 0; indices[8] = 3;
            indices[9] = 4; indices[10] = 7; indices[11] = 3;
            indices[12] = 3; indices[13] = 7; indices[14] = 6;
            indices[15] = 3; indices[16] = 6; indices[17] = 2;
            indices[18] = 1; indices[19] = 5; indices[20] = 6;
            indices[21] = 1; indices[22] = 5; indices[23] = 2;
            indices[24] = 4; indices[25] = 5; indices[26] = 6;
            indices[27] = 4; indices[28] = 7; indices[29] = 6;
            indices[30] = 0; indices[31] = 1; indices[32] = 5;
            indices[33] = 0; indices[34] = 4; indices[35] = 5;

            vertexBuffer.SetData<VertexPositionColor>(vertices);
            indexBuffer.SetData(indices);
        }
    }

}

