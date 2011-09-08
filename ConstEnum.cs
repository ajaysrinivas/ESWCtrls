
namespace ESWCtrls
{
    #region TreeView

    /// <summary>
    /// The types of selections to allow
    /// </summary>
    public enum SelectMode
    {
        /// <summary>No selection is allowed</summary>
        None,
        /// <summary>Only one node maybe selected at a time</summary>
        Single,
        /// <summary>Multiple nodes maybe selected</summary>
        Multiple,
        /// <summary>Only one node maybe selected at a time, and causes post back</summary>
        SinglePostback,
        /// <summary>Multiple nodes maybe selected, and causes post back</summary>
        MultiplePostback
    }

    #endregion

    #region DropDownMenu

    /// <summary>Controls which arrows to display for Drop down menus</summary>
    public enum ArrowDisplayType
    {
        /// <summary>No arrows are displayed</summary>
        None,
        /// <summary>Both arrows are displayed ( Default )</summary>
        Both,
        /// <summary>Only the top arrows are displayed</summary>
        TopOnly,
        /// <summary>Only the sub items arrows are displayed</summary>
        SubOnly
    }

    #endregion

    #region Style

    /// <summary>The background repeat enumeration</summary>
    public enum BackgroundRepeat
    {
        /// <summary>Not Set</summary>
        NotSet,
        /// <summary>Repeat in Both x and Y</summary>
        Repeat,
        /// <summary>No Repeat</summary>
        NoRepeat,
        /// <summary>Repeat only in X</summary>
        RepeatX,
        /// <summary>Repeat only in Y</summary>
        RepeatY,
        /// <summary>Inherit from parent</summary>
        Inherit
    }

    /// <summary>The background postion enumeration</summary>
    public enum BackgroundPosition
    {
        /// <summary>Not Set</summary>
        NotSet,
        /// <summary>Position the background to the top left</summary>
        LeftTop,
        /// <summary>Position the background to the left middle</summary>
        LeftCenter,
        /// <summary>Position the background in the bottom left</summary>
        LeftBottom,
        /// <summary>Position the background to the top centre</summary>
        CenterTop,
        /// <summary>Position the background to the middle</summary>
        CenterCenter,
        /// <summary>Position the background to the bottom centre</summary>
        CenterBottom,
        /// <summary>Position the background to the top right</summary>
        RightTop,
        /// <summary>Position the background to the right centre</summary>
        RightCenter,
        /// <summary>Position the background to the bottom right</summary>
        RightBottom,
        /// <summary>Inherit value from parent</summary>
        Inherit
    }

    /// <summary>The element position enumeration</summary>
    public enum ElementPosition
    {
        /// <summary>No positioning set</summary>
        NotSet,
        /// <summary>Static positioning (the most common default)</summary>
        Static,
        /// <summary>Taken out of normal flow, position relative to the whole document</summary>
        Absolute,
        /// <summary>Taken out of normal flow, relative to the viewport (window) does not move with scroll</summary>
        Fixed,
        /// <summary>Taken out of normal flow, realtive to the offset parent</summary>
        Relative,
        /// <summary>Inherits value from parent</summary>
        Inherit
    }

    /// <summary>The float type of the element</summary>
    public enum ElementFloat
    {
        /// <summary>No float type set set</summary>
        NotSet,
        /// <summary>Float left</summary>
        Left,
        /// <summary>Float Right</summary>
        Right,
        /// <summary>No floating (usual default)</summary>
        None,
        /// <summary>Inherit floating property from above</summary>
        Inherit
    }

    /// <summary>The type of list</summary>
    public enum ListStyleType
    {
        /// <summary>Leave to default (disc for "ul", decimal for "ol")</summary>
        NotSet,
        /// <summary>The marker is traditional Armenian numbering</summary>
        Armenian,
        /// <summary>The marker is a circle</summary>
        Circle,
        /// <summary>The marker is plain ideographic numbers</summary>
        CjkIdeographic,
        /// <summary>The marker is a number. This is default for &lt;ol&gt;</summary>
        Decimal,
        /// <summary>The marker is a number with leading zeros (01, 02, 03, etc.)</summary>
        DecimalLeadingZero,
        /// <summary>The marker is a filled circle. This is default for &lt;ul&gt;</summary>
        Disc,
        /// <summary>The marker is traditional Georgian numbering</summary>
        Georgian,
        /// <summary>The marker is traditional Hebrew numbering</summary>
        Hebrew,
        /// <summary>The marker is traditional Hiragana numbering</summary>
        Hiragana,
        /// <summary>The marker is traditional Hiragana iroha numbering</summary>
        HiraganaIroha,
        /// <summary>The value of the listStyleType property is inherited from parent element</summary>
        Inherit,
        /// <summary>The marker is traditional Katakana numbering</summary>
        Katakana,
        /// <summary>The marker is traditional Katakana iroha numbering</summary>
        KatakanaIroha,
        /// <summary>The marker is lower-alpha (a, b, c, d, e, etc.)</summary>
        LowerAlpha,
        /// <summary>The marker is lower-greek</summary>
        LowerGreek,
        /// <summary>The marker is lower-latin (a, b, c, d, e, etc.)</summary>
        LowerLatin,
        /// <summary>The marker is lower-roman (i, ii, iii, iv, v, etc.)</summary>
        LowerRoman,
        /// <summary>No marker is shown</summary>
        None,
        /// <summary>The marker is a square</summary>
        Square,
        /// <summary>The marker is upper-alpha (A, B, C, D, E, etc.)</summary>
        UpperAlpha,
        /// <summary>The marker is upper-latin (A, B, C, D, E, etc.)</summary>
        UpperLatin,
        /// <summary>The marker is upper-roman (I, II, III, IV, V, etc.)</summary>
        UpperRoman 	
    }

    /// <summary>The position of the markers in the list</summary>
    public enum ListStylePosition
    {
        /// <summary>Not Set</summary>
        NotSet,
        /// <summary>Indents marker and text, marker appear inside the content flow</summary>
        Inside,
        /// <summary>Keeps the marker to the left. The marker appears outside the content flow (default)</summary>
        Outside,
        /// <summary>Inherit value from parent</summary>
        Inherit
    }

    /// <summary>The display types for an element</summary>
    public enum Display
    {
        /// <summary>Not Set</summary>
        NotSet,
        /// <summary>The element will generate no box at all</summary>
        None,
        /// <summary>The element will generate a block box (a line break before and after the element)</summary>
        Block,
        /// <summary>The element will generate an inline box (no line break before or after the element). This is default</summary>
        Inline,
        /// <summary>The element will generate a block box, laid out as an inline box</summary>
        InlineBlock,
        /// <summary>The element will generate an inline box (like &lt;table&gt;, with no line break before or after)</summary>
        InlineTable,
        /// <summary>The element will generate a block box, and an inline box for the list marker</summary>
        ListItem,
        /// <summary>The element will generate a block or inline box, depending on context</summary>
        RunIn,
        /// <summary>The element will behave like a table (like &lt;table&gt;, with a line break before and after)</summary>
        Table,
        /// <summary>The element will behave like a table caption (like &lt;caption&gt;)</summary>
        TableCaption,
        /// <summary>The element will behave like a table cell</summary>
        TableCell,
        /// <summary>The element will behave like a table column</summary>
        TableColumn,
        /// <summary>The element will behave like a table column group (like &lt;colgroup&gt;)</summary>
        TableColumnGroup,
        /// <summary>The element will behave like a table footer row group</summary>
        TableFooterGroup,
        /// <summary>The element will behave like a table header row group</summary>
        TableHeaderGroup,
        /// <summary>The element will behave like a table row</summary>
        TableRow,
        /// <summary>The element will behave like a table row group</summary>
        TableRowGroup,
        /// <summary>Specifies that the value of the display property should be inherited from the parent element</summary>
        Inherit
    }

    #endregion

    #region Button

    /// <summary>
    /// Generates buttons on the fly with a particular style
    /// </summary>
    public enum GenerateStyle
    {
        /// <summary>Uses either the standard button, or just the image</summary>
        None,
        /// <summary>Draws the button with a XP style</summary>
        XP,
        /// <summary>Draws the button with a XP Silver style</summary>
        XPSilver,
        /// <summary>Draws the button with a Vista style</summary>
        Vista,
        /// <summary>Draws the button in a oxygen style</summary>
        Oxygen,
        /// <summary>Draws the button in the Calypso style</summary>
        Calypso
    }

    #endregion

    #region PopupBox/Dialog

    /// <summary>The positions the box can take</summary>
    public enum Position
    {
        /// <summary>Top of the screen in the centre, margin affects the position from the top</summary>
        CenterTop,
        /// <summary>The centre of the screen (default) margin has no affect</summary>
        CenterCenter,
        /// <summary>Bottom of the screen in the centre, margin affects the position from the bottom</summary>
        CenterBottom,
        /// <summary>Top Left of screen, margin is distance from top and left</summary>
        LeftTop,
        /// <summary>Left of screen in the middle, margin is distance from the left</summary>
        LeftCenter,
        /// <summary>Bottom Left of screen, margin is distance from the left and bottom</summary>
        LeftBottom,
        /// <summary>Top Right of screen, margin is distance from the right and top</summary>
        RightTop,
        /// <summary>Right of screen in the middle, margin is distance from the right</summary>
        RightCenter,
        /// <summary>Bottom Right of screen, margin is distance from the right and bottom</summary>
        RightBottom
    }

    #endregion

    #region Validation

    #region Enum

    /// <summary>
    /// Conditions for the validation
    /// </summary>
    public enum Condition
    {
        /// <summary>One must be valid</summary>
        OR,
        /// <summary>Only one must be valid</summary>
        XOR,
        /// <summary>All must be valid</summary>
        AND
    }

    #endregion

    #endregion

    #region Script

    /// <summary>
    /// The diffrent types of scripts available
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// The General scripts holds some low-level stuff 
        /// </summary>
        General,
        /// <summary>
        /// jQuery for general use
        /// </summary>
        jQuery,
        /// <summary>
        /// All the available script types
        /// </summary>
        All
    }

    #endregion

    #region Tabs

    /// <summary>
    /// The way the tabs behave
    /// </summary>
    public enum TabStyle { 
        /// <summary>
        /// The tab will be processed serverside, Only the current page is rendered
        /// </summary>
        ServerSide, 
        /// <summary>
        /// The tab will be processed client side, good for maintaining diffrent styles from jquery.
        /// </summary>
        ClientSide, 
        /// <summary>
        /// Uses jquery to handle tabs
        /// </summary>
        jQuery 
    }

    #endregion

    #region Slider

    /// <summary>
    /// The orientation of a control
    /// </summary>
    public enum Orientation
    {
        /// <summary>Horizontal orientation</summary>
        Horizontal,
        /// <summary>Vertical orientation</summary>
        Vertical
    }

    /// <summary>
    /// The diffrent range types for a slider
    /// </summary>
    public enum RangeType
    {
        /// <summary>No range</summary>
        None,
        /// <summary>Range between two handles</summary>
        Range,
        /// <summary>Range from minimum to handle</summary>
        Minimum,
        /// <summary>Range from handle to maximum</summary>
        Maximum
    }

    #endregion

    #region SortColumn

    /// <summary>
    /// Whether to show image or text or both
    /// </summary>
    public enum ImageText
    {
        /// <summary>Show both image and text if available</summary>
        Both,
        /// <summary>Show text only, even if an image is set</summary>
        TextOnly,
        /// <summary>Show Image only</summary>
        ImageOnly
    }

    #endregion
}
