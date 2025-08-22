using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    public class DropDownButton : Button
    {
        public DropDownButton()
        {
            Menu = null;
            ShowMenuUnderCursor = false;
            ShowSplit = true;
        }



        [DefaultValue(null), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContextMenuStrip Menu { get; set; }
        [DefaultValue(false), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowMenuUnderCursor { get; set; }
        [DefaultValue(true), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowSplit { get; set; }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Menu != null && e.Button == MouseButtons.Left)
                Menu.Show(this, ShowMenuUnderCursor ?
                    new Point(e.Location.X, e.Location.Y - (PointToScreen(e.Location).Y + Menu.Size.Height > Screen.PrimaryScreen.WorkingArea.Height ? Menu.Size.Height : 0)) :
                    new Point(0, PointToScreen(new Point(Left, Bottom)).Y + Menu.Size.Height > Screen.PrimaryScreen.WorkingArea.Height ? -Menu.Size.Height : Height));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Menu != null)
            {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                var brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark;
                e.Graphics.FillPolygon(brush, arrows);
                if (ShowSplit)
                    using (var separatorPen = new Pen(Brushes.DarkGray) { DashStyle = DashStyle.Dot })
                    {
                        var lineX = ClientRectangle.Width - 20;
                        e.Graphics.DrawLine(separatorPen, lineX, arrowY - 4, lineX, arrowY + 8);
                    }
            }
        }
    }
}