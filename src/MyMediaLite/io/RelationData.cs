// Copyright (C) 2010 Zeno Gantner
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
using System.Globalization;
using System.IO;
using MyMediaLite.data;
using MyMediaLite.data_type;
using MyMediaLite.util;


namespace MyMediaLite.io
{
	/// <summary>
	/// Class that offers static methods to read (binary) relation over entities into SparseBooleanMatrix objects.
	///
	/// The expected (sparse) line format is:
	/// ENTITY_ID whitespace ENTITY_ID
	/// for the relations that hold.
	/// </summary>
	public class RelationData
	{
		/// <summary>
		/// Read binary attribute data from file
		/// </summary>
		/// <param name="filename">the name of the file to be read from</param>
		/// <param name="mapping">the mapping object for the given entity type</param>
		/// <returns>the relation data</returns>
		static public SparseBooleanMatrix Read(string filename, EntityMapping mapping)
		{
            using ( var reader = new StreamReader(filename) )
			{
				return Read(reader, mapping);
			}
		}

		/// <summary>
		/// Read binary relation data from file
		/// </summary>
		/// <param name="reader">a StreamReader to be read from</param>
		/// <param name="mapping">the mapping object for the given entity type</param>
		/// <returns>the relation data</returns>
		static public SparseBooleanMatrix Read(StreamReader reader, EntityMapping mapping)
		{
			var matrix = new SparseBooleanMatrix();
			
			var ni = new NumberFormatInfo();
			ni.NumberDecimalDigits = '.';
			
			char[] split_chars = new char[]{ '\t', ' ' };
			string line;

			while (!reader.EndOfStream)
			{
	           	line = reader.ReadLine();
				
				// ignore empty lines
				if (line.Trim().Equals(string.Empty))
					continue;

	            string[] tokens = line.Split(split_chars);

				if (tokens.Length != 2)
					throw new IOException("Expected exactly two columns: " + line);

				int entity1_id = mapping.ToInternalID(int.Parse(tokens[0]));
				int entity2_id = mapping.ToInternalID(int.Parse(tokens[1]));

               	matrix[entity1_id, entity2_id] = true;
			}

			return matrix;
		}
	}
}
