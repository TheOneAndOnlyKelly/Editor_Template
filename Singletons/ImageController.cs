using System;
using System.Drawing;
using System.Windows.Forms;
using Editor_Template.Utilities;
using EnumerationLib;
using ERes = Editor_Template.Properties.Resources;

namespace Editor_Template.Singletons
{
	public class ImageController : KellyControls.CommonClasses.ImageController
	{
		#region [ Private Static Variables ]

		private new static readonly Lazy<ImageController> lazy = new Lazy<ImageController>(() => new ImageController());

		#endregion [ Private Static Variables ]

		#region [ Properties ]
		
		public new static ImageController Instance { get { return lazy.Value; } }

		#endregion [ Properties ]

		#region [ Annotations ]

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <returns>The composited image</returns>
		public Bitmap AddAnnotation(Bitmap image, Annotation annotation)
		{
			return AddAnnotation(image, (int)annotation, AnchorStyles.Top | AnchorStyles.Left, Point.Empty, true);
		}

		protected override Bitmap GetAnnotationImage(int annotation)
		{
			return GetAnnotationImage(EnumHelper.GetEnumFromValue<Annotation>(annotation));
		}

		/// <summary>
		/// Returns the image from the Resources that correspond to the enum.
		/// </summary>
		private Bitmap GetAnnotationImage(Annotation annotation)
		{
			switch (annotation)
			{
				case Annotation.Add:
					return ERes.annotation_add;
				case Annotation.As:
					return ERes.annotation_as;
				case Annotation.Attribute:
					return ERes.annotation_attribute;
				case Annotation.Branch:
					return ERes.annotation_branch;

				case Annotation.Calculate:
					return ERes.annotation_calculate;
				case Annotation.Check:
					return ERes.annotation_check;
				case Annotation.Clear:
					return ERes.annotation_clear;
				case Annotation.Compare:
					return ERes.annotation_compare;
				case Annotation.Copy:
					return ERes.annotation_copy;
				case Annotation.Create:
					return ERes.annotation_create;

				case Annotation.Database:
					return ERes.annotation_database;
				case Annotation.Delete:
					return ERes.annotation_delete;
				case Annotation.Delete_Small:
					return ERes.annotation_delete_small;
				case Annotation.Dislike:
					return ERes.annotation_dislike;
				case Annotation.DontFind:
					return ERes.annotation_dontfind;

				case Annotation.Edit:
					return ERes.annotation_edit;
				case Annotation.Edit_State:
					return ERes.annotation_edit_state;
				case Annotation.Edit_Vertical:
					return ERes.annotation_edit_vertical;
				case Annotation.Export:
					return ERes.annotation_export;

				case Annotation.File:
					return ERes.annotation_file;
				case Annotation.Find:
					return ERes.annotation_find;
				case Annotation.First:
					return ERes.annotation_first;
				case Annotation.Flyout:
					return ERes.annotation_flyout;
				case Annotation.Flag:
					return ERes.annotation_flag;
				case Annotation.From:
					return ERes.annotation_from;

				case Annotation.Gear:
					return ERes.annotation_gear;

				case Annotation.Import:
					return ERes.annotation_import;

				case Annotation.Last:
					return ERes.annotation_last;
				case Annotation.Like:
					return ERes.annotation_like;

				case Annotation.Max:
					return ERes.annotation_maximum;
				case Annotation.Min:
					return ERes.annotation_minimum;

				case Annotation.New_State:
					return ERes.annotation_new_state;
				case Annotation.Not:
					return ERes.annotation_not;
				case Annotation.Not_Available:
					return ERes.annotation_notavailable;
				case Annotation.Note:
					return ERes.annotation_note;
				case Annotation.Number:
					return ERes.annotation_number;

				case Annotation.Open:
					return ERes.annotation_open;

				case Annotation.Play:
					return ERes.annotation_play;
				case Annotation.Document:
					return ERes.annotation_Document;

				case Annotation.Question:
					return ERes.annotation_question;

				case Annotation.Record:
					return ERes.annotation_record;
				case Annotation.Right:
					return ERes.annotation_right;

				case Annotation.Stop:
					return ERes.annotation_stop;

				case Annotation.Text:
					return ERes.annotation_text;
				case Annotation.To:
					return ERes.annotation_to;
				case Annotation.Tool:
					return ERes.annotation_tool;

				case Annotation.Unused_State:
					return ERes.annotation_unused_state;

				case Annotation.Verify:
					return ERes.annotation_verify;
				case Annotation.View:
					return ERes.annotation_view;

				case Annotation.Warning:
					return ERes.annotation_warning;
				case Annotation.Warning_State:
					return ERes.annotation_warning_state;
				case Annotation.Watch:
					return ERes.annotation_watch;
				case Annotation.Wrench:
					return ERes.annotation_wrench;

				default:
					return null;
			}
		}

		#endregion [ Annotations ]

		#region [ Bitmaps ]

		public override void BuildJoinedImagesList()
		{
			base.BuildJoinedImagesList();
		}

		public override Bitmap GetBitmap(int objectType)
		{
			return GetBitmap(EnumHelper.GetEnumFromValue<ImageType>(objectType));
		}

		/// <summary>
		/// Finds the bitmap in the program resources that corresponds to the enumeration passed in.
		/// These bitmaps are used to populate the tree and menu system with images.
		/// </summary>
		/// <param name="objectType">Enum indicating the bitmap desired</param>
		/// <returns>If the bitmap is defined for the enum, returns it, else returns null</returns>
		public Bitmap GetBitmap(ImageType objectType)
		{
			var Offset = new Point(-2, 0);
			//const AnchorStyles ANCHOR_BR = AnchorStyles.Bottom | AnchorStyles.Right;
			//const AnchorStyles ANCHOR_BL = AnchorStyles.Bottom | AnchorStyles.Left;
			//const AnchorStyles ANCHOR_TL = AnchorStyles.Top | AnchorStyles.Left;

			switch (objectType)
			{

				#region [ A ]

				case ImageType.Add:
					return ERes.add;

				case ImageType.Asterisk:
					return ERes.asterisk;

				case ImageType.Attribute:
					return ERes.attribute;

				#endregion [ A ]

				#region [ B ]

				case ImageType.Balloon:
					return ERes.balloon;

				case ImageType.Batch:
					return ERes.batch;

				case ImageType.Bold:
					return ERes.bold;

				case ImageType.Bold_Selected:
					return ERes.bold_selected;

				case ImageType.Branch:
					return ERes.branch;

				case ImageType.Bullet:
					return ERes.bullet;

				#endregion [ B ]

				#region [ C ]

				case ImageType.Calculate:
					return ERes.calculate;

				case ImageType.Calendar:
					return ERes.calendar;

				case ImageType.Cancel:
					return ERes.cancel;

				case ImageType.Case_Sensitive:
					return ERes.case_sensitive;

				case ImageType.Chart_Bar:
					return ERes.chart_bar;

				case ImageType.Chart_Curve:
					return ERes.chart_curve;

				case ImageType.Check:
					return ERes.check;

				case ImageType.Check_Sql:
					return ERes.check_sql;

				case ImageType.Check_Xml:
					return ERes.checkxml;

				case ImageType.Clean:
					return ERes.clean;

				case ImageType.Clear:
					return ERes.clear;

				case ImageType.Clone:
					return ERes.clone;

				case ImageType.Close:
					return ERes.close;

				case ImageType.Cloud:
					return ERes.cloud;

				case ImageType.Code:
					return ERes.code;

				case ImageType.Color:
					return ERes.color;

				case ImageType.Column:
					return ERes.column;

				case ImageType.Combo:
					return ERes.combo;

				case ImageType.Compare:
					return ERes.compare;

				case ImageType.Complete:
					return ERes.complete;

				case ImageType.Copy:
					return ERes.copy;

				case ImageType.Create:
					return ERes.create;

				case ImageType.CSharp:
					return ERes.csharp;

				case ImageType.Cursor:
					return ERes.cursor;

				case ImageType.Cut:
					return ERes.cut;

				#endregion [ C ]

				#region [ D ]

				case ImageType.Database:
					return ERes.database;

				case ImageType.Delete:
					return ERes.delete;

				case ImageType.Description:
					return ERes.desc;

				case ImageType.Disline:
					return ERes.dislike;

				#endregion [ D ]

				#region [ E ]

				case ImageType.Edit:
					return ERes.edit;

				case ImageType.Editor:
					return ERes.editor;

				case ImageType.Engine:
					return ERes.engine;

				case ImageType.Error:
					return ERes.error;

				case ImageType.Exit:
					return ERes.exit;

				case ImageType.Export:
					return ERes.export;

				#endregion [ E ]

				#region [ F ]

				case ImageType.Field:
					return ERes.field;

				case ImageType.FieldValue:
					return ERes.fieldValue;

				case ImageType.File:
					return ERes.file;

				case ImageType.File_Open:
					return ERes.file_open;

				case ImageType.Find:
					return ERes.find;

				case ImageType.Flag:
					return ERes.flag;

				case ImageType.Flyout:
					return ERes.flyout;

				case ImageType.Folder_Closed:
					return ERes.folder_closed;

				case ImageType.Folder_Open:
					return ERes.folder_open;

				case ImageType.Font:
					return ERes.font;

				case ImageType.Edit_Text:
					return ERes.edit_text;

				case ImageType.Font_Size:
					return ERes.fontsize;

				#endregion [ F ]

				#region [ G-H ]

				case ImageType.Grabbed:
					return ERes.grabbed;

				case ImageType.Group:
					return ERes.group;

				#endregion [ G-H ]

				#region [ I ]

				case ImageType.Image:
					return ERes.image;

				case ImageType.Image_Export:
					return ERes.image_export;

				case ImageType.Image_Import:
					return ERes.image_import;

				case ImageType.Image_Missing:
					return ERes.image_missing;

				case ImageType.Image_Resize:
					return ERes.image_resize;

				case ImageType.Import:
					return ERes.import;

				case ImageType.Infinity:
					return ERes.infinity;

				case ImageType.Info:
					return ERes.info;

				case ImageType.Italic:
					return ERes.italic;

				case ImageType.Italic_Selected:
					return ERes.italic_selected;

				#endregion [ I ]

				#region [ J-L ]

				case ImageType.JSON_Script:
					return ERes.jsonScript;

				case ImageType.Key:
					return ERes.key;

				case ImageType.Keyboard:
					return ERes.keyboard;

				case ImageType.Label:
					return ERes.label;

				case ImageType.List:
					return ERes.list;

				case ImageType.Lock:
					return ERes._lock;

				case ImageType.Lock_State:
					return ERes.lock_state;

				case ImageType.Log:
					return ERes.log;

				#endregion [ J-L ]

				#region [ M ]

				case ImageType.Merge:
					return ERes.merge;

				case ImageType.Message:
					return ERes.message;

				case ImageType.Move:
					return ERes.move;

				case ImageType.Move_Down:
					return ERes.move_down;

				case ImageType.Move_Selected:
					return ERes.move_selected;

				case ImageType.Move_To_Bottom:
					return ERes.move_to_bottom;

				case ImageType.Move_To_Top:
					return ERes.move_to_top;

				case ImageType.Move_Up:
					return ERes.move_up;

				#endregion [ M ]

				#region [ N ]

				case ImageType.Next:
					return ERes.next;

				case ImageType.Node:
					return ERes.node;

				case ImageType.Not:
					return ERes.not;

				#endregion [ N ]

				#region [ O ]

				case ImageType.Ok:
					return ERes.ok;

				case ImageType.Open:
					return ERes.open;

				case ImageType.Option:
					return ERes.option;

				case ImageType.Option_Checked:
					return ERes.option_checked;

				case ImageType.Output:
					return ERes.output;

				#endregion [ O ]

				#region [ P ]

				case ImageType.Palette:
					return ERes.palette;

				case ImageType.Pan:
					return ERes.pan;

				case ImageType.Paste:
					return ERes.paste;

				case ImageType.PDF:
					return ERes.pdf;

				case ImageType.PopUp:
					return ERes.popup;

				case ImageType.Previous:
					return ERes.previous;

				case ImageType.Print:
					return ERes.print;

				case ImageType.Print_Preview:
					return ERes.print_preview;

				case ImageType.Print_Setup:
					return ERes.print_setup;

				case ImageType.Document:
					return ERes.Document;

				case ImageType.Properties:
					return ERes.properties;

				#endregion [ P ]

				#region [ Q ]

				#endregion [ Q ]

				#region [ R ]

				case ImageType.Recent:
					return ERes.recent;

				case ImageType.Record:
					return ERes.record;

				case ImageType.Redo:
					return ERes.redo;

				case ImageType.Refresh:
					return ERes.refresh;

				case ImageType.Rename:
					return ERes.rename;

				case ImageType.Rotate_Left:
					return ERes.rotate_left;

				case ImageType.Rotate_Right:
					return ERes.rotate_right;

				case ImageType.Row:
					return ERes.row;

				case ImageType.Ruler:
					return ERes.ruler;

				case ImageType.Run:
					return ERes.run;

				#endregion [ R ]

				#region [ S ]

				case ImageType.Save:
					return ERes.save;

				case ImageType.Save_All:
					return ERes.save_all;

				case ImageType.Save_As:
					return ERes.save_as;

				case ImageType.Save_Close:
					return ERes.save_close;

				case ImageType.Score:
					return ERes.score;

				case ImageType.Script:
					return ERes.script;

				case ImageType.Section:
					return ERes.section;

				case ImageType.Select:
					return ERes.select;

				case ImageType.Select_All:
					return ERes.select_all;

				case ImageType.Select_Selected:
					return ERes.select_selected;

				case ImageType.Snippet:
					return ERes.snippet;

				case ImageType.Settings:
					return ERes.settings;

				case ImageType.Sort:
					return ERes.sort;

				case ImageType.Sync:
					return ERes.sync;

				#endregion [ S ]

				#region [ T ]

				case ImageType.Team:
					return ERes.team;

				case ImageType.Text:
					return ERes.text;

				case ImageType.Text_Tool:
					return ERes.text_tool;

				case ImageType.Text_Tool_Selected:
					return ERes.text_tool_selected;

				case ImageType.Toggle_Guides:
					return ERes.toggle_guides;

				case ImageType.Tool:
					return ERes.tool;

				case ImageType.Tree:
					return ERes.tree;

				case ImageType.Tree_Collapse:
					return ERes.tree_collapse;

				case ImageType.Tree_Expand:
					return ERes.tree_expand;

				#endregion [ T ]

				#region [ U ]

				case ImageType.Undefined:
					return ERes.undefined;

				case ImageType.Undo:
					return ERes.undo;

				case ImageType.Underline:
					return ERes.underline;

				case ImageType.Underline_Selected:
					return ERes.underline_selected;

				case ImageType.Ungroup:
					return ERes.ungroup;

				case ImageType.Unlock:
					return ERes.unlock;

				case ImageType.User:
					return ERes.user;

				case ImageType.User_Blue:
					return ERes.user_blue;

				case ImageType.User_Gold:
					return ERes.user_gold;

				#endregion [ U ]

				#region [ V-Z ]

				case ImageType.Version:
					return ERes.version;

				case ImageType.Visible:
					return ERes.visible;

				case ImageType.Warning:
					return ERes.warning;

				case ImageType.Xml:
					return ERes.xml;

				case ImageType.Zoom:
					return ERes.zoom;

				case ImageType.Zoom_Minus:
					return ERes.zoom_minus;

				case ImageType.Zoom_Original_Size:
					return ERes.zoom_original_size;

				case ImageType.Zoom_Plus:
					return ERes.zoom_plus;

				case ImageType.Zoom_Selected:
					return ERes.zoom_selected;

				case ImageType.Zoom_To_Fit:
					return ERes.zoom_to_fit;

				#endregion [ V-Z ]

				default:
					return ERes.image_missing;
			}
		}

		#endregion [ Bitmaps ]

		#region [ Folders ]

		/// <summary>
		/// Determines which Icon is currently selected and returns the enum equivalent to the closed image.
		/// If it is not in the list, returns the original value.
		/// </summary>
		/// <param name="imageType"></param>
		/// <returns></returns>
		public override int GetClosedFolder(int imageType)
		{
			var OpenIcon = EnumHelper.GetEnumFromValue<ImageType>(imageType);
			
			switch (OpenIcon)
			{
				case ImageType.Folder_Open:
					return (int)ImageType.Folder_Closed;

				default:
					return imageType;
			}
		}

		/// <summary>
		/// Determines which Icon is currently selected and returns the enum equivalent to the open image.
		/// If it is not in the list, returns the original value.
		/// </summary>
		/// <param name="imageType"></param>
		/// <returns></returns>
		public new int GetOpenFolder(int imageType)
		{
			var ClosedIcon = EnumHelper.GetEnumFromValue<ImageType>(imageType);

			switch (ClosedIcon)
			{
				case ImageType.Folder_Closed:
					return (int)ImageType.Folder_Open;

				default:
					return imageType;
			}
		}

		#endregion [ Folders ]
	}
}
