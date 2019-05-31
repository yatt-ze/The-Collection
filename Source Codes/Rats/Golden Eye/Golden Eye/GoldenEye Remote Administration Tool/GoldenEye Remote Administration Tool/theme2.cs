#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

#endregion Imports

//|------DO-NOT-REMOVE------|
//
// Creator: HazelDev
// Site   : HazelDev.com
// Moded By : DocXi Team
// Version: 1.2.1
//
//|------DO-NOT-REMOVE------|

namespace MonoFlat
{
    #region RoundRectangle

    public sealed class RoundRectangle
    {
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }
    }

    #endregion RoundRectangle

    #region ThemeContainer

    [ToolboxBitmap(typeof(Form))]
    public class ThemeContainer : ContainerControl
    {
        #region Enums

        public enum MouseState
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        #endregion Enums

        #region Variables

        private void prevente(Graphics G/*,string text*/, int W, int H)
        {
            SizeF L = G.MeasureString("", new Font("Arial", 8F));
            G.DrawString("", new Font("Arial", 8F), Brushes.White, new Rectangle(W - (int)L.Width - 7, H - (int)L.Height - 4, W, H));
        }

        private Rectangle HeaderRect;
        protected MouseState State;
        private int MoveHeight;
        private Point MouseP = new Point(0, 0);
        private bool Cap = false;
        private bool HasShown;

        #endregion Variables

        #region Properties

        /*private string preventext_ = "toto";
        public string preventeText
        {
            get {return preventext_; }
            set { preventext_ = value;Invalidate(); }
        }*/
        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        /*private Color BGG_ = Color.FromArgb(181, 41, 42);
        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("BackGround Color")]
        public Color BGG
        {
            get
            {
                return BGG_;
            }
            set { BGG_ = value; Invalidate(); }
        }*/
        private bool _Sizable = true;

        public bool Sizable
        {
            get
            {
                return _Sizable;
            }
            set
            {
                _Sizable = value;
            }
        }

        private bool _SmartBounds = true;

        public bool SmartBounds
        {
            get
            {
                return _SmartBounds;
            }
            set
            {
                _SmartBounds = value;
            }
        }

        private bool _RoundCorners = true;

        public bool RoundCorners
        {
            get
            {
                return _RoundCorners;
            }
            set
            {
                _RoundCorners = value;
                Invalidate();
            }
        }

        private bool _IsParentForm;

        protected bool IsParentForm
        {
            get
            {
                return _IsParentForm;
            }
        }

        protected bool IsParentMdi
        {
            get
            {
                if (Parent == null)
                {
                    return false;
                }
                return Parent.Parent != null;
            }
        }

        private bool _ControlMode;

        protected bool ControlMode
        {
            get
            {
                return _ControlMode;
            }
            set
            {
                _ControlMode = value;
                Invalidate();
            }
        }

        private FormStartPosition _StartPosition;

        public FormStartPosition StartPosition
        {
            get
            {
                if (_IsParentForm && !_ControlMode)
                {
                    return ParentForm.StartPosition;
                }
                else
                {
                    return _StartPosition;
                }
            }
            set
            {
                _StartPosition = value;

                if (_IsParentForm && !_ControlMode)
                {
                    ParentForm.StartPosition = value;
                }
            }
        }

        #endregion Properties

        #region EventArgs

        protected sealed override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null)
            {
                return;
            }
            _IsParentForm = Parent is Form;

            if (!_ControlMode)
            {
                InitializeMessages();

                if (_IsParentForm)
                {
                    this.ParentForm.FormBorderStyle = FormBorderStyle.None;
                    this.ParentForm.TransparencyKey = Color.Fuchsia;

                    if (!DesignMode)
                    {
                        ParentForm.Shown += FormShown;
                    }
                }
                Parent.BackColor = BackColor;
                //   Parent.MinimumSize = New Size(261, 65)
            }
        }

        protected sealed override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!_ControlMode)
            {
                HeaderRect = new Rectangle(0, 0, Width - 14, MoveHeight - 7);
            }
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                SetState(MouseState.Down);
            }
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
            {
                if (HeaderRect.Contains(e.Location))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[0]);
                }
                else if (_Sizable && !(Previous == 0))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[Previous]);
                }
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
            {
                if (_Sizable && !_ControlMode)
                {
                    InvalidateMouse();
                }
            }
            if (Cap)
            {
                Parent.Location = (System.Drawing.Point)((object)(System.Convert.ToDouble(MousePosition) - System.Convert.ToDouble(MouseP)));
            }
        }

        protected override void OnInvalidated(System.Windows.Forms.InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            ParentForm.Text = Text;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (_ControlMode || HasShown)
            {
                return;
            }

            if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
            {
                Rectangle SB = Screen.PrimaryScreen.Bounds;
                Rectangle CB = ParentForm.Bounds;
                ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
            }
            HasShown = true;
        }

        #endregion EventArgs

        #region Mouse & Size

        private void SetState(MouseState current)
        {
            State = current;
            Invalidate();
        }

        private Point GetIndexPoint;
        private bool B1x;
        private bool B2x;
        private bool B3;
        private bool B4;

        private int GetIndex()
        {
            GetIndexPoint = PointToClient(MousePosition);
            B1x = GetIndexPoint.X < 7;
            B2x = GetIndexPoint.X > Width - 7;
            B3 = GetIndexPoint.Y < 7;
            B4 = GetIndexPoint.Y > Height - 7;

            if (B1x && B3)
            {
                return 4;
            }
            if (B1x && B4)
            {
                return 7;
            }
            if (B2x && B3)
            {
                return 5;
            }
            if (B2x && B4)
            {
                return 8;
            }
            if (B1x)
            {
                return 1;
            }
            if (B2x)
            {
                return 2;
            }
            if (B3)
            {
                return 3;
            }
            if (B4)
            {
                return 6;
            }
            return 0;
        }

        private int Current;
        private int Previous;

        private void InvalidateMouse()
        {
            Current = GetIndex();
            if (Current == Previous)
            {
                return;
            }

            Previous = Current;
            switch (Previous)
            {
                case 0:
                    Cursor = Cursors.Default;
                    break;

                case 6:
                    Cursor = Cursors.SizeNS;
                    break;

                case 8:
                    Cursor = Cursors.SizeNWSE;
                    break;

                case 7:
                    Cursor = Cursors.SizeNESW;
                    break;
            }
        }

        private Message[] Messages = new Message[9];

        private void InitializeMessages()
        {
            Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
            for (int I = 1; I <= 8; I++)
            {
                Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
            }
        }

        private void CorrectBounds(Rectangle bounds)
        {
            if (Parent.Width > bounds.Width)
            {
                Parent.Width = bounds.Width;
            }
            if (Parent.Height > bounds.Height)
            {
                Parent.Height = bounds.Height;
            }

            int X = Parent.Location.X;
            int Y = Parent.Location.Y;

            if (X < bounds.X)
            {
                X = bounds.X;
            }
            if (Y < bounds.Y)
            {
                Y = bounds.Y;
            }

            int Width = bounds.X + bounds.Width;
            int Height = bounds.Y + bounds.Height;

            if (X + Parent.Width > Width)
            {
                X = Width - Parent.Width;
            }
            if (Y + Parent.Height > Height)
            {
                Y = Height - Parent.Height;
            }

            Parent.Location = new Point(X, Y);
        }

        private bool WM_LMBUTTONDOWN;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WM_LMBUTTONDOWN && m.Msg == 513)
            {
                WM_LMBUTTONDOWN = false;

                SetState(MouseState.Over);
                if (!_SmartBounds)
                {
                    return;
                }

                if (IsParentMdi)
                {
                    CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
                }
                else
                {
                    CorrectBounds(Screen.FromControl(Parent).WorkingArea);
                }
            }
        }

        #endregion Mouse & Size

        protected override void CreateHandle()
        {
            base.CreateHandle();
        }

        public ThemeContainer()
        {
            SetStyle((ControlStyles)(139270), true);
            BackColor = Color.FromArgb(32, 41, 50);
            Padding = new Padding(10, 70, 10, 9);
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            MoveHeight = 66;
            Font = new Font("Segoe UI", 9);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;

            G.Clear(Color.FromArgb(32, 41, 50));
            G.FillRectangle(new SolidBrush(BG), new Rectangle(0, 0, Width, 60));

            if (_RoundCorners == true)
            {
                // Draw Left upper corner
                G.FillRectangle(Brushes.Fuchsia, 0, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 1, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 2, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 3, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, 2, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, 3, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 1, 1, 1, 1);

                G.FillRectangle(new SolidBrush(BG), 1, 3, 1, 1);
                G.FillRectangle(new SolidBrush(BG), 1, 2, 1, 1);
                G.FillRectangle(new SolidBrush(BG), 2, 1, 1, 1);
                G.FillRectangle(new SolidBrush(BG), 3, 1, 1, 1);

                // Draw right upper corner
                G.FillRectangle(Brushes.Fuchsia, Width - 1, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 2, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 3, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 4, 0, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, 2, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, 3, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 2, 1, 1, 1);

                G.FillRectangle(new SolidBrush(BG), Width - 2, 3, 1, 1);
                G.FillRectangle(new SolidBrush(BG), Width - 2, 2, 1, 1);
                G.FillRectangle(new SolidBrush(BG), Width - 3, 1, 1, 1);
                G.FillRectangle(new SolidBrush(BG), Width - 4, 1, 1, 1);

                // Draw Left bottom corner
                G.FillRectangle(Brushes.Fuchsia, 0, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, Height - 2, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, Height - 3, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 0, Height - 4, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 2, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 3, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 1, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, 1, Height - 2, 1, 1);

                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), 1, Height - 3, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), 1, Height - 4, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), 3, Height - 2, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), 2, Height - 2, 1, 1);

                // Draw right bottom corner
                G.FillRectangle(Brushes.Fuchsia, Width - 1, Height, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 2, Height, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 3, Height, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 4, Height, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 2, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 3, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 2, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 3, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 4, Height - 1, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 1, Height - 4, 1, 1);
                G.FillRectangle(Brushes.Fuchsia, Width - 2, Height - 2, 1, 1);

                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), Width - 2, Height - 3, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), Width - 2, Height - 4, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), Width - 4, Height - 2, 1, 1);
                G.FillRectangle(new SolidBrush(Color.FromArgb(32, 41, 50)), Width - 3, Height - 2, 1, 1);
            }

            G.DrawString(Text, new Font("Microsoft Sans Serif", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 254, 255)), new Rectangle(20, 20, Width - 1, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            prevente(G,/*preventeText,*/Width, Height);
        }
    }

    #endregion ThemeContainer

    #region ControlBox

    public class ControlBox : Control
    {
        #region Enums

        public enum ButtonHoverState
        {
            Minimize,
            Maximize,
            Close,
            None
        }

        #endregion Enums

        #region Variables

        private ButtonHoverState ButtonHState = ButtonHoverState.None;

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        private bool _EnableMaximize = true;

        public bool EnableMaximizeButton
        {
            get { return _EnableMaximize; }
            set
            {
                _EnableMaximize = value;
                Invalidate();
            }
        }

        private bool _EnableMinimize = true;

        public bool EnableMinimizeButton
        {
            get { return _EnableMinimize; }
            set
            {
                _EnableMinimize = value;
                Invalidate();
            }
        }

        private bool _EnableHoverHighlight = false;

        public bool EnableHoverHighlight
        {
            get { return _EnableHoverHighlight; }
            set
            {
                _EnableHoverHighlight = value;
                Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(100, 25);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int X = e.Location.X;
            int Y = e.Location.Y;
            if (Y > 0 && Y < (Height - 2))
            {
                if (X > 0 && X < 34)
                {
                    ButtonHState = ButtonHoverState.Minimize;
                }
                else if (X > 33 && X < 65)
                {
                    ButtonHState = ButtonHoverState.Maximize;
                }
                else if (X > 64 && X < Width)
                {
                    ButtonHState = ButtonHoverState.Close;
                }
                else
                {
                    ButtonHState = ButtonHoverState.None;
                }
            }
            else
            {
                ButtonHState = ButtonHoverState.None;
            }
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (ButtonHState)
            {
                case ButtonHoverState.Close:
                    Parent.FindForm().Close();
                    break;

                case ButtonHoverState.Minimize:
                    if (_EnableMinimize == true)
                    {
                        Parent.FindForm().WindowState = FormWindowState.Minimized;
                    }
                    break;

                case ButtonHoverState.Maximize:
                    if (_EnableMaximize == true)
                    {
                        if (Parent.FindForm().WindowState == FormWindowState.Normal)
                        {
                            Parent.FindForm().WindowState = FormWindowState.Maximized;
                        }
                        else
                        {
                            Parent.FindForm().WindowState = FormWindowState.Normal;
                        }
                    }
                    break;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ButtonHState = ButtonHoverState.None;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        #endregion EventArgs

        public ControlBox()
            : base()
        {
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            try
            {
                Location = new Point(Parent.Width - 112, 15);
            }
            catch (Exception)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(BG);

            if (_EnableHoverHighlight == true)
            {
                switch (ButtonHState)
                {
                    case ButtonHoverState.None:
                        G.Clear(BG);
                        break;

                    case ButtonHoverState.Minimize:
                        if (_EnableMinimize == true)
                        {
                            G.FillRectangle(new SolidBrush(BG), new Rectangle(3, 0, 30, Height));
                        }
                        break;

                    case ButtonHoverState.Maximize:
                        if (_EnableMaximize == true)
                        {
                            G.FillRectangle(new SolidBrush(BG), new Rectangle(35, 0, 30, Height));
                        }
                        break;

                    case ButtonHoverState.Close:
                        G.FillRectangle(new SolidBrush(BG), new Rectangle(66, 0, 35, Height));
                        break;
                }
            }

            //Close
            G.DrawString("r", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(Width - 16, 8), new StringFormat { Alignment = StringAlignment.Center });

            //Maximize
            switch (Parent.FindForm().WindowState)
            {
                case FormWindowState.Maximized:
                    if (_EnableMaximize == true)
                    {
                        G.DrawString("2", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("2", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    break;

                case FormWindowState.Normal:
                    if (_EnableMaximize == true)
                    {
                        G.DrawString("1", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("1", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    break;
            }

            //Minimize
            if (_EnableMinimize == true)
            {
                G.DrawString("0", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(20, 7), new StringFormat { Alignment = StringAlignment.Center });
            }
            else
            {
                G.DrawString("0", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(20, 7), new StringFormat { Alignment = StringAlignment.Center });
            }
        }
    }

    #endregion ControlBox

    #region Button

    public class Button : Control
    {
        #region Variables

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Button Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion Variables

        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = new PointF();
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = (float)((Area.Width - ImageArea.Width) / 2);
                    break;

                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;

                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;
                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = (float)((Area.Height - ImageArea.Height) / 2);
                    break;

                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;

                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion Image Designer

        #region Properties

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        public ContentAlignment ImageAlign
        {
            get
            {
                return _ImageAlign;
            }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get
            {
                return this._TextAlignment;
            }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return this._TextColor;
            }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Focus();
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion EventArgs

        public Button()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint), true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            ForeColor = Color.FromArgb(255, 255, 255);
            Size = new Size(146, 41);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(BG); // P1 = Border color
            P3 = new Pen(Color.FromArgb(40, 0, 0, 0)); // P3 = Border color when pressed
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);
            }

            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);
            InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), BG, BG, 90.0F);
            PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(40, 0, 0, 0), Color.FromArgb(40, 0, 0, 0), 90.0F);
            switch (MouseState)
            {
                case 0:
                    //Inactive
                    G.FillPath(InactiveGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(new Pen(BG), Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;

                case 1:
                    //Pressed
                    G.FillPath(PressedGB, Shape);
                    // Fill button body with PressedGB color gradient
                    G.DrawPath(P3, Shape);
                    // Draw button border [PressedGB]

                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion Button

    #region Social Button

    public class SocialButton : Control
    {
        #region Variables

        private Image _Image;
        private Size _ImageSize;
        private Color EllipseColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion Properties

        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = new Size(54, 54);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            EllipseColor = BG;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            EllipseColor = Color.FromArgb(66, 76, 85);
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            EllipseColor = Color.FromArgb(40, 0, 0, 0);
            Focus();
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            EllipseColor = BG;
            Refresh();
        }

        #endregion EventArgs

        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = new PointF();
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = (float)((Area.Width - ImageArea.Width) / 2);
                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = (float)((Area.Height - ImageArea.Height) / 2);
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
            }
            return SF;
        }

        #endregion Image Designer

        public SocialButton()
        {
            DoubleBuffered = true;
            EllipseColor = Color.FromArgb(66, 76, 85);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;

            PointF ImgPoint = ImageLocation(GetStringFormat(ContentAlignment.MiddleCenter), Size, ImageSize);
            G.FillEllipse(new SolidBrush(EllipseColor), new Rectangle(0, 0, 53, 53));

            // HINTS:
            // The best size for the drawn image is 32x32\
            // The best matching color of drawn image is (RGB: 31, 40, 49)
            if (Image != null)
            {
                G.DrawImage(_Image, (int)ImgPoint.X, (int)ImgPoint.Y, ImageSize.Width, ImageSize.Height);
            }
        }
    }

    #endregion Social Button

    #region Label

    public class t_Label : Label
    {
        public t_Label()
        {
            Font = new Font("Segoe UI", 9);
            ForeColor = Color.FromArgb(116, 125, 132);
            BackColor = Color.Transparent;
        }
    }

    #endregion Label

    #region Link Label

    public class t_LinkLabel : LinkLabel
    {
        public t_LinkLabel()
        {
            Font = new Font("Segoe UI", 9, FontStyle.Regular);
            BackColor = Color.Transparent;
            LinkColor = Color.FromArgb(181, 41, 42);
            ActiveLinkColor = Color.FromArgb(153, 34, 34);
            VisitedLinkColor = Color.FromArgb(181, 41, 42);
            LinkBehavior = LinkBehavior.NeverUnderline;
        }
    }

    #endregion Link Label

    #region Header Label

    public class HeaderLabel : Label
    {
        public HeaderLabel()
        {
            Font = new Font("Segoe UI", 11, FontStyle.Bold);
            ForeColor = Color.FromArgb(255, 255, 255);
            BackColor = Color.Transparent;
        }
    }

    #endregion Header Label

    #region Toggle Button

    [DefaultEvent("ToggledChanged")]
    public class Toggle_button : Control
    {
        #region Enums

        public enum _Type
        {
            CheckMark,
            OnOff,
            YesNo,
            IO
        }

        #endregion Enums

        #region Variables

        public delegate void ToggledChangedEventHandler();

        private ToggledChangedEventHandler ToggledChangedEvent;

        public event ToggledChangedEventHandler ToggledChanged
        {
            add
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Combine(ToggledChangedEvent, value);
            }
            remove
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Remove(ToggledChangedEvent, value);
            }
        }

        private bool _Toggled;
        private _Type ToggleType;
        private Rectangle Bar;
        private int _Width;
        private int _Height;

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        public bool Toggled
        {
            get
            {
                return _Toggled;
            }
            set
            {
                _Toggled = value;
                Invalidate();
                if (ToggledChangedEvent != null)
                    ToggledChangedEvent();
            }
        }

        public _Type Type
        {
            get
            {
                return ToggleType;
            }
            set
            {
                ToggleType = value;
                Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = new Size(76, 33);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Toggled = !Toggled;
            Focus();
        }

        #endregion EventArgs

        public Toggle_button()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint), true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);
            _Width = Width - 1;
            _Height = Height - 1;

            GraphicsPath GP = default(GraphicsPath);
            GraphicsPath GP2 = new GraphicsPath();
            Rectangle BaseRect = new Rectangle(0, 0, _Width, _Height);
            Rectangle ThumbRect = new Rectangle(_Width / 2, 0, 38, _Height);

            G.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode)2;
            G.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode)2;
            G.TextRenderingHint = (System.Drawing.Text.TextRenderingHint)5;
            G.Clear(BackColor);

            GP = RoundRectangle.RoundRect(BaseRect, 4);
            ThumbRect = new Rectangle(4, 4, 36, _Height - 8);
            GP2 = RoundRectangle.RoundRect(ThumbRect, 4);
            G.FillPath(new SolidBrush(Color.FromArgb(66, 76, 85)), GP);
            G.FillPath(new SolidBrush(Color.FromArgb(32, 41, 50)), GP2);

            if (_Toggled)
            {
                GP = RoundRectangle.RoundRect(BaseRect, 4);
                ThumbRect = new Rectangle((_Width / 2) - 2, 4, 36, _Height - 8);
                GP2 = RoundRectangle.RoundRect(ThumbRect, 4);
                G.FillPath(new SolidBrush(BG), GP);
                G.FillPath(new SolidBrush(Color.FromArgb(32, 41, 50)), GP2);
            }

            // Draw string
            switch (ToggleType)
            {
                case _Type.CheckMark:
                    if (Toggled)
                    {
                        G.DrawString("ü", new Font("Wingdings", 18, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 19, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("r", new Font("Marlett", 14, FontStyle.Regular), Brushes.DimGray, Bar.X + 59, Bar.Y + 18, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;

                case _Type.OnOff:
                    if (Toggled)
                    {
                        G.DrawString("ON", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("OFF", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 57, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;

                case _Type.YesNo:
                    if (Toggled)
                    {
                        G.DrawString("YES", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 19, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("NO", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 56, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;

                case _Type.IO:
                    if (Toggled)
                    {
                        G.DrawString("I", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("O", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 57, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
            }
        }
    }

    #endregion Toggle Button

    #region CheckBox

    [DefaultEvent("CheckedChanged")]
    public class CheckBox : Control
    {
        #region Variables

        private int X;
        private bool _Checked = false;
        private GraphicsPath Shape;

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Cheked Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        public delegate void CheckedChangedEventHandler(object sender);

        private CheckedChangedEventHandler CheckedChangedEvent;

        public event CheckedChangedEventHandler CheckedChanged
        {
            add
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
            }
            remove
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            X = e.Location.X;
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            _Checked = !_Checked;
            Focus();
            if (CheckedChangedEvent != null)
                CheckedChangedEvent(this);
            base.OnMouseDown(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.Height = 16;

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
            Invalidate();
        }

        #endregion EventArgs

        public CheckBox()
        {
            Width = 148;
            Height = 16;
            Font = new Font("Microsoft Sans Serif", 9);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);

            if (_Checked)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(1, 1, 16 - 2, 16 - 2));
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(1, 1, 16 - 2, 16 - 2));
            }

            if (Enabled == true)
            {
                if (_Checked)
                {
                    G.DrawString("a", new Font("Marlett", 16), new SolidBrush(BG), new Point(-5, -3));
                }
            }
            else
            {
                if (_Checked)
                {
                    G.DrawString("a", new Font("Marlett", 16), new SolidBrush(Color.Gray), new Point(-5, -3));
                }
            }

            G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(116, 125, 132)), new Point(20, 0));
        }
    }

    #endregion CheckBox

    #region Radio Button

    [DefaultEvent("CheckedChanged")]
    public class RadioButton : Control
    {
        #region Variables

        private int X;
        private bool _Checked;

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Checked Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                InvalidateControls();
                if (CheckedChangedEvent != null)
                    CheckedChangedEvent(this);
                Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        public delegate void CheckedChangedEventHandler(object sender);

        private CheckedChangedEventHandler CheckedChangedEvent;

        public event CheckedChangedEventHandler CheckedChanged
        {
            add
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
            }
            remove
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
            {
                @Checked = true;
            }
            Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            X = e.X;
            Invalidate();
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            int textSize = 0;
            textSize = (int)(this.CreateGraphics().MeasureString(Text, Font).Width);
            this.Width = 28 + textSize;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Height = 17;
        }

        #endregion EventArgs

        public RadioButton()
        {
            Width = 159;
            Height = 17;
            DoubleBuffered = true;
        }

        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
            {
                return;
            }

            foreach (Control _Control in Parent.Controls)
            {
                if (_Control != this && _Control is RadioButton)
                {
                    ((RadioButton)_Control).Checked = false;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;

            G.FillEllipse(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));

            if (_Checked)
            {
                G.DrawString("a", new Font("Marlett", 15), new SolidBrush(BG), new Point(-3, -2));
            }

            G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(116, 125, 132)), new Point(20, 0));
        }
    }

    #endregion Radio Button

    #region TextBox

    [DefaultEvent("TextChanged")]
    public class t_TextBox : Control
    {
        #region Variables

        public TextBox MonoFlatTB = new TextBox();
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private Image _Image;
        private Size _ImageSize;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;
        private GraphicsPath Shape;

        #endregion Variables

        #region Properties

        public HorizontalAlignment TextAlignment
        {
            get
            {
                return ALNType;
            }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }

        public int MaxLength
        {
            get
            {
                return _maxchars;
            }
            set
            {
                _maxchars = value;
                MonoFlatTB.MaxLength = MaxLength;
                Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get
            {
                return isPasswordMasked;
            }
            set
            {
                MonoFlatTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }

        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (MonoFlatTB != null)
                {
                    MonoFlatTB.ReadOnly = value;
                }
            }
        }

        public bool Multiline
        {
            get
            {
                return _Multiline;
            }
            set
            {
                _Multiline = value;
                if (MonoFlatTB != null)
                {
                    MonoFlatTB.Multiline = value;

                    if (value)
                    {
                        MonoFlatTB.Height = Height - 23;
                    }
                    else
                    {
                        Height = MonoFlatTB.Height + 23;
                    }
                }
                Invalidate();
            }
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;

                if (Image == null)
                {
                    MonoFlatTB.Location = new Point(8, 10);
                }
                else
                {
                    MonoFlatTB.Location = new Point(35, 11);
                }
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion Properties

        #region EventArgs

        private void _Enter(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(181, 41, 42));
            Refresh();
        }

        private void _Leave(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(32, 41, 50));
            Refresh();
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = MonoFlatTB.Text;
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            MonoFlatTB.Text = Text;
            Invalidate();
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            MonoFlatTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            MonoFlatTB.Font = Font;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                MonoFlatTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                MonoFlatTB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                MonoFlatTB.Height = Height - 23;
            }
            else
            {
                //Height = MonoFlatTB.Height + 23;
            }

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            MonoFlatTB.Focus();
        }

        public void _TextChanged(System.Object sender, System.EventArgs e)
        {
            Text = MonoFlatTB.Text;
        }

        public void _BaseTextChanged(System.Object sender, System.EventArgs e)
        {
            MonoFlatTB.Text = Text;
        }

        #endregion EventArgs

        public void AddTextBox()
        {
            MonoFlatTB.Location = new Point(8, 10);
            MonoFlatTB.Text = String.Empty;
            MonoFlatTB.BorderStyle = BorderStyle.None;
            MonoFlatTB.TextAlign = HorizontalAlignment.Left;
            MonoFlatTB.Font = new Font("Tahoma", 11);
            MonoFlatTB.UseSystemPasswordChar = UseSystemPasswordChar;
            MonoFlatTB.Multiline = false;
            MonoFlatTB.BackColor = Color.FromArgb(66, 76, 85);
            MonoFlatTB.ScrollBars = ScrollBars.None;
            MonoFlatTB.KeyDown += _OnKeyDown;
            MonoFlatTB.Enter += _Enter;
            MonoFlatTB.Leave += _Leave;
            MonoFlatTB.TextChanged += OnBaseTextChanged;
        }

        public t_TextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddTextBox();
            Controls.Add(MonoFlatTB);

            P1 = new Pen(Color.FromArgb(32, 41, 50));
            B1 = new SolidBrush(Color.FromArgb(66, 76, 85));
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(176, 183, 191);

            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 43);
            DoubleBuffered = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            if (Image == null)
            {
                MonoFlatTB.Width = Width - 18;
            }
            else
            {
                MonoFlatTB.Width = Width - 45;
            }

            MonoFlatTB.TextAlign = TextAlignment;
            MonoFlatTB.UseSystemPasswordChar = UseSystemPasswordChar;

            G.Clear(Color.Transparent);

            G.FillPath(B1, Shape);
            G.DrawPath(P1, Shape);

            if (Image != null)
            {
                G.DrawImage(_Image, 5, 8, 24, 24);
                // 24x24 is the perfect size of the image
            }

            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion TextBox

    #region Panel

    public class Panel : ContainerControl
    {
        private GraphicsPath Shape;

        public Panel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.FromArgb(39, 51, 63);
            this.Size = new Size(187, 117);
            Padding = new Padding(5, 5, 5, 5);
            DoubleBuffered = true;
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            var G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.HighQuality;

            G.Clear(Color.FromArgb(32, 41, 50)); // Set control background to transparent
            G.FillPath(new SolidBrush(Color.FromArgb(39, 51, 63)), Shape); // Draw RTB background
            G.DrawPath(new Pen(Color.FromArgb(39, 51, 63)), Shape); // Draw border

            G.Dispose();
            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            B.Dispose();
        }
    }

    #endregion Panel

    #region Separator

    public class Separator : Control
    {
        public Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = (System.Drawing.Size)(new Point(120, 10));
        }

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(BG), 0, 5, Width, 5);
        }
    }

    #endregion Separator

    #region TrackBar

    [DefaultEvent("ValueChanged")]
    public class TrackBar : Control
    {
        #region Enums

        public enum ValueDivisor
        {
            By1 = 1,
            By10 = 10,
            By100 = 100,
            By1000 = 1000
        }

        #endregion Enums

        #region Variables

        private Rectangle FillValue;
        private Rectangle PipeBorder;
        private Rectangle TrackBarHandleRect;
        private bool Cap;
        private int ValueDrawer;

        private Size ThumbSize = new Size(14, 14);
        private Rectangle TrackThumb;

        private int _Minimum = 0;
        private int _Maximum = 10;
        private int _Value = 0;

        private bool _JumpToMouse = false;
        private ValueDivisor DividedValue = ValueDivisor.By1;

        #endregion Variables

        #region Properties

        private Color BG_ = Color.FromArgb(181, 41, 42);

        [DefaultValue(typeof(Color), "181, 41, 42"), DisplayName("Top Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value >= _Maximum)
                {
                    value = _Maximum - 10;
                }
                if (_Value < value)
                {
                    _Value = value;
                }

                _Minimum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value <= _Minimum)
                {
                    value = _Minimum + 10;
                }
                if (_Value > value)
                {
                    _Value = value;
                }

                _Maximum = value;
                Invalidate();
            }
        }

        public delegate void ValueChangedEventHandler();

        private ValueChangedEventHandler ValueChangedEvent;

        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Combine(ValueChangedEvent, value);
            }
            remove
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Remove(ValueChangedEvent, value);
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    if (value < _Minimum)
                    {
                        _Value = _Minimum;
                    }
                    else
                    {
                        if (value > _Maximum)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = value;
                        }
                    }
                    Invalidate();
                    if (ValueChangedEvent != null)
                        ValueChangedEvent();
                }
            }
        }

        public ValueDivisor ValueDivison
        {
            get
            {
                return DividedValue;
            }
            set
            {
                DividedValue = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public float ValueToSet
        {
            get
            {
                return _Value / (int)DividedValue;
            }
            set
            {
                Value = (int)(value * (int)DividedValue);
            }
        }

        public bool JumpToMouse
        {
            get
            {
                return _JumpToMouse;
            }
            set
            {
                _JumpToMouse = value;
                Invalidate();
            }
        }

        #endregion Properties

        #region EventArgs

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            checked
            {
                bool flag = this.Cap && e.X > -1 && e.X < this.Width + 1;
                if (flag)
                {
                    this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width - 11));
                TrackBarHandleRect = new Rectangle(ValueDrawer, 0, 25, 25);
                Cap = TrackBarHandleRect.Contains(e.Location);
                Focus();
                if (_JumpToMouse)
                {
                    this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }

        #endregion EventArgs

        public TrackBar()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer), true);

            Size = new Size(80, 22);
            MinimumSize = new Size(47, 22);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 22;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;

            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            TrackThumb = new Rectangle(7, 10, Width - 16, 2);
            PipeBorder = new Rectangle(1, 10, Width - 3, 2);

            try
            {
                this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width));
            }
            catch (Exception)
            {
            }

            TrackBarHandleRect = new Rectangle(ValueDrawer, 0, 3, 20);

            G.FillRectangle(new SolidBrush(Color.FromArgb(124, 131, 137)), PipeBorder);
            FillValue = new Rectangle(0, 10, TrackBarHandleRect.X + TrackBarHandleRect.Width - 4, 3);

            G.ResetClip();

            G.SmoothingMode = SmoothingMode.Default;
            G.DrawRectangle(new Pen(Color.FromArgb(124, 131, 137)), PipeBorder); // Draw pipe border
            G.FillRectangle(new SolidBrush(BG), FillValue);

            G.ResetClip();

            G.SmoothingMode = SmoothingMode.HighQuality;

            G.FillEllipse(new SolidBrush(BG), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);
            G.DrawEllipse(new Pen(BG), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);
        }
    }

    #endregion TrackBar

    #region NotificationBox

    public class NotificationBox : Control
    {
        #region Variables

        private Point CloseCoordinates;
        private bool IsOverClose;
        private int _BorderCurve = 8;
        private GraphicsPath CreateRoundPath;
        private string NotificationText = null;
        private Type _NotificationType;
        private bool _RoundedCorners;
        private bool _ShowCloseButton;
        private Image _Image;
        private Size _ImageSize;

        #endregion Variables

        #region Enums

        // Create a list of Notification Types
        public enum Type
        {
            @Notice,
            @Success,
            @Warning,
            @Error
        }

        #endregion Enums

        #region Custom Properties

        private Color BG_ = Color.FromArgb(111, 177, 199);

        [DefaultValue(typeof(Color), "111, 177, 199"), DisplayName("Notif Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        // Create a NotificationType property and add the Type enum to it
        public Type NotificationType
        {
            get
            {
                return _NotificationType;
            }
            set
            {
                _NotificationType = value;
                Invalidate();
            }
        }

        // Boolean value to determine whether the control should use border radius
        public bool RoundCorners
        {
            get
            {
                return _RoundedCorners;
            }
            set
            {
                _RoundedCorners = value;
                Invalidate();
            }
        }

        // Boolean value to determine whether the control should draw the close button
        public bool ShowCloseButton
        {
            get
            {
                return _ShowCloseButton;
            }
            set
            {
                _ShowCloseButton = value;
                Invalidate();
            }
        }

        // Integer value to determine the curve level of the borders
        public int BorderCurve
        {
            get
            {
                return _BorderCurve;
            }
            set
            {
                _BorderCurve = value;
                Invalidate();
            }
        }

        // Image value to determine whether the control should draw an image before the header
        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        // Size value - returns the image size
        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion Custom Properties

        #region EventArgs

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Decides the location of the drawn ellipse. If mouse is over the correct coordinates, "IsOverClose" boolean will be triggered to draw the ellipse
            if (e.X >= Width - 19 && e.X <= Width - 10 && e.Y > CloseCoordinates.Y && e.Y < CloseCoordinates.Y + 12)
            {
                IsOverClose = true;
            }
            else
            {
                IsOverClose = false;
            }
            // Updates the control
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Disposes the control when the close button is clicked
            if (_ShowCloseButton == true)
            {
                if (IsOverClose)
                {
                    Dispose();
                }
            }
        }

        #endregion EventArgs

        internal GraphicsPath CreateRoundRect(Rectangle r, int curve)
        {
            // Draw a border radius
            try
            {
                CreateRoundPath = new GraphicsPath(FillMode.Winding);
                CreateRoundPath.AddArc(r.X, r.Y, curve, curve, 180.0F, 90.0F);
                CreateRoundPath.AddArc(r.Right - curve, r.Y, curve, curve, 270.0F, 90.0F);
                CreateRoundPath.AddArc(r.Right - curve, r.Bottom - curve, curve, curve, 0.0F, 90.0F);
                CreateRoundPath.AddArc(r.X, r.Bottom - curve, curve, curve, 90.0F, 90.0F);
                CreateRoundPath.CloseFigure();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + "Value must be either \'1\' or higher", "Invalid Integer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Return to the default border curve if the parameter is less than "1"
                _BorderCurve = 8;
                BorderCurve = 8;
            }
            return CreateRoundPath;
        }

        public NotificationBox()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw), true);

            Font = new Font("Tahoma", 9);
            this.MinimumSize = new Size(100, 40);
            RoundCorners = false;
            ShowCloseButton = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            // Declare Graphics to draw the control
            Graphics GFX = e.Graphics;
            // Declare Color to paint the control's Text, Background and Border
            Color ForeColor = new Color();
            Color BackgroundColor = new Color();
            Color BorderColor = new Color();
            // Determine the header Notification Type font
            Font TypeFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            // Decalre a new rectangle to draw the control inside it
            Rectangle MainRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            // Declare a GraphicsPath to create a border radius
            GraphicsPath CrvBorderPath = CreateRoundRect(MainRectangle, _BorderCurve);

            GFX.SmoothingMode = SmoothingMode.HighQuality;
            GFX.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            GFX.Clear(Parent.BackColor);

            switch (_NotificationType)
            {
                case Type.Notice:
                    BackgroundColor = BG;
                    BorderColor = BG;
                    ForeColor = Color.White;
                    break;

                case Type.Success:
                    BackgroundColor = Color.FromArgb(91, 195, 162);
                    BorderColor = Color.FromArgb(91, 195, 162);
                    ForeColor = Color.White;
                    break;

                case Type.Warning:
                    BackgroundColor = Color.FromArgb(254, 209, 108);
                    BorderColor = Color.FromArgb(254, 209, 108);
                    ForeColor = Color.DimGray;
                    break;

                case Type.Error:
                    BackgroundColor = Color.FromArgb(217, 103, 93);
                    BorderColor = Color.FromArgb(217, 103, 93);
                    ForeColor = Color.White;
                    break;
            }

            if (_RoundedCorners == true)
            {
                GFX.FillPath(new SolidBrush(BackgroundColor), CrvBorderPath);
                GFX.DrawPath(new Pen(BorderColor), CrvBorderPath);
            }
            else
            {
                GFX.FillRectangle(new SolidBrush(BackgroundColor), MainRectangle);
                GFX.DrawRectangle(new Pen(BorderColor), MainRectangle);
            }

            switch (_NotificationType)
            {
                case Type.Notice:
                    NotificationText = "NOTICE";
                    break;

                case Type.Success:
                    NotificationText = "SUCCESS";
                    break;

                case Type.Warning:
                    NotificationText = "WARNING";
                    break;

                case Type.Error:
                    NotificationText = "ERROR";
                    break;
            }

            if (Image == null)
            {
                GFX.DrawString(NotificationText, TypeFont, new SolidBrush(ForeColor), new Point(10, 5));
                GFX.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(10, 21, Width - 17, Height - 5));
            }
            else
            {
                GFX.DrawImage(_Image, 12, 4, 16, 16);
                GFX.DrawString(NotificationText, TypeFont, new SolidBrush(ForeColor), new Point(30, 5));
                GFX.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(10, 21, Width - 17, Height - 5));
            }

            CloseCoordinates = new Point(Width - 26, 4);

            if (_ShowCloseButton == true)
            {
                // Draw the close button
                GFX.DrawString("r", new Font("Marlett", 7, FontStyle.Regular), new SolidBrush(Color.FromArgb(130, 130, 130)), new Rectangle(Width - 20, 10, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }

            CrvBorderPath.Dispose();
        }
    }

    #endregion NotificationBox

    #region tabcontrol

    public class t_TabControl : TabControl
    {
        private static List<WeakReference> __ENCList = new List<WeakReference>();

        [DebuggerNonUserCode]
        private static void __ENCAddToList(object value)
        {
            List<WeakReference> _ENCList = t_TabControl.__ENCList;
            Monitor.Enter(_ENCList);
            checked
            {
                try
                {
                    bool flag = t_TabControl.__ENCList.Count == t_TabControl.__ENCList.Capacity;
                    if (flag)
                    {
                        int num = 0;
                        int arg_3F_0 = 0;
                        int num2 = t_TabControl.__ENCList.Count - 1;
                        int num3 = arg_3F_0;
                        while (true)
                        {
                            int arg_90_0 = num3;
                            int num4 = num2;
                            if (arg_90_0 > num4)
                            {
                                break;
                            }
                            WeakReference weakReference = t_TabControl.__ENCList[num3];
                            flag = weakReference.IsAlive;
                            if (flag)
                            {
                                bool flag2 = num3 != num;
                                if (flag2)
                                {
                                    t_TabControl.__ENCList[num] = t_TabControl.__ENCList[num3];
                                }
                                num++;
                            }
                            num3++;
                        }
                        t_TabControl.__ENCList.RemoveRange(num, t_TabControl.__ENCList.Count - num);
                        t_TabControl.__ENCList.Capacity = t_TabControl.__ENCList.Count;
                    }
                    t_TabControl.__ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
                }
                finally
                {
                    Monitor.Exit(_ENCList);
                }
            }
        }

        private Color BG_ = Color.FromArgb(111, 177, 199);

        [DefaultValue(typeof(Color), "111, 177, 199"), DisplayName("TabS Color")]
        public Color BG
        {
            get
            {
                return BG_;
            }
            set { BG_ = value; Invalidate(); }
        }

        private Color AT_ = Color.FromArgb(111, 177, 199);

        [DefaultValue(typeof(Color), "111, 177, 199"), DisplayName("Tab Color")]
        public Color AT
        {
            get
            {
                return AT_;
            }
            set { AT_ = value; Invalidate(); }
        }

        private Color TX_ = Color.White;

        [DefaultValue(typeof(Color), "White"), DisplayName("TextTab Color")]
        public Color TX
        {
            get
            {
                return TX_;
            }
            set { TX_ = value; Invalidate(); }
        }

        public t_TabControl()
        {
            t_TabControl.__ENCAddToList(this);
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            Size size = new Size(0, 34);
            this.ItemSize = size;
            size = new Size(24, 0);
            this.Padding = (Point)size;
            this.Font = new Font("Arial", 12f);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Alignment = TabAlignment.Top;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.Clear(this.Parent.BackColor);
            Color color = default(Color);
            int arg_3D_0 = 0;
            checked
            {
                int num = this.TabCount - 1;
                int num2 = arg_3D_0;
                while (true)
                {
                    int arg_369_0 = num2;
                    int num3 = num;
                    if (arg_369_0 > num3)
                    {
                        break;
                    }
                    Rectangle tabRect = this.GetTabRect(num2);
                    bool flag = num2 == this.SelectedIndex;
                    Point location;
                    if (flag)
                    {
                        color = TX;
                        Graphics arg_C5_0 = graphics;
                        Pen arg_C5_1 = new Pen(BG);
                        Point point = new Point(tabRect.X - 2, tabRect.Height - 1);
                        Point arg_C5_2 = point;
                        location = new Point(tabRect.X + tabRect.Width - 2, tabRect.Height - 1);
                        arg_C5_0.DrawLine(arg_C5_1, arg_C5_2, location);
                        Graphics arg_11B_0 = graphics;
                        Pen arg_11B_1 = new Pen(AT);
                        location = new Point(tabRect.X - 2, tabRect.Height);
                        Point arg_11B_2 = location;
                        point = new Point(tabRect.X + tabRect.Width - 2, tabRect.Height);
                        arg_11B_0.DrawLine(arg_11B_1, arg_11B_2, point);
                    }
                    else
                    {
                        color = Color.FromArgb(160, 160, 160);
                        Graphics arg_18D_0 = graphics;
                        Pen arg_18D_1 = new Pen(Color.Transparent);
                        location = new Point(tabRect.X - 2, tabRect.Height - 1);
                        Point arg_18D_2 = location;
                        Point point = new Point(tabRect.X + tabRect.Width - 2, tabRect.Height - 1);
                        arg_18D_0.DrawLine(arg_18D_1, arg_18D_2, point);
                        Graphics arg_1E0_0 = graphics;
                        Pen arg_1E0_1 = new Pen(Color.Transparent);
                        location = new Point(tabRect.X - 2, tabRect.Height);
                        Point arg_1E0_2 = location;
                        point = new Point(tabRect.X + tabRect.Width - 2, tabRect.Height);
                        arg_1E0_0.DrawLine(arg_1E0_1, arg_1E0_2, point);
                    }
                    flag = (num2 != 0);
                    if (flag)
                    {
                        Graphics arg_23D_0 = graphics;
                        Pen arg_23D_1 = new Pen(Color.Transparent);
                        location = new Point(tabRect.X - 4, tabRect.Height - 7);
                        Point arg_23D_2 = location;
                        Point point = new Point(tabRect.X + 4, tabRect.Y + 6);
                        arg_23D_0.DrawLine(arg_23D_1, arg_23D_2, point);
                    }
                    location = tabRect.Location;
                    int x = (int)Math.Round(unchecked((double)location.X + (double)tabRect.Width / 2.0 - (double)(graphics.MeasureString(this.TabPages[num2].Text, this.Font).Width / 2f)));
                    location = tabRect.Location;
                    int y = (int)Math.Round(unchecked((double)location.Y + (double)tabRect.Height / 2.0 - (double)(graphics.MeasureString(this.TabPages[num2].Text, this.Font).Height / 2f)));
                    Graphics arg_329_0 = graphics;
                    string arg_329_1 = this.TabPages[num2].Text;
                    Font arg_329_2 = this.Font;
                    Brush arg_329_3 = new SolidBrush(color);
                    location = new Point(x, y);
                    arg_329_0.DrawString(arg_329_1, arg_329_2, arg_329_3, location);
                    try
                    {
                        this.TabPages[num2].BackColor = this.Parent.BackColor;
                    }
                    catch (Exception arg_34F_0)
                    {
                        // ProjectData.SetProjectError(arg_34F_0);
                        // ProjectData.ClearProjectError();
                    }
                    num2++;
                }
            }
        }
    }

    #endregion tabcontrol
}