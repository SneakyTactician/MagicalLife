﻿using MagicalLifeAPI.Entities.Entity_Components.Movement_Rules;
namespace MagicalLifeAPI.Entities.Humanoid
{
    /// <summary>
    /// A class that holds logic for control of regular humans.
    /// </summary>
    public class Human : Living
    {
        public Human(int health, int movementSpeed) : base(health, movementSpeed, new StandardMovement())
        {
        }

        public override string GetTextureName()
        {
            return "Basic Human.png";
        }
    }
}