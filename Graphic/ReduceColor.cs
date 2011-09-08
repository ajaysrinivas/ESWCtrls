using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESWCtrls.Graphic
{
    /// <summary>
    /// Reduces the number of colors in an image to a maximum
    /// </summary>
    public unsafe class ReduceColor
    {
        /// <summary>
        /// Constructor takes the variables and processes the image
        /// </summary>
        /// <param name="image">The image to reduce</param>
        /// <param name="maxColors">The maximum number of colors to use value must be between 2 and 255</param>
        public ReduceColor(Bitmap image, int maxColors)
        {
            if (maxColors < 2 || maxColors > 255)
                throw new ArgumentOutOfRangeException("maxColors", maxColors, "The maximum number of colors should be between 2 and 255");

            int maxColorBits = 8;
            for (int i = 1, j = 2; i < 9; ++i, j *= 2)
            {
                if (maxColors <= j)
                {
                    maxColorBits = i;
                    break;
                }
            }

            Octree tree = new Octree(maxColorBits);
            Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);
            Bitmap copy = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            Bitmap output = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImageUnscaled(image, bounds);
            }

            BitmapData imgData = null;
            try
            {
                imgData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                firstPass(tree, imgData, image.Width, image.Height);
                output.Palette = palette(tree, output.Palette, maxColors);
                secondPass(tree, imgData, output, image.Width, image.Height, bounds, maxColors);
            }
            finally { copy.UnlockBits(imgData); }

            _rst = output;
        }

        /// <summary>
        /// Returns the result of the processing
        /// </summary>
        public Bitmap Result
        {
            get { return _rst; }
        }

        private void firstPass(Octree tree, BitmapData data, int width, int height)
        {
            byte* srcRow = (byte*)data.Scan0.ToPointer();
            Int32* srcPxl;

            for (int row = 0; row < height; ++row)
            {
                srcPxl = (Int32*)srcRow;
                for (int col = 0; col < width; ++col, ++srcPxl)
                    tree.addColor((Color32*)srcPxl);

                srcRow += data.Stride;
            }
        }

        private void secondPass(Octree tree, BitmapData data, Bitmap output, int width, int height, Rectangle bounds, int maxColors)
        {
            BitmapData outData = null;
            try
            {
                outData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                byte* srcRow = (byte*)data.Scan0.ToPointer();
                Int32* srcPxl = (Int32*)srcRow;
                Int32* prvPxl = srcPxl;

                byte* dstRow = (byte*)outData.Scan0.ToPointer();
                byte* dstPxl = dstRow;

                byte pxlValue = processPxl(tree, (Color32*)srcPxl, maxColors);
                *dstPxl = pxlValue;

                for (int row = 0; row < height; ++row)
                {
                    srcPxl = (Int32*)srcRow;
                    dstPxl = dstRow;

                    for (int col = 0; col < width; ++col, ++srcPxl, ++dstPxl)
                    {
                        if (*prvPxl == *srcPxl)
                        {
                            pxlValue = processPxl(tree, (Color32*)srcPxl, maxColors);
                            prvPxl = srcPxl;
                        }
                        *dstPxl = pxlValue;
                    }

                    srcRow += data.Stride;
                    dstRow += outData.Stride;
                }
            }
            finally { output.UnlockBits(outData); }
        }

        private byte processPxl(Octree tree, Color32* pxl, int maxColors)
        {
            byte idx = (byte)maxColors;
            if (pxl->Alpha > 0)
                idx = (byte)tree.paletteIndex(pxl);

            return idx;
        }

        private ColorPalette palette(Octree tree, ColorPalette old, int maxColors)
        {
            ArrayList palette = tree.palette(maxColors - 1);
            for (int i = 0; i < palette.Count; ++i)
                old.Entries[i] = (Color)palette[i];

            old.Entries[maxColors] = Color.FromArgb(0, 0, 0, 0);
            return old;
        }

        /// <summary>
        /// Class which does the quantization for the graphics
        /// </summary>
        internal class Octree
        {
            #region public

            /// <summary>
            /// Construct the octree
            /// </summary>
            /// <param name="maxColorBits">The maximum number of significant bits in the image</param>
            public Octree(int maxColorBits)
            {
                _maxColorBits = maxColorBits;
                _leafCount = 0;
                _reducibleNodes = new OctreeNode[9];
                _root = new OctreeNode(0, _maxColorBits, this);
                _previousColor = 0;
                _previousNode = null;
            }

            /// <summary>
            /// Add a given color value to the octree
            /// </summary>
            /// <param name="pixel"></param>
            public void addColor(Color32* pixel)
            {
                // Check if this request is for the same color as the last
                if (_previousColor == pixel->ARGB)
                {
                    if (null == _previousNode)
                    {
                        _previousColor = pixel->ARGB;
                        _root.addColor(pixel, _maxColorBits, 0, this);
                    }
                    else
                        _previousNode.inc(pixel);
                }
                else
                {
                    _previousColor = pixel->ARGB;
                    _root.addColor(pixel, _maxColorBits, 0, this);
                }
            }

            /// <summary>
            /// Reduce the depth of the tree
            /// </summary>
            public void reduce()
            {
                int index;

                for (index = _maxColorBits - 1; (index > 0) && (null == _reducibleNodes[index]); ++index) ;

                OctreeNode node = _reducibleNodes[index];
                _reducibleNodes[index] = node.nextReducible;

                _leafCount -= node.reduce();
                _previousNode = null;
            }

            /// <summary>
            /// Convert the nodes in the octree to a palette with a maximum of colorCount colors
            /// </summary>
            /// <param name="colorCount">The maximum number of colors</param>
            /// <returns>An arraylist with the palettized colors</returns>
            public ArrayList palette(int colorCount)
            {
                while (leaves > colorCount)
                    reduce();

                // Now palettize the nodes
                ArrayList palette = new ArrayList(leaves);
                int paletteIndex = 0;
                _root.constructPalette(palette, ref paletteIndex);

                // And return the palette
                return palette;
            }

            /// <summary>
            /// Get the palette index for the passed color
            /// </summary>
            /// <param name="pixel"></param>
            /// <returns></returns>
            public int paletteIndex(Color32* pixel)
            {
                return _root.getPaletteIndex(pixel, 0);
            }

            /// <summary>
            /// Get/Set the number of leaves in the tree
            /// </summary>
            public int leaves
            {
                get { return _leafCount; }
                set { _leafCount = value; }
            }

            #endregion

            #region private

            /// <summary>
            /// Return the array of reducible nodes
            /// </summary>
            protected OctreeNode[] reducibleNodes
            {
                get { return _reducibleNodes; }
            }

            /// <summary>
            /// Keep track of the previous node that was quantized
            /// </summary>
            /// <param name="node">The node last quantized</param>
            protected void trackPrevious(OctreeNode node)
            {
                _previousNode = node;
            }

            #endregion

            #region variables

            private static int[] mask = new int[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

            private OctreeNode _root;
            private int _leafCount;
            private OctreeNode[] _reducibleNodes;
            private int _maxColorBits;
            private OctreeNode _previousNode;
            private int _previousColor;

            #endregion

            #region octree node

            /// <summary>
            /// Class which encapsulates each node in the tree
            /// </summary>
            protected class OctreeNode
            {
                /// <summary>
                /// Construct the node
                /// </summary>
                /// <param name="level">The level in the tree = 0 - 7</param>
                /// <param name="colorBits">The number of significant color bits in the image</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public OctreeNode(int level, int colorBits, Octree octree)
                {
                    // Construct the new node
                    _leaf = (level == colorBits);

                    _red = _green = _blue = 0;
                    _pixelCount = 0;

                    // If a leaf, increment the leaf count
                    if (_leaf)
                    {
                        octree.leaves++;
                        _nextReducible = null;
                        _children = null;
                    }
                    else
                    {
                        // Otherwise add this to the reducible nodes
                        _nextReducible = octree.reducibleNodes[level];
                        octree.reducibleNodes[level] = this;
                        _children = new OctreeNode[8];
                    }
                }

                /// <summary>
                /// Add a color into the tree
                /// </summary>
                /// <param name="pixel">The color</param>
                /// <param name="colorBits">The number of significant color bits</param>
                /// <param name="level">The level in the tree</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public void addColor(Color32* pixel, int colorBits, int level, Octree octree)
                {
                    // Update the color information if this is a leaf
                    if (_leaf)
                    {
                        inc(pixel);
                        // Setup the previous node
                        octree.trackPrevious(this);
                    }
                    else
                    {
                        // Go to the next level down in the tree
                        int shift = 7 - level;
                        int index = ((pixel->Red & mask[level]) >> (shift - 2)) |
                           ((pixel->Green & mask[level]) >> (shift - 1)) |
                           ((pixel->Blue & mask[level]) >> (shift));

                        OctreeNode child = _children[index];

                        if (child == null)
                        {
                            // Create a new child node & store in the array
                            child = new OctreeNode(level + 1, colorBits, octree);
                            _children[index] = child;
                        }

                        // Add the color to the child node
                        child.addColor(pixel, colorBits, level + 1, octree);
                    }

                }

                /// <summary>
                /// Get/Set the next reducible node
                /// </summary>
                public OctreeNode nextReducible
                {
                    get { return _nextReducible; }
                    set { _nextReducible = value; }
                }

                /// <summary>
                /// Return the child nodes
                /// </summary>
                public OctreeNode[] children
                {
                    get { return _children; }
                }

                /// <summary>
                /// Reduce this node by removing all of its children
                /// </summary>
                /// <returns>The number of leaves removed</returns>
                public int reduce()
                {
                    _red = _green = _blue = 0;
                    int children = 0;

                    // Loop through all children and add their information to this node
                    for (int index = 0; index < 8; index++)
                    {
                        if (null != _children[index])
                        {
                            _red += _children[index]._red;
                            _green += _children[index]._green;
                            _blue += _children[index]._blue;
                            _pixelCount += _children[index]._pixelCount;
                            ++children;
                            _children[index] = null;
                        }
                    }

                    // Now change this to a leaf node
                    _leaf = true;

                    // Return the number of nodes to decrement the leaf count by
                    return (children - 1);
                }

                /// <summary>
                /// Traverse the tree, building up the color palette
                /// </summary>
                /// <param name="palette">The palette</param>
                /// <param name="paletteIndex">The current palette index</param>
                public void constructPalette(ArrayList palette, ref int paletteIndex)
                {
                    if (_leaf)
                    {
                        // Consume the next palette index
                        _paletteIndex = paletteIndex++;

                        // And set the color of the palette entry
                        palette.Add(Color.FromArgb(_red / _pixelCount, _green / _pixelCount, _blue / _pixelCount));
                    }
                    else
                    {
                        // Loop through children looking for leaves
                        for (int index = 0; index < 8; index++)
                        {
                            if (_children[index] != null)
                                _children[index].constructPalette(palette, ref paletteIndex);
                        }
                    }
                }

                /// <summary>
                /// Return the palette index for the passed color
                /// </summary>
                public int getPaletteIndex(Color32* pixel, int level)
                {
                    int paletteIndex = _paletteIndex;

                    if (!_leaf)
                    {
                        int shift = 7 - level;
                        int index = ((pixel->Red & mask[level]) >> (shift - 2)) |
                           ((pixel->Green & mask[level]) >> (shift - 1)) |
                           ((pixel->Blue & mask[level]) >> (shift));

                        if (_children[index] != null)
                            paletteIndex = _children[index].getPaletteIndex(pixel, level + 1);
                        else
                            throw new Exception("Unexpected Index error");
                    }

                    return paletteIndex;
                }

                /// <summary>
                /// Increment the pixel count and add to the color information
                /// </summary>
                public void inc(Color32* pixel)
                {
                    _pixelCount++;
                    _red += pixel->Red;
                    _green += pixel->Green;
                    _blue += pixel->Blue;
                }

                private bool _leaf;
                private int _pixelCount;
                private int _red;
                private int _green;
                private int _blue;
                private OctreeNode[] _children;
                private OctreeNode _nextReducible;
                private int _paletteIndex;

            }

            #endregion
        }

        Bitmap _rst;
    }
}
