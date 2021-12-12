/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mechanical
{
    /// <summary>
    /// The engine class is a base class for games.
    /// </summary>
    public class Engine : Game
    {
        #region Variables
        /// <summary>
        /// The arguments passed into the program.
        /// </summary>
        public string[] Arguments { get; set; }

        /// <summary>
        /// The Graphics Device Manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        /// <summary>
        /// The main directory for the content manager.
        /// </summary>
        public string ContentDirectory { get; set; } = "Content";

        /// <summary>
        /// The title of the game window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The size of the window.
        /// </summary>
        public Vector2 WindowSize { get => Window.ClientBounds.Size.ToVector2(); }

        /// <summary>
        /// The window's position.
        /// </summary>
        public Vector2 WindowPosition { get => Window.Position.ToVector2(); set => Window.Position = value.ToPoint(); }

        /// <summary>
        /// The witdth of the application's window.
        /// </summary>
        public int WindowWidth { 
            get => (int)WindowSize.X; 
            set
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = value;
                GraphicsDeviceManager.ApplyChanges();
            } 
        }

        /// <summary>
        /// The height of the application's window.
        /// </summary>
        public int WindowHeight { 
            get => (int)WindowSize.Y;              
            set
            {
                GraphicsDeviceManager.PreferredBackBufferHeight = value;
                GraphicsDeviceManager.ApplyChanges();
            }
        }

        /// <summary>
        /// The X position of the window.
        /// </summary>
        public int X { get => (int)WindowPosition.X; set => WindowPosition.SetX(value); }

        /// <summary>
        /// The Y position of the window.
        /// </summary>
        public int Y { get => (int)WindowPosition.Y; set => WindowPosition.SetY(value); }

        /// <summary>
        /// If the game is fullscreen.
        /// </summary>
        public bool IsFullscreen { get; set; }

        /// <summary>
        /// The clear color.
        /// </summary>
        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        /// The time since the last frame.
        /// </summary>
        public double DeltaTime { get; set; }

        /// <summary>
        /// The raw delta time without modification from the <see cref="TimeScale"/>.
        /// </summary>
        public double RawDeltaTime { get; set; }

        /// <summary>
        /// The number to change the <see cref="DeltaTime"/> by.
        /// </summary>
        public float TimeScale { get; set; } = 1;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

#if DEBUG
        /// <summary>
        /// If the game should exit when the ESC key is pressed. DEBUG only.
        /// </summary>
        public bool ExitOnEscape { get; set; } = true;
#endif

        /// <summary>
        /// The camera of the game.
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// The instance of the engine.
        /// </summary>
        public static Engine Instance { get; private set; }

        /// <summary>
        /// The size of the game. This will be the size the of render targets created that will be used to draw the scene to.
        /// </summary>
        public Vector2 GameSize { get; set; } = new Vector2(1280, 720);

        /// <summary>
        /// The width of the game window.
        /// </summary>
        public int GameWidth { get => (int)GameSize.X; set => GameSize.SetX(value); }

        /// <summary>
        /// The height of the game window.
        /// </summary>
        public int GameHeight { get => (int)GameSize.Y; set => GameSize.SetY(value); }

        /// <summary>
        /// The width of the camera.
        /// </summary>
        public int CameraWidth { get => Camera.Width; set => Camera.Width = value; }

        /// <summary>
        /// The height of the camera.
        /// </summary>
        public int CameraHeight { get => Camera.Height; set => Camera.Height = value; }

        /// <summary>
        /// The screen.
        /// </summary>
        public Screen Screen { get; protected set; }

        /// <summary>
        /// The width of the render target created by the <see cref="Screen"/>.
        /// </summary>
        private int RenderTargetWidth { get; set; }

        /// <summary>
        /// The width of the render target created by the <see cref="Screen"/>.
        /// </summary>
        private int RenderTargetHeight { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// The main constructor for the Engine.
        /// </summary>
        /// <param name="args">The arguments passed into the game.</param>
        public Engine(string[] args, int targetWidth, int targetHeight, int windowWidth, int windowHeight, bool fullscreen) : base()
        {
            Arguments = args;
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = ContentDirectory;
            IsMouseVisible = true;
            Instance = this;

            GraphicsDeviceManager.PreferredBackBufferWidth = windowWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = windowHeight;
            GraphicsDeviceManager.IsFullScreen = fullscreen;
            GraphicsDeviceManager.ApplyChanges();

            RenderTargetWidth = targetWidth;
            RenderTargetHeight = targetHeight;

            IsFullscreen = fullscreen;
        }
        #endregion

        #region Setup

        /// <summary>
        /// Setup the game window.
        /// </summary>
        public virtual void SetupWindow()
        {
            Window.Title = Title;

            // center
            WindowPosition = Window.Position.ToVector2();

            GraphicsDeviceManager.PreferredBackBufferWidth = WindowWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = WindowHeight;
            GraphicsDeviceManager.IsFullScreen = IsFullscreen;
            GraphicsDeviceManager.ApplyChanges();
            
            IsMouseVisible = false;

            // make the screen.
            Screen = new Screen(RenderTargetWidth, RenderTargetHeight);

#if DEBUG
            Window.AllowUserResizing = true;
#endif
        }

        /// <summary>
        /// The default begin function for the spritebatch.
        /// </summary>
        /// <remarks>
        /// Change this when the camera class and draw class is set up.
        /// </remarks>
        /// <param name="effect"></param>
        /// <param name="transformMatrix"></param>
        public virtual void DefaultBeginBatch(Effect effect = null)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, effect: effect, transformMatrix: Camera.TransformationMatrix);
        }

        protected override void Initialize()
        {
            SetupWindow();
            Camera = new Camera(GraphicsDevice.Viewport);

            MechController.Initialize();

            //GraphicsDeviceManager.DeviceCreated += OnGraphicsDeviceCreated;
            //GraphicsDeviceManager.DeviceReset += OnGraphicsDeviceReset;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Drawing.SpriteBatch = SpriteBatch;

            Drawing.LoadContent(Content, this);

            base.LoadContent();
        }

        #endregion

        #region Frame-By-Frame
        protected override void Update(GameTime gameTime)
        {
            RawDeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeScale;

            // update the input system.
            MechController.Update((float)DeltaTime);
            MechKeyboard.Update((float)DeltaTime);
            MechMouse.Update((float)DeltaTime);
            Keybinds.Update((float)DeltaTime);

#if DEBUG
            if (MechKeyboard.IsKeyDown(Keys.Escape) && ExitOnEscape) Exit();
#endif

            base.Update(gameTime);
        }

        protected override bool BeginDraw()
        {
            // draw using the target.
            GraphicsDevice.SetRenderTarget(Screen.RenderTarget);
            GraphicsDevice.Clear(ClearColor);

            DefaultBeginBatch();
            return base.BeginDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            // stop game rendering.
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // draw the screen.
            Screen.Draw(this);

            base.EndDraw();
        }

        #endregion

        #region Finished
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        #endregion

    }
}
