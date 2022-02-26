/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
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
        public float DeltaTime { get; set; }

        /// <summary>
        /// The raw delta time without modification from the <see cref="TimeScale"/>.
        /// </summary>
        public float RawDeltaTime { get; set; }

        /// <summary>
        /// The number to change the <see cref="DeltaTime"/> by.
        /// </summary>
        public float TimeScale { get; set; } = 1;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// If the game should exit when the ESC key is pressed.
        /// </summary>
        public bool ExitOnEscape { get; set; } = true;

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
        public Vector2 GameSize
        {   
            get => new Vector2(Screen.TargetWidth, Screen.TargetHeight); 
            set 
            { 
                Screen.ScreenWidth = (int)value.X;
                Screen.ScreenHeight = (int)value.Y;
            } 
        }
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

        /// <summary>
        /// If the game is in debug mode. Debug mode will allow the console to be used and other debuging tools.
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// If the game will allow the console.
        /// </summary>
        public bool AllowConsole { get; set; } = true;

        /// <summary>
        /// If the game is paused.
        /// </summary>
        public bool Paused { get; set; } = false;

        /// <summary>
        /// If the game should use the <see cref="Scene.DebugDraw(bool)"/> function when drawing.
        /// </summary>
        public bool ShouldDebugDraw { get; set; } = false;

        #endregion

        #region Constructor
        /// <summary>
        /// The main constructor for the Engine.
        /// </summary>
        /// <param name="args">The arguments passed into the game.</param>
        /// <param name="fullscreen">If the game should start in fullscreen.</param>
        /// <param name="targetHeight">The height of the game's render target.</param>
        /// <param name="targetWidth">The wdith of the game's render target.</param>
        /// <param name="windowHeight">The height of the window.</param>
        /// <param name="windowWidth">The width of the window.</param>
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

            // make the screen.
            Screen = new Screen(RenderTargetWidth, RenderTargetHeight);
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


#if DEBUG
            DebugMode = true;
#else
            DebugMode = false;
#endif

            Window.AllowUserResizing = DebugMode;
        }

        /// <summary>
        /// The default begin function for the spritebatch.
        /// </summary>
        /// <remarks>
        /// Change this when the camera class and draw class is set up.
        /// </remarks>
        /// <param name="effect"></param>
        public virtual void DefaultBeginBatch(Effect effect = null)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, effect: effect, transformMatrix: Camera.TransformationMatrix);
        }

        protected override void Initialize()
        {
            SetupWindow();

            MechController.Initialize();

            if (DebugMode || AllowConsole)
            {
                Console.Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Drawing.SpriteBatch = SpriteBatch;

            Drawing.LoadContent(Content, this);

            //SceneManager.LoadContent(Content);

            Camera = new Camera(new Viewport(0, 0, GameWidth, GameHeight));

            if (DebugMode || AllowConsole)
            {
                Console.LoadContent(Content);
            }

            base.LoadContent();
        }

#endregion

        #region Frame-By-Frame
        protected override void Update(GameTime gameTime)
        {
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeScale;

            // update the input system.
            MechController.Update(DeltaTime);
            MechKeyboard.Update(DeltaTime);
            MechMouse.Update(DeltaTime);
            Keybinds.Update(DeltaTime);

            if (MechKeyboard.IsKeyDown(Keys.Escape) && ExitOnEscape && DebugMode) Exit();

            if (DebugMode || AllowConsole)
            {

                if (Console.IsOpen)
                {
                    Console.Update(DeltaTime);
                }

                if (AllowConsole && MechKeyboard.WasKeyClicked(Keys.OemTilde))
                {
                    Console.Toggle();
                }
            }
            
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

            // draw the screen
            GraphicsDevice.Clear(Color.Black);

            // draw the screen.
            Screen.Draw(this);

            StartRenderTargetDraw();
            if (Console.IsOpen && (AllowConsole || DebugMode))
            {
                Console.Draw();
            }
            SpriteBatch.End();

            base.EndDraw();
        }

        /// <summary>
        /// This function is called when the game's render target is about to be drawn to the back buffer.
        /// </summary>
        protected virtual void StartRenderTargetDraw()
        {
            SpriteBatch.Begin();
        }

#endregion

#region Finished
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
#endregion

#region Misc

        /// <summary>
        /// This function will toggle fullscreen.
        /// 
        /// <para>
        /// Internally calls <see cref="SetFullscreen"/>
        /// </para>
        /// </summary>
        public void ToggleFullscreen()
        {
            SetFullscreen(!IsFullscreen);
        }

        /// <summary>
        /// Set the game to be fullscreen.
        /// </summary>
        /// <param name="fullscreen">If the game is fullscreen or not.</param>
        /// <param name="windowWidth">The width to set the window to if the window is not fullscreen.</param>
        /// <param name="windowHeight">The width to set the window to if the window is not fullscreen.</param>
        public void SetFullscreen(bool fullscreen, int windowWidth = 1280, int windowHeight = 720)
        {
            IsFullscreen = fullscreen;

            if (IsFullscreen)
            {
                //https://stackoverflow.com/a/4149170
                // https://creativecommons.org/licenses/by-sa/3.0/ 
                GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.Viewport.Width;
                GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.Viewport.Height;
                // end license
                GraphicsDeviceManager.IsFullScreen = true;
                GraphicsDeviceManager.ApplyChanges();
            }
            else
            {
                GraphicsDeviceManager.IsFullScreen = false;
                GraphicsDeviceManager.PreferredBackBufferWidth = windowWidth;
                GraphicsDeviceManager.PreferredBackBufferHeight = windowHeight;
                GraphicsDeviceManager.ApplyChanges();
            }
        }

#endregion

    }
}
