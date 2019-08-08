﻿using MLAPI.Asset;
using MLAPI.Util.RandomUtils;
using MLAPI.Visual;
using MLAPI.Visual.Rendering;
using MLAPI.World.Base;
using ProtoBuf;

namespace MLCoreMod.Core.Items
{
    [ProtoContract]
    public class StoneRubble : Item
    {
        public StoneRubble(int count) :
            base("Stone Rubble",
            new System.Collections.Generic.List<string>
            {
                "Stone Rubble",
                "Is dropped when stone is mined"
            },
            9999,
            count,
            TextureLoader.TextureStoneRubble1, 15, DescriptionValues.DisplayName)
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            ComponentHasTexture visuals = this.GetExactComponent<ComponentHasTexture>();
            visuals.Visuals.Add(new StaticTexture(AssetManager.NameToIndex[this.GetRandomStoneRubbleTexture()], RenderLayer.MainObject));
        }

        public StoneRubble()
        {
        }

        private string GetRandomStoneRubbleTexture()
        {
            int r = StaticRandom.Rand(0, 2);
            string ret;

            if (r == 0)
            {
                ret = TextureLoader.TextureStoneRubble1;
            }
            else
            {
                ret = TextureLoader.TextureStoneRubble2;
            }

            return ret;
        }

        public override Item GetDeepCopy(int amount)
        {
            return new StoneRubble(amount);
        }
    }
}