/*
 * NPlot - A charting library for .NET
 * 
 * Windows.PlotSurface2d.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace NPlot.Windows
{

	/// <summary>
	/// A Windows.Forms PlotSurface2D control.
	/// </summary>
	/// <remarks>
	/// Unfortunately it's not possible to derive from both Control and NPlot.PlotSurface2D.
	/// </remarks>
	[ ToolboxBitmapAttribute(typeof(NPlot.Windows.PlotSurface2D),"PlotSurface2D.ico") ]
	public partial class PlotSurface2D : System.Windows.Forms.Control, IPlotSurface2D, ISurface
	{

        private System.Windows.Forms.ToolTip coordinates_;

		private System.Collections.ArrayList selectedObjects_;
        private NPlot.PlotSurface2D ps_;

		private Axis xAxis1ZoomCache_;
		private Axis yAxis1ZoomCache_;
		private Axis xAxis2ZoomCache_;
		private Axis yAxis2ZoomCache_;

        /// <summary>
		/// Flag to display a coordinates in a tooltip.
		/// </summary>
		[ 
		Category("PlotSurface2D"),
		Description("Whether or not to show coordinates in a tool tip when the mouse hovers above the plot area."),
		Browsable(true),
		Bindable(true)
		]
		public bool ShowCoordinates
		{
			get
			{
				return this.coordinates_.Active;
			}
			set
			{
				this.coordinates_.Active = value;
			}
		}


		/// <summary>
		/// Default constructor.
		/// </summary>
		public PlotSurface2D()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            // double buffer, and update when resize.
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.ResizeRedraw = true;

			ps_ = new NPlot.PlotSurface2D();

            this.InteractionOccured += new InteractionHandler( OnInteractionOccured );
            this.PreRefresh += new PreRefreshHandler( OnPreRefresh );
		}


		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		/// <remarks>Modified! :-)</remarks>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.coordinates_ = new System.Windows.Forms.ToolTip(this.components);
			// 
			// PlotSurface2D
			// 
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Size = new System.Drawing.Size(328, 272);

		}


        KeyEventArgs lastKeyEventArgs_ = null;
        /// <summary>
        /// the key down callback
        /// </summary>
        /// <param name="e">information pertaining to the event</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            lastKeyEventArgs_ = e;
        }

        /// <summary>
        /// The key up callback.
        /// </summary>
        /// <param name="e">information pertaining to the event</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            lastKeyEventArgs_ = e;
        }

        /// <summary>
		/// the paint event callback.
		/// </summary>
		/// <param name="pe">the PaintEventArgs</param>
		protected override void OnPaint( PaintEventArgs pe )
		{
			DoPaint( pe, this.Width, this.Height );
			base.OnPaint(pe);
		}


		/// <summary>
		/// All functionality of the OnPaint method is provided by this function.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="pe">the PaintEventArgs from paint event.</param>
		/// <param name="width">width of the control</param>
		/// <param name="height">height of the control</param>
		public void DoPaint( PaintEventArgs pe, int width, int height )
		{
            this.PreRefresh(this);

            foreach (Interactions.Interaction i in interactions_)
            {
                i.DoPaint(pe,width,height);
            }

            /*
            // make sure don't redraw after a refresh.
            this.horizontalBarPos_ = -1;
            this.verticalBarPos_ = -1;
            */

            Graphics g = pe.Graphics;
			
			Rectangle border = new Rectangle( 0, 0, width, height );

			if ( g == null ) 
			{
				throw (new NPlotException("null graphics context!"));
			}
			
			if ( ps_ == null )
			{
				throw (new NPlotException("null NPlot.PlotSurface2D"));
			}
			
			if ( border == Rectangle.Empty )
			{
				throw (new NPlotException("null border context"));
			}

			this.Draw( g, border );
		}


		/// <summary>
		/// Draws the plot surface on the supplied graphics surface [not the control surface].
		/// </summary>
		/// <param name="g">The graphics surface on which to draw</param>
		/// <param name="bounds">A bounding box on this surface that denotes the area on the
		/// surface to confine drawing to.</param>
		public void Draw( Graphics g, Rectangle bounds )
		{

			// If we are not in design mode then draw as normal.
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) 
			{ 
				this.drawDesignMode( g, bounds );
			}

			ps_.Draw( g, bounds );
		
		}


		/// <summary>
		/// Draw a lightweight representation of us for design mode.
		/// </summary>
		private void drawDesignMode( Graphics g, Rectangle bounds )
		{
			g.DrawRectangle( new Pen(Color.Black), bounds.X + 2, bounds.Y + 2, bounds.Width-4, bounds.Height-4 );
			g.DrawString( "PlotSurface2D: " + this.Title, this.TitleFont, this.TitleBrush, bounds.X + bounds.Width/2.0f, bounds.Y + bounds.Height/2.0f );
		}


		/// <summary>
		/// Clears the plot and resets to default values.
		/// </summary>
		public void Clear()
		{
			xAxis1ZoomCache_ = null;
			yAxis1ZoomCache_ = null;
			xAxis2ZoomCache_ = null;
			yAxis2ZoomCache_ = null;
			ps_.Clear();
            interactions_.Clear();
        }


		/// <summary>
		/// Adds a drawable object to the plot surface. If the object is an IPlot, 
		/// the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">The IDrawable object to add to the plot surface.</param>
		public void Add( IDrawable p )
		{
			ps_.Add( p );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface against the specified axes. If
		/// the object is an IPlot, the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">the IDrawable object to add to the plot surface</param>
		/// <param name="xp">the x-axis to add the plot against.</param>
		/// <param name="yp">the y-axis to add the plot against.</param>
		public void Add( IDrawable p, NPlot.PlotSurface2D.XAxisPosition xp, NPlot.PlotSurface2D.YAxisPosition yp )
		{
			ps_.Add( p, xp, yp );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface. If the object is an IPlot, 
		/// the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">The IDrawable object to add to the plot surface.</param>
		/// <param name="zOrder">The z-ordering when drawing (objects with lower numbers are drawn first)</param>
		public void Add( IDrawable p, int zOrder )
		{
			ps_.Add( p, zOrder );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface against the specified axes. If
		/// the object is an IPlot, the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">the IDrawable object to add to the plot surface</param>
		/// <param name="xp">the x-axis to add the plot against.</param>
		/// <param name="yp">the y-axis to add the plot against.</param>
		/// <param name="zOrder">The z-ordering when drawing (objects with lower numbers are drawn first)</param>
		public void Add( IDrawable p, NPlot.PlotSurface2D.XAxisPosition xp,
			NPlot.PlotSurface2D.YAxisPosition yp, int zOrder )
		{
			ps_.Add( p, xp, yp , zOrder);
		}

		/// <summary>
		/// Whether or not the title will be scaled according to size of the plot 
		/// surface.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description("Whether or not the title will be scaled according to size of the plot surface."),
		Category("PlotSurface2D")
		]
		public bool AutoScaleTitle
		{
			get
			{
				return ps_.AutoScaleTitle;
			}
			set
			{
				ps_.AutoScaleTitle = value;
			}
		}


		/// <summary>
		/// When plots are added to the plot surface, the axes they are attached to
		/// are immediately modified to reflect data of the plot. If 
		/// AutoScaleAutoGeneratedAxes is true when a plot is added, the axes will
		/// be turned in to auto scaling ones if they are not already [tick marks,
		/// tick text and label size scaled to size of plot surface]. If false,
		/// axes will not be autoscaling.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description( "When plots are added to the plot surface, the axes they are attached to are immediately modified " +
			"to reflect data of the plot. If AutoScaleAutoGeneratedAxes is true when a plot is added, the axes will be " +
			"turned in to auto scaling ones if they are not already [tick marks, tick text and label size scaled to size " +
			"of plot surface]. If false, axes will not be autoscaling." ),
		Category("PlotSurface2D")
		]
		public bool AutoScaleAutoGeneratedAxes
		{
			get
			{
				return ps_.AutoScaleAutoGeneratedAxes;
			}
			set
			{
				ps_.AutoScaleAutoGeneratedAxes = value;
			}
		}


		/// <summary>
		/// The plot surface title.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("The plot surface title"),
		Browsable(true),
		Bindable(true)
		]
		public string Title
		{
			get 
			{
				return ps_.Title;
			}
			set 
			{
				ps_.Title = value;
				//helpful in design view. But crap in applications!
				//this.Refresh();
			}
		}


		/// <summary>
		/// The font used to draw the title.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("The font used to draw the title."),
		Browsable(true),
		Bindable(false)
		]
		public Font TitleFont 
		{
			get 
			{
				return ps_.TitleFont;
			}
			set 
			{
				ps_.TitleFont = value;
			}
		}


		/// <summary>
		/// Padding of this width will be left between what is drawn and the control border.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("Padding of this width will be left between what is drawn and the control border."),
		Browsable(true),
		Bindable(true)
		]
		public int Padding
		{
			get
			{
				return ps_.Padding;
			}
			set
			{
				ps_.Padding = value;
			}
		}


		/// <summary>
		/// The first abscissa axis.
		/// </summary>
		/// 
		[
		Browsable(false)
		]
		public Axis XAxis1
		{
			get
			{
				return ps_.XAxis1;
			}
			set
			{
				ps_.XAxis1 = value;
			}
		}


		/// <summary>
		/// The first ordinate axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis YAxis1
		{
			get
			{
				return ps_.YAxis1;
			}
			set
			{
				ps_.YAxis1 = value;
			}
		}


		/// <summary>
		/// The second abscissa axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis XAxis2
		{
			get
			{
				return ps_.XAxis2;
			}
			set
			{
				ps_.XAxis2 = value;
			}
		}


		/// <summary>
		/// The second ordinate axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis YAxis2
		{
			get
			{
				return ps_.YAxis2;
			}
			set
			{
				ps_.YAxis2 = value;
			}
		}


		/// <summary>
		/// The physical XAxis1 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalXAxis1Cache
		{
			get
			{
				return ps_.PhysicalXAxis1Cache;
			}
		}


		/// <summary>
		/// The physical YAxis1 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalYAxis1Cache
		{
			get
			{
				return ps_.PhysicalYAxis1Cache;
			}
		}


		/// <summary>
		/// The physical XAxis2 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalXAxis2Cache
		{
			get
			{
				return ps_.PhysicalXAxis2Cache;
			}
		}


		/// <summary>
		/// The physical YAxis2 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalYAxis2Cache
		{
			get
			{
				return ps_.PhysicalYAxis2Cache;
			}
		}


		/// <summary>
		/// A color used to paint the plot background. Mutually exclusive with PlotBackImage and PlotBackBrush
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Category("PlotSurface2D"),
		Description("Set the plot background color."),
		Browsable(true),
		Bindable(false)
		]
		public System.Drawing.Color PlotBackColor
		{
			set
			{
				ps_.PlotBackColor = value;
			}
		}

		/// <summary>
		/// Sets the title to be drawn using a solid brush of this color.
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Browsable(false),
		Bindable(false)
		]
		public Color TitleColor
		{
			set
			{
				ps_.TitleColor = value;
			}
		}


		/// <summary>
		/// The brush used for drawing the title.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description("The brush used for drawing the title."),
		Category("PlotSurface2D")
		]
		public Brush TitleBrush
		{
			get
			{
				return ps_.TitleBrush;
			}
			set
			{
				ps_.TitleBrush = value;
			}
		}


		/// <summary>
		/// Set smoothing mode for drawing plot objects.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("Set smoothing mode for drawing plot objects."),
		Browsable(true),
		Bindable(true)
		]
		public System.Drawing.Drawing2D.SmoothingMode SmoothingMode 
		{ 
			get
			{
				return ps_.SmoothingMode;
			}
			set
			{
				ps_.SmoothingMode = value;
			}
		}


        /// <summary>
		/// Mouse down event handler.
		/// </summary>
		/// <param name="e">the event args.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			DoMouseDown(e);
			base.OnMouseDown(e);
		}


        /// <summary>
		/// All functionality of the OnMouseDown function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		public void DoMouseDown( MouseEventArgs e )
		{
			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
                i.DoMouseDown(e,this);
				dirty |= i.DoMouseDown(e,this);
            }
			if (dirty)
			{
				Refresh();
			}
		}

        /// <summary>
        /// Mouse Wheel event handler.
        /// </summary>
        /// <param name="e">the event args</param>
		protected override void OnMouseWheel( System.Windows.Forms.MouseEventArgs e )
		{
			DoMouseWheel(e);
			base.OnMouseWheel(e);
		}


        /// <summary>
        /// All functionality of the OnMouseWheel function is containd here.
        /// This allows use of the all encompasing PlotSurface.
        /// </summary>
        /// <param name="e">the event args.</param>
        public void DoMouseWheel(MouseEventArgs e)
        {

			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
                i.DoMouseWheel(e, this);
				dirty |= i.DoMouseWheel(e, this);
            }
			if (dirty)
			{
				Refresh();
			}
        }


        /// <summary>
		/// All functionality of the OnMouseMove function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		/// <param name="ctr">The control that the mouse event happened in.</param>
		public void DoMouseMove( MouseEventArgs e, Control ctr )
		{
			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
                i.DoMouseMove(e, ctr, lastKeyEventArgs_);
				dirty |= i.DoMouseMove(e, ctr, lastKeyEventArgs_);
            }
			if (dirty)
			{
				Refresh();
			}

            // Update coordinates if necessary. 

			if ( coordinates_.Active )
			{
				// we are here
				Point here = new Point( e.X, e.Y );
				if ( ps_.PlotAreaBoundingBoxCache.Contains(here) )
				{
					coordinates_.ShowAlways = true;
					
					// according to M�ns Erlandson, this can sometimes be the case.
					if (this.PhysicalXAxis1Cache == null)
						return;
					if (this.PhysicalYAxis1Cache == null)
						return;

					double x = this.PhysicalXAxis1Cache.PhysicalToWorld( here, true );
					double y = this.PhysicalYAxis1Cache.PhysicalToWorld( here, true );
					string s = "";
					if (!DateTimeToolTip)
					{
						s = "(" + x.ToString("g4") + "," + y.ToString("g4") + ")"; 
					}
					else
					{
						DateTime dateTime = new DateTime((long)x);
						s = dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + y.ToString("f4");
					}
                    //Bug fix. Windows 7 will do an infinate loop if this is set.
                    if (coordinates_.GetToolTip(this) != s)
                        coordinates_.SetToolTip(this, s);
				}
				else
				{
					coordinates_.ShowAlways = false;
				}
			}

		}


		/// <summary>
		/// MouseMove event handler.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			DoMouseMove( e, this );
			base.OnMouseMove( e );
		}
		

		/// <summary>
		/// MouseLeave event handler. It has to invalidate the control to get rid of
		/// any remnant of vertical and horizontal guides.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseLeave(EventArgs e)        
		{

			DoMouseLeave( e, this );
			base.OnMouseLeave(e);
		}
	

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <param name="ctr"></param>
		public void DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr) 
		{
			bool dirty = false;
			foreach (Interactions.Interaction i in interactions_)            
			{
				dirty = i.DoMouseLeave(e, this) || dirty;
			}
			if (dirty)
				Refresh();
		}


		/// <summary>
		/// When true, tool tip will display x value as a DateTime. Quick hack - this will probably be 
		/// changed at some point.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("PlotSurface2D"),
		Description("When true, tool tip will display x value as a DateTime. Quick hack - this will probably be changed at some point.")
		]
		public bool DateTimeToolTip
		{
			get
			{
				return dateTimeToolTip_;
			}
			set
			{
				dateTimeToolTip_ = value;
			}
		}
		private bool dateTimeToolTip_ = false;


		/// <summary>
		/// All functionality of the OnMouseUp function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		/// <param name="ctr">The control that the mouse event happened in.</param>
		public void DoMouseUp( MouseEventArgs e, System.Windows.Forms.Control ctr )
		{
			bool dirty = false;

            foreach (Interactions.Interaction i in interactions_)
            {
				dirty |= i.DoMouseUp(e,ctr);
            }
			if (dirty)
			{
				Refresh();
			}

            if (e.Button == MouseButtons.Right)
            {
                Point here = new Point(e.X, e.Y);
                selectedObjects_ = ps_.HitTest(here);
                if (rightMenu_ != null)
                    rightMenu_.Menu.Show(ctr, here);
            }
  
        }


		/// <summary>
		/// mouse up event handler.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseUp( MouseEventArgs e )
		{
			DoMouseUp(e, this);
			base.OnMouseUp(e);
		}


		/// <summary>
		/// sets axes to be those saved in the cache.
		/// </summary>
		public void OriginalDimensions()
		{
			if ( xAxis1ZoomCache_ != null )
			{
                this.XAxis1 = xAxis1ZoomCache_;
                this.XAxis2 = xAxis2ZoomCache_;
                this.YAxis1 = yAxis1ZoomCache_;
                this.YAxis2 = yAxis2ZoomCache_;

                xAxis1ZoomCache_ = null;
                xAxis2ZoomCache_ = null;
                yAxis1ZoomCache_ = null;
                yAxis2ZoomCache_ = null;
            }					
			this.Refresh();
		}

        private void DrawHorizontalSelection(Point start, Point end, System.Windows.Forms.UserControl ctr)
        {
            // the clipping rectangle in screen coordinates
            Rectangle clip = ctr.RectangleToScreen(
                new Rectangle(
                (int)ps_.PlotAreaBoundingBoxCache.X,
                (int)ps_.PlotAreaBoundingBoxCache.Y,
                (int)ps_.PlotAreaBoundingBoxCache.Width,
                (int)ps_.PlotAreaBoundingBoxCache.Height));

            start = ctr.PointToScreen(start);
            end = ctr.PointToScreen(end);

            ControlPaint.FillReversibleRectangle(
                new Rectangle((int)Math.Min(start.X,end.X), (int)clip.Y, (int)Math.Abs(end.X-start.X), (int)clip.Height),
                Color.White );

        }


		/// <summary>
		/// Add an axis constraint to the plot surface. Axis constraints can
		/// specify relative world-pixel scalings, absolute axis positions etc.
		/// </summary>
		/// <param name="c">The axis constraint to add.</param>
		public void AddAxesConstraint( AxesConstraint c )
		{
			ps_.AddAxesConstraint( c );
		}


		/// <summary>
		/// Print the chart as currently shown by the control
		/// </summary>
		/// <param name="preview">If true, show print preview window.</param>
		public void Print( bool preview ) 
		{
			PrintDocument printDocument = new PrintDocument();
			printDocument.PrintPage += new PrintPageEventHandler(NPlot_PrintPage);
			printDocument.DefaultPageSettings.Landscape = true;
				 	
			DialogResult result;
			if (!preview) 
			{
				PrintDialog dlg = new PrintDialog();
				dlg.Document = printDocument;
				result = dlg.ShowDialog();
			} 
			else 
			{
				PrintPreviewDialog dlg = new PrintPreviewDialog();
				dlg.Document = printDocument;
				result = dlg.ShowDialog();
			}
			if (result == DialogResult.OK) 
			{
				try 
				{
					printDocument.Print();
				}								 				
				catch 
				{
					Console.WriteLine( "caught\n" );
				}
			}
		}


		private void NPlot_PrintPage(object sender, PrintPageEventArgs ev) 
		{
			Rectangle r = ev.MarginBounds;
			this.Draw( ev.Graphics, r );
			ev.HasMorePages = false;
		}
	
		
		/// <summary>
		/// Coppies the chart currently shown in the control to the clipboard as an image.
		/// </summary>
		public void CopyToClipboard()
		{
			System.Drawing.Bitmap b = new System.Drawing.Bitmap( this.Width, this.Height );
			Graphics g = Graphics.FromImage( b );
			g.Clear(Color.White);
			this.Draw( g, new Rectangle( 0, 0, b.Width-1, b.Height-1 ) );
			Clipboard.SetDataObject( b, true );
		}

        /// <summary>
        /// Remove a drawable object from the plot surface.
        /// </summary>
        /// <param name="p">the drawable to remove</param>
        /// <param name="updateAxes">whether or not to update the axes after removing the idrawable.</param>
        public void Remove(IDrawable p, bool updateAxes)
        {
            ps_.Remove(p, updateAxes);
        }


        /// <summary>
		/// Gets an array list containing all drawables currently added to the PlotSurface2D.
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public List<IDrawable> Drawables
		{
			get
			{
				return ps_.Drawables;
			}
		}


		/// <summary>
		/// Sets the right context menu. Custom menus can be designed by overriding
		/// NPlot.Windows.PlotSurface2D.ContextMenu.
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public NPlot.Windows.PlotSurface2D.PlotContextMenu RightMenu
		{
			get
			{
				return rightMenu_;
			}
			set
			{
				rightMenu_ = value;
				if (rightMenu_ != null)
				{
					rightMenu_.PlotSurface2D = this;
				}
			}
		}
		private NPlot.Windows.PlotSurface2D.PlotContextMenu rightMenu_ = null;


		/// <summary>
		/// Gets an instance of a NPlot.Windows.PlotSurface2D.ContextMenu that
		/// is useful in typical situations.
		/// </summary>
		public static PlotContextMenu DefaultContextMenu
		{
			get
			{
				return new NPlot.Windows.PlotSurface2D.PlotContextMenu();
			}
		}


        /// <summary>
        /// Allows access to the PlotSurface2D.
        /// </summary>
        [
		Browsable(false),
		Bindable(false)
		]
        public NPlot.PlotSurface2D Inner
        {
            get
            {
                return ps_;
            }
        }


        /// <summary>
        /// Remembers the current axes - useful in interactions.
        /// </summary>
        public void CacheAxes()
        {
            if (xAxis1ZoomCache_ == null && xAxis2ZoomCache_ == null &&
                 yAxis1ZoomCache_ == null && yAxis2ZoomCache_ == null)
            {
                if (this.XAxis1 != null)
                {
                    xAxis1ZoomCache_ = (Axis)this.XAxis1.Clone();
                }
                if (this.XAxis2 != null)
                {
                    xAxis2ZoomCache_ = (Axis)this.XAxis2.Clone();
                }
                if (this.YAxis1 != null)
                {
                    yAxis1ZoomCache_ = (Axis)this.YAxis1.Clone();
                }
                if (this.YAxis2 != null)
                {
                    yAxis2ZoomCache_ = (Axis)this.YAxis2.Clone();
                }
            }
        }


       

        private List<Interactions.Interaction> interactions_ = new List<Interactions.Interaction>();


        /// <summary>
        /// Adds and interaction to the plotsurface that adds functionality that responds 
        /// to a set of mouse / keyboard events. 
        /// </summary>
        /// <param name="i">the interaction to add.</param>
        public void AddInteraction(Interactions.Interaction i)
        {
            interactions_.Add(i);
        }


		/// <summary>
		/// Remove a previously added interaction
		/// </summary>
		/// <param name="i">interaction to remove</param>
		public void RemoveInteraction(Interactions.Interaction i)             
		{
			interactions_.Remove(i);
		}


        /// <summary>
        /// This is the signature of the function used for InteractionOccurred events.
        /// 
        /// TODO: expand this to include information about the event. 
        /// </summary>
        /// <param name="sender"></param>
        public delegate void InteractionHandler(object sender);
        

        /// <summary>
        /// Event is fired when an interaction happens with the plot that causes it to be modified.
        /// </summary>
        public event InteractionHandler InteractionOccured;

        /// <summary>
        /// Default function called when plotsurface modifying interaction occured. 
        /// 
        /// Override this, or add method to InteractionOccured event.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnInteractionOccured(object sender)
        {
            // do nothing.
        }

        /// <summary>
        /// This is the signature of the function used for PreRefresh events.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void PreRefreshHandler(object sender);


        /// <summary>
        /// Event fired when we are about to paint.
        /// </summary>
        public event PreRefreshHandler PreRefresh;


        /// <summary>
        /// Default function called just before a refresh happens.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnPreRefresh(object sender)
        {
            // do nothing.
        }


		#region class PlotContextMenu
		/// <summary>
		/// Summary description for ContextMenu.
		/// </summary>
		public class PlotContextMenu
		{

			#region IPlotMenuItem
			/// <summary>
			/// elements of the MenuItems array list must implement this interface.
			/// </summary>
			public interface IPlotMenuItem
			{
				/// <summary>
				/// Gets the Windows.Forms.MenuItem associated with the PlotMenuItem
				/// </summary>
				System.Windows.Forms.MenuItem MenuItem { get; }

				/// <summary>
				/// This method is called for each menu item before the menu is 
				/// displayed. It is useful for implementing check marks, disabling
				/// etc.
				/// </summary>
				/// <param name="plotContextMenu"></param>
				void OnPopup( PlotContextMenu plotContextMenu );
			}
			#endregion
			#region PlotMenuSeparator
			/// <summary>
			/// A plot menu item for separators.
			/// </summary>
			public class PlotMenuSeparator : IPlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="index"></param>
				public PlotMenuSeparator( int index )
				{
					menuItem_ = new System.Windows.Forms.MenuItem();
					index_ = index;

					menuItem_.Index = index_;
					menuItem_.Text = "-";
				}

				private int index_;

				/// <summary>
				/// Index of this menu item in the menu.
				/// </summary>
				public int Index
				{
					get
					{
						return index_;
					}
				}

				private System.Windows.Forms.MenuItem menuItem_;
				/// <summary>
				/// The Windows.Forms.MenuItem associated with this IPlotMenuItem
				/// </summary>
				public System.Windows.Forms.MenuItem MenuItem
				{
					get
					{
						return menuItem_;
					}
				}

				/// <summary>
				/// 
				/// </summary>
				/// <param name="plotContextMenu"></param>
				public void OnPopup( PlotContextMenu plotContextMenu )
				{
					// do nothing.
				}

			}
			#endregion
			#region PlotMenuItem
			/// <summary>
			/// A Plot menu item suitable for specifying basic menu items
			/// </summary>
			public class PlotMenuItem : IPlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Menu item text</param>
				/// <param name="index">Index in the manu</param>
				/// <param name="callback">EventHandler to call if menu selected.</param>
				public PlotMenuItem( string text, int index, EventHandler callback )
				{
					text_ = text;
					index_ = index;
					callback_ = callback;

					menuItem_ = new System.Windows.Forms.MenuItem();
					
					menuItem_.Index = index;
					menuItem_.Text = text;
					menuItem_.Click += new System.EventHandler(callback);

				}

				private string text_;
				/// <summary>
				/// The text to put in the menu for this menu item.
				/// </summary>
				public string Text
				{
					get
					{
						return text_;
					}
				}

				private int index_;
				/// <summary>
				/// Index of this menu item in the menu.
				/// </summary>
				public int Index
				{
					get
					{
						return index_;
					}
				}

				private EventHandler callback_;
				/// <summary>
				/// EventHandler to call if menu selected.
				/// </summary>
				public EventHandler Callback
				{
					get
					{
						return callback_;
					}
				}

				private System.Windows.Forms.MenuItem menuItem_;
				/// <summary>
				/// The Windows.Forms.MenuItem associated with this IPlotMenuItem
				/// </summary>
				public System.Windows.Forms.MenuItem MenuItem
				{
					get
					{
						return menuItem_;
					}
				}

				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public virtual void OnPopup( PlotContextMenu plotContextMenu )
				{
					// do nothing.
				}

			}
			#endregion
			#region PlotZoomBackMenuItem
			/// <summary>
			/// A Plot Menu Item that provides necessary functionality for the
			/// zoom back menu item (graying out if zoomed right out in addition
			/// to basic functionality).
			/// </summary>
			public class PlotZoomBackMenuItem : PlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Text associated with this item in the menu.</param>
				/// <param name="index">Index of this item in the menu.</param>
				/// <param name="callback">EventHandler to call when menu item is selected.</param>
				public PlotZoomBackMenuItem( string text, int index, EventHandler callback )
					: base( text, index, callback )
				{
				}

				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public override void OnPopup( PlotContextMenu plotContextMenu )
				{
					this.MenuItem.Enabled = plotContextMenu.plotSurface2D_.xAxis1ZoomCache_ != null;
				}

			}
			#endregion
			#region PlotShowCoordinatesMenuItem
			/// <summary>
			/// A Plot Menu Item that provides necessary functionality for the
			/// show coordinates menu item (tick mark toggle in addition to basic
			/// functionality).
			/// </summary>
			public class PlotShowCoordinatesMenuItem : PlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Text associated with this item in the menu.</param>
				/// <param name="index">Index of this item in the menu.</param>
				/// <param name="callback">EventHandler to call when menu item is selected.</param>
				public PlotShowCoordinatesMenuItem( string text, int index, EventHandler callback )
					: base( text, index, callback )
				{
				}

				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public override void OnPopup( PlotContextMenu plotContextMenu )
				{
					this.MenuItem.Checked = plotContextMenu.plotSurface2D_.ShowCoordinates;
				}
			}
			#endregion

			private System.Windows.Forms.ContextMenu rightMenu_ = null;
			private ArrayList menuItems_ = null;


			/// <summary>
			/// Gets an arraylist of all PlotMenuItems that comprise the
			/// menu. If this list is changed, this class must be told to
			/// update using the Update method.
			/// </summary>
			public ArrayList MenuItems
			{
				get
				{
					return menuItems_;
				}
			}

			/// <summary>
			/// The PlotSurface2D associated with the context menu. Generally, the user
			/// should not set this. It is used internally by PlotSurface2D.
			/// </summary>
			public Windows.PlotSurface2D PlotSurface2D
			{
				set
				{
					this.plotSurface2D_ = value;
				}
			}

			/// <summary>
			/// The PlotSurface2D associated with the context menu. Classes inherited
			/// from PlotContextMenu will likely use this to implement their functionality.
			/// </summary>
			protected Windows.PlotSurface2D plotSurface2D_;

			
			/// <summary>
			/// Sets the context menu according to the IPlotMenuItem's in the provided
			/// ArrayList. The current menu items can be obtained using the MenuItems
			/// property and extended if desired.
			/// </summary>
			/// <param name="menuItems"></param>
			public void SetMenuItems(ArrayList menuItems)
			{
				this.menuItems_ = menuItems;

				this.rightMenu_ = new System.Windows.Forms.ContextMenu();
			
				foreach (IPlotMenuItem item in menuItems_)
				{
					this.rightMenu_.MenuItems.Add( item.MenuItem );
				}

				this.rightMenu_.Popup += new System.EventHandler(this.rightMenu__Popup);
			}


			/// <summary>
			/// Constructor creates
			/// </summary>
			public PlotContextMenu()
			{
				ArrayList menuItems = new ArrayList();

				menuItems = new ArrayList();
				menuItems.Add( new PlotZoomBackMenuItem( "Original Dimensions", 0, new EventHandler(this.mnuOriginalDimensions_Click) ) );
				menuItems.Add( new PlotShowCoordinatesMenuItem( "Show World Coordinates", 1, new EventHandler(this.mnuDisplayCoordinates_Click) ) ); 
				menuItems.Add( new PlotMenuSeparator(2) );
				menuItems.Add( new PlotMenuItem( "Print", 3, new EventHandler(this.mnuPrint_Click )) );
				menuItems.Add( new PlotMenuItem( "Print Preview", 4, new EventHandler(this.mnuPrintPreview_Click) ) );
				menuItems.Add( new PlotMenuItem( "Copy To Clipboard", 5, new EventHandler(this.mnuCopyToClipboard_Click) ) );

				this.SetMenuItems( menuItems );
			}


			private void mnuOriginalDimensions_Click(object sender, System.EventArgs e)
			{
				plotSurface2D_.OriginalDimensions();
			}

			private void mnuCopyToClipboard_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.CopyToClipboard();
			}

			private void mnuPrint_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.Print( false );
			}

			private void mnuPrintPreview_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.Print( true );
			}

			private void mnuDisplayCoordinates_Click(object sender, System.EventArgs e)
			{
				plotSurface2D_.ShowCoordinates = !plotSurface2D_.ShowCoordinates;
			}

			private void rightMenu__Popup(object sender, System.EventArgs e)
			{
				foreach (IPlotMenuItem item in menuItems_)
				{
					item.OnPopup( this );
				}
			}

			/// <summary>
			/// Gets the Windows.Forms context menu managed by this object.
			/// </summary>
			public System.Windows.Forms.ContextMenu Menu
			{
				get
				{
					return rightMenu_;
				}
			}

		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		private System.ComponentModel.IContainer components;

	}

}