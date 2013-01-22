using System;
using System.Collections.Generic;
using System.Linq;
using Frenzied.Core.GamePlay;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frenzied.Screens
{
    public class GamePlayScreen : GameScreen
    {
        private List<BlockContainer> _blockContainers = new List<BlockContainer>();
        private BlockGenerator _blockGenerator;

        public GamePlayScreen(Game game) 
            : base(game)
        {            
        }

        public override void LoadContent()
        {
            var midScreenX = this.Game.GraphicsDevice.Viewport.Bounds.Width / 2;
            var midScreenY = this.Game.GraphicsDevice.Viewport.Bounds.Height / 2;


            // load block containers
            var offset = 100;

            this._blockContainers.Add(new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY - BlockContainer.Size.Y - offset)));
            this._blockContainers.Add(new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X - offset, midScreenY - offset)));
            this._blockContainers.Add(new BlockContainer(this.Game, new Vector2(midScreenX + offset, midScreenY - offset)));
            this._blockContainers.Add(new BlockContainer(this.Game, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY + BlockContainer.Size.Y - offset)));

            this._blockGenerator = new BlockGenerator(this.Game, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY - offset), this._blockContainers);


            base.LoadContent();
        }

        public override void HandleInput(Core.Input.InputState input)
        {
            if (input.CurrentMouseState.LeftButton== ButtonState.Pressed && input.LastMouseState.LeftButton== ButtonState.Released)
            {
                var mouseState = input.CurrentMouseState;
                foreach (var container in this._blockContainers)
                {
                    if (!container.Bounds.Contains(mouseState.X, mouseState.Y)) 
                        continue;

                    if (this._blockGenerator.IsEmpty)
                        continue;

                    if (! container.IsEmpty(this._blockGenerator.CurretBlock.Location))
                        continue;

                    container.AddBlock(this._blockGenerator.CurretBlock);
                    this._blockGenerator.Generate();

                    break;
                }
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this._blockGenerator.Update(gameTime);

            foreach (var container in this._blockContainers)
            {
                container.Update(gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var container in this._blockContainers)
            {
                container.Draw(gameTime);
            }

            this._blockGenerator.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
