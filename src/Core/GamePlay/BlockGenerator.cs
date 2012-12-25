using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Frenzied.Core.GamePlay
{
    public class BlockGenerator : DrawableGameComponent
    {
        public static Vector2 Size = new Vector2(205, 205);

        public Vector2 Position { get; protected set; }
        public Rectangle Bounds { get; protected set; }
        public Block CurretBlock { get; private set; }

        private readonly Random _randomizer = new Random(Environment.TickCount);

        public BlockGenerator(Game game, Vector2 position)
            : base(game)
        {
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            this.CurretBlock = null;
        }

        public bool IsEmpty
        {
            get { return this.CurretBlock == null; }
        }

        public override void Update(GameTime gameTime)
        {
            if(this.IsEmpty)
                this.Generate();

            base.Update(gameTime);
        }

        public Block UseCurrentBlock()
        {
            var block = this.CurretBlock;
            this.Generate();
            return block;
        }

        private void Generate()
        {
            var location = _randomizer.Next(4);
            var color = _randomizer.Next(3);
            this.CurretBlock = new Block((BlockLocation)location, (BlockColor)color);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
