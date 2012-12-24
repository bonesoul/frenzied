using System.Collections.Generic;
using Frenzied.Core.GamePlay;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens
{
    public enum BlockContainerPosition
    {
        top,
        left,
        right,
        bottom
    }

    public class GamePlayScreen : GameScreen
    {
        private Dictionary<Color, Texture2D> _blockTextures=new Dictionary<Color, Texture2D>();

        private Dictionary<BlockContainerPosition, BlockContainer> _blockContainers = new Dictionary<BlockContainerPosition, BlockContainer>();

        public GamePlayScreen()
        {            
        }

        public override void LoadContent()
        {
            this._blockContainers.Add(BlockContainerPosition.top, new BlockContainer(ScreenManager.Instance.Game, new Vector2(ScreenManager.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - BlockContainer.Size.X/2, ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height/2 - BlockContainer.Size.Y)));
            this._blockContainers.Add(BlockContainerPosition.bottom, new BlockContainer(ScreenManager.Instance.Game, new Vector2(ScreenManager.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - BlockContainer.Size.X/2, ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height / 2 + BlockContainer.Size.Y/2)));
            this._blockContainers.Add(BlockContainerPosition.left, new BlockContainer(ScreenManager.Instance.Game, new Vector2(ScreenManager.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 - BlockContainer.Size.X, ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height / 2 - BlockContainer.Size.Y/2)));
            this._blockContainers.Add(BlockContainerPosition.right, new BlockContainer(ScreenManager.Instance.Game, new Vector2(ScreenManager.Instance.GraphicsDevice.Viewport.Bounds.Width / 2 + BlockContainer.Size.X, ScreenManager.Game.GraphicsDevice.Viewport.Bounds.Height / 2 - BlockContainer.Size.Y / 2)));

            this._blockTextures[Color.Orange] = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Blocks/OrangeBlock");
            this._blockTextures[Color.Purple] = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Blocks/PurpleBlock");
            this._blockTextures[Color.Green] = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Blocks/GreenBlock");            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (var pair in this._blockContainers)
            {
                pair.Value.Update(gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var pair in this._blockContainers)
            {
                pair.Value.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
