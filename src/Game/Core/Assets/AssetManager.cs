/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Frenzied.Core.GamePlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Core.Assets
{
    public interface IAssetManager
    {
        Texture2D BlockContainerTexture { get; }
        Texture2D BackgroundTexture { get; }
        Dictionary<Color, Texture2D> BlockTextures { get; }
        SpriteFont Verdana { get; }
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

        public AssetManager(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IAssetManager), this); // export service.   
            _instance = this;
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

            this.Verdana = Game.Content.Load<SpriteFont>(@"Fonts/Verdana");
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
                default:
                    return null;
            }
        }

    }
}
