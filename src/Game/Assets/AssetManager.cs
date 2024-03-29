﻿/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Assets
{
    public interface IAssetManager
    {
        Texture2D BlockContainerTexture { get; }
        Dictionary<Color, Texture2D> BlockTextures { get; }
        Texture2D PieContainerTexture { get; }
        Dictionary<Color, Texture2D> PieTextures { get; }
        Texture2D BlockProgressBar { get; }
        SpriteFont Verdana { get; }
        SpriteFont GoodDog { get; }

        Texture2D StudioIntro { get; }

        AssetManager.SoundsEffects Sounds { get; }
    }


    public class AssetManager:GameComponent, IAssetManager
    {
        #if MONOGAME && METRO
            private const string EffectShaderExtension = ".Metro.mgfxo";
        #elif MONOGAME && WINPHONE8
            private const string EffectShaderExtension = ".WinPhone8.mgfxo";
        #else 
            private const string EffectShaderExtension = ""; 
        #endif


            private static AssetManager _instance; // the instance.

        public static AssetManager Instance
        {
            get { return _instance; }
        }

        public Texture2D BlockContainerTexture { get; private set; }
        public Dictionary<Color, Texture2D> BlockTextures { get; private set; }
        public Texture2D PieContainerTexture { get; private set; }
        public Dictionary<Color, Texture2D> PieTextures { get; private set; }
        public Texture2D BlockProgressBar { get; private set; }
        public SpriteFont Verdana { get; private set; }
        public SpriteFont GoodDog { get; private set; }

        public Texture2D StudioIntro { get; private set; }

        public SoundsEffects Sounds { get; private set; }

        private static readonly Dictionary<string, object> LoadedAssets;

        static  AssetManager()
        {
            LoadedAssets = new Dictionary<string, object>();
        }

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

            this.BlockTextures = new Dictionary<Color, Texture2D>();
            this.BlockContainerTexture = Game.Content.Load<Texture2D>(@"Textures/Blocks/BlockContainer");
            this.BlockTextures[Color.Orange] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/OrangeBlock");
            this.BlockTextures[Color.Purple] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/PurpleBlock");
            this.BlockTextures[Color.Green] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/GreenBlock");
            this.BlockTextures[Color.Blue] = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/BlueBlock");

            this.PieTextures = new Dictionary<Color, Texture2D>();
            this.PieContainerTexture = Game.Content.Load<Texture2D>(@"Textures/Pies/Container");
            this.PieTextures[Color.Orange] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Orange");
            this.PieTextures[Color.Purple] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Purple");
            this.PieTextures[Color.Green] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Green");
            this.PieTextures[Color.Blue] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Blue");
            this.PieTextures[Color.Red] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Red");
            this.PieTextures[Color.Brown] = this.Game.Content.Load<Texture2D>(@"Textures/Pies/Brown");

            this.BlockProgressBar = this.Game.Content.Load<Texture2D>(@"Textures/Blocks/BlockProgressBar");

            this.StudioIntro = this.Game.Content.Load<Texture2D>(@"Textures/Common/Intro");

            this.Verdana = Game.Content.Load<SpriteFont>(@"Fonts/Verdana");
            this.GoodDog = Game.Content.Load<SpriteFont>(@"Fonts/GoodDog");

            this.Sounds.LoadContent(this.Game);
        }

        /// <summary>
        /// Loads an effect shared file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Effect LoadEffectShader(string path)
        {
            // Note that monogame requires special compiled shaders with mgfxo extension.
            return this.Game.Content.Load<Effect>(path + EffectShaderExtension);
        }

        /// <summary>
        /// Sounds container.
        /// </summary>
        public class SoundsEffects
        {
            public BlockEffects BlockEffect { get; private set; }

            public SoundEffect Explode { get; private set; }
            public SoundEffect Timeout { get; private set; }

            public SoundsEffects()
            {
                this.BlockEffect = new BlockEffects();
            }

            public void LoadContent(Game game)
            {
                #if !WINPHONE8
                this.Explode = game.Content.Load<SoundEffect>(@"Sounds/Explode/0");
                this.Timeout = game.Content.Load<SoundEffect>(@"Sounds/Timeout/0");
                #endif
                
                this.BlockEffect.LoadContent(game);
            }

            public class BlockEffects
            {
                private Random _random = new Random();
                private readonly List<SoundEffect> _effects = new List<SoundEffect>();

                public void LoadContent(Game game)
                {
                    #if !WINPHONE8
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/0"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/1"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/2"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/3"));
                    this._effects.Add(game.Content.Load<SoundEffect>(@"Sounds/Blocks/4"));
                    #endif
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