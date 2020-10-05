using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;
using System.IO;

namespace ImageSlideShow.Core.ExifLibrary
{
#if DEBUG
    /// <summary>
    /// Displays a list of bins on a scrollable control.
    /// </summary>
    public class BinaryMapViewer : UserControl
    {
        #region "Member Variables"
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.VScrollBar mScrollBar;

        public delegate void OnBinSelect(object sender, Bin bin);
        [Category("Behavior")]
        public event OnBinSelect BinSelect;

        protected MemoryBinStream mMap;
        protected Color[] mMarkerColor;
        protected int mBinsize;
        protected BorderStyle mBorderStyle;

        private bool canscroll;
        private int hoveredbin;
        private int selectedbin;
        private string tip;
        private bool tipleft, tiptop;
        private int n, cols, rows;
        private float xbin, ybin;
        private bool showtext;
        #endregion

        #region "Properties"
        /// <summary>
        /// Gets or sets the size of the bins in pixels.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(6)]
        [Description("Gets or sets the size of the bins in pixels.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BinSize
        {
            get
            {
                return mBinsize;
            }
            set
            {
                mBinsize = value;
                if (mBinsize < 4) mBinsize = 4;
                UpdateLayout();
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Window")]
        [Description("Gets or sets the background color for the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the style of the border around the control.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(BorderStyle.Fixed3D)]
        [Description("Gets or sets the style of the border around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return mBorderStyle;
            }
            set
            {
                mBorderStyle = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the binmap.
        /// </summary>
        [Browsable(false)]
        public MemoryBinStream Map
        {
            get
            {
                return mMap;
            }
            set
            {
                mMap = value;
                mMap.Seek(0, System.IO.SeekOrigin.Begin);
                UpdateLayout();
            }
        }
        #endregion

        #region "Instance Methods"
        /// <summary>
        /// Sets the colors used for bin markers.
        /// </summary>
        public void SetMarker(byte marker, Color color)
        {
            mMarkerColor[marker] = color;
        }

        /// <summary>
        /// Selects the specified bin.
        /// </summary>
        public void SelectBin(Bin bin)
        {
            long offset = mMap.Position;
            mMap.Seek(0, System.IO.SeekOrigin.Begin);
            while (!mMap.EOF)
            {
                Bin s = mMap.Read();
                if (s.GetHashCode() == bin.GetHashCode())
                {
                    mMap.Seek(s.Offset, System.IO.SeekOrigin.Begin);
                    selectedbin = s.GetHashCode();
                    Refresh();
                    return;
                }
            }
            mMap.Seek(offset, System.IO.SeekOrigin.Begin);
            return;
        }
        #endregion

        #region "Constructors"
        public BinaryMapViewer()
        {
            mBinsize = 6;
            tipleft = true;
            tiptop = true;
            selectedbin = -1;
            mMap = new MemoryBinStream();
            BackColor = SystemColors.Window;
            mBorderStyle = BorderStyle.Fixed3D;
            mMarkerColor = new Color[256];
            mMarkerColor[0] = Color.White;
            mMarkerColor[1] = Color.Black;
            mMarkerColor[2] = Color.FromArgb(238, 236, 225);  // SOI
            mMarkerColor[3] = Color.FromArgb(75, 172, 198); // APP1
            mMarkerColor[4] = Color.FromArgb(247, 150, 70); // TIFF
            mMarkerColor[5] = Color.FromArgb(155, 187, 89); // IFD
            mMarkerColor[6] = Color.FromArgb(204, 192, 217); // Field
            mMarkerColor[7] = Color.FromArgb(128, 100, 162); // Field Data
            mMarkerColor[8] = Color.FromArgb(255, 80, 80); // Thumbnail

            canscroll = false;
            InitializeComponent();

            UpdateLayout();
        }
        #endregion

        #region "Event Handlers"
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw the border
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            if (mBorderStyle == BorderStyle.FixedSingle)
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Flat);
            else if (mBorderStyle == BorderStyle.Fixed3D)
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.SunkenInner);

            if (mMap.Length == 0) return;
            if (cols == 0 || rows == 0) return;

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float x = 0, y = 0;
            long offset = mMap.Position;
            long loc = offset;
            bool inwindow = true;
            float xoffset = (mBorderStyle != BorderStyle.None ? 1.0f : 0.0f);
            float yoffset = (mBorderStyle != BorderStyle.None ? 1.0f : 0.0f);
            while (!mMap.EOF && inwindow)
            {
                Bin s = mMap.Read();
                bool hi = (s.GetHashCode() == hoveredbin);
                bool sel = (s.GetHashCode() == selectedbin);
                Color color = mMarkerColor[s.Marker];
                // Draw bins
                for (long i = loc - s.Offset; i < s.Length; i++)
                {
                    if (y >= 0 && y < rows)
                    {
                        if (s.Length == 1)
                            DrawSingleBin(e.Graphics, x * xbin + xoffset, y * ybin + yoffset, color, sel, hi);
                        else if (i == 0)
                            DrawBinLeft(e.Graphics, x * xbin + xoffset, y * ybin + yoffset, color, sel, hi);
                        else if (i == s.Length - 1)
                            DrawBinRight(e.Graphics, x * xbin + xoffset, y * ybin + yoffset, color, sel, hi);
                        else
                            DrawBinCenter(e.Graphics, x * xbin + xoffset, y * ybin + yoffset, color, sel, hi);
                    }
                    x++;
                    if (x >= cols)
                    {
                        x = 0;
                        y++;
                    }
                    if (y > rows)
                    {
                        inwindow = false;
                        break;
                    }
                }
                // Draw bin text
                if (showtext && s.Length > 2)
                {
                    float tx = x - (s.Offset + s.Length - loc) + 1;
                    float ty = y;
                    while (tx < 0)
                    {
                        tx += cols;
                        ty--;
                    }
                    if (ty >= 0 && ty < rows)
                    {
                        Rectangle r = new Rectangle((int)(tx * xbin), (int)(ty * ybin + yoffset), (int)(xbin * (s.Length - 2)), (int)ybin);
                        TextRenderer.DrawText(e.Graphics, s.Name, this.Font, r, ForeColor, TextFormatFlags.EndEllipsis | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                    }
                }

                loc = mMap.Position;
            }
            mMap.Seek(offset, System.IO.SeekOrigin.Begin);

            if (tip != "")
                DrawTip(e.Graphics, tip, 5, 5, !tipleft, !tiptop);
        }

        private void DrawTip(Graphics g, string s, float x, float y, bool mirrorx, bool mirrory)
        {
            // Calculate drawing bounds
            Rectangle bounds = ClientRectangle;
            if (mBorderStyle != BorderStyle.None)
                bounds.Inflate(-1, -1);
            if (canscroll)
                bounds.Width -= mScrollBar.Width;

            Size sz = TextRenderer.MeasureText(g, s, this.Font, new Size(bounds.Width - 2 * (int)x, bounds.Height));
            if (sz.Width > bounds.Width - 2 * (int)x)
                sz.Width = bounds.Width - 2 * (int)x;
            x = (mirrorx ? bounds.Width - sz.Width - x : x);
            y = (mirrory ? bounds.Height - sz.Height - y : y);
            RectangleF r = new RectangleF(x, y, sz.Width, sz.Height);
            r.Inflate(2, 2);
            r.Offset(3, 3);
            SolidBrush br = new SolidBrush(Color.FromArgb(200, Color.Gray));
            g.FillRectangle(br, r);
            r.Offset(-3, -3);
            g.FillRectangle(SystemBrushes.Info, r);
            g.DrawRectangle(SystemPens.InfoText, r.Left, r.Top, r.Width, r.Height);
            Rectangle rt = new Rectangle((int)x, (int)y, (int)sz.Width, (int)sz.Height);
            string[] lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int lineheight = sz.Height / lines.Length;
            foreach (string ss in lines)
            {
                TextRenderer.DrawText(g, ss, this.Font, rt, SystemColors.InfoText, TextFormatFlags.EndEllipsis | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.NoPrefix);
                rt.Y += lineheight;
            }
        }

        private void DrawBinLeft(Graphics g, float x, float y, Color color, bool selected, bool highlight)
        {
            Brush rb = new SolidBrush(color);
            Pen rp = Pens.Black;

            g.FillEllipse(rb, x, y, xbin, ybin);
            g.FillRectangle(rb, x + xbin / 2.0f, y, xbin / 2.0f + 1.0f, ybin);
            g.DrawArc(rp, x, y, xbin, ybin, 90.0f, 180.0f);
            g.DrawLine(rp, x + xbin / 2.0f, y, x + xbin + 1.0f, y);
            g.DrawLine(rp, x + xbin / 2.0f, y + ybin, x + xbin + 1.0f, y + ybin);

            if (selected)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, SystemColors.Highlight)), x, y, xbin, ybin);

            if (highlight)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, Color.White)), x, y, xbin, ybin);
        }

        private void DrawBinRight(Graphics g, float x, float y, Color color, bool selected, bool highlight)
        {
            Brush rb = new SolidBrush(color);
            Pen rp = Pens.Black;

            g.FillEllipse(rb, x, y, xbin, ybin);
            g.FillRectangle(rb, x - 1.0f, y, xbin / 2.0f + 1.0f, ybin);
            g.DrawArc(rp, x, y, xbin, ybin, 270.0f, 180.0f);
            g.DrawLine(rp, x - 1.0f, y, x + xbin / 2.0f, y);
            g.DrawLine(rp, x - 1.0f, y + ybin, x + xbin / 2.0f, y + ybin);

            if (selected)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, SystemColors.Highlight)), x - 1.0f, y, xbin + 1.0f, ybin);

            if (highlight)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, Color.White)), x - 1.0f, y, xbin + 1.0f, ybin);
        }

        private void DrawBinCenter(Graphics g, float x, float y, Color color, bool selected, bool highlight)
        {
            Brush rb = new SolidBrush(color);
            Pen rp = Pens.Black;
            Pen op = new Pen(color);

            g.FillRectangle(rb, x - 1.0f, y, xbin + 2.0f, ybin);
            g.DrawLine(rp, x - 1.0f, y, x + xbin + 1.0f, y);
            g.DrawLine(rp, x - 1.0f, y + ybin, x + xbin + 1.0f, y + ybin);

            if (selected)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, SystemColors.Highlight)), x - 1.0f, y, xbin + 2.0f, ybin);

            if (highlight)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, Color.White)), x - 1.0f, y, xbin + 2.0f, ybin);
        }

        private void DrawSingleBin(Graphics g, float x, float y, Color color, bool selected, bool highlight)
        {
            Brush rb = new SolidBrush(color);
            Pen rp = Pens.Black;

            g.FillEllipse(rb, x, y, xbin, ybin);
            g.DrawEllipse(rp, x, y, xbin, ybin);

            if (selected)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, SystemColors.Highlight)), x, y, xbin, ybin);

            if (highlight)
                g.FillRectangle(new SolidBrush(Color.FromArgb(96, Color.White)), x, y, xbin, ybin);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mMap.Length == 0) return;

            int x = (int)Math.Floor((float)e.X / xbin);
            int y = (int)Math.Floor((float)e.Y / ybin);
            if (y < 0 || y > rows)
            {
                Refresh();
                return;
            }

            long offset = mMap.Position;
            long loc = y * cols + x + mMap.Position;
            if (loc < mMap.Length)
            {
                mMap.Seek(loc, System.IO.SeekOrigin.Begin);
                Bin s = mMap.Read();
                hoveredbin = s.GetHashCode();
                tip = s.Name + Environment.NewLine +
                      "Offset: " + s.Offset.ToString() + Environment.NewLine +
                      "Length: " + s.Length.ToString();
                tipleft = true; // (e.X > ClientRectangle.Width / 2);
                tiptop = (e.Y > ClientRectangle.Height / 2);

                mMap.Seek(offset, System.IO.SeekOrigin.Begin);
            }
            else
            {
                tip = "";
                hoveredbin = -1;
            }
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            mScrollBar.Left = ClientRectangle.Right - mScrollBar.Width - (mBorderStyle != BorderStyle.None ? 1 : 0);
            mScrollBar.Top = ClientRectangle.Top + (mBorderStyle != BorderStyle.None ? 1 : 0);
            mScrollBar.Height = ClientRectangle.Height - (mBorderStyle != BorderStyle.None ? 2 : 0);

            UpdateLayout();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left) return;

            int x = (int)Math.Floor((float)e.X / xbin);
            int y = (int)Math.Floor((float)e.Y / ybin);
            if (y < 0 || y > rows)
            {
                Refresh();
                return;
            }

            long offset = mMap.Position;
            long loc = y * cols + x + mMap.Position;
            if (loc < mMap.Length)
            {
                mMap.Seek(loc, System.IO.SeekOrigin.Begin);
                Bin s = mMap.Read();
                selectedbin = s.GetHashCode();
                mMap.Seek(offset, System.IO.SeekOrigin.Begin);
                if (BinSelect != null)
                    BinSelect(this, s);
            }
            else
            {
                selectedbin = -1;
            }
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            hoveredbin = -1;
            tip = "";
            Refresh();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int scroll = mScrollBar.Value;
            scroll -= Math.Sign(e.Delta) * mScrollBar.LargeChange;
            if (scroll < mScrollBar.Minimum)
                scroll = mScrollBar.Minimum;
            if (scroll > mScrollBar.Maximum - mScrollBar.LargeChange + 1)
                scroll = mScrollBar.Maximum - mScrollBar.LargeChange + 1;
            mScrollBar.Value = scroll;
            mMap.Seek(scroll * cols, System.IO.SeekOrigin.Begin);
            Refresh();
        }

        private void mScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            mMap.Seek(e.NewValue * cols, System.IO.SeekOrigin.Begin);
            Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region "Private Helper Methods"
        /// <summary> 
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            this.mScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // mScrollBar
            // 
            this.mScrollBar.Location = new System.Drawing.Point(149, 41);
            this.mScrollBar.Name = "mScrollBar";
            this.mScrollBar.Size = new System.Drawing.Size(17, 80);
            this.mScrollBar.TabIndex = 0;
            this.mScrollBar.Visible = false;
            this.mScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mScrollBar_Scroll);
            // 
            // BinaryMapViewer
            // 
            this.Controls.Add(this.mScrollBar);
            this.DoubleBuffered = true;
            this.Name = "BinaryMapViewer";
            this.Size = new System.Drawing.Size(200, 240);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Updates the layout of the control.
        /// </summary>
        private void UpdateLayout()
        {
            hoveredbin = -1;
            tip = "";

            // Calculate drawing bounds
            Rectangle bounds = ClientRectangle;
            if (mBorderStyle != BorderStyle.None)
                bounds.Inflate(-1, -1);
            if (canscroll)
                bounds.Width -= mScrollBar.Width;

            // Number of rows and columns
            cols = bounds.Width / mBinsize;
            rows = bounds.Height / mBinsize;
            n = cols * rows;

            // Adjust the bin size so that we cover the entire client area
            xbin = (float)bounds.Width / (float)cols;
            ybin = (float)bounds.Height / (float)rows;

            // Determine if we have enough space to display bin names
            Size sz = TextRenderer.MeasureText(this.CreateGraphics(), "A", this.Font);
            showtext = (ybin > sz.Height + 4);

            UpdateScroll();
        }

        /// <summary>
        /// Updates the scroll bar.
        /// </summary>
        private void UpdateScroll()
        {
            if (mMap.Length > rows * cols)
            {
                mScrollBar.Minimum = 0;
                mScrollBar.Maximum = (int)Math.Ceiling((float)mMap.Length / (float)cols) - 1;
                mScrollBar.SmallChange = 1;
                mScrollBar.LargeChange = rows;
                if (mScrollBar.Value > mScrollBar.Maximum + 1 - mScrollBar.LargeChange)
                {
                    mScrollBar.Value = mScrollBar.Maximum + 1 - mScrollBar.LargeChange;
                    mMap.Seek(mScrollBar.Value * cols, System.IO.SeekOrigin.Begin);
                }
                if (!canscroll)
                {
                    canscroll = true;
                    mScrollBar.Visible = true;
                    UpdateLayout();
                }
            }
            else if (canscroll)
            {
                canscroll = false;
                mScrollBar.Visible = false;
                UpdateLayout();
            }

            Refresh();
        }
        #endregion
    }

    #region "Public Classes"
    /// <summary>
    /// Represents a stream of bins.
    /// </summary>
    public interface IBinStream
    {
        /// <summary>
        /// Reads the next bin in the stream.
        /// </summary>
        Bin Read();
        /// <summary>
        /// Writes a bin to the current position.
        /// </summary>
        void Write(Bin bin);
        /// <summary>
        /// Seeks to the given offset from the given position.
        /// </summary>
        void Seek(long offset, SeekOrigin origin);
        /// <summary>
        /// Seeks to the start of the next bin from the current position.
        /// </summary>
        void SeekBin();
        /// <summary>
        /// Gets or sets the position of the stream.
        /// </summary>
        long Position { get; set; }
        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        long Length { get; }
        /// <summary>
        /// Indicates that the end of stream is reached.
        /// </summary>
        bool EOF { get; }
    }

    /// <summary>
    /// Represents a memory stream of bins.
    /// </summary>
    public class MemoryBinStream : IBinStream
    {
        private SortedList<long, Bin> list;
        protected long mPosition;

        public MemoryBinStream()
        {
            list = new SortedList<long, Bin>();
            mPosition = 0;
        }

        /// <summary>
        /// Reads the next bin in the stream.
        /// </summary>
        public Bin Read()
        {
            // Find and return the bin
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (mPosition >= obj.Key && mPosition < obj.Key + obj.Value.Length)
                {
                    long offset = obj.Key;
                    mPosition = offset + obj.Value.Length;
                    return obj.Value;
                }
            }

            // Return a null bin
            long start = 0;
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key + obj.Value.Length <= mPosition)
                {
                    start = obj.Key + obj.Value.Length;
                }
            }
            long end = 0;
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key > mPosition)
                {
                    end = obj.Key;
                    break;
                }
            }
            mPosition = start;
            Bin bin = new Bin("Null", 0, end - start);
            bin.Offset = mPosition;
            mPosition += bin.Length;
            return bin;
        }

        /// <summary>
        /// Writes a bin to the current position.
        /// </summary>
        public void Write(Bin bin)
        {
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if ((mPosition >= obj.Key) && (mPosition < obj.Key + obj.Value.Length))
                    throw new Exception("Cannot overwrite stream.");
            }
            bin.Offset = mPosition;
            list.Add(mPosition, bin);
            mPosition += bin.Length;
        }

        /// <summary>
        /// Seeks to the given offset from the given position.
        /// </summary>
        public void Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                mPosition = offset;
            else if (origin == SeekOrigin.End)
                mPosition = Length - offset;
            else
                mPosition += offset;
        }

        /// <summary>
        /// Seeks to the start of the next bin from the current position.
        /// </summary>
        public void SeekBin()
        {
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key > mPosition)
                {
                    mPosition = obj.Key;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the stream.
        /// </summary>
        public long Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        public long Length
        {
            get
            {
                if (list.Count == 0)
                    return 0;

                long length = 0;
                foreach (KeyValuePair<long, Bin> obj in list)
                {
                    length = obj.Key + obj.Value.Length;
                }

                return length;
            }
        }

        /// <summary>
        /// Indicates that the end of stream is reached.
        /// </summary>
        public bool EOF
        {
            get
            {
                return (mPosition >= Length);
            }
        }
    }

    /// <summary>
    /// Represents a bin of given size.
    /// </summary>
    public struct Bin
    {
        /// <summary>
        /// Returns the hash code for this bin.
        /// </summary>
        public override int GetHashCode()
        {
            return Offset.GetHashCode();
        }

        /// <summary>
        /// Gets the offset of this bin from the start of stream.
        /// </summary>
        public long Offset { get; internal set; }
        /// <summary>
        /// Gets or sets the name of the bin.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the marker id used for displaying the bin.
        /// </summary>
        public byte Marker { get; set; }
        /// <summary>
        /// Gets or sets the length of the bin.
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Gets or sets the user-defined data associated with this bin.
        /// </summary>
        public object Tag { get; set; }

        public Bin(string name, byte marker, long length, object tag)
            : this()
        {
            Name = name;
            Marker = marker;
            Length = length;
            Tag = tag;
        }

        public Bin(string name, byte marker, long length)
            : this(name, marker, length, null)
        {
            ;
        }
    }
    #endregion
#endif
}
