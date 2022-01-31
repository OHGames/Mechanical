/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    [DataContract]
    /// <summary>
    /// A list to handle GUI Elements
    /// </summary>
    public class GUIElementList : GameLikeList<GUIElement>
    {
        /// <summary>
        /// If it is safe to add the new element.
        /// </summary>
        private bool isSafe = true;

        /// <summary>
        /// The elements that have been just added to awake.
        /// </summary>
        private List<GUIElement> justAdded = new List<GUIElement>();

        public override void Add(GUIElement item)
        {
            if (!Contains(item))
            {
                if (isSafe) items.Add(item);
                else
                {
                    toAdd.Enqueue(item);
                }
            }
            else
            {
                throw new Exception($"The canvas element, {item.Name}, is already added.");
            }
        }

        public override void Add(params GUIElement[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Add(items[i]);
            }
        }

        public override bool Contains(GUIElement item)
        {
            return items.Contains(item);
        }

        public override void DebugDraw(bool editorRender)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].DebugDraw(editorRender);
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw();
            }
        }

        public override List<GUIElement> GetAll()
        {
            return items;
        }

        public override void Initialize()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Initialize();
            }
        }

        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].LoadContent(content);
            }
        }

        public override void Remove(GUIElement item)
        {
            if (Contains(item))
            {
                if (isSafe) items.Remove(item);
                else
                {
                    toRemove.Enqueue(item);
                }
            }
            else
            {
                throw new Exception($"The canvas element, {item.Name}, is not in the list!");
            }
        }

        public override void Update(float deltaTime)
        {
            isSafe = false;
            if (toAdd.Count > 0)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    GUIElement e = toAdd.Dequeue();

                    items.Add(e);
                    e.OnAdded();
                    justAdded.Add(e);
                }
            }
            if (toRemove.Count > 0)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    GUIElement e = toRemove.Dequeue();
                    items.Remove(e);
                    e.OnRemoved();
                }
            }

            if (justAdded.Count > 0)
            {
                // awake
                justAdded.ForEach(e => e.Awake());

                justAdded.Clear();
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(deltaTime);
            }

        }
    }
}
