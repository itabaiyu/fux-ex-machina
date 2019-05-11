using System;
using System.Collections.Generic;

namespace FuxExMachinaLibrary.Weights
{
    public static class NoteWeightGenerator
    {
        private static readonly Random Random = new Random();

        public static List<NoteWeight> Generate(int totalNoteChoices)
        {
            var sum = 0;
            var targetSum = totalNoteChoices * 1000;

            var weightBreakpoints = new List<int>();


            for (var i = 0; i < totalNoteChoices; i++)
            {
                var next = Random.Next(targetSum);
                weightBreakpoints.Add(next);
                sum += next;
            }

            //scale to the desired target sum
            var scale = 1d * targetSum / sum;
            sum = 0;
            for (var i = 0; i < totalNoteChoices; i++)
            {
                weightBreakpoints[i] = (int)(weightBreakpoints[i] * scale);

                sum += weightBreakpoints[i];
            }

            //take rounding issues into account
            while (sum++ < targetSum)
            {
                var i = Random.Next(totalNoteChoices);
                weightBreakpoints[i] = weightBreakpoints[i] + 1;
            }

            var noteWeights = new List<NoteWeight>();
            var weightBreakpointFloor = 0;

            foreach (var weightBreakpoint in weightBreakpoints)
            {
                var noteWeight = new NoteWeight
                {
                    WeightValue = weightBreakpoint,
                    WeightBreakpointFloor = weightBreakpointFloor == 0 ? weightBreakpointFloor : weightBreakpointFloor + 1,
                    WeightBreakpointCeiling = weightBreakpointFloor + weightBreakpoint
                };

                noteWeights.Add(noteWeight);
                weightBreakpointFloor += weightBreakpoint;
            }

            return noteWeights;
        }
    }
}