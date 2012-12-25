using System;
using System.Collections.Generic;
using Frenzied.Core.GamePlay;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Dictionary<BlockContainerPosition, BlockContainer> _blockContainers = new Dictionary<BlockContainerPosition, BlockContainer>();
        private BlockGenerator _blockGenerator;

        public GamePlayScreen(Game game) 
            : base(game)
        {            
        }

        public override void LoadContent()
        {
            var midScreenX = this.Game.GraphicsDevice.Viewport.Bounds.Width/2;
            var midScreenY = this.Game.GraphicsDevice.Viewport.Bounds.Height/2;


            // load block containers
            var offset = 100;

            this._blockContainers.Add(BlockContainerPosition.top,
                                      new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X/2, midScreenY - BlockContainer.Size.Y - offset)));
            this._blockContainers.Add(BlockContainerPosition.left,
                                      new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X - offset, midScreenY-offset)));
            this._blockContainers.Add(BlockContainerPosition.right,
                                      new BlockContainer(this.Game, new Vector2(midScreenX + offset, midScreenY-offset)));
            this._blockContainers.Add(BlockContainerPosition.bottom,
                                      new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X/2,midScreenY + BlockContainer.Size.Y - offset)));

            this._blockGenerator = new BlockGenerator(this.Game, new Vector2(midScreenX - BlockContainer.Size.X/2,
                                                                  midScreenY + BlockContainer.Size.Y - offset));


            base.LoadContent();
        }

        public override void HandleInput(Core.Input.InputState input)
        {
            if (input.CurrentMouseState.LeftButton== ButtonState.Pressed && input.LastMouseState.LeftButton== ButtonState.Released)
            {
                var mouseState = input.CurrentMouseState;
                foreach (var pair in this._blockContainers)
                {
                    var container = pair.Value;
                    if (container.Bounds.Contains(mouseState.X, mouseState.Y))
                    {
                        if (this._blockGenerator.IsEmpty)
                            continue;

                        var block = this._blockGenerator.UseCurrentBlock();
                        container.AddBlock(block);

                        break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this._blockGenerator.Update(gameTime);

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
