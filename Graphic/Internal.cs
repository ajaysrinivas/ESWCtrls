using System.Drawing;
using System.Runtime.InteropServices;

namespace ESWCtrls.Graphic
{

    [StructLayout(LayoutKind.Explicit)]
    internal struct Color32
    {
        [FieldOffset(0)]
        public byte Blue;
        [FieldOffset(1)]
        public byte Green;
        [FieldOffset(2)]
        public byte Red;
        [FieldOffset(3)]
        public byte Alpha;

        [FieldOffset(0)]
        public int ARGB;

        public Color Color
        {
            get { return Color.FromArgb(Alpha, Red, Green, Blue); }
        }
    }

    internal unsafe class XColor
    {
        public XColor()
        {
            _r = _g = _b = _a = 0;
        }

        public XColor(int red, int green, int blue, int alpha)
        {
            _r = red;
            _g = green;
            _b = blue;
            _a = alpha;
        }

        public XColor(Color32* col)
        {
            _r = col->Red;
            _g = col->Green;
            _b = col->Blue;
            _a = col->Alpha;
        }

        public int Red
        {
            get { return _r; }
            set { _r = value; }
        }

        public int Green
        {
            get { return _g; }
            set { _g = value; }
        }

        public int Blue
        {
            get { return _b; }
            set { _b = value; }
        }

        public int Alpha
        {
            get { return _a; }
            set { _a = value; }
        }

        public static XColor operator +(XColor col1, XColor col2)
        {
            XColor rst = new XColor();
            rst._r = col1._r + col2._r;
            rst._g = col1._g + col2._g;
            rst._b = col1._b + col2._b;
            rst._a = col1._a + col2._a;
            return rst;
        }

        public static XColor operator /(XColor col, int val)
        {
            XColor rst = new XColor();
            rst._r = col._r / val;
            rst._g = col._g / val;
            rst._b = col._b / val;
            rst._a = col._a / val;
            return rst;
        }

        public Color rgba
        {
            get { return Color.FromArgb(_a, _r, _g, _b); }
            set
            {
                _r = value.R;
                _g = value.G;
                _b = value.B;
                _a = value.A;
            }
        }

        public Color rgb
        {
            get { return Color.FromArgb(_r, _g, _b); }
            set
            {
                _r = value.R;
                _g = value.G;
                _b = value.B;
            }
        }

        public Color32 col32
        {
            get
            {
                Color32 rst = new Color32();
                if(_r < 0)
                    rst.Red = 0;
                else if(_r > 255)
                    rst.Red = 255;
                else
                    rst.Red = (byte)_r;

                if(_g < 0)
                    rst.Green = 0;
                else if(_g > 255)
                    rst.Green = 255;
                else
                    rst.Green = (byte)_g;

                if(_b < 0)
                    rst.Blue = 0;
                else if(_b > 255)
                    rst.Blue = 255;
                else
                    rst.Blue = (byte)_b;

                if(_a < 0)
                    rst.Alpha = 0;
                else if(_a > 255)
                    rst.Alpha = 255;
                else
                    rst.Alpha = (byte)_a;

                return rst;
            }
        }

        private int _r;
        private int _g;
        private int _b;
        private int _a;
    }

    internal class ColorList : System.Collections.Generic.List<XColor>
    {
        public XColor average()
        {
            XColor rst = new XColor();
            foreach(XColor col in this)
            {
                rst += col;
            }

            rst /= Count;
            return rst;
        }
    }
}
