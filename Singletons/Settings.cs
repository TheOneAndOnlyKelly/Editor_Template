using System.IO;
using System.Xml;
using Editor_Template.Interfaces;
using KellyControls.CommonClasses;
using XmlHelperLib;

namespace Editor_Template.Singletons
{
	public class Settings
	{
		public enum SettingsStyle
		{
			Registry,
			Xml
		}

		#region [ Constants ]

		public const string SAVE_PATH_DELIMITER = "|";

		#endregion [ Constants ]

		#region [ Private Variables ]

		private static readonly Settings _instance = new Settings();
		private ISettings _iSettings = null;
		private string _settingsFileName = string.Empty;

		#endregion [ Private Variables ]

		#region [ Properties ]

		public static Settings Instance
		{
			get { return _instance; }
		}

		public SettingsStyle Style
		{
			set
			{
				if (value == SettingsStyle.Registry)
				{
					//_iSettings = new Registry_Settings();
				}
				else
				{
					//_iSettings = new Xml_Settings(Path.Combine(Workshop.GetDocumentPath(), "Elf.settings"));
					_iSettings = new Xml_Settings("Elf.settings");
				}
			}
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		static Settings()
		{ }

		private Settings()
		{
			_iSettings = new Xml_Settings();
		}

		#endregion [ Constructors ]

		#region [ Get Methods ]

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Boolean value found on the path, or the defaultValue</returns>
		public bool GetValue(string path, bool defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Integer value found on the path, or the defaultValue</returns>
		public int GetValue(string path, int defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Byte value found on the path, or the defaultValue</returns>
		public byte GetValue(string path, byte defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Floating point value found on the path, or the defaultValue</returns>
		public float GetValue(string path, float defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Double value found on the path, or the defaultValue</returns>
		public double GetValue(string path, double defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public string GetValue(string path, string defaultValue)
		{
			return _iSettings.GetValue(path, defaultValue);
		}

		#endregion [ Get Methods ]

		#region [ Remove Methods ]

		/// <summary>
		/// Removes the node at this path
		/// </summary>
		/// <param name="path">path to the node to remove</param>
		public void RemoveValue(string path)
		{
			_iSettings.RemoveValue(path);
		}

		#endregion [ Remove Methods ]

		#region [ Save Methods ]

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">String value to save</param>
		public void SetValue(string path, string value)
		{
			_iSettings.SetValue(path, value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Integer value to save</param>
		public void SetValue(string path, int value)
		{
			_iSettings.SetValue(path, value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Floating point value to save</param>
		public void SetValue(string path, float value)
		{
			_iSettings.SetValue(path, value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Boolean value to save</param>
		public void SetValue(string path, bool value)
		{
			_iSettings.SetValue(path, value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Double value to save</param>
		public void SetValue(string path, double value)
		{
			_iSettings.SetValue(path, value);
		}

		#endregion [ Save Methods ]

		/// <summary>
		/// Concatenates the path with a name, delimited by the pre-defined path delimiter character.
		/// </summary>
		/// <param name="path">Pre-built path</param>
		/// <param name="newNode">Name to be appended</param>
		public string AppendPath(string path, string newNode)
		{
			return path + Settings.SAVE_PATH_DELIMITER + newNode;
		}

		public void Save()
		{
			_iSettings.Save();
		}

		///// <summary>
		///// Delete the settings file from the Document path, and remove any entry from the registry
		///// </summary>
		//public void Zap()
		//{
		//	// First zap the file
		//	string FileName = Path.Combine(GlobalController.GetDocumentPath(), "AdjPreview.settings");
		//	FileInfo FI = new FileInfo(FileName);
		//	if (FI.Exists)
		//		FI.Delete();
		//	FI = null;

		//	FileName = Path.Combine(GlobalController.GetDocumentPath(), "Elf.settings");
		//	FI = new FileInfo(FileName);
		//	if (FI.Exists)
		//		FI.Delete();
		//	FI = null;

		//	// Now zap the entry in the registry
		//	string RPath = string.Format(Registry.REGISTRY_ADDIN_PATH, "Adjustable Preview").Replace("\\Settings", string.Empty);
		//	RegistryKey registrykeyHKLM = Microsoft.Win32.Registry.CurrentUser;
		//	string keyPath = RPath;
		//	registrykeyHKLM.DeleteSubKeyTree(keyPath);
		//	registrykeyHKLM.Close();
		//	registrykeyHKLM = null;
		//}

	}


	#region [ Class Registry_Settings ]
	/*
	internal class Registry_Settings : ISettings
	{
		
		#region [ Private Variables ]

		private Registry _registry = null;

		#endregion [ Private Variables ]

		#region [ Properties ]

		#endregion [ Properties ]

		#region [ Constructors ]

		public Registry_Settings()
		{ 
			_registry = new Registry(string.Format(Registry.REGISTRY_ADDIN_PATH, "Adjustable Preview"));
		}

		#endregion [ Constructors ]

		#region [ Get Methods ]

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Boolean value found on the path, or the defaultValue</returns>
		public bool GetValue(string path, bool defaultValue)
		{
			return _registry.GetValue(PathForRegistry(path), defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Boolean value found on the path, or the defaultValue</returns>
		public bool GetValue(XmlNode node, string path, bool defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			bool Value = defaultValue;
			if (bool.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Integer value found on the path, or the defaultValue</returns>
		public int GetValue(string path, int defaultValue)
		{
			return _registry.GetValue(PathForRegistry(path), defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Integer value found on the path, or the defaultValue</returns>
		public int GetValue(XmlNode node, string path, int defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			int Value = defaultValue;
			if (int.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Floating point value found on the path, or the defaultValue</returns>
		public float GetValue(string path, float defaultValue)
		{
			return _registry.GetValue(PathForRegistry(path), defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Floating point value found on the path, or the defaultValue</returns>
		public float GetValue(XmlNode node, string path, float defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			float Value = defaultValue;
			if (float.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public string GetValue(string path, string defaultValue)
		{
			return _registry.GetValue(PathForRegistry(path), defaultValue);
		}

		#endregion [ Get Methods ]

		#region [ Save Methods ]

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">String value to save</param>
		public void SetValue(string path, string value)
		{
			_registry.SetValue(PathForRegistry(path), value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Integer value to save</param>
		public void SetValue(string path, int value)
		{
			_registry.SetValue(PathForRegistry(path), value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Floating point value to save</param>
		public void SetValue(string path, float value)
		{
			_registry.SetValue(PathForRegistry(path), value);
		}

		/// <summary>
		/// Saves the value to the settings object along the path indicated
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Boolean value to save</param>
		public void SetValue(string path, bool value)
		{
			_registry.SetValue(PathForRegistry(path), value);
		}
		
		#endregion [ Save Methods ]

		private string PathForRegistry(string path)
		{
			path = path.Replace(@"|", @"\");
			return path;
		}
	}
		*/
	#endregion [ Class Registry_Settings ]

	#region [ Class Xml_Settings ]

	internal class Xml_Settings : Disposable, ISettings
	{
		#region [ Private Variables ]

		private XmlDocument _settingsXmlDoc = null;
		private XmlHelper _xmlHelper = new XmlHelper();
		private string _settingsFileName = string.Empty;

		#endregion [ Private Variables ]

		#region [ Constructors ]

		public Xml_Settings()
		{
			_settingsXmlDoc = new XmlDocument();
		}

		public Xml_Settings(string filename)
			: this()
		{
			_settingsFileName = filename;

			bool LoadXmlFile = false;

			if (!File.Exists(_settingsFileName))
				LoadXmlFile = false;
			else
			{
				FileInfo fi = new FileInfo(_settingsFileName);
				if (fi.Length == 0)
					LoadXmlFile = false;
				else
					LoadXmlFile = XmlHelper.IsValidXml(_settingsFileName);
			}

			if (LoadXmlFile)
				_settingsXmlDoc.Load(_settingsFileName);
			else
				_settingsXmlDoc.LoadXml("<Settings />");
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Clean up all child objects here, unlink all events and dispose
		/// </summary>
		protected override void DisposeChildObjects()
		{
			base.DisposeChildObjects();
			_settingsXmlDoc = null;
			_xmlHelper = null;
		}

		/// <summary>
		/// Converts the generic path to an XPath
		/// </summary>
		private string PathForXml(string path)
		{
			path = path.Replace(@"|", "/");
			path = path.Replace(" ", string.Empty);
			path = path.Replace("-", string.Empty);
			return path;
		}

		#region [ Get Methods ]

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Boolean value found on the path, or the defaultValue</returns>
		public bool GetValue(string path, bool defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return bool.Parse( _xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue.ToString()));
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Boolean value found on the path, or the defaultValue</returns>
		public bool GetValue(XmlNode node, string path, bool defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			bool Value = defaultValue;
			if (bool.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Integer value found on the path, or the defaultValue</returns>
		public int GetValue(string path, int defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return int.Parse(_xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue.ToString()));
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Byte value found on the path, or the defaultValue</returns>
		public byte GetValue(string path, byte defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return byte.Parse(_xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue.ToString()));
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Integer value found on the path, or the defaultValue</returns>
		public int GetValue(XmlNode node, string path, int defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			int Value = defaultValue;
			if (int.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Floating point value found on the path, or the defaultValue</returns>
		public float GetValue(string path, float defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return float.Parse(_xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue.ToString()));
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Value found on the path, or the defaultValue</returns>
		public double GetValue(string path, double defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return double.Parse(_xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue.ToString()));
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>Floating point value found on the path, or the defaultValue</returns>
		public float GetValue(XmlNode node, string path, float defaultValue)
		{
			string Result = GetValue(node, path, defaultValue.ToString());
			float Value = defaultValue;
			if (float.TryParse(Result, out Value))
				return Value;
			else
				return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the settings object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public string GetValue(string path, string defaultValue)
		{
			if (_settingsXmlDoc == null)
				return defaultValue;
			return _xmlHelper.GetNodeValue(_settingsXmlDoc.DocumentElement, PathForXml(path), defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public string GetValue(XmlNode node, string path, string defaultValue)
		{
			return _xmlHelper.GetNodeValue(node, PathForXml(path), defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="attributeName">Name of the attribute to load in</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public string GetValueAttribute(XmlNode node, string path, string attributeName, string defaultValue)
		{
			return _xmlHelper.GetAttribute(node.SelectSingleNode(path), attributeName, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the xml object along the path indicated. If the value is not present on that path, returns the default value.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path to locate the value</param>
		/// <param name="attributeName">Name of the attribute to load in</param>
		/// <param name="defaultValue">Value to retrieve if the path or data is missing</param>
		/// <returns>String value found on the path, or the defaultValue</returns>
		public int GetValueAttribute(XmlNode node, string path, string attributeName, int defaultValue)
		{
			return int.Parse(_xmlHelper.GetAttribute(node.SelectSingleNode(path), attributeName, defaultValue.ToString()));
		}

		#endregion [ Get Methods ]

		#region [ Remove Methods ]

		/// <summary>
		/// Removes the node at this path
		/// </summary>
		/// <param name="path">path to the node to remove</param>
		public void RemoveValue(string path)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.RemoveNode(_settingsXmlDoc, path);
		}

		#endregion [ Remove Methods ]

		#region [ Save Methods ]

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, string value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, int value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value.ToString());
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, byte value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value.ToString());
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, float value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value.ToString("0.00"));
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, double value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value.ToString("0.00"));
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(string path, bool value)
		{
			if (_settingsXmlDoc != null)
				_xmlHelper.SetNodeValue(_settingsXmlDoc, "Settings/" + PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(XmlNode node, string path, string value)
		{
			_xmlHelper.SetValue(node, PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(XmlNode node, string path, int value)
		{
			_xmlHelper.SetValue(node, PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(XmlNode node, string path, byte value)
		{
			_xmlHelper.SetValue(node, PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(XmlNode node, string path, bool value)
		{
			_xmlHelper.SetValue(node, PathForXml(path), value);
		}

		/// <summary>
		/// Saves the value using the path indicated.
		/// </summary>
		/// <param name="node">Xml node to search</param>
		/// <param name="path">Path on the xml object to save</param>
		/// <param name="value">Value to save</param>
		public void SetValue(XmlNode node, string path, float value)
		{
			_xmlHelper.SetValue(node, PathForXml(path), value);
		}

		#endregion [ Save Methods ]

		public void Save()
		{
			if ((_settingsFileName ?? string.Empty).Length > 0)
				_settingsXmlDoc.Save(_settingsFileName);
		}

		#endregion [ Methods ]
	}

	#endregion [ Class Xml_Settings ]
}
