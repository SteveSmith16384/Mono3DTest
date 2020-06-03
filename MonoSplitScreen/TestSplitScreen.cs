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
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        //private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0.001f, 4), new Vector3(0, 0, 0), Vector3.UnitZ);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

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
        }

        protected override void UnloadContent()
        {
        }

        float angle = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            angle += 0.01f;
            world = Matrix.CreateRotationZ(angle);

            ReadControllers();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //DrawModel(model, world, view, projection);

            Viewport original = graphics.GraphicsDevice.Viewport;

            graphics.GraphicsDevice.Viewport = topViewport;
            DrawModel(model, world, topView, projection);

            graphics.GraphicsDevice.Viewport = sideViewport;
            DrawModel(model, world, sideView, projection);

            graphics.GraphicsDevice.Viewport = frontViewport;
            DrawModel(model, world, frontView, projection);

            graphics.GraphicsDevice.Viewport = perspectiveViewport;
            DrawModel(model, world, perspectiveView, projection);

            GraphicsDevice.Viewport = original;
            
            base.Draw(gameTime);
        }


        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
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
    }

}

