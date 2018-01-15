﻿using MagicalLifeAPI.Entities.Util;
using MagicalLifeAPI.Universal;

namespace MagicalLifeAPI.Entities
{
    /// <summary>
    /// All living things inherit from this, and utilize it.
    /// </summary>
    public abstract class Living : Unique
    {
        /// <summary>
        /// How many hit points this creature has.
        /// </summary>
        public Attribute Health { get; set; }

        /// <summary>
        /// How fast this creature can move.
        /// </summary>
        public Attribute MovementSpeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Living"/> base class.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="movementSpeed"></param>
        public Living(int health, int movementSpeed)
        {
            this.Health = new Attribute(health);
            this.MovementSpeed = new Attribute(movementSpeed);
        }

        /// <summary>
        /// Returns the name of the texture.
        /// </summary>
        /// <returns></returns>
        public abstract string GetTextureName();
    }
}