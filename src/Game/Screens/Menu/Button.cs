/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenzied.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Menu
{
    public class Button
    {
        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler Selected;

        public Texture2D Texture { get; protected set; }
        public string TextureName { get; protected set; }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return this._position; }
            set
            {
                this._position = value;
                this.Bounds = new Rectangle((int) this.Position.X, (int) this.Position.Y, this.Texture.Width,
                                            this.Texture.Height);
            }
        }

        public Rectangle Bounds { get; protected set; }

        public Button(string textureName)
        {
            this.TextureName = textureName;
        }

        public void LoadContent()
        {
            this.Texture = ScreenManager.Instance.Game.Content.Load<Texture2D>(this.TextureName);
        }

        public void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Draw(this.Texture,this.Position, Color.White);
        }

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }
    }
}
