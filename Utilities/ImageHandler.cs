using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ERes = Editor_Template.Properties.Resources;

namespace Editor_Template.Utilities
{
	public static class ImageHandler
	{
		#region [ Static Variables ]
		
		private static List<JoinedImages> _joinedImages = new List<JoinedImages>();

		#endregion [ Static Variables ]

		#region [ Static Methods ]

		#region [ Annotations ]

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation)
		{
			return AddAnnotation(image, annotation, AnchorStyles.Top | AnchorStyles.Left, Point.Empty, true);
		}

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <param name="offset">Amount to offset the source image</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation, Point offset)
		{
			return AddAnnotation(image, annotation, AnchorStyles.Top | AnchorStyles.Left, offset, true);
		}

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation, AnchorStyles anchor)
		{
			return AddAnnotation(image, annotation, anchor, Point.Empty, true);
		}

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <param name="offset">Amount to offset the source image</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation, AnchorStyles anchor, Point offset)
		{
			return AddAnnotation(image, annotation, anchor, offset, true);
		}

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <param name="overlap">Indicates whether the annotation image should be over the image, or if the image should be on top of the annotation image.</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation, AnchorStyles anchor, bool overlap)
		{
			return AddAnnotation(image, annotation, anchor, Point.Empty, overlap);
		}

		/// <summary>
		/// Combines an image with a bitmap representing an Annotation
		/// </summary>
		/// <param name="image">Source image to modify</param>
		/// <param name="annotation">Annotation enum that indicates what Icon to insert</param>
		/// <param name="anchor">AnchorStyles enum that indicates how the annotation should be positioned relative to the image.</param>
		/// <param name="offset">Amount to offset the source image</param>
		/// <param name="overlap">Indicates whether the annotation image should be over the image, or if the image should be on top of the annotation image.</param>
		/// <returns>The composited image</returns>
		public static Bitmap AddAnnotation(Bitmap image, Annotation annotation, AnchorStyles anchor, Point offset, bool overlap)
		{
			if (image == null)
				throw new System.ArgumentNullException("image cannot be null.");

			Bitmap Copy = new Bitmap(image.Width, image.Height);

			Bitmap annotationImage = GetAnnotationImage(annotation);
			if (annotationImage == null)
				throw new Exception("annotation not found: " + annotation.ToString());

			int Width = annotationImage.Width;
			int Height = annotationImage.Height;
			int X = 0;
			int Y = 0;

			if ((anchor & AnchorStyles.Left) == AnchorStyles.Left)
				X = 0;
			else if ((anchor & AnchorStyles.Right) == AnchorStyles.Right)
				X = Copy.Width - Width;
			else
				X = (Copy.Width - Width) / 2;

			if ((anchor & AnchorStyles.Top) == AnchorStyles.Top)
				Y = 0;
			else if ((anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
				Y = Copy.Height - Height;
			else
				Y = (Copy.Height - Height) / 2;

			using (Graphics g = Graphics.FromImage(Copy))
			{
				g.Clear(Color.Transparent);
				if (overlap)
					g.DrawImage(image, new Rectangle(offset.X, offset.Y, image.Width, image.Height));
				g.DrawImage(annotationImage, new Rectangle(X, Y, Width, Height));
				if (!overlap)
					g.DrawImage(image, new Rectangle(offset.X, offset.Y, image.Width, image.Height));
			}


			return Copy;
		}

		/// <summary>
		/// Returns the image from the Resources that correspond to the enum.
		/// </summary>
		private static Bitmap GetAnnotationImage(Annotation annotation)
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
				case Annotation.Profile:
					return ERes.annotation_profile;

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

		/// <summary>
		/// Finds the bitmap in the program resources that corresponds to the enumeration passed in.
		/// These bitmaps are used to populate the tree and menu system with images.
		/// </summary>
		/// <param name="objectType">Enum indicating the bitmap desired</param>
		/// <returns>If the bitmap is defined for the enum, returns it, else returns null</returns>
		public static Bitmap GetBitmap(ImageType objectType)
		{
			var Offset = new Point(-2, 0);
			const AnchorStyles ANCHOR_BR = AnchorStyles.Bottom | AnchorStyles.Right;
			const AnchorStyles ANCHOR_BL = AnchorStyles.Bottom | AnchorStyles.Left;
			const AnchorStyles ANCHOR_TL = AnchorStyles.Top | AnchorStyles.Left;

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

				case ImageType.Font_Edit:
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

				case ImageType.Profile:
					return ERes.profile;

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

		/// <summary>
		/// Create the requested image and composite it based on the annotation passed in.
		/// </summary>
		/// <param name="objectType">NodeType enum indicating the object</param>
		/// <param name="annotation">Annotation to add to the image</param>
		/// <returns>Composited Bitmap</returns>
		public static Bitmap GetBitmap(ImageType objectType, Annotation annotation)
		{
			if (_joinedImages == null)
				return AddAnnotation(GetBitmap(objectType), annotation);
			var Joined = _joinedImages.Where(j => j.ImageType == objectType && j.Annotation == annotation).ToList();
			return Joined.Count > 0 ? Joined[0].Bitmap : AddAnnotation(GetBitmap(objectType), annotation);
		}

		public static void BuildJoinedImagesList()
		{
			if (_joinedImages.Count > 0)
				return;

			//_joinedImages.Add(new JoinedImages(ImageType.Document, Annotation.New, ERes.Document_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Tool, Annotation.New, ERes.tool_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Rule, Annotation.New, ERes.rule_new));
			//_joinedImages.Add(new JoinedImages(ImageType.RuleTemplate, Annotation.New, ERes.rule_template_new));
			//_joinedImages.Add(new JoinedImages(ImageType.ColumnHeader, Annotation.New, ERes.column_header_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Column, Annotation.New, ERes.column_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Row, Annotation.New, ERes.row_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Action, Annotation.New, ERes.action_new));
			//_joinedImages.Add(new JoinedImages(ImageType.InterviewQuestion, Annotation.New, ERes.interviewquestion_new));
			//_joinedImages.Add(new JoinedImages(ImageType.PrequalificationQuestion, Annotation.New, ERes.prequalification_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Verification, Annotation.New, ERes.verification_new));
			//_joinedImages.Add(new JoinedImages(ImageType.Section, Annotation.New, ERes.section_new));
		}


		#endregion [ Bitmaps ]

		#region [ Folders ]

		/// <summary>
		/// Determines which Icon is currently selected and returns the enum equivalent to the closed image.
		/// If it is not in the list, returns the original value.
		/// </summary>
		/// <param name="imageType"></param>
		/// <returns></returns>
		public static ImageType GetClosedFolder(int imageType)
		{
			var OpenIcon = EnumerationLib.EnumHelper.GetEnumFromValue<ImageType>(imageType);

			switch (OpenIcon)
			{
				case ImageType.Folder_Open:
					return ImageType.Folder_Closed;

				default:
					return OpenIcon;
			}
		}

		/// <summary>
		/// Determines which Icon is currently selected and returns the enum equivalent to the open image.
		/// If it is not in the list, returns the original value.
		/// </summary>
		/// <param name="imageType"></param>
		/// <returns></returns>
		public static ImageType GetOpenFolder(int imageType)
		{
			var ClosedIcon = EnumerationLib.EnumHelper.GetEnumFromValue<ImageType>(imageType);

			switch (ClosedIcon)
			{
				case ImageType.Folder_Closed:
					return ImageType.Folder_Open;

				default:
					return ClosedIcon;
			}
		}

		#endregion [ Folders ]

		#region [ Icons ]

		/// <summary>
		/// Pulls out an icon from the Resources based the ImageType
		/// </summary>
		/// <param name="imageType">Enumeration indicating which icon to return</param>
		/// <param name="isNew">Indicates if the icon should have a "new" annotation. Relies on if that icon was precreated.</param>
		/// <returns>System.Drawing.Icon</returns>
		public static Icon GetIcon(ImageType imageType, bool isNew = false)
		{
			switch (imageType)
			{

				case ImageType.Editor:
					return ERes.icon_editor;

				case ImageType.JSON_Script:
					return ERes.icon_jsonScript;

				case ImageType.Script:
					return ERes.icon_script;

				case ImageType.Settings:
					return ERes.icon_settings;

				case ImageType.Text:
					return ERes.icon_text;

				case ImageType.Tree:
					return ERes.icon_tree;

				case ImageType.Xml:
					return ERes.icon_xml;

				default:
					return null;
			}
		}

		#endregion [ Icons ]
		
		#region [ Misc ]

		/// <summary>
		/// Creates an image that contains the text specified.
		/// </summary>
		/// <param name="message">Message to print on bitmap.</param>
		/// <param name="background">Color to use for the background.</param>
		/// <param name="foreground">Color to use for the foreground.</param>
		/// <returns></returns>
		public static Bitmap CreateErrorMessageImage(string message, Color background, Color foreground)
		{
			Bitmap Output = null;
			message += "  ";
			int Width, Height;

			using (Font Font = new Font("Arial", 12f))
			{
				using (Bitmap Temp = new Bitmap(16, 16))
				using (Graphics g = Graphics.FromImage(Temp))
				{
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
					StringFormat Format = new StringFormat();
					Format.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, message.Length) });
					Region[] r = g.MeasureCharacterRanges(message, Font, new Rectangle(0, 0, 1000, 1000), Format);
					RectangleF rect = r[0].GetBounds(g);
					Width = (int)rect.Width;
					Height = (int)rect.Height;
					Format = null;
				}
				Output = new Bitmap(Width + 32, Height + 16);
				using (Graphics g = Graphics.FromImage(Output))
				{
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
					g.Clear(background);
					using (Pen Pen = new Pen(foreground))
						g.DrawRectangle(Pen, new Rectangle(0, 0, Output.Width - 1, Output.Height - 1));
					using (SolidBrush Brush = new SolidBrush(foreground))
						g.DrawString(message, Font, Brush, new Rectangle(16, 8, Width + 20, Height), new StringFormat());
				}
			}
			return Output;
		}

		/// <summary>
		/// Creates a fuzzy drop shadow on the graphics device, defined by the GraphicsPath passed in.
		/// http://www.codeproject.com/Articles/15847/Fuzzy-DropShadows-in-GDI
		/// </summary>
		/// <param name="g">Graphics object to use.</param>
		/// <param name="path">Path for the fuzzy drop shadow to follow.</param>
		/// <param name="shadowColor">Color to make the drop shadow. In the example on the website, Color.DimGray was used.</param>
		/// <param name="transparency">Transparency of the shadow. In the example on the website, a value of 180 was used.</param>
		public static void FuzzyDropShadow(Graphics g, GraphicsPath path, Color shadowColor, byte transparency)
		{
			using (PathGradientBrush Brush = new PathGradientBrush(path))
			{
				// Set the wrapmode so that the colors will layer themselves from the outer edge in
				Brush.WrapMode = WrapMode.Clamp;

				// Create a color blend to manage our colors and positions and
				// since we need 3 colors set the default length to 3
				ColorBlend ColorBlend = new ColorBlend(3);

				// Here is the important part of the shadow making process, remember the clamp mode on the colorblend object layers the colors from
				// the outside to the center so we want our transparent color first followed by the actual shadow color. Set the shadow color to a 
				// slightly transparent DimGray, I find that it works best.
				ColorBlend.Colors = new Color[] { Color.Transparent, Color.FromArgb(transparency, shadowColor), Color.FromArgb(transparency, shadowColor) };

				// Our color blend will control the distance of each color layer we want to set our transparent color to 0 indicating that the 
				// transparent color should be the outer most color drawn, then our Dimgray color at about 10% of the distance from the edgen
				//ColorBlend.Positions = new float[] { 0f, .1f, 1f };
				ColorBlend.Positions = new float[] { 0f, .4f, 1f };

				// Assign the color blend to the pathgradientbrush
				Brush.InterpolationColors = ColorBlend;

				// Fill the shadow with our pathgradientbrush
				g.FillPath(Brush, path);
			}
		}

		/// <summary>
		/// Gets the image format from the image.
		/// </summary>
		public static ImageFormat GetImageFormat(Image img)
		{
			if (img.RawFormat.Equals(ImageFormat.Jpeg))
				return ImageFormat.Jpeg;
			if (img.RawFormat.Equals(ImageFormat.Bmp))
				return ImageFormat.Bmp;
			if (img.RawFormat.Equals(ImageFormat.Png))
				return ImageFormat.Png;
			if (img.RawFormat.Equals(ImageFormat.Emf))
				return ImageFormat.Emf;
			if (img.RawFormat.Equals(ImageFormat.Exif))
				return ImageFormat.Exif;
			if (img.RawFormat.Equals(ImageFormat.Gif))
				return ImageFormat.Gif;
			if (img.RawFormat.Equals(ImageFormat.Icon))
				return ImageFormat.Icon;
			if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
				return ImageFormat.MemoryBmp;
			if (img.RawFormat.Equals(ImageFormat.Tiff))
				return ImageFormat.Tiff;
			else
				return ImageFormat.Wmf;
		}

		/// <summary>
		/// Based on the raw format of the bitmap object, return the extension that best fits.
		/// </summary>
		/// <param name="bitmap">Bitmap object to be examined.</param>
		/// <returns>Returns extention based on raw format.</returns>
		public static string GetFileExtension(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap cannot be null.");

			ImageFormat Format = GetImageFormat(bitmap);

			if (Format.Equals(ImageFormat.Jpeg))
				return "jpg";
			else if (Format.Equals(ImageFormat.Bmp))
				return "bmp";
			else if (Format.Equals(ImageFormat.Emf))
				return "emf";
			else if (Format.Equals(ImageFormat.Exif))
				return "exf";
			else if (Format.Equals(ImageFormat.Gif))
				return "gif";
			else if (Format.Equals(ImageFormat.MemoryBmp))
				return "mbmp";
			else if (Format.Equals(ImageFormat.Png))
				return "png";
			else if (Format.Equals(ImageFormat.Tiff))
				return "tif";
			else if (Format.Equals(ImageFormat.Wmf))
				return "wmf";
			else
				return string.Empty;
		}

		/// <summary>
		/// Loads a bitmap from file.
		/// </summary>
		/// <param name="filename">Name of the file to load.</param>
		/// <returns>Bitmap object</returns>
		public static Bitmap LoadBitmapFromFile(string filename)
		{
			try
			{
				return new Bitmap(filename);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		/// <summary>
		/// Loads a bitmap from an encoded string.
		/// </summary>
		/// <param name="encodedStream">Encoded byte string</param>
		/// <returns>Bitmap object</returns>
		public static Bitmap LoadBitmapFromEncoded(string encodedStream)
		{
			byte[] imageByteArray = Convert.FromBase64String(encodedStream);
			if (imageByteArray.Length == 0)
				return null;

			return new Bitmap(new MemoryStream(imageByteArray));
		}

		/// <summary>
		/// http://stackoverflow.com/questions/9356694/tint-property-when-drawing-image-with-vb-net
		/// Tints a bitmap using the specified color and intensity.
		/// </summary>
		/// <param name="b">Bitmap to be tinted</param>
		/// <param name="color">Color to use for tint</param>
		/// <returns>A bitmap with the requested Tint</returns>
		public static Bitmap TintBitmap(Bitmap b, Color color)
		{
			Bitmap b2 = new Bitmap(b.Width, b.Height);
			ImageAttributes ia = new ImageAttributes();

			ColorMatrix cMatrix = new ColorMatrix(new float[][] {
							new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
							new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
							new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
							new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
							new float[] { (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, 0.0f, 1.0f }
						});

			ia.SetColorMatrix(cMatrix);
			Graphics g = Graphics.FromImage(b2);
			g.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, ia);

			cMatrix = null;
			g.Dispose();
			g = null;

			return b2;
		}
			
		public static Color ColorMixer(Color c1, Color c2)
		{
			int Red = (c1.R + c2.R) / 2;
			int Green = (c1.G + c2.G) / 2;
			int Blue = (c1.B + c2.B) / 2;

			return Color.FromArgb((byte)Red, (byte)Green, (byte)Blue);
		}

		#endregion [ Misc ]

		#endregion [ Static Methods ]

		#region [ Structs ]

		public struct JoinedImages
		{
			public ImageType ImageType { get; set; }
			public Annotation Annotation { get; set; }
			public Bitmap Bitmap { get; set; }

			public JoinedImages(ImageType imageType, Annotation annotation, Bitmap bitmap)
			{
				ImageType = imageType;
				Annotation = annotation;
				Bitmap = bitmap;
			}
		}

		#endregion [ Structs ]
	}
}
