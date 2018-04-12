using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

		private Texture2D whitePixel;

		private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
		private Matrix view; //= Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
		private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
		
		
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
			model = Content.Load<Model>("coin");

			whitePixel = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
			whitePixel.SetData(new [] { Color.AliceBlue });
			
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		// Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);

		private Vector3 cameraPosition = new Vector3(-20, 0, 0);

		private double xzPlaneViewAngle = 0.0d;

		private double rotation = 0.0f;
		
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

			// create new view Matrix

			this.rotation += gameTime.ElapsedGameTime.TotalSeconds;
			//Console.WriteLine($"{rotation}");
			
			var ks = Keyboard.GetState();

			if (ks.IsKeyDown(Keys.Z)) xzPlaneViewAngle += 1 * gameTime.ElapsedGameTime.TotalSeconds;
			if (ks.IsKeyDown(Keys.C)) xzPlaneViewAngle += -1 * gameTime.ElapsedGameTime.TotalSeconds;

			Vector3 target = new Vector3((float)Math.Cos(xzPlaneViewAngle), 0, (float)Math.Sin(xzPlaneViewAngle));

			target.Normalize();

			Vector3 forward = target;
			Vector3 backwards = -forward;

			Vector3 right = new Vector3(forward.Z, 0, -forward.X);
			Vector3 left = -right;

			var movementVector = new Vector3();

			if (ks.IsKeyDown(Keys.W)) movementVector += forward;
			if (ks.IsKeyDown(Keys.S)) movementVector += backwards;
			if (ks.IsKeyDown(Keys.A)) movementVector += right;
			if (ks.IsKeyDown(Keys.D)) movementVector += left;

			if (ks.IsKeyDown(Keys.Q)) movementVector += new Vector3(0, 1, 0);
			if (ks.IsKeyDown(Keys.E)) movementVector += new Vector3(0, -1, 0);

			if (movementVector.LengthSquared() > 0f) movementVector.Normalize();
			movementVector *= 5.0f;
			
			this.cameraPosition += movementVector * (float)gameTime.ElapsedGameTime.TotalSeconds;


			// create target vector

			this.view = Matrix.CreateLookAt(this.cameraPosition, this.cameraPosition + 5f * target, Vector3.UnitY);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values />
		protected override void Draw(GameTime gameTime)
		{
		
			GraphicsDevice.Clear(Color.CornflowerBlue);
		
			#region ResetGraphic
		
			ResetGraphic();
		
			#endregion
			#region render 3D
			BeginRender3D();

			DrawModel(model, world, view, projection);
			
			#endregion
			#region render 2D
		
			//Render 2D here
			#endregion
		
		}
		
		public void ResetGraphic()
		{			  
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		
		}
		
		public void BeginRender3D()
		{
			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		}

		private void DrawModel(Model _model, Matrix _world, Matrix _view, Matrix _projection)
		{
			var transformMatrices = new Matrix[_model.Bones.Count];
			_model.CopyAbsoluteBoneTransformsTo(transformMatrices);
			
			foreach (ModelMesh mesh in _model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = Matrix.CreateScale(0.001f) * Matrix.CreateRotationY((float) (rotation)) * _world * transformMatrices[mesh.ParentBone.Index];
					effect.View = _view;
					effect.Projection = _projection;

					effect.TextureEnabled = true;
					effect.Texture = whitePixel;
					
					effect.EnableDefaultLighting();
					//effect.LightingEnabled = true; // Turn on the lighting subsystem.

					effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); // some diffuse light
					effect.DirectionalLight0.Direction = new Vector3(1, 1, 0);  // coming along the x-axis
					effect.DirectionalLight0.SpecularColor = new Vector3(0.05f, 0.05f, 0.05f); // a tad of specularity

					effect.AmbientLightColor = new Vector3(0.15f, 0.15f, 0.215f); // Add some overall ambient light.
					//effect.EmissiveColor = new Vector3(1, 0, 0); // Sets some strange emmissive lighting.  This just looks weird.

				}

				mesh.Draw();
			}
		}
	}
}
