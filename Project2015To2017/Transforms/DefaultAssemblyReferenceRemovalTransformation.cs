using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Project2015To2017.Definition;

namespace Project2015To2017.Transforms
{
	public sealed class DefaultAssemblyReferenceRemovalTransformation : ITransformation
	{
		public void Transform(Project definition)
		{
			if (definition.AssemblyReferences == null)
			{
				definition.AssemblyReferences = ImmutableArray<AssemblyReference>.Empty;
				return;
			}

			var (assemblyReferences, removeQueue) = definition.AssemblyReferences
				.Split(IsNonDefaultIncludedAssemblyReference);

			foreach (var assemblyReference in removeQueue)
			{
				assemblyReference.DefinitionElement?.Remove();
			}

			definition.AssemblyReferences = assemblyReferences;
		}


		private static bool IsNonDefaultIncludedAssemblyReference(AssemblyReference assemblyReference)
		{
			var name = assemblyReference.Include;
			return !new[]
			{
				"System",
				"System.Core",
				"System.Data",
				"System.Drawing",
				"System.IO.Compression.FileSystem",
				"System.Numerics",
				"System.Runtime.Serialization",
				"System.Xml",
				"System.Xml.Linq"
			}.Contains(name);
		}
	}
}