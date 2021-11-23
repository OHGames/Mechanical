using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The camera is a game window to the 2D world!
    /// 
    /// <para>
    /// This whole class is pretty much from https://github.com/JamesMcMahon/monocle-engine/blob/master/Monocle/Util/Camera.cs and https://github.com/Yeti47/Yetibyte.Himalaya/blob/master/Yetibyte.Himalaya/GameElements/Camera.cs
    /// </para>
    /// </summary>
    public sealed class Camera
    {

        /// <summary>
        /// The minimum zoom.
        /// </summary>
        public const float MIN_ZOOM = 0;

        /// <summary>
        /// The max zoom
        /// </summary>
        public const float MAX_ZOOM = 1000;

        /// <summary>
        /// The transformation matrix is pluged into the <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix?)"/> function. It changes the way things are rendered.
        /// </summary>
        public Matrix TransformationMatrix { get; private set; }

        /// <summary>
        /// The inverse of the <see cref="TransformationMatrix"/>.
        /// </summary>
        public Matrix Inverse { get => Matrix.Invert(TransformationMatrix); }

        /// <summary>
        /// The zoom will be a number 0-1.
        /// </summary>
        public float Zoom 
        { 
            get => zoom;
            set
            {
                zoom = value.Clamp(MIN_ZOOM, MAX_ZOOM);
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The zoom, but private.
        /// </summary>
        private float zoom = 100;

        /// <summary>
        /// The position of the camera.
        /// </summary>
        public Vector2 Position 
        { 
            get => position;
            set
            {
                position = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// Position, but private.
        /// </summary>
        private Vector2 position = Vector2.Zero;

        /// <summary>
        /// The origin of the camera.
        /// </summary>
        public Vector2 Origin 
        { 
            get => origin;
            set
            {
                origin = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The origin but private.
        /// </summary>
        private Vector2 origin = Vector2.Zero;

        /// <summary>
        /// The rotation of the camera.
        /// </summary>
        public float Rotation 
        { 
            get => rotation;
            set
            {
                rotation = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The rotation, but private.
        /// </summary>
        private float rotation;

        /// <summary>
        /// A refrence to the viewport.
        /// </summary>
        private Viewport viewport;

        /// <summary>
        /// The width.
        /// </summary>
        public int Width 
        { 
            get => viewport.Width;
            set
            {
                viewport.Width = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The height.
        /// </summary>
        public int Height 
        { 
            get => viewport.Height;
            set
            {
                viewport.Height = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The x psoition of the camera.
        /// </summary>
        public float X
        {
            get => position.X;
            set
            {
                position.X = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// The y location.
        /// </summary>
        public float Y
        {
            get => position.Y;
            set
            {
                position.Y = value;
                UpdateMatrix();
            }
        }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            Width = viewport.Width;
            Height = viewport.Height;
        }

        public Camera(int width, int height) : this(new Viewport(0, 0, width, height))
        {

        }

        private void UpdateMatrix()
        {

            // do this so we dont have 5000 zoom.
            float z = zoom / 100;

            TransformationMatrix = Matrix.Identity
                * Matrix.CreateTranslation(new Vector3(-new Vector2((int)Math.Floor(position.X), (int)Math.Floor(position.Y)), 0))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateScale(new Vector3(z, z, 1))
                * Matrix.CreateTranslation(new Vector3((int)Math.Floor(origin.X), (int)Math.Floor(origin.Y), 0));

        }

        /// <summary>
        /// Make the origin the center of the screen.
        /// </summary>
        public void CenterOrigin()
        {
            origin = new Vector2(Width / 2, Height / 2);
            UpdateMatrix();
        }


        /// <summary>
        /// Takes a psoition on screen and places it in the world position.
        /// </summary>
        /// <returns></returns>
        public Vector2 ScreenToWorld()
        {
            return Vector2.Transform(position, Inverse);
        }

        /// <summary>
        /// Takes a world position and places it on screen.
        /// </summary>
        /// <returns></returns>
        public Vector2 WorldToScreen()
        {
            return Vector2.Transform(position, TransformationMatrix);
        }

        /// <summary>
        /// Sets the camera's position.
        /// </summary>
        /// <param name="position">Where the camera should look</param>
        public void Target(Vector2 position)
        {
            this.position = position;
            UpdateMatrix();
        }

        /// <summary>
        /// Move the camera by a specified amount.
        /// </summary>
        /// <param name="amount"></param>
        public void MoveBy(Vector2 amount)
        {
            this.position += amount;
            UpdateMatrix();
        }

    }
}
