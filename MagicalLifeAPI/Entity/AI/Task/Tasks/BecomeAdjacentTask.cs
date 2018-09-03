﻿using MagicalLifeAPI.DataTypes;
using MagicalLifeAPI.Entity.AI.Task.Qualifications;
using MagicalLifeAPI.Filing.Logging;
using MagicalLifeAPI.Pathfinding;
using MagicalLifeAPI.Util;
using MagicalLifeAPI.World;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalLifeAPI.Entity.AI.Task.Tasks
{
    [ProtoContract]
    public class BecomeAdjacentTask : MagicalTask
    {

        [ProtoMember(1)]
        public Point2D Target { get; private set; }

        [ProtoMember(3)]
        public Point2D AdjacentLocation { get; private set; }

        public BecomeAdjacentTask(Guid boundID, Point2D target) : base(Dependencies.None, boundID, new List<Qualification> { new CanMoveQualification() })
        {
            this.Target = target;
        }

        public override void MakePreparations(Living l)
        {
            List<Point2D> result = WorldUtil.GetNeighboringTiles(this.Target, l.Dimension);
            result.RemoveAll(x => !World.Data.World.GetTile(l.Dimension, x.X, x.Y).IsWalkable);

            int closestIndex = Algorithms.GetClosestPoint2D(result, l.MapLocation);
            this.AdjacentLocation = result[closestIndex];
            List<PathLink> path = MainPathFinder.GetRoute(l.Dimension, l.MapLocation, result[closestIndex]);
            l.QueuedMovement.Clear();
            Extensions.EnqueueCollection(l.QueuedMovement, path);
        }

        public override void Reset()
        {
            //We don't need to do anything to reset this.
        }

        public override void Tick(Living l)
        {
            if (l.MapLocation.Equals(this.AdjacentLocation))
            {
                MasterLog.DebugWriteLine(this.ID.ToString());
                this.CompleteTask();
            }
        }
    }
}
