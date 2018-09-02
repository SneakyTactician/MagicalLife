﻿using MagicalLifeAPI.Entity;
using MagicalLifeAPI.Entity.AI.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalLifeServer.JobSystem
{
    /// <summary>
    /// Manages all of the TaskDrivers for a player.
    /// </summary>
    public class TaskManager
    {
        public List<TaskDriver> TaskDrivers { get; private set; }

        public void AddTask(MagicalTask task)
        {
            this.TaskDrivers.Add(new TaskDriver(task));
        }

        /// <summary>
        /// Gets a task for the creature.
        /// This also reserves the task for the creature.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public void AssignTask(Living l)
        {
            List<MagicalTask> allCompatibleTasks = new List<MagicalTask>();

            //Get all compatible jobs
            foreach (TaskDriver item in this.TaskDrivers)
            {
                allCompatibleTasks.AddRange(item.GetCompatibleJobs(l));
            }

            foreach (MagicalTask item in allCompatibleTasks)
            {
                //Has the job been reserved for the unemployed creature
                if (item.ToilingWorker == l.ID)
                {
                    this.AssignJob(l, item);
                    return;
                }
            }

            this.AssignJob(l, allCompatibleTasks[0]);
        }

        private void AssignJob(Living l, MagicalTask task)
        {
            l.Task = task;
            task.MakePreparations(l);
            task.ToilingWorker = l.ID;
        }
    }
}
