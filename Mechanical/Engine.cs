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

        /// <summary>
        /// The arguments passed into the program.
        /// </summary>
        public string[] Arguments { get; set; }

        /// <summary>
        /// The Graphics Device.
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
        /// The size of the window
        /// </summary>
        public Vector2 WindowSize { get; set; } = new Vector2(1280, 720);

        /// <summary>
        /// The window's position
        /// </summary>
        public Vector2 WindowPosition { get; set; }

        /// <summary>
        /// The witdth of the game window
        /// </summary>
        public int Width { get => (int)WindowSize.X; set => WindowSize = new Vector2(value, WindowSize.Y); }

        /// <summary>
        /// The height of the game window.
        /// </summary>
        public int Height { get => (int)WindowSize.Y; set => WindowSize = new Vector2(WindowSize.X, value); }

        /// <summary>
        /// The X position of the window.
        /// </summary>
        public int X { get => (int)WindowPosition.X; set => WindowPosition = new Vector2(value, WindowPosition.Y); }

        /// <summary>
        /// The Y position of the window.
        /// </summary>
        public int Y { get => (int)WindowPosition.Y; set => WindowPosition = new Vector2(WindowPosition.Y, value); }

        /// <summary>
        /// If the game is fullscreen.
        /// </summary>
        public bool IsFullscreen { get; set; }

        /// <summary>
        /// The clear color.
        /// </summary>
        public Color ClearColor { get; set; }

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
        /// The main constructor for the Engine.
        /// </summary>
        /// <param name="args">The arguments passed into the game.</param>
        public Engine(string[] args) : base()
        {
            Arguments = args;
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = ContentDirectory;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Setup the game window.
        /// </summary>
        public virtual void SetupWindow()
        {
            Window.Title = Title;

            // center
            WindowPosition = Window.Position.ToVector2();

            GraphicsDeviceManager.PreferredBackBufferWidth = Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = Height;
            GraphicsDeviceManager.IsFullScreen = IsFullscreen;
            GraphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// The default draw function for the spritebatch.
        /// </summary>
        /// <remarks>
        /// Change this when the camera class and draw class is set up.
        /// </remarks>
        /// <param name="effect"></param>
        /// <param name="transformMatrix"></param>
        public virtual void DefaultBeginBatch(Effect effect = null, Matrix? transformMatrix = null)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, effect: effect, transformMatrix: transformMatrix);
        }

        protected override void Initialize()
        {
            SetupWindow();
            Camera = new Camera(GraphicsDevice.Viewport);

            MechController.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Drawing.SpriteBatch = SpriteBatch;

            Drawing.LoadContent(Content, this);

            base.LoadContent();
        }

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
            DefaultBeginBatch(transformMatrix: Camera.TransformationMatrix);
            return base.BeginDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            SpriteBatch.End();
            base.EndDraw();
        }

    }
}
