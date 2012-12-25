using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frenzied.Core.GamePlay;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Core.Assets
{
    public interface IAssetManager
    {
        Texture2D BlockContainerTexture { get; }
        Texture2D BackgroundTexture { get; }
        Dictionary<Color, Texture2D> BlockTextures { get; }
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
        }

        public Texture2D GetBlockTexture(Block block)
        {
            switch (block.Color)
            {
                case BlockColor.orange:
                    return this.BlockTextures[Color.Orange];
                    break;
                case BlockColor.purple:
                    return this.BlockTextures[Color.Purple];
                    break;
                case BlockColor.green:
                    return this.BlockTextures[Color.Green];
                    break;
                default:
                    return null;
            }
        }

    }
}
