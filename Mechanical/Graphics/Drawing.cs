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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// This class handles drawing. Use this instead of the <see cref="SpriteBatch"/>.
    /// </summary>
    public static class Drawing
    {
        #region Variables
        /// <summary>
        /// A refrence to teh spritebatch from the <see cref="Engine"/> class.
        /// </summary>
        public static SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// A texture that is a 1x1 white pixel.
        /// </summary>
        public static Texture2D Pixel { get; private set; }

        /// <summary>
        /// A refrence to the engine.
        /// </summary>
        private static Engine engine;

        /// <summary>
        /// The current effect for drawing.
        /// </summary>
        private static Effect currentEffect;

        /// <summary>
        /// An effect to render all draw calls with.
        /// <para>
        /// Set to null to not have a global effect
        /// </para>
        /// </summary>
        public static Effect GlobalEffect { get; set; }

        /// <summary>
        /// If the global effect is always applied.
        /// </summary>
        public static bool ForceGlobalEffect { get; set; } = true;

        /// <summary>
        /// If the Draw String functions should exclude the fonts.
        /// </summary>
        public static bool ExcludeFontFromGlobalEffect { get; set; } = false;

        /// <summary>
        /// The previous global effect.
        /// </summary>
        private static Effect prevoiousGlobalEffect;

        /// <summary>
        /// If the global effect changed.
        /// </summary>
        private static bool GlobalEffectChanged => prevoiousGlobalEffect != GlobalEffect;

        #endregion

        #region Functions
        /// <summary>
        /// Load the content for the draw class.
        /// </summary>
        /// <param name="content">The content manager.</param>
        /// <param name="engine">A refrence to the engine.</param>
        public static void LoadContent(ContentManager content, Engine engine)
        {
            Drawing.engine = engine;
            Pixel = content.Load<Texture2D>("Mechanical/pixel");
        }

        /// <summary>
        /// If there is a new effect, stop the batch. This stops then starts the batch again.
        /// </summary>
        /// <param name="effect">The new effect.</param>
        private static void StopBatchIfNeeded(Effect effect)
        {
            if (GlobalEffectChanged)
            {
                prevoiousGlobalEffect = GlobalEffect;
                currentEffect = GlobalEffect;
                Begin();
            }
            // dont stop batch because the effect is global.
            if (ForceGlobalEffect && GlobalEffect != null)
            {
                return;
            }
            else if (effect != currentEffect)
            {
                currentEffect = effect;
                SpriteBatch.End();
                Begin();
                // set effect back again
                currentEffect = GlobalEffect;
            }
        }

        /// <summary>
        /// Start the spritebatch.
        /// </summary>
        public static void Begin()
        {
            // todo: add tranform.
            //if (ForceGlobalEffect && GlobalEffect != null)
            //{
            //    engine.DefaultBeginBatch(GlobalEffect);
            //}
            //else if (GlobalEffect != null && currentEffect == null)
            //{
            //    engine.DefaultBeginBatch(GlobalEffect);
            //}
            //else
            //{
                engine.DefaultBeginBatch(currentEffect, engine.Camera.TransformationMatrix);
            //}
        }

        public static void End() => SpriteBatch.End();

        #endregion

        #region Draw Wrapper

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Color color, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, destinationRectangle, color);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float layerDepth, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Vector2 position, Color color, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, position, color);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, position, sourceRectangle, color);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this sprite.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this sprite.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }
        #endregion

        #region Draw String Wrapper

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.DrawString(spriteFont, text, position, color);
        }

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this string.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this string.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this string.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this string.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this string.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this string.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null)
        {
            StopBatchIfNeeded(effect);
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="effect"></param>
        public static void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, Effect effect = null) => DrawString(spriteFont, text.ToString(), position, color, effect);

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        /// <param name="effect"></param>
        public static void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null) => DrawString(spriteFont, text.ToString(), position, color, rotation, origin, scale, spriteEffects, layerDepth, effect);

        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this string.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this string.</param>
        /// <param name="spriteEffects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this string.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth, Effect effect = null) => DrawString(spriteFont, text.ToString(), position, color, rotation, origin, scale, spriteEffects, layerDepth, effect);

        #endregion


        // Code inside this regoin was based of of http://web.archive.org/web/20210701013907/https://archive.codeplex.com/?p=xnadebugdrawer
        // MS-PL Lisence
        // Minor changes made, copyright https://bayinx.wordpress.com/author/theglyph/ (guilherme silva)
        // article used https://bayinx.wordpress.com/2011/11/07/how-to-draw-lines-circles-and-polygons-using-spritebatch-in-xna/
        #region Shape Outlines

        /// <summary>
        /// Draw a line from one point to another.
        /// </summary>
        /// <param name="start">The first position</param>
        /// <param name="end">The second position.</param>
        /// <param name="color">The color to draw.</param>
        /// <param name="width">How thick the line is.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width, Effect effect = null)
        {
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float length = Vector2.Distance(start, end);

            Draw(Pixel, start, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0, effect);
        }

        /// <summary>
        /// Draw a line from one point to another.
        /// </summary>
        /// <param name="x1">The first point's x location</param>
        /// <param name="y1">The first point's y location</param>
        /// <param name="x2">The second point's x location</param>
        /// <param name="y2">The second point's y location</param>
        /// <param name="color">The color to draw.</param>
        /// <param name="width">How thick the line is.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawLine(float x1, float y1, float x2, float y2, Color color, float width, Effect effect = null) => DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, width, effect);

        /// <summary>
        /// Draw a polygon.
        /// </summary>
        /// <param name="verticies">The verticies of the shape.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The thickness of each line.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawPolygon(Vector2[] verticies, Color color, float width, Effect effect = null)
        {
            int count = verticies.Length;
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLine(verticies[i], verticies[i + 1], color, width, effect);
                }
                // connect last line to first.
                DrawLine(verticies[count - 1], verticies[0], color, width, effect);
            }
        }

        /// <summary>
        /// Draw a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color to draw.</param>
        /// <param name="lineWidth">How thick the lines are.</param>
        /// <param name="effect">An effect to apply.</param>
        public static void DrawRectangle(Rectangle rectangle, Color color, float lineWidth, Effect effect = null)
        {
            Vector2[] verticies = new Vector2[4];
            verticies[0] = new Vector2(rectangle.Left, rectangle.Top);
            verticies[1] = new Vector2(rectangle.Right, rectangle.Top);
            verticies[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            verticies[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            DrawPolygon(verticies, color, lineWidth, effect);
        }

        /// <summary>
        /// Draw a circle.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="lineWidth">The width of the line.</param>
        /// <param name="segments">The amount of segments in the circle. You can specify as many as posible 3 or more.</param>
        /// <param name="effect">The effect to apply.</param>
        /// <exception cref="ArgumentException">When the segment count is less than 3 and when the radius is 0.</exception>
        public static void DrawCircle(Vector2 center, float radius, Color color, float lineWidth, int segments = 16, Effect effect = null)
        {
            if (segments < 3) throw new ArgumentException("The segments must be 3 or more when drawing a circle. Use DrawLine() instead for 2 segments and just draw a sprite for one segment.");

            if (radius < 0) throw new ArgumentException("The radius must be above 0");

            Vector2[] verticies = new Vector2[segments];

            double increment = Math.PI * 2 / segments;
            double theta = 0;

            for (int i = 0; i < segments; i++)
            {
                verticies[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(verticies, color, lineWidth, effect);

        }

        #endregion

        // TODO: add shape batching.

    }
}
