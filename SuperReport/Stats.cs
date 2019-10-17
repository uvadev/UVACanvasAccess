using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace SuperReport {
    
    internal readonly struct Stats {
        
        [PublicAPI]
        public ImmutableList<decimal> Data { get; }
        
        [PublicAPI]
        public decimal Mean { get; }
        
        [PublicAPI]
        public decimal Q1 { get; }
        
        [PublicAPI]
        public decimal Median { get; }
        
        [PublicAPI]
        public decimal Q3 { get; }
        
        [PublicAPI]
        public decimal Mode { get; }
        
        [PublicAPI]
        public double Sigma { get; }
        
        public Stats(IEnumerable<decimal> data) {
            Data = data.OrderBy(d => d)
                       .ToImmutableList();

            Mean = Data.Average();
            
            var q1Point = Data.Count / 4;
            var q2Point = Data.Count / 2;
            var q3Point = Data.Count / 4 * 3;

            var dataCountOdd = Data.Count % 2 != 0;
            
            if (q1Point == 0 || dataCountOdd) {
                Q1 = Data[q1Point];
            } else { 
                Q1 = (Data[q1Point] + Data[q1Point - 1]) / 2;
            }

            if (q2Point == 0 || dataCountOdd) {
                Median = Data[q2Point];
            } else {
                Median = (Data[q2Point] + Data[q2Point - 1]) / 2;
            }
                        
            if (q3Point == 0 || dataCountOdd) {
                Q3 = Data[q3Point];
            } else {
                Q3 = (Data[q3Point] + Data[q3Point - 1]) / 2;
            }
            
            Mode = Data.GroupBy(s => s)
                       .OrderByDescending(g => g.Count())
                       .First()
                       .Key;

            var mean = Mean;
            Sigma = Math.Sqrt(Data.Aggregate(0.0, (acc, s) => acc + Math.Pow(Convert.ToDouble(s - mean), 2)) / Data.Count);
        }
    }
}
