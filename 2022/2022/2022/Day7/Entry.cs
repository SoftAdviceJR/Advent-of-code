using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace _2022.Day7
{
	[DebuggerDisplay("{Name, nq}")]
	public abstract class Entry
	{
		public abstract int Size { get; }

		public string Name { get; set; }
	}


	public class FileEntry : Entry
	{
		private readonly int _size;

		public override int Size => _size;

		public FileEntry(int size, string name)
		{
			_size = size;
			Name = name;
		}
	}

	public class DirectoryEntry : Entry
	{
		public DirectoryEntry Parent { get; }

		public List<Entry> Children { get; set; } = new List<Entry>();

		public override int Size => Children.Sum(c => c.Size);

		public DirectoryEntry(string name)
		{
			Name = name;
		}

		public DirectoryEntry(string name, DirectoryEntry parent)
		{
			Name = name;
			Parent = parent;
		}

		public IEnumerable<DirectoryEntry> GetAllDirectories()
		{
			yield return this;

			foreach (var entry in Children)
			{
				if (entry.GetType() == typeof(DirectoryEntry))
					foreach (var dir in ((DirectoryEntry)entry).GetAllDirectories())
					{
						yield return dir;
					}
			}
		}

		public DirectoryEntry GetDirectory(string name)
		{
			return (DirectoryEntry)Children.FirstOrDefault(d => d.GetType() == typeof(DirectoryEntry) && d.Name == name) ?? throw new InvalidOperationException($"Could not find subdirectory {name} in {this.Name}");
		}
	}


}
