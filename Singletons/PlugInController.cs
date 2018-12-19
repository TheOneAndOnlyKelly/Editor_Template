using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KellyControls.PlugInToolBtn;
using KellyControls.PlugInToolBtn.Interfaces;
using KellyControls.PlugInToolBtn.ToolGroups;
using KellyControls.PlugInToolBtn.Tools;

namespace Editor_Template.Singletons
{
	internal class PlugInController
	{
		#region [ Private Static Variables ]

		/// <summary>
		/// List of all PlugInTool objects, holding Tools loaded from the found assemblies
		/// </summary>
		private static PlugInToolList _plugInToolList;

		/// <summary>
		/// List of all PlugInToolGroup objects, holding ToolGroups loaded from the found assemblies
		/// </summary>
		private static PlugInToolGroupList _plugInToolGroupList;

		#endregion [ Private Static Variables ]

		#region [ Internal Static Properties ]

		/// <summary>
		/// Returns the list of PlugInTools. If the list is empty, then retrieves the list of PlugInTools from found Assemblies
		/// </summary>
		internal static PlugInToolList PlugInToolList
		{
			get
			{
				if (_plugInToolList == null)
					GetAllPlugIns();
				return _plugInToolList;
			}
		}

		/// <summary>
		/// Returns the list of PlugInToolGroups. If the list is empty, then retrieves the list of PlugInToolGroups from found Assemblies
		/// </summary>
		internal static PlugInToolGroupList PlugInToolGroupList
		{
			get
			{
				if (_plugInToolGroupList == null)
					GetAllPlugIns();
				return _plugInToolGroupList;
			}
		}
		
		#endregion [ Internal Static Properties ]

		#region [ Private Static Methods ]

		/// <summary>
		/// Finds all the Tools and ToolGroups from this and other Assemblies.
		/// </summary>
		private static void GetAllPlugIns()
		{
			if (_plugInToolList == null)
				_plugInToolList = new PlugInToolList();
			else
				_plugInToolList.Clear();

			if (_plugInToolGroupList == null)
				_plugInToolGroupList = new PlugInToolGroupList();
			else
				_plugInToolGroupList.Clear();

			// Get a list of all the other Assemblies that live in the same folder as this Assembly
			var PlugInAssemblies = LoadPlugInAssemblies();

			// Find all the ToolGroups in the Tools assembly, the initial Assembly.
			foreach (IPlugInToolGroup toolGroup in FindToolGroupsFromAssemblies(PlugInAssemblies))
			{
				_plugInToolGroupList.Add(new PlugInToolGroup(toolGroup, toolGroup.ID));
			}

			// Find all the Tools in the Tools assembly, the initial Assembly.
			foreach (IPlugInTool tool in FindToolsFromAssemblies(PlugInAssemblies))
			{
				_plugInToolList.Add(new PlugInTool(tool, tool.ID));
			}
			
			// Now sort the list in ID order.
			var Sorted = new SortedList<int, PlugInTool>();
			foreach (PlugInTool pTool in _plugInToolList)
			{
				Sorted.Add(pTool.ID, pTool);
			}

			_plugInToolList.Clear();
			foreach (var Pair in Sorted)
				_plugInToolList.Add(Pair.Value);
		}

		/// <summary>
		/// http://blogs.msdn.com/b/shawnfa/archive/2009/06/08/more-implicit-uses-of-cas-policy-loadfromremotesources.aspx
		/// Since this application only trusts a handful of LoadFrom operations,
		///	we'll put them all into the same AppDomain which is a simple sandbox
		///	with a full trust grant set.  The application itself will not enable
		///	loadFromRemoteSources, but instead data all of the trusted loads
		///	into this domain.
		/// </summary>
		/// <returns></returns>
		private static List<Assembly> LoadPlugInAssemblies()
		{
			var Directory = Assembly.GetEntryAssembly().ManifestModule.Assembly.Location;
			var ExecutingFile = new FileInfo(Directory);

			var LibraryList = new List<FileInfo>
			{
				ExecutingFile
			};

			// File information of the currently executing assembly.
			var ThisFile = new FileInfo(Assembly.GetExecutingAssembly().Location);

			// Get all the DLL libraries in the folder where the current assembly lives
			var files = ThisFile.Directory.GetFiles("*.dll");

			if (files != null)
				foreach (var file in files)
					LibraryList.Add(file);

			var AllAssemblies = new List<Assembly>();

			foreach (var file in LibraryList)
			{
				try
				{
					//if (file.FullName != ThisFile.FullName)
						AllAssemblies.Add(Assembly.LoadFile(file.FullName));
				}
				//catch (NotSupportedException ex)
				//{
				//	Debug.WriteLine(ex.ToString());
				//}
				//catch (BadImageFormatException ex)
				//{
				//	Debug.WriteLine(ex.ToString());
				//}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
				}
			}

			// Check to see if any of the listed assemblies has a class that implements the IPlugInTool or IPlugInToolGroup interface			
			var ValidAssemblies = new List<Assembly>();
			foreach (var Assembly in AllAssemblies)
			{
				var AvailableTypes = GetTypes(Assembly);
				var ToolList = AvailableTypes.FindAll(delegate (Type t)
				{
					var InterfaceTypes = new List<Type>(t.GetInterfaces());
					var ToolAttr = t.GetCustomAttributes(typeof(Tool), true);
					var GroupAttr = t.GetCustomAttributes(typeof(ToolGroup), true);

					return (((ToolAttr?.Length > 0) && InterfaceTypes.Contains(typeof(IPlugInTool))) ||
							((GroupAttr?.Length > 0) && InterfaceTypes.Contains(typeof(IPlugInToolGroup))));
				});
				if (ToolList?.Count > 0)
					ValidAssemblies.Add(Assembly);
			}
			return ValidAssemblies;
		}

		/// <summary>
		/// Retrieve the list of ToolGroups from the Assemblies that implement the PluggedToolCore attribute.
		/// </summary>
		/// <param name="assemblies">List of Assemblies previously retrieved.</param>
		private static List<IPlugInToolGroup> FindToolGroupsFromAssemblies(List<Assembly> assemblies)
		{
			var AvailableTypes = GetTypes(assemblies);

			// Get a list of objects that implement the IPlugInToolGroup interface AND 
			// have the ToolGroup
			var ToolList = AvailableTypes.FindAll(delegate (Type t)
			{
				var interfaceTypes = new List<Type>(t.GetInterfaces());
				var GroupAttr = t.GetCustomAttributes(typeof(ToolGroup), true);
				
				return (!(GroupAttr == null || GroupAttr.Length == 0)) &&
					interfaceTypes.Contains(typeof(IPlugInToolGroup));
			});

			AvailableTypes = null;

			// Convert the list of Objects to an instantiated list of IPlugInToolGroups
			return ToolList.ConvertAll<IPlugInToolGroup>(delegate (Type t)
			{
				return Activator.CreateInstance(t) as IPlugInToolGroup;
			});
		}

		/// <summary>
		/// Retrieve the list of Tools from the Assemblies that implement the PluggedToolCore attribute.
		/// </summary>
		/// <param name="assemblies">List of Assemblies previously retrieved.</param>
		private static List<IPlugInTool> FindToolsFromAssemblies(List<Assembly> assemblies)
		{
			var AvailableTypes = GetTypes(assemblies);

			// Get a list of objects that implement the IPlugInTool interface AND 
			// have the Tool class attribute
			var ToolList = AvailableTypes.FindAll(delegate (Type t)
			{
				var interfaceTypes = new List<Type>(t.GetInterfaces());
				var ToolAttr = t.GetCustomAttributes(typeof(Tool), true);

				return ((ToolAttr?.Length > 0) && 
						interfaceTypes.Contains(typeof(IPlugInTool)));

				//return (!(ToolAttr == null || ToolAttr.Length == 0)) &&
				//		(!(CoreAttr == null || CoreAttr.Length == 0)) &&
				//		interfaceTypes.Contains(typeof(IPlugInTool));
			});

			AvailableTypes = null;

			// Convert the list of Objects to an instantiated list of IPlugInTools
			return ToolList.ConvertAll<IPlugInTool>(delegate (Type t)
			{
				return Activator.CreateInstance(t) as IPlugInTool;
			});
		}
		

		/// <summary>
		/// Retrieves a list of Types from all the Assemblies.
		/// </summary>
		/// <param name="assemblies">List of Assemblies.</param>
		private static List<Type> GetTypes(List<Assembly> assemblies)
		{
			var AvailableTypes = new List<Type>();
			foreach (var Assembly in assemblies)
				AvailableTypes.AddRange(GetTypes(Assembly));
			return AvailableTypes;
		}

		private static List<Type> GetTypes(Assembly assembly)
		{
			var AvailableTypes = new List<Type>();
			try
			{
				AvailableTypes.AddRange(assembly.GetTypes());
			}
			catch (ReflectionTypeLoadException)
			{ }
			catch (Exception)
			{
				throw;
			}
			return AvailableTypes;
		}

		#endregion [ Private Static Methods ]

	}
}
