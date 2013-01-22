using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frenzied.Extensions;

namespace Frenzied.Debugging
{
    using System;
    using System.Globalization;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace VoxeliqStudios.Voxeliq.Debugging
    {
        /// <summary>
        /// Allows interaction with the statistics service.
        /// </summary>
        public interface IStatistics
        {
            /// <summary>
            /// Returns current FPS.
            /// </summary>
            int FPS { get; }

            /// <summary>
            /// Returns used memory size as string.
            /// </summary>
            /// <returns></returns>
            string GetMemoryUsed();
        }

        public sealed class Statistics : DrawableGameComponent, IStatistics
        {
            // fps stuff.
            public int FPS { get; private set; } // the current FPS.
            private int _frameCounter = 0; // the frame count.
            private TimeSpan _elapsedTime = TimeSpan.Zero;

            // drawn-text stuff.
            private string _drawnBlocks;
            private string _totalBlocks;

            // resources.
            private SpriteBatch _spriteBatch;
            private SpriteFont _spriteFont;

            // for grabbing internal string, we should init string builder capacity and max capacity ctor so that, grabbed internal string is always valid.
            // http://www.gavpugh.com/2010/03/23/xnac-stringbuilder-to-string-with-no-garbage/
            private readonly StringBuilder _stringBuilder = new StringBuilder(512, 512);

            /// <summary>
            /// Creates a new statistics service instance.
            /// </summary>
            /// <param name="game"></param>
            public Statistics(Game game)
                : base(game)
            {
                this.Game.Services.AddService(typeof (IStatistics), this); // export the service.
            }

            /// <summary>
            /// Initializes the statistics service.
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
            }

            /// <summary>
            /// Loads required assets & content.
            /// </summary>
            protected override void LoadContent()
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _spriteFont = Game.Content.Load<SpriteFont>(@"Fonts/calibri");
            }

            /// <summary>
            /// Calculates the FPS.
            /// </summary>
            /// <param name="gameTime"></param>
            public override void Update(GameTime gameTime)
            {
                this._elapsedTime += gameTime.ElapsedGameTime;
                if (this._elapsedTime < TimeSpan.FromSeconds(1))
                    return;

                this._elapsedTime -= TimeSpan.FromSeconds(1);
                this.FPS = _frameCounter;
                this._frameCounter = 0;
            }

            /// <summary>
            /// Draws the statistics.
            /// </summary>
            /// <param name="gameTime"></param>
            public override void Draw(GameTime gameTime)
            {
                // Attention: DO NOT use string.format as it's slower than string concat.

                _frameCounter++;

                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);


                // FPS
                _stringBuilder.Length = 0;
                _stringBuilder.Append("fps:");
                _stringBuilder.AppendNumber(this.FPS);
                _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(5, 5), Color.White);

                // mem used
                _stringBuilder.Length = 0;
                _stringBuilder.Append("mem:");
                _stringBuilder.Append(this.GetMemoryUsed());
                _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(75, 5), Color.White);

                _spriteBatch.End();
            }

            /// <summary>
            /// Returns used memory size as string.
            /// </summary>
            /// <returns></returns>
            public string GetMemoryUsed()
            {
                return this.GetMemSize(GC.GetTotalMemory(false));
            }

            /// <summary>
            /// Returns pretty memory size text.
            /// </summary>
            /// <param name="size"></param>
            /// <returns></returns>
            private string GetMemSize(long size)
            {
                int i;
                string[] suffixes = {"B", "KB", "MB", "GB", "TB"};
                double dblSByte = 0;
                for (i = 0; (int) (size/1024) > 0; i++, size /= 1024) dblSByte = size/1024.0;
                return dblSByte.ToString("0.00") + suffixes[i];
            }
        }
    }
}
