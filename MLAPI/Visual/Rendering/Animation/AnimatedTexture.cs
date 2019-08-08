﻿using System.Reflection;
using MLAPI.Asset;
using MLAPI.DataTypes;
using MLAPI.Visual.Rendering.Map;
using ProtoBuf;

namespace MLAPI.Visual.Rendering.Animation
{
    /// <summary>
    /// Animations.
    /// </summary>
    [ProtoContract]
    public class AnimatedTexture : AbstractVisual
    {
        [ProtoMember(1)]
        private SpriteSheet AnimationFrames;//Find a way to render a specific texture from here

        /// <summary>
        /// The animation sequences that this animated texture can do.
        /// </summary>
        [ProtoMember(2)]
        private AnimationSequence[] Sequences;

        /// <summary>
        /// The index of the currently playing sequence in <see cref="Sequences"/>.
        /// </summary>
        [ProtoMember(3)]
        private int PlayingSequence = -1;

        [ProtoMember(4)]
        public bool HasFinished { get; private set; } = true;

        /// <summary>
        /// The index of the last played frame.
        /// </summary>
        [ProtoMember(5)]
        public int LastFrame { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="sequences"></param>
        /// <param name="spriteSheet">The texture name of the sprite sheet.</param>
        /// <param name="xmlResourcePath">The resource path to load the XML data about the sprite sheet as an embedded resource.</param>
        public AnimatedTexture(int priority, AnimationSequence[] sequences, string spriteSheet, string xmlResourcePath, Assembly containingAsm) : base(priority)
        {
            this.Sequences = sequences;

            SpriteSheetReader reader = new SpriteSheetReader();
            this.AnimationFrames = reader.Read(xmlResourcePath, AssetManager.NameToIndex[spriteSheet], containingAsm);
        }

        public AnimatedTexture()
        {
        }

        public override void Render(MapBatch batch, Point2D screenTopLeft)
        {
            if (this.PlayingSequence == -1)
            {
                batch.Draw(this.AnimationFrames.Sprites,
                    screenTopLeft, this.AnimationFrames.GetSection(this.LastFrame), this.Priority);
            }
            else
            {
                bool isDone = this.Sequences[this.PlayingSequence].Tick(out int frame);

                this.LastFrame = frame;
                this.HasFinished = isDone;

                batch.Draw(this.AnimationFrames.Sprites,
                    screenTopLeft, this.AnimationFrames.GetSection(frame), this.Priority);

                if (this.HasFinished)
                {
                    this.PlayingSequence = -1;
                }
            }
        }

        /// <summary>
        /// Starts playing the specified animation sequence. Stops and resets any previously playing sequences.
        /// </summary>
        /// <param name="sequenceId">The ID of the sequence to initiate.</param>
        public void StartSequence(int sequenceId)
        {
            if (this.PlayingSequence != -1 && this.HasFinished)
            {
                this.Sequences[this.PlayingSequence].ResetToBeginning();
            }

            if (this.HasFinished)
            {
                this.PlayingSequence = sequenceId;
            }
        }
    }
}