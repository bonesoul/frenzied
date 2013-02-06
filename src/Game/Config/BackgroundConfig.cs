namespace Frenzied.Config
{
    public class BackgroundConfig
    {
        #region configurable parameters

        /// <summary>
        /// Gets or sets the metaball radius.
        /// </summary>
        public int MetaballRadius { get; set; }

        /// <summary>
        /// Gets or sets the metaball scale.
        /// </summary>
        public float MetaballScale { get; set; }

        /// <summary>
        /// Gets or sets the number of metaballs.
        /// </summary>
        public int MetaballCount  { get; set; }

        #endregion

        /// <summary>
        /// Creates a new instance of background component config.
        /// </summary>
        public BackgroundConfig()
        {
            // set the defaults.
            this.MetaballRadius = 128;
            this.MetaballScale = 1f;
            this.MetaballCount = 50;
        }

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return true;
        }
    }
}
