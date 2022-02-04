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
using System;
using System.Collections.Generic;
using System.Text;
using ElectricalMechanicalBridge;

namespace Mechanical
{
    /// <summary>
    /// The camera is a game window to the 2D world!
    /// 
    /// <para>
    /// This whole class is pretty much from https://github.com/JamesMcMahon/monocle-engine/blob/master/Monocle/Util/Camera.cs and https://github.com/Yeti47/Yetibyte.Himalaya/blob/master/Yetibyte.Himalaya/GameElements/Camera.cs slight tweaks made and variables added
    /// </para>
    /// 
    /// TODO: make camera move when target is not in bounds.
    /// </summary>
    public sealed class Camera
    {
        #region Variables
        /// <summary>
        /// The minimum zoom.
        /// </summary>
        public const float MIN_ZOOM = 100;

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
        /// The rotation of the camera. This is in radians.
        /// </summary>
        public float RotationRadians 
        { 
            get => rotationRadians;
            set
            {
                rotationRadians = value;
                UpdateMatrix();
            } 
        }

        /// <summary>
        /// The rotation of the camera. This is in degrees.
        /// </summary>
        public float Rotation
        {
            get => rotationRadians.ToDegrees();
            set
            {
                rotationRadians = value.ToRadians();
                UpdateMatrix();
            }
        }

        /// <summary>
        /// The rotation, but private.
        /// </summary>
        private float rotationRadians;

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

        /// <summary>
        /// The actual zoom of the camera.
        /// </summary>
        public float ActualZoom
        {
            get => zoom / 100;
            set { zoom = value.Clamp(MIN_ZOOM, MAX_ZOOM) * 100; UpdateMatrix(); }
        }

        /// <summary>
        /// The rectangle of the camera.
        /// </summary>
        public Rectangle CameraRect
        {
            get => new Rectangle((int)X - Width / 2, (int)Y - Height / 2, Width, Height);
        }

        /// <summary>
        /// The padded version of the rectangle. This is used for culling.
        /// </summary>
        public Rectangle CameraRectPadded
        {
            get
            {
                Rectangle r = CameraRect;
                r.Inflate(Padding, Padding);
                return r;
            }
        }

        /// <summary>
        /// The padding to be used on the <see cref="CameraRectPadded"/>
        /// </summary>
        public float Padding { get; set; } = 100;

        #endregion

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
                * Matrix.CreateRotationZ(rotationRadians)
                * Matrix.CreateScale(new Vector3(z, z, 1))
                * Matrix.CreateTranslation(new Vector3((int)Math.Floor(origin.X), (int)Math.Floor(origin.Y), 0));

        }

        [EditorFunction("Origin", "Camera")]
        /// <summary>
        /// Make the origin the center of the screen.
        /// </summary>
        public void CenterOrigin()
        {
            Origin = new Vector2(Width / 2, Height / 2);
        }


        /// <summary>
        /// Takes a psoition on screen and places it in the world position.
        /// </summary>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Inverse);
        }

        /// <summary>
        /// Takes a world position and places it on screen.
        /// </summary>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, TransformationMatrix);
        }

        /// <summary>
        /// Sets the camera's position.
        /// </summary>
        /// <param name="position">Where the camera should look</param>
        public void Target(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Move the camera by a specified amount.
        /// </summary>
        /// <param name="amount"></param>
        public void MoveBy(Vector2 amount)
        {
            Position += amount;
        }

    }
}
