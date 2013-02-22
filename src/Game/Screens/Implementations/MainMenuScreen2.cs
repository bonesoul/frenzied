/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.GamePlay;
using Frenzied.GamePlay.Implementations.PieMode;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.PostProcessing.Effects;
using Frenzied.Screens.Menu;
using Frenzied.Screens.Scenes;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Frenzied.Screens.Implementations
{
    public class MainMenuScreen2 : GameScreen
    {
        // common stuff.
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;

        // post-process effects
        private RenderTarget2D scene;
        private SketchEffect _sketchEffect;

        // game logo
        private Texture2D _textureGameLogo;
        private int _targetGameLogoWidth;
        private float _actualGameLogoScale;
        private float _pulsatedGameLogoScale;
        private Vector2 _gameLogoPosition;
        private const float PulsateFactor = 0.01f;

        private Dictionary<string, Button> _buttons;

        // required services.       
        private IBackgroundScene _backgroundScene;

        public override void Initialize()
        {
            // import required services.
            this._backgroundScene = ServiceHelper.GetService<IBackgroundScene>(typeof(IBackgroundScene));

            this.EnabledGestures = GestureType.Tap;

            base.Initialize();
        }

        public override void LoadContent()
        {
            // init. common stuff.
            this._viewport = ScreenManager.GraphicsDevice.Viewport;
            this._spriteBatch = ScreenManager.SpriteBatch;

            // let background-scene load it's contentes.
            this._backgroundScene.LoadContent();

            // game logo stuff.
            this._textureGameLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Common\game-logo");
            this._targetGameLogoWidth = this._viewport.Width/2;
            this._actualGameLogoScale = (float)_targetGameLogoWidth / this._textureGameLogo.Width;
            this._gameLogoPosition = new Vector2(this._viewport.Width / 2 - _targetGameLogoWidth / 2, 25);

            this._buttons = new Dictionary<string, Button>()
                                {
                                    {"Play", new Button(@"Textures/Menu/Play")},
                                    {"Kids", new Button(@"Textures/Menu/Kids")},
                                    {"Settings", new Button(@"Textures/Menu/Settings")},
                                    {"Credits", new Button(@"Textures/Menu/Credits")},
                                    {"Board", new Button(@"Textures/Menu/Board")},
                                    {"Twitter", new Button(@"Textures/Menu/Social/Twitter")},
                                    {"Facebook", new Button(@"Textures/Menu/Social/Facebook")},
                                    {"Youtube", new Button(@"Textures/Menu/Social/Youtube")},
                                };

            foreach (var pair in this._buttons)
            {
                pair.Value.LoadContent();
            }

            this._buttons["Play"].Position = new Vector2(100, this._viewport.Height * 0.25f);
            this._buttons["Kids"].Position = new Vector2(100, this._viewport.Height * 0.40f);
            this._buttons["Settings"].Position = new Vector2(100, this._viewport.Height * 0.55f);
            this._buttons["Credits"].Position = new Vector2(100, this._viewport.Height * 0.70f);
            this._buttons["Board"].Position = new Vector2(this._viewport.Width - this._buttons["Board"].Texture.Width - 25, this._viewport.Height - this._buttons["Board"].Texture.Height);
            this._buttons["Twitter"].Position=new Vector2(1000,50);
            this._buttons["Facebook"].Position = new Vector2(1080, 50);
            this._buttons["Youtube"].Position = new Vector2(1160, 50);

            this._buttons["Play"].Selected += ButtonPlay_Selected;
            this._buttons["Kids"].Selected += ButtonKids_Selected;
            this._buttons["Settings"].Selected += ButtonSettings_Selected;
            this._buttons["Credits"].Selected += ButtonCredits_Selected;
            this._buttons["Board"].Selected += ButtonBoard_Selected;
            this._buttons["Twitter"].Selected += ButtonTwitter_Selected;
            this._buttons["Facebook"].Selected += ButtonFacebook_Selected;
            this._buttons["Youtube"].Selected += ButtonYoutube_Selected;

            // load contents for post-process effects
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect = new SketchEffect(ScreenManager.Game, ScreenManager.SpriteBatch);

            // Create custom rendertarget for the scene.
            scene = new RenderTarget2D(ScreenManager.GraphicsDevice, this._viewport.Width, this._viewport.Height);

            base.LoadContent();
        }

        private void ButtonPlay_Selected(object sender, EventArgs e)
        {
            // create the score manager
            var mode = new PieMode();

            var scoreManager = new ScoreManager(this.ScreenManager.Game);
            this.ScreenManager.Game.Components.Add(scoreManager);

            ScreenManager.AddScreen(new GameplayScreen(mode), ControllingPlayer);
        }

        private void ButtonKids_Selected(object sender, EventArgs e)
        {
            
        }

        private void ButtonSettings_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), ControllingPlayer);
        }

        private void ButtonCredits_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new AboutScreen(), ControllingPlayer);
        }

        private void ButtonBoard_Selected(object sender, EventArgs e)
        {
            PlatformManager.PlatformHelper.LaunchURI("http://www.int6.org/");
        }

        private void ButtonTwitter_Selected(object sender, EventArgs e)
        {
            PlatformManager.PlatformHelper.LaunchURI("http://www.twitter.com/int6games/");
        }

        private void ButtonFacebook_Selected(object sender, EventArgs e)
        {
            PlatformManager.PlatformHelper.LaunchURI("https://www.facebook.com/Int6Studios");
        }

        private void ButtonYoutube_Selected(object sender, EventArgs e)
        {
            PlatformManager.PlatformHelper.LaunchURI("http://www.youtube.com/user/Int6Games");
        }

        public override void HandleInput(GameTime gameTime, Input.InputState input)
        {
            if (input.CurrentMouseState.LeftButton != ButtonState.Pressed || input.LastMouseState.LeftButton != ButtonState.Released)
                return;

            this.HandleClick(gameTime, input.CurrentMouseState.X, input.CurrentMouseState.Y);

            base.HandleInput(gameTime, input);
        }

        public override void HandleGestures(GameTime gameTime, InputState input)
        {
            if (input.Gestures.Count == 0)
                return;

            foreach (var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                    this.HandleClick(gameTime, (int)gesture.Position.X, (int)gesture.Position.Y);
            }
        }

        private void HandleClick(GameTime gameTime, int X, int Y)
        {
            foreach (var pair in this._buttons)
            {
                if (!pair.Value.Bounds.Contains(X, Y)) 
                    continue;

                pair.Value.OnSelectEntry();
                break;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect.UpdateJitter(gameTime);

            // Pulsate the game-logo.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            this._pulsatedGameLogoScale = this._actualGameLogoScale + pulsate * PulsateFactor;

            // update the background scene.
            this._backgroundScene.Update();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            // create a render target for the scene which will be later using with post process effect.
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(scene);

            this._spriteBatch.Begin();

            // draw background scene.
            this._backgroundScene.Draw(BackgroundScene.Season.Spring);

            // draw game-logo.
            this._spriteBatch.Draw(this._textureGameLogo, this._gameLogoPosition, null, Color.White, 0f, Vector2.Zero,
                                   this._pulsatedGameLogoScale, SpriteEffects.None, 0);

            // draw buttons.
            foreach (var pair in this._buttons)
            {
                pair.Value.Draw(gameTime);
            }

            this._spriteBatch.End();

            // apply post-process effect.
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect.Apply(scene);

            base.Draw(gameTime);
        }
    }
}
