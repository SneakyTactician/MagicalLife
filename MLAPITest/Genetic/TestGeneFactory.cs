﻿using MLAPI.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLAPITest.Genetic
{
    public class TestGeneFactory : IGeneFactory
    {
        private Random Rng { get; set; }

        public TestGeneFactory()
        {
            this.Rng = new Random();
        }

        public Gene[] GenerateGenes(int length)
        {
            Gene[] genes = new Gene[length];

            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = new Gene(this.Rng.Next(0, 100));
            }

            return genes;
        }

        public Gene[] GenerateGenes()
        {
            return this.GenerateGenes(10);
        }
    }
}