using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;

namespace SympleLib.OpenXML
{
    public class XLSStyle
    {
        public XLSStyle()
        {
            this.BackgroundColor = XLColor.NoColor;
            this.HorizintalAlign = XLAlignmentHorizontalValues.Left;
            this.FontColor = XLColor.Black;
            this.Bold = false;
            this.Italic = false;
            this.FontSize = 11;
        }

        public IXLColor BackgroundColor { get; set; }
        public XLAlignmentHorizontalValues HorizintalAlign { get; set; }

        public IXLColor FontColor { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public int FontSize { get; set; }

        public void ApplyTo(IXLStyle TargetObjStyle)
        {
            TargetObjStyle.Fill.BackgroundColor = this.BackgroundColor;
            TargetObjStyle.Alignment.Horizontal = this.HorizintalAlign;
            TargetObjStyle.Font.FontColor = this.FontColor;
            TargetObjStyle.Font.Bold = this.Bold;
            TargetObjStyle.Font.Italic = this.Italic;
            TargetObjStyle.Font.FontSize = this.FontSize;
        }
    }
}
