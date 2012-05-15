
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
        Calypso,
        /// <summary>Uses the custom images provided</summary>
        Custom
    }

    /// <summary>The Icons available for the jQuery button</summary>
    public enum JButtonIcon
    {
        /// <summary>No icon</summary>
        None,

        /// <summary>Carat Icon North or ^</summary>
        Carat,
        /// <summary>Carat North East</summary>
        CaratNE,
        /// <summary>Carat East or Greater Than</summary>
        CaratE,
        /// <summary>Carat South East</summary>
        CaratSE,
        /// <summary>Carat South</summary>
        CaratS,
        /// <summary>Carat South West</summary>
        CaratSW,
        /// <summary>Carat West or Less Than</summary>
        CaratW,
        /// <summary>Carat North West</summary>
        CaratNW,
        /// <summary>Carats North - South</summary>
        CaratN_S,
        /// <summary>Carats East - West or Greater Than and Less Than</summary>
        CaratE_W,

        /// <summary>Triangle North</summary>
        TriangleN,
        /// <summary>Triangle North East</summary>
        TriangleNE,
        /// <summary>Triangle East</summary>
        TriangleE,
        /// <summary>Triangle South East</summary>
        TriangleSE,
        /// <summary>Triangle South</summary>
        TriangleS,
        /// <summary>Triangle South West</summary>
        TriangleSW,
        /// <summary>Triangle West</summary>
        TriangleW,
        /// <summary>Triangle North West</summary>
        TriangleNW,
        /// <summary>Triangles North - South</summary>
        TriangleN_S,
        /// <summary>Triagnles East - West</summary>
        TriangleE_W,

        /// <summary>Arrow North</summary>
        ArrowN,
        /// <summary>Arrow North East</summary>
        ArrowNE,
        /// <summary>Arrow East</summary>
        ArrowE,
        /// <summary>Arrow South East</summary>
        ArrowSE,
        /// <summary>Arrow South</summary>
        ArrowS,
        /// <summary>Arrow South West</summary>
        ArrowSW,
        /// <summary>Arrow West</summary>
        ArrowW,
        /// <summary>Arrow North West</summary>
        ArrowNW,
        /// <summary>Arrow North - South</summary>
        ArrowN_S,
        /// <summary>Arrow NE - SW</summary>
        ArrowNE_SW,
        /// <summary>Arrow East - West</summary>
        ArrowE_W,
        /// <summary>Arrow SE - NW</summary>
        ArrowSE_NW,
        /// <summary>Arrow Stop North</summary>
        ArrowStopN,
        /// <summary>Arrow Stop East</summary>
        ArrowStopE,
        /// <summary>Arrow Stop South</summary>
        ArrowStopS,
        /// <summary>Arrow Stop West</summary>
        ArrowStopW,
        /// <summary>Arrow Return North</summary>
        ArrowReturnN,
        /// <summary>Arrow Return East</summary>
        ArrowReturnE,
        /// <summary>Arrow Return West</summary>
        ArrowReturnW,
        /// <summary>Arrow Return South</summary>
        ArrowReturnS,


        /// <summary>Arrow Thick North</summary>
        ArrowThickN,
        /// <summary>Arrow Thick North East</summary>
        ArrowThickNE,
        /// <summary>Arrow Thick East</summary>
        ArrowThickE,
        /// <summary>Arrow Thick South East</summary>
        ArrowThickSE,
        /// <summary>Arrow Thick South</summary>
        ArrowThickS,
        /// <summary>Arrow Thick South West</summary>
        ArrowThickSW,
        /// <summary>Arrow Thick West</summary>
        ArrowThickW,
        /// <summary>Arrow Thick North West</summary>
        ArrowThickNW,
        /// <summary>Arrow Thick North - South</summary>
        ArrowThickN_S,
        /// <summary>Arrow Thick NE - SW</summary>
        ArrowThickNE_SW,
        /// <summary>Arrow Thick East - West</summary>
        ArrowThickE_W,
        /// <summary>Arrow Thick SE - NW</summary>
        ArrowThickSE_NW,
        /// <summary>Arrow Thick Stop North</summary>
        ArrowThickStopN,
        /// <summary>Arrow Thick Stop East</summary>
        ArrowThickStopE,
        /// <summary>Arrow Thick Stop South</summary>
        ArrowThickStopS,
        /// <summary>Arrow Thick Stop West</summary>
        ArrowThickStopW,
        /// <summary>Arrow Thick Return North</summary>
        ArrowThickReturnN,
        /// <summary>Arrow Thick Return East</summary>
        ArrowThickReturnE,
        /// <summary>Arrow Thick Return West</summary>
        ArrowThickReturnW,
        /// <summary>Arrow Thick Return South</summary>
        ArrowThickReturnS,

        /// <summary>Arrow Refresh North</summary>
        ArrowRefreshN,
        /// <summary>Arrow Refresh East</summary>
        ArrowRefreshE,
        /// <summary>Arrow Refresh South</summary>
        ArrowRefreshS,
        /// <summary>Arrow Refresh West</summary>
        ArrowRefreshW,
        /// <summary>Arrow All NSEW</summary>
        ArrowAll,
        /// <summary>Arrow All 4 Diag</summary>
        ArrowAllDiag,

        /// <summary>External Link</summary>
        ExtLink,
        /// <summary>New Window</summary>
        NewWindow,
        /// <summary>refresh</summary>
        Refresh,
        /// <summary>Shuffle</summary>
        Shuffle,
        /// <summary>Transfer</summary>
        Transfer,
        /// <summary>Transfer Thick</summary>
        TransferThick,
        /// <summary>Folder Collapsed</summary>
        FolderCollapsed,
        /// <summary>Folder Open</summary>
        FolderOpen,
        /// <summary>Document</summary>
        Document,
        /// <summary>Document B</summary>
        DocumentB,
        /// <summary>Note</summary>
        Note,
        /// <summary>Mail Closed</summary>
        MailClosed,
        /// <summary>Mail Open</summary>
        MailOpen,
        /// <summary>Suitcase</summary>
        Suitcase,
        /// <summary>Comment</summary>
        Comment,
        /// <summary>Person</summary>
        Person,
        /// <summary>Print</summary>
        Print,
        /// <summary>Trash</summary>
        Trash,
        /// <summary>Locked</summary>
        Locked,
        /// <summary>Unlocked</summary>
        Unlocked,
        /// <summary>bookmark</summary>
        Bookmark,
        /// <summary>Tag</summary>
        Tag,
        /// <summary>Home</summary>
        Home,
        /// <summary>Flag</summary>
        Flag,
        /// <summary>Calculator</summary>
        Calculator,
        /// <summary>Cart</summary>
        Cart,
        /// <summary>Pencil</summary>
        Pencil,
        /// <summary>Clock</summary>
        Clock,
        /// <summary>Disk</summary>
        Disk,
        /// <summary>Calendar</summary>
        Calendar,
        /// <summary>Zoom In</summary>
        ZoomIn,
        /// <summary>Zoom Out</summary>
        ZoomOut,
        /// <summary>Search</summary>
        Search,
        /// <summary>Wrench</summary>
        Wrench,
        /// <summary>Spanner</summary>
        Spanner,
        /// <summary>Gear</summary>
        Gear,
        /// <summary>Heart</summary>
        Heart,
        /// <summary>Star</summary>
        Star,
        /// <summary>Link</summary>
        Link,
        /// <summary>Cancel</summary>
        Cancel,
        /// <summary>Plus</summary>
        Plus,
        /// <summary>Plus Thick</summary>
        PlusThick,
        /// <summary>Minus</summary>
        Minus,
        /// <summary>Minus Thick</summary>
        MinusThick,
        /// <summary>Close</summary>
        Close,
        /// <summary>Close Thick</summary>
        CloseThink,
        /// <summary>Key</summary>
        Key,
        /// <summary>Light Bulb</summary>
        LightBulb,
        /// <summary>Scissors</summary>
        Scissors,
        /// <summary>Clipboard</summary>
        Clipboard,
        /// <summary>Copy</summary>
        Copy,
        /// <summary>Contact</summary>
        Contact,
        /// <summary>Image</summary>
        Image,
        /// <summary>Video</summary>
        Video,
        /// <summary>Script</summary>
        Script,
        /// <summary>Alert</summary>
        Alert,
        /// <summary>Information</summary>
        Info,
        /// <summary>Notice</summary>
        Notice,
        /// <summary>Help</summary>
        Help,
        /// <summary>Check</summary>
        Check,
        /// <summary>Bullet</summary>
        Bullet,
        /// <summary>Radio Off</summary>
        RadioOff,
        /// <summary>Radio On</summary>
        RadioOn,
        /// <summary>Pin West</summary>
        PinWest,
        /// <summary>Pin South</summary>
        PinSouth,
        /// <summary>Play</summary>
        Play,
        /// <summary>Pause</summary>
        Pause,
        /// <summary>Seek Next</summary>
        SeekNext,
        /// <summary>Seek Previous</summary>
        SeekPrev,
        /// <summary>Seek End</summary>
        SeekEnd,
        /// <summary>Seek First</summary>
        SeekFirst,
        /// <summary>Stop</summary>
        Stop,
        /// <summary>Eject</summary>
        Eject,
        /// <summary>Volume Off</summary>
        VolumeOff,
        /// <summary>volume On</summary>
        VolumeOn,
        /// <summary>Power</summary>
        Power,
        /// <summary>Signal Diagonal</summary>
        SignalDiag,
        /// <summary>Signal</summary>
        Signal,
        /// <summary>Battery 0</summary>
        Battery0,
        /// <summary>Battery 1</summary>
        Battery1,
        /// <summary>Battery 2</summary>
        Battery2,
        /// <summary>Battery 3</summary>
        Battery3,

        /// <summary>Circle Plus</summary>
        CirclePlus,
        /// <summary>Circle Minus</summary>
        CircleMinus,
        /// <summary>Circle Close</summary>
        CircleClose,
        /// <summary>Circle Triangle North</summary>
        CircleTriangleNorth,
        /// <summary>Circle Triangle East</summary>
        CircleTriangleEast,
        /// <summary>Circle Triangle West</summary>
        CircleTriangleWest,
        /// <summary>Circle Triangle South</summary>
        CircleTriangleSouth,
        /// <summary>Circle Arrow North</summary>
        CircleArrowNorth,
        /// <summary>Circle Arrow East</summary>
        CircleArrowEast,
        /// <summary>Circle Arrow South</summary>
        CircleArrowSouth,
        /// <summary>Circle Arrow West</summary>
        CircleArrowWest,
        /// <summary>Circle Zoom In</summary>
        CircleZoomIn,
        /// <summary>Circle Zoom Out</summary>
        CircleZoomOut,
        /// <summary>Circle Check</summary>
        CircleCheck,

        /// <summary>Circle Small Plus</summary>
        CircleSmallPlus,
        /// <summary>Circle Small Minus</summary>
        CircleSmallMinus,
        /// <summary>Circle Small Close</summary>
        CircleSmallClose,

        /// <summary>Square Small Plus</summary>
        SquareSmallPlus,
        /// <summary>Square Small Minus</summary>
        SquareSmallMinus,
        /// <summary>Square Small Close</summary>
        SquareSmallClose,

        /// <summary>Grip Dotted Vertical</summary>
        GripDottedVertical,
        /// <summary>Grip Dotted Horizontal</summary>
        GripDottedHorizontal,
        /// <summary>Grip Solid Vertical</summary>
        GripSolidVertical,
        /// <summary>Grip Solid Horizontal</summary>
        GripSolidHorizontal,
        /// <summary>Grip Small Diagonal</summary>
        GripSmallDiagonal,
        /// <summary>Grip Diagonal</summary>
        GripDiagonal
    }

    #endregion

    #region Positioning

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

    /// <summary>
    /// How to handle cllisions in position
    /// </summary>
    public enum Collision
    {
        /// <summary>Don't do collision detection</summary>
        None,
        /// <summary>Flip to the opposite side, to see if it will fit, if it won't center</summary>
        Flip,
        /// <summary>Do its best to reposition to make the element fit</summary>
        Fit,
        /// <summary>Horizontal <see cref="None">None</see>, Vertical <see cref="Flip">Flip</see></summary>
        NoneFlip,
        /// <summary>Horizontal <see cref="None">None</see>, Vertical <see cref="Fit">Fit</see></summary>
        NoneFit,
        /// <summary>Horizontal <see cref="Flip">Flip</see>, Vertical <see cref="None">None</see></summary>
        FlipNone,
        /// <summary>Horizontal <see cref="Flip">Flip</see>, Vertical <see cref="Fit">Fit</see></summary>
        FlipFit,
        /// <summary>Horizontal <see cref="Fit">Fit</see>, Vertical <see cref="None">None</see></summary>
        FitNone,
        /// <summary>Horizontal <see cref="Fit">Fit</see>, Vertical <see cref="Flip">Flip</see></summary>
        FitFlip
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

    #region Tabs

    /// <summary>
    /// The way the tabs behave
    /// </summary>
    public enum TabStyle
    {
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

    #region SortColumn / HelpPoint

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

    #region Date Picker

    /// <summary>
    /// The min/max control modes how the control should control the min/max of the other control
    /// </summary>
    public enum RangeMode
    {
        /// <summary>
        /// Uses min/max date on the other control. So the limit is hard fixed on the control
        /// </summary>
        Fixed,
        /// <summary>
        /// Alters the current value directly instead, to allow min/max values to push the other
        /// </summary>
        /// <example>
        /// If this control has the minimumdatectrl set, if when its value is set, its greater than
        /// the other control, its sets the other controls value to the same.
        /// </example>
        Sliding
    }

    #endregion

    #region Constants

    internal static class Constants
    {
        internal static string[] PositionStrings = new string[] { "top", "center", "bottom", "left top", "left", "left bottom", "right top", "right", "right bottom" };
        internal static string[] CollisionStrings = new string[] { "none", "flip", "fit", "none flip", "none fit", "flip none", "flip fit", "fit none", "fit flip" };
        internal static string[] IconStrings = new string[] 
        {
            "",
            "carat-1-n", "carat-1-ne", "carat-1-e", "carat-1-se", "carat-1-s", "carat-1-sw", "carat-1-w", "carat-1-nw", "carat-2-n-s", "carat-2-e-w",
            "triangle-1-n", "triangle-1-ne", "triangle-1-e", "triangle-1-se", "triangle-1-s", "triangle-1-sw", "triangle-1-w", "triangle-1-nw", "triangle-2-n-s", "triangle-2-e-w",

            "arrow-1-n", "arrow-1-ne", "arrow-1-e", "arrow-1-se", "arrow-1-s", "arrow-1-sw", "arrow-1-w", "arrow-1-nw", 
            "arrow-2-n-s", "arrow-2-ne-sw", "arrow-2-e-w", "arrow-2-se-nw", 
            "arrowstop-1-n", "arrowstop-1-e", "arrowstop-1-s", "arrowstop-1-w", 
            "arrowreturn-1-n", "arrowreturn-1-e", "arrowreturn-1-s", "arrowreturn-1-w",

            "arrowthick-1-n", "arrowthick-1-ne", "arrowthick-1-e", "arrowthick-1-se", "arrowthick-1-s", "arrowthick-1-sw", "arrowthick-1-w", "arrowthick-1-nw", 
            "arrowthick-2-n-s", "arrowthick-2-ne-sw", "arrowthick-2-e-w", "arrowthick-2-se-nw", 
            "arrowthickstop-1-n", "arrowthickstop-1-e", "arrowthickstop-1-s", "arrowthickstop-1-w", 
            "arrowthickreturn-1-n", "arrowthickreturn-1-e", "arrowthickreturn-1-s", "arrowthickreturn-1-w",

            "arrowrefresh-1-n", "arrowrefresh-1-e", "arrowrefresh-1-s", "arrowrefresh-1-w", "arrow-4", "arrow-4-diag",

            "extlink", "newwin", "refresh", "shuffle", "transfer-e-w", "transferthick-e-w", "folder-collapsed", "folder-open", "document", "document-b", "note", "mail-closed", "mail-open",
            "suitcase", "comment", "person", "print", "trash", "locked", "unlocked", "bookmark", "tag", "home", "flag", "calculator", "cart", "pencil", "clock", "disk", "calendar",
            "zoomin", "zoomout", "search", "wrench", "wrench", "gear", "heart", "star", "link", "cancel", "plus", "plusthick", "minus", "minusthick", "close", "closethick", "key",
            "lightbulb", "scissors", "clipboard", "copy", "contact", "image", "video", "script", "alert", "info", "notice", "help", "check", "bullet", "radio-off", "radio-on", 
            "pin-w", "pin-s", "play", "pause", "seek-next", "seek-prev", "seek-end", "seek-first", "stop", "eject", "volume-off", "volume-on", "power", "signal-diag", "signal", 
            "battery-0", "battery-1", "battery-2", "battery-3",

            "circle-plus", "circle-minus", "circle-close", "circle-triangle-n", "circle-triangle-e", "circle-triangle-s", "circle-triangle-w",
            "circle-arrow-n", "circle-arrow-e", "circle-arrow-s", "circle-arrow-w", "circle-zoomin", "circle-zoomout", "circle-check", 

            "circlsmall-plus", "circlesmall-minus", "circlesmall-close", "squaresmall-plus", "squaresmall-minus", "squaresmall-close", 

            "grip-dotted-vertical", "grip-dotted-horizontal", "grid-solid-vertical", "grip-solid-horizontal", "gripsmall-diagonal-se", "grip-diagonal-se"
        };
    }

    #endregion
}
