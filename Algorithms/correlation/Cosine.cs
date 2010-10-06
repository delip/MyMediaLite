// Copyright (C) 2010 Steffen Rendle, Zeno Gantner
//
// This file is part of MyMediaLite.
//
// MyMediaLite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MyMediaLite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MyMediaLite.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using MyMediaLite.data_type;
using MyMediaLite.util;


namespace MyMediaLite.correlation
{
	/// <summary>
	/// Class for storing cosine similarities
	/// </summary>
	public class Cosine : CorrelationMatrix
	{
		/// <inheritdoc />
		public Cosine(int num_entities) : base(num_entities) { }
		
		/// <inheritdoc />
		public Cosine(CorrelationMatrix correlation_matrix)
		{
			this.num_entities = correlation_matrix.data.dim1;
			this.data = correlation_matrix.data;
		}

		/// <inheritdoc />
		public override void ComputeCorrelations(SparseBooleanMatrix entity_data)
		{
			Console.Error.Write("Computation of cosine similarity for {0} entities... ", num_entities);

            for (int i = 0; i < num_entities; i++)
			{
				data.Set(i, i, 1);
                if (entity_data[i].Count == 0)
                    continue;

			    HashSet<int> attributes_i = entity_data[i];

				if (i % 100 == 99)
					Console.Error.Write(".");
				if (i % 5000 == 4999)
					Console.Error.WriteLine("{0}/{1}", i, num_entities);

				for (int j = i + 1; j < num_entities; j++)
                    if (entity_data[j].Count > 0)
                    {
						float corr = ComputeCorrelation(attributes_i, entity_data[j]);
						data.Set(i, j, corr);
						data.Set(j, i, corr);
            	    }
			}

			Console.Error.WriteLine();
		}

		/// <inheritdoc />
		public static float ComputeCorrelation(HashSet<int> vector_i, HashSet<int> vector_j)
		{
			int cntr = 0;
            foreach (int k in vector_j)
			{
            	if (vector_i.Contains(k))
	            	cntr++;
            }
            return  (float) cntr / (float) Math.Sqrt(vector_i.Count * vector_j.Count);
		}
	}
}