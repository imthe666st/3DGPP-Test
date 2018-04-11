using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RenderTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

	    private Model model;

	    private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
	    private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
	    private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 800f / 480f, 0.1f, 100f);

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			model = Content.Load<Model>("cup");
		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
			spriteBatch.Begin();
	        DrawModel(model, world, view, projection);
			spriteBatch.End();

            base.Draw(gameTime);
        }

	    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
	    {
		    foreach (ModelMesh mesh in model.Meshes)
		    {
			    foreach (BasicEffect effect in mesh.Effects)
			    {
				    effect.World = world;
				    effect.View = view;
				    effect.Projection = projection;

				    effect.EnableDefaultLighting();
				    //effect.LightingEnabled = true; // Turn on the lighting subsystem.

				    //effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.2f, 0.2f); // a reddish light
				    //effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
				    //effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

				    //effect.AmbientLightColor = new Vector3(1f, 1f, 1f); // Add some overall ambient light.
				    ////effect.EmissiveColor = new Vector3(1, 0, 0); // Sets some strange emmissive lighting.  This just looks weird.

				}

				mesh.Draw();
		    }
	    }
	}
}
