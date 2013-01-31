/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.GamePlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Assets
{
    public interface IAssetManager
    {
        Texture2D BlockContainerTexture { get; }
        Texture2D BackgroundTexture { get; }
        Dictionary<Color, Texture2D> BlockTextures { get; }
        SpriteFont Verdana { get; }
        SpriteFont GoodDog { get; }

        AssetManager.SoundsEffects Sounds { get; }
    }

    public class AssetManager:GameComponent, IAssetManager
    {
        private static AssetManager _instance; // the instance.

        public static AssetManager Instance
        {
            get { return _instance; }
        }

        public Texture2D BlockContainerTexture { get; private set; }
        public Texture2D BackgroundTexture { get; private set; }
        public Dictionary<Color, Texture2D> BlockTextures { get; private set; }
        public SpriteFont Verdana { get; private set; }
        public SpriteFont GoodDog { get; private set; }

        public AssetManager.SoundsEffects Sounds { get; private set; }

        public AssetManager(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IAssetManager), this); // export service.   
            _instance = this;

            this.Sounds = new SoundsEffects();
        }

        public override void Initialize()
        {
            this.LoadContent();
            base.Initialize();
        }

        public void LoadContent()
        {            
            this.BlockContainerTexture = Game.Content.Load<Texture2D>(@"Textures/BlockContainer");
            this.BackgroundTexture = Game.Content.Load<Texture2D>(@"Textures/Background");

            this.BlockTextures = new Dictionary<Color, Texture2D>();
            this.BlockTextures[Color.Orange] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/OrangeBlock");
            this.BlockTextures[Color.Purple] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/PurpleBlock");
            this.BlockTextures[Color.Green] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/GreenBlock");
            this.BlockTextures[Color.Blue] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/BlueBlock");

            this.Verdana = Game.Content.Load<SpriteFont>(@"Fonts/Verdana");
            this.GoodDog = Game.Content.Load<SpriteFont>(@"Fonts/GoodDog");

            this.Sounds.LoadContent(this.Game);
        }

        public Texture2D GetBlockTexture(Block block)
        {
            switch (block.Color)
            {
                case BlockColor.Orange:
                    return this.BlockTextures[Color.Orange];
                case BlockColor.Purple:
                    return this.BlockTextures[Color.Purple];
                case BlockColor.Green:
                    return this.BlockTextures[Color.Green];
                case BlockColor.Blue:
                    return this.BlockTextures[Color.Blue];
                default:
                    return null;
            }
        }

        /// <summary>
        /// Sounds container.
        /// </summary>
        public class SoundsEffects
        {
            public BlockEffect CoinEffect { get; private set; }

            public SoundEffect Explode { get; private set; }

            public SoundsEffects()
            {
                this.CoinEffect = new BlockEffect();
            }

            public void LoadContent(Game game)
            {
                this.Explode = game.Content.Load<SoundEffect>(@"Sounds/Explode/0");
                this.CoinEffect.LoadContent(game);
            }

            public class BlockEffect
            {
                private Random _random = new Random();
                private readonly List<SoundEffect> _effects = new List<SoundEffect>();

                public void LoadContent(Game game)
                {
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/0"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/1"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/2"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/3"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/4"));
                }

                public void PlayRandom()
                {
                    var id = _random.Next(0, this._effects.Count);
                    this._effects[id].Play();
                }
            }
        }
    }
}
