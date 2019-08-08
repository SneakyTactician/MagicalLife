﻿using System;
using MLAPI.Asset;
using MLAPI.Entity;
using MLAPI.Entity.AI.Task;
using MLAPI.Entity.AI.Task.Tasks;
using MLAPI.World.Base;
using ProtoBuf;

namespace MLAPI.Crafting
{
    /// <summary>
    /// For simple item recipes.
    /// Take items in, craft them, create new items.
    /// No benches required for these recipes.
    /// </summary>
    [ProtoContract]
    public class SimpleItemRecipe : IRecipe
    {
        [ProtoMember(1)]
        public RequiredItem[] RequiredItems { get; internal set; }

        /// <summary>
        /// The itemID of the item that this recipe constructs.
        /// </summary>
        [ProtoMember(2)]
        public int OutputItemId { get; internal set; }

        /// <summary>
        /// An example of the output that this recipe creates.
        /// </summary>
        /// <param name="requiredItems"></param>
        /// <param name="exampleOutput"></param>
        [ProtoMember(3)]
        private Item ExampleOutput { get; set; }

        [ProtoMember(4)]
        private Guid Id { get; set; }

        /// <summary>
        /// Keywords that can be used to more easily find this recipe.
        /// </summary>
        [ProtoMember(5)]
        private string[] RecipeKeywords { get; set; }

        public SimpleItemRecipe(Item exampleOutput, Guid constantGuid, string[] recipeKeywords, params RequiredItem[] requiredItems)
        {
            this.RequiredItems = requiredItems;
            this.ExampleOutput = exampleOutput;
            this.Id = constantGuid;
            this.RecipeKeywords = recipeKeywords;
        }

        /// <summary>
        /// Crafts the item and removes the used ingredients from the inventory.
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="craftAmount">The amount to be crafted.</param>
        /// <returns>Returns null if the item cannot be crafted.</returns>
        public Item Craft(Inventory inventory, int craftAmount)
        {
            if (this.CanCraft(inventory) >= craftAmount)
            {
                foreach (RequiredItem requiredItem in this.RequiredItems)
                {
                    inventory.RemoveSomeOfItem(requiredItem.Item.ItemId, requiredItem.Count);
                }

                Item output = this.ExampleOutput.GetDeepCopy(craftAmount);
                output.CurrentlyStacked = craftAmount;
                return output;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines the number of an item we could craft based upon the provided inventory.
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public int CanCraft(Inventory inventory)
        {
            int limiter = int.MaxValue;

            foreach (RequiredItem requiredItem in this.RequiredItems)
            {
                //The amount the inventory has of the item.
                int quantityStored = inventory.HasItem(requiredItem.Item.ItemId);
                //The amount of this recipe we could craft if this were the only item.
                int craftable = quantityStored / requiredItem.Count;

                if (craftable < limiter)
                {
                    limiter = craftable;
                }
            }

            return limiter;
        }

        public Item GetExampleOutput()
        {
            return this.ExampleOutput;
        }

        public Guid GetUniqueId()
        {
            return this.Id;
        }

        public int GetDisplayTextureId()
        {
            return AssetManager.NameToIndex[this.ExampleOutput.TextureName];
        }

        public string GetDisplayName()
        {
            return this.ExampleOutput.Name;
        }

        public string[] GetKeywords()
        {
            return this.RecipeKeywords;
        }

        public string GetModName()
        {
            return this.ExampleOutput.ModFrom;
        }

        public void Clicked()
        {
            throw new NotImplementedException();//Need to create some sort of recipe viewing window to display this recipe in
        }

        public void SpecialClicked()
        {
            TaskManager.Manager.AddTask(new CraftSimpleItemTask(Guid.NewGuid(), this, 1));
        }
    }
}