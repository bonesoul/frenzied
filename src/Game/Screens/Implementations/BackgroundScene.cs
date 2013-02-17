/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Implementations
{
    public interface IBackgroundScene
    {
        void LoadContent();
        void Update();
        void Draw(BackgroundScene.Season season);
    }

    public class BackgroundScene : IBackgroundScene
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;
        private readonly Random _random = new Random();

        private const int CloudCount = 5;
        private const int SeasonCount = 2;

        // textures.
        private readonly Dictionary<Season, Texture2D> _textureBackgrounds;
        private readonly Dictionary<Season, Texture2D> _textureMeadows; 
        private readonly Dictionary<Season, Dictionary<int, Texture2D>> _textureClouds;

        private Vector2[] _cloudPositions;
        private MovementDirection[] _cloudMovementDirection;
        private float[] _cloudMovementSpeeds;

        private float _meadowHeight;
        private Rectangle _meadowBounds;

        public BackgroundScene(Game game)
        {
            this._game = game;

            // export the service.
            this._game.Services.AddService(typeof(IBackgroundScene), this);

            // init containers.
            this._textureBackgrounds = new Dictionary<Season, Texture2D>();
            this._textureMeadows = new Dictionary<Season, Texture2D>();
            this._textureClouds = new Dictionary<Season, Dictionary<int, Texture2D>>();
            this._textureClouds[Season.Spring] = new Dictionary<int, Texture2D>();
            this._textureClouds[Season.Autumn] = new Dictionary<int, Texture2D>();

            this._cloudPositions = new Vector2[CloudCount];
            this._cloudMovementDirection = new MovementDirection[CloudCount];
            this._cloudMovementSpeeds = new float[CloudCount];
        }

        public void LoadContent()
        {
            // init common stuff.
            this._viewport = this._game.GraphicsDevice.Viewport;
            this._spriteBatch = ScreenManager.Instance.SpriteBatch;

            // load backgrounds textures.
            _textureBackgrounds.Add(Season.Spring, this._game.Content.Load<Texture2D>(@"Textures\Menu\Background"));
            _textureBackgrounds.Add(Season.Autumn, this._game.Content.Load<Texture2D>(@"Textures\Menu\AutumnBackground"));

            // load meadow textures.
            this._textureMeadows.Add(Season.Spring, this._game.Content.Load<Texture2D>(@"Textures\Menu\Meadow"));
            this._textureMeadows.Add(Season.Autumn, this._game.Content.Load<Texture2D>(@"Textures\Menu\AutumnMeadow"));

            // poisition the clouds.
            this._cloudPositions[0] = new Vector2(25, 25);
            this._cloudPositions[1] = new Vector2(this._viewport.Width - 500, 150);
            this._cloudPositions[2] = new Vector2(300, 250);
            this._cloudPositions[3] = new Vector2(400, 350);
            this._cloudPositions[4] = new Vector2(this._viewport.Width - 700, 450);

            // load the cloud textures and decide initial movement direction & speed.
            for (int i = 0; i < CloudCount; i++)
            {
                this._textureClouds[Season.Spring].Add(i, this._game.Content.Load<Texture2D>(string.Format(@"Textures\Menu\Clouds\BlueCloud{0}", i + 1)));
                this._textureClouds[Season.Autumn].Add(i, this._game.Content.Load<Texture2D>(string.Format(@"Textures\Menu\Clouds\YellowCloud{0}", i + 1)));
                this._cloudMovementDirection[i] = _random.Next(100)%2 == 0 ? MovementDirection.Right : MovementDirection.Left;
                this._cloudMovementSpeeds[i] = _random.Next(1, 6) * 0.1f;
            }

            // calculate meadow stuff.
            this._meadowHeight = (this._viewport.Width*this._textureMeadows[Season.Spring].Height) / this._textureMeadows[Season.Spring].Width;
            this._meadowBounds = new Rectangle(0, (int)(this._viewport.Height - this._meadowHeight), this._viewport.Width, (int)this._meadowHeight);
        }

        public void Update()
        {
            for (int i = 0; i < CloudCount; i++)
            {
                // move the clouds.
                this._cloudPositions[i].X = _cloudMovementDirection[i] == MovementDirection.Right
                                                ? this._cloudPositions[i].X + this._cloudMovementSpeeds[i]
                                                : this._cloudPositions[i].X - this._cloudMovementSpeeds[i];

                // make sure clouds stay in viewport.
                if (this._cloudPositions[i].X > this._viewport.Width)
                    this._cloudMovementDirection[i] = MovementDirection.Left;
                else if (this._cloudPositions[i].X < -this._textureClouds[Season.Spring][i].Width)
                    this._cloudMovementDirection[i] = MovementDirection.Right;
            }
        }

        public void Draw(Season season)
        {
            // draw background.
            this._spriteBatch.Draw(this._textureBackgrounds[season], this._viewport.Bounds, null, Color.White);

            // draw clouds.
            for (int i = 0; i < CloudCount; i++)
            {
                this._spriteBatch.Draw(this._textureClouds[season][i], this._cloudPositions[i], Color.White);
            }

            // draw meadow.
            this._spriteBatch.Draw(this._textureMeadows[season], this._meadowBounds, null, Color.White);
        }

        /// <summary>
        /// Background scene seasons.
        /// </summary>
        public enum Season
        {
            Spring,
            Autumn,
        }

        /// <summary>
        /// Movement direction for clouds.
        /// </summary>
        public enum MovementDirection
        {
            Left,
            Right
        }
    }
}
