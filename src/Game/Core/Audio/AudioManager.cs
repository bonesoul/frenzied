/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Frenzied.Core.Audio
{
    public interface IAudioManager
    {
    }

    public class AudioManager:GameComponent, IAudioManager
    {
        private Song _backgroundSong;


        public AudioManager(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IAudioManager), this); // export service.   
        }

        public override void Initialize()
        {
            this._backgroundSong = Game.Content.Load<Song>(@"Music/Dragon Soul");
            this.PlayBackroundSong();

            base.Initialize();
        }

        private void PlayBackroundSong()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this._backgroundSong);
            MediaPlayer.Volume = 0.3f;
        }
    }
}
