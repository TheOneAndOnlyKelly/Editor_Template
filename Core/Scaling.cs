using System.Diagnostics;
using System.Drawing;
using Editor_Template.Utilities;

namespace Editor_Template.Core
{
	public class Scaling : Base
	{
		#region [ Constants ]

		public const float MAX_ZOOM = 5f;
		public const float MIN_ZOOM = 1f;
		public const float ZOOM_100 = 1f;

		// Property Names


		#endregion [ Constants ]

		#region [ Private Variables ]

		protected int? _cellSize = null;
		protected float? _zoom = null;
		protected bool? _showGridLines = null;
		protected Size _latticeSize = Size.Empty;

		// Pre-calculated values
		private int _cellGrid = 0;
		private float _cellScaleF = 0;
		private float _cellZoomF = 0;

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Calculated size of the display Canvas.
		/// </summary>
		[DebuggerHidden()]
		public Size CanvasSize
		{
			get { return CalcCanvasSize(_latticeSize); }
		}

		/// <summary>
		/// Calculated size of the display Canvas.
		/// </summary>
		[DebuggerHidden()]
		public SizeF CanvasSizeF
		{
			get { return CalcCanvasSizeF(_latticeSize); }
		}

		/// <summary>
		/// Calculated width of a Cell plus the width of the space betwen the Cells.
		/// </summary>
		[DebuggerHidden()]
		public int CellGrid
		{
			get { return _cellGrid; }
		}

		/// <summary>
		/// Calculated width of a Cell plus the width of the space betwen the Cells.
		/// </summary>
		[DebuggerHidden()]
		public float CellGridF
		{
			get { return (float)_cellGrid; }
		}

		/// <summary>
		/// Calculated width of a Cell plus the width of the space betwen the Cells, multiplied by the current Zoom amount.
		/// </summary>
		[DebuggerHidden()]
		public int CellScale
		{
			get { return (int)_cellScaleF; }
		}

		/// <summary>
		/// Calculated width of a Cell plus the width of the space betwen the Cells, multiplied by the current Zoom amount.
		/// </summary>
		[DebuggerHidden()]
		public float CellScaleF
		{
			get { return _cellScaleF; }
		}

		/// <summary>
		/// Size of the Cells (in pixels)
		/// </summary>
		public virtual int? CellSize
		{
			get { return _cellSize; }
			set
			{
				if (_cellSize != value)
				{
					_cellSize = value;
					OnPropertyChanged(Constants.PROPERTY_CELL_SIZE, true);
				}
			}
		}

		/// <summary>
		/// Calculated width of a Cell, multiplied by the current Zoom amount.
		/// </summary>
		[DebuggerHidden()]
		public int CellZoom
		{
			get { return (int)_cellZoomF; }
		}

		/// <summary>
		/// Calculated width of a Cell, multiplied by the current Zoom amount.
		/// </summary>
		[DebuggerHidden()]
		public float CellZoomF
		{
			get { return _cellZoomF; }
		}

		/// <summary>
		/// Calculated width of the space between Cells.
		/// </summary>
		[DebuggerHidden()]
		public int GridLineWidth
		{
			get { return (_showGridLines.GetValueOrDefault(true) ? 1 : 0); }
		}

		/// <summary>
		/// Calculated width of the space between Cells factoring Zoom
		/// </summary>
		[DebuggerHidden()]
		public float GridLineWidthZoom
		{
			get { return this.GridLineWidth * _zoom.GetValueOrDefault(0); }
		}

		/// <summary>
		/// Size of the Lattice
		/// </summary>
		public virtual Size LatticeSize
		{
			get { return _latticeSize; }
			set
			{
				if (!_latticeSize.Equals(value) && !value.IsEmpty)
				{
					_latticeSize = value;
					OnPropertyChanged(Constants.PROPERTY_LATTICE_SIZE, true);
				}
			}
		}

		/// <summary>
		/// Indicates if Grid Lines should be displayed between the Cells
		/// </summary>
		public virtual bool? ShowGridLines
		{
			get { return _showGridLines; }
			set
			{
				if (_showGridLines != value)
				{
					_showGridLines = value;
					OnPropertyChanged(Constants.PROPERTY_SHOW_GRID_LINES, true);
				}
			}
		}

		/// <summary>
		/// Zoom amount. 
		/// </summary>
		public virtual float? Zoom
		{
			get { return _zoom; }
			set
			{
				float? Temp = value;
				if (Temp == null)
				{
					_zoom = value;
					return;
				}
				if (Temp < MIN_ZOOM)
					Temp = MIN_ZOOM;
				else if (Temp > MAX_ZOOM)
					Temp = MAX_ZOOM;

				//if ((value < MIN_ZOOM) || (value > MAX_ZOOM))
				//return;

				//if (_zoom != value)
				if (_zoom != Temp)
				{
					//_zoom = value;
					_zoom = Temp;
					OnPropertyChanged(Constants.PROPERTY_ZOOM, true);
				}
			}
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		/// <summary>        
		/// The default Constructor.        
		/// </summary>        
		public Scaling()
			: base()
		{ }

		public Scaling(bool setDefaults)
			: this()
		{
			if (setDefaults)
			{
				_cellSize = 1;
				_zoom = ZOOM_100;
				_showGridLines = true;
				_latticeSize = new Size(64, 32);
				RecalculateScalingValues();
			}
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Calculates what the Canvas Size would be for a give Lattice Size.
		/// </summary>
		/// <param name="latticeSize">SizeF to use for the lattice.</param>
		public SizeF CalcCanvasSizeF(SizeF latticeSize)
		{
			return new SizeF((_latticeSize.Width * _cellScaleF) + GridLineWidthZoom, (_latticeSize.Height * _cellScaleF) + GridLineWidthZoom);
		}

		/// <summary>
		/// Calculates what the Canvas Size would be for a give Lattice Size.
		/// </summary>
		/// <param name="latticeSize">Size to use for the lattice.</param>
		public Size CalcCanvasSize(Size latticeSize)
		{
			SizeF Calc = CalcCanvasSizeF(latticeSize);
			return new Size((int)Calc.Width, (int)Calc.Height);
		}

		/// <summary>
		///  Clears out all the value for the properies and protected virtual variables. Used to initialize the object initially, and when loading new data
		/// </summary>
		public virtual void Clear()
		{
			Clear(false);
		}

		/// <summary>
		///  Clears out all the value for the properies and protected virtual variables. Used to initialize the object initially, and when loading new data
		/// </summary>
		/// <param name="setDefaults">Sets the properties to be default values instead of nulling them.</param>
		public virtual void Clear(bool setDefaults)
		{
			if (setDefaults)
			{
				_cellSize = 1;
				_zoom = ZOOM_100;
				_showGridLines = true;
				_latticeSize = new Size(64, 32);
			}
			else
			{
				_cellSize = null;
				_zoom = null;
				_latticeSize = Size.Empty;
				_showGridLines = null;
			}
			RecalculateScalingValues();
		}

		/// <summary>
		/// Copy the data from the source object to this one.
		/// </summary>
		/// <param name="source">Object to copy from</param>
		public void CopyFrom(Scaling source)
		{
			this.SuppressEvents = true;

			_cellSize = source.CellSize;
			_zoom = source.Zoom;
			_showGridLines = source.ShowGridLines;
			_latticeSize = source.LatticeSize;
			_cellGrid = source.CellGrid;
			_cellScaleF = source.CellScaleF;
			_cellZoomF = source.CellZoomF;

			this.SuppressEvents = false;
		}

		/// <summary>
		/// Indicates whether there is data stored in this object
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				if (!_latticeSize.IsEmpty)
					return false;
				if (_cellSize != null)
					return false;
				if (_showGridLines != null)
					return false;
				if (_zoom != null)
					return false;
				return true;
			}
		}

		/// <summary>
		/// Recalculates the values representing size variables, such as CellGrid, etc. Called when one or more of the properties that make up these scales have been changed.
		/// </summary>
		public void RecalculateScalingValues()
		{
			_cellGrid = _cellSize.GetValueOrDefault(1) + GridLineWidth;
			_cellScaleF = _cellGrid * _zoom.GetValueOrDefault(1f);
			_cellZoomF = _cellSize.GetValueOrDefault(1) * _zoom.GetValueOrDefault(1f);
		}

		#endregion [ Methods ]

		#region [ Events ]

		/// <summary>
		/// Called when a property's value has been changed. Raises the PropertyChanged event with the name of the Property. 
		/// If indicated, sets the Dirty flag for this object.
		/// </summary>
		/// <param name="propertyName">Name of the property that has been changed.</param>
		/// <param name="setDirty">Indicates whether the Dirty flag should be set to true.</param>
		protected override void OnPropertyChanged(string propertyName, bool setDirty)
		{
			RecalculateScalingValues();
			base.OnPropertyChanged(propertyName, setDirty);
		}

		#endregion [ Events ]
	}
}
