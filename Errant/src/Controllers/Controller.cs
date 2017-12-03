﻿using Errant.src.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.Controllers {
    public class Controller : GameComponent {

        //SHOULD THIS BE A GAMECOMPONENT?

        Entity possessedEntity;

        public Controller(Application application) : base(application) {
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public void Possess(Entity entity) {
            possessedEntity = entity;
            entity.Possess(this);
        }
    }
}
