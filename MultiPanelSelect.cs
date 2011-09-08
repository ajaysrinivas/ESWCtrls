//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Globalization;
//using System.Security.Permissions;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.Adapters;
//using System.Web.Util;
//using System.Drawing;
//using System.Drawing.Design;
//using System.Web.UI.WebControls;
//using System.Web.UI.Design;


//namespace ESWCtrls
//{
//    using Internal;
//    using Graphic;

//    /// <summary>
//    /// TreeView control, clean rendering, and works with AJAX
//    /// </summary>
//    /// <remarks></remarks>
//    [ToolboxData("<{0}:MultiPanelSelect runat=\"server\"></{0}:MultiPanelSelect>")]
//    public class MultiPanelSelect : DataBoundControl, IPostBackEventHandler
//    {
//        #region Properties

//        #region Data

//        /// <summary>
//        /// The list of items in the control
//        /// </summary>
//        [DefaultValue(typeof(ListItemCollection), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
//        public ListItemCollection Items
//        {
//            get 
//            {
//                if (_items == null)
//                    _items = new ListItemCollection();
//                return _items; 
//            }
//            set { _items = value; }
//        }

//        /// <summary>
//        /// The field for the text of the items
//        /// </summary>
//        [Category("Data"), DefaultValue(null)]
//        public string DataTextField
//        {
//            get
//            {
//                if (ViewState["DataTextField"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["DataTextField"];
//            }
//            set
//            {
//                ViewState["DataTextField"] = value;
//                if (Initialized)
//                    OnDataPropertyChanged();
//            }
//        }

//        /// <summary>
//        /// The field for the value of the items
//        /// </summary>
//        [Category("Data"), DefaultValue(null)]
//        public string DataValueField
//        {
//            get
//            {
//                if (ViewState["DataValueField"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["DataValueField"];
//            }
//            set
//            {
//                ViewState["DataValueField"] = value;
//                if (Initialized)
//                    OnDataPropertyChanged();
//            }
//        }

//        #endregion

//        #region Appearance

//        /// <summary>
//        /// The style for the panels
//        /// </summary>
//        [Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
//        public AdvStyle TitleStyle
//        {
//            get
//            {
//                if (_titleStyle == null)
//                {
//                    _titleStyle = new AdvStyle();
//                    if (IsTrackingViewState)
//                        ((IStateManager)_titleStyle).TrackViewState();
//                }
//                return _titleStyle;
//            }
//        }

//        /// <summary>
//        /// The style for the panels
//        /// </summary>
//        [Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
//        public AdvStyle PanelStyle
//        {
//            get
//            {
//                if (_panelStyle == null)
//                {
//                    _panelStyle = new AdvStyle();
//                    if (IsTrackingViewState)
//                        ((IStateManager)_panelStyle).TrackViewState();
//                }
//                return _panelStyle;
//            }
//        }

//        /// <summary>
//        /// Whether the button is generated and what type to use
//        /// </summary>
//        [Category("Appearance"), DefaultValue(Button.GenerateStyles.None)]
//        public Button.GenerateStyles ButtonGenerateStyle
//        {
//            get
//            {
//                if (ViewState["GenerateStyle"] == null)
//                    return Button.GenerateStyles.None;
//                else
//                    return (Button.GenerateStyles)ViewState["GenerateStyle"];
//            }
//            set { ViewState["GenerateStyle"] = value; }
//        }

//        /// <summary>
//        /// The image to display for the add button
//        /// </summary>
//        [Bindable(true), Category("Appearance"), DefaultValue(typeof(string), "string.Empty"), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
//        public string AddImageUrl
//        {
//            get
//            {
//                if (ViewState["AddImageUrl"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["AddImageUrl"];
//            }
//            set { ViewState["AddImageUrl"] = value; }
//        }

//        /// <summary>
//        /// The image to display for the remove button
//        /// </summary>
//        [Bindable(true), Category("Appearance"), DefaultValue(typeof(string), "string.Empty"), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
//        public string RemoveImageUrl
//        {
//            get
//            {
//                if (ViewState["RemoveImageUrl"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["RemoveImageUrl"];
//            }
//            set { ViewState["RemoveImageUrl"] = value; }
//        }

//        /// <summary>
//        /// The image to display for the add all button
//        /// </summary>
//        [Bindable(true), Category("Appearance"), DefaultValue(typeof(string), "string.Empty"), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
//        public string AddAllImageUrl
//        {
//            get
//            {
//                if (ViewState["AddAllImageUrl"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["AddAllImageUrl"];
//            }
//            set { ViewState["AddAllImageUrl"] = value; }
//        }

//        /// <summary>
//        /// The image to display for the remove all button
//        /// </summary>
//        [Bindable(true), Category("Appearance"), DefaultValue(typeof(string), "string.Empty"), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
//        public string RemoveAllImageUrl
//        {
//            get
//            {
//                if (ViewState["RemoveAllImageUrl"] == null)
//                    return string.Empty;
//                else
//                    return (string)ViewState["RemoveAllImageUrl"];
//            }
//            set { ViewState["RemoveAllImageUrl"] = value; }
//        }

//        #endregion

//        #region Behaviour

//        /// <summary>
//        /// If generating images whether to cache them
//        /// </summary>
//        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
//        public string CacheName
//        {
//            get
//            {
//                if (ViewState["CacheName"] == null)
//                    return null;
//                else
//                    return (string)ViewState["CacheName"];
//            }
//            set { ViewState["CacheName"] = value; }
//        }

//        /// <summary>
//        /// The validation group for the button.
//        /// </summary>
//        [Bindable(true), Category("Behaviour"), DefaultValue("")]
//        public string ValidationGroup
//        {
//            get
//            {
//                if (ViewState["ValidationGroup"] == null)
//                    return "";
//                else
//                    return (string)ViewState["ValidationGroup"];
//            }
//            set { ViewState["ValidationGroup"] = value; }
//        }

//        /// <summary>
//        /// Whether the button cause validation
//        /// </summary>
//        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
//        public bool CausesValidation
//        {
//            get
//            {
//                if (ViewState["CausesValidation"] == null)
//                    return false;
//                else
//                    return (bool)ViewState["CausesValidation"];
//            }
//            set { ViewState["CausesValidation"] = value; }
//        }

//        #endregion

//        #endregion

//        #region ViewState

//        ///
//        protected override void LoadViewState(object savedState)
//        {
//            object[] state = (object[])savedState;
//            if (state != null)
//            {
//                if (state[0] != null)
//                    base.LoadViewState(state[0]);

//                Items.Clear();
//                if (state[1] != null)
//                    ((IStateManager)Items).LoadViewState(state[1]);

//                if (state[2] != null)
//                    ((IStateManager)PanelStyle).LoadViewState(state[2]);

//                if (state[3] != null)
//                    ((IStateManager)TitleStyle).LoadViewState(state[3]);

//                //if (state[4] != null)
//                //    ((IStateManager)SelectedNodeStyle).LoadViewState(state[4]);

//                //if (state[5] != null)
//                //    ((IStateManager)HoverNodeStyle).LoadViewState(state[5]);

//            }
//        }

//        /// 
//        protected override object SaveViewState()
//        {
//            object[] state = new object[6];
//            state[0] = base.SaveViewState();
//            state[1] = ((IStateManager)Items).SaveViewState();
//            if (_panelStyle != null)
//                state[2] = ((IStateManager)_panelStyle).SaveViewState();

//            if (_titleStyle != null)
//                state[3] = ((IStateManager)_titleStyle).SaveViewState();

//            //if (_selectStyle != null)
//            //    state[4] = ((IStateManager)_selectStyle).SaveViewState();

//            //if (_hoverStyle != null)
//            //    state[5] = ((IStateManager)_hoverStyle).SaveViewState();

//            if (state[0] == null && state[1] == null)
//                return null;
//            else
//                return state;
//        }

//        ///
//        protected override void TrackViewState()
//        {
//            base.TrackViewState();
//            if (_panelStyle != null)
//                ((IStateManager)_panelStyle).TrackViewState();

//            if (_titleStyle != null)
//                ((IStateManager)_titleStyle).TrackViewState();

//            //if (_selectStyle != null)
//            //    ((IStateManager)_selectStyle).TrackViewState();

//            //if (_hoverStyle != null)
//            //    ((IStateManager)_hoverStyle).TrackViewState();

//            ((IStateManager)Items).TrackViewState();
//        }

//        #endregion

//        #region IPostBackEventHandler Members

//        ///
//        public void RaisePostBackEvent(string eventArgument)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region Databinding

//        ///
//        protected override void PerformSelect()
//        {
//            if (!IsBoundUsingDataSourceID)
//                OnDataBinding(EventArgs.Empty);

//            GetData().Select(CreateDataSourceSelectArguments(), onDataSourceViewSelectCallback);

//            RequiresDataBinding = false;
//            MarkAsDataBound();

//            OnDataBound(EventArgs.Empty);
//        }

//        private void onDataSourceViewSelectCallback(IEnumerable data)
//        {
//            if (IsBoundUsingDataSourceID)
//                OnDataBinding(EventArgs.Empty);
//            PerformDataBinding(data);
//        }

//        ///
//        protected override void PerformDataBinding(IEnumerable data)
//        {
//            base.PerformDataBinding(data);

//            if (data != null)
//            {
//                foreach (object dataItem in data)
//                {
//                    if (DataTextField.Length > 0 && DataValueField.Length > 0)
//                        Items.Add(new ListItem(DataBinder.GetPropertyValue(dataItem, DataTextField, null), DataBinder.GetPropertyValue(dataItem, DataValueField, null)));
//                    else if (DataTextField.Length > 0)
//                        Items.Add(DataBinder.GetPropertyValue(dataItem, DataTextField, null));
//                    else if (DataValueField.Length > 0)
//                        Items.Add(DataBinder.GetPropertyValue(dataItem, DataValueField, null));
//                    else
//                    {
//                        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(dataItem);
//                        if (props.Count > 0)
//                        {
//                            if (props[0].GetValue(dataItem) != null)
//                                Items.Add(props[0].GetValue(dataItem).ToString());
//                            else
//                                Items.Add(string.Empty);
//                        }
//                    }
//                }
//            }
//        }

//        #endregion

//        #region Render

//        ///
//        protected override void OnPreRender(EventArgs e)
//        {
//            base.OnPreRender(e);

//            if (ButtonGenerateStyle != Button.GenerateStyles.None)
//                prepareButtons();
//        }

//        ///
//        protected override void Render(HtmlTextWriter writer)
//        {
//            base.AddAttributesToRender(writer);
//            writer.RenderBeginTag("table");

//            //Titles
//            writer.RenderBeginTag("tr");

//            writer.RenderBeginTag("th");
//            TitleStyle.AddAttributesToRender(writer);
//            writer.Write("Panel 1");
//            writer.RenderEndTag();

//            writer.RenderBeginTag("th");
//            writer.RenderEndTag();

//            writer.RenderBeginTag("th");
//            TitleStyle.AddAttributesToRender(writer);
//            writer.Write("Panel 1");
//            writer.RenderEndTag();

//            writer.RenderEndTag();

//            // Panels
//            writer.RenderBeginTag("tr");

//            writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "6");
//            writer.RenderBeginTag("td");
//            PanelStyle.AddAttributesToRender(writer);
//            writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "multiple");
//            writer.RenderBeginTag(HtmlTextWriterTag.Select);
//            renderOptions(writer, false);
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderBeginTag("td");
//            writer.RenderEndTag();

//            writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "6");
//            writer.RenderBeginTag("td");
//            PanelStyle.AddAttributesToRender(writer);
//            writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "multiple");
//            writer.RenderBeginTag(HtmlTextWriterTag.Select);
//            renderOptions(writer, true);
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderEndTag();

//            // Buttons
//            writer.RenderBeginTag("tr");
//            writer.RenderBeginTag("td");
//            writer.AddAttribute(HtmlTextWriterAttribute.Value, ">");
//            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
//            writer.RenderBeginTag(HtmlTextWriterTag.Input);writer.RenderEndTag();
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderBeginTag("tr");
//            writer.RenderBeginTag("td");
//            writer.AddAttribute(HtmlTextWriterAttribute.Value, "<");
//            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
//            writer.RenderBeginTag(HtmlTextWriterTag.Input); writer.RenderEndTag();
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderBeginTag("tr");
//            writer.RenderBeginTag("td");
//            writer.AddAttribute(HtmlTextWriterAttribute.Value, ">>");
//            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
//            writer.RenderBeginTag(HtmlTextWriterTag.Input); writer.RenderEndTag();
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderBeginTag("tr");
//            writer.RenderBeginTag("td");
//            writer.AddAttribute(HtmlTextWriterAttribute.Value, "<<");
//            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
//            writer.RenderBeginTag(HtmlTextWriterTag.Input); writer.RenderEndTag();
//            writer.RenderEndTag();
//            writer.RenderEndTag();

//            writer.RenderEndTag();
//        }

//        #endregion

//        #region Private

//        private void renderOptions( HtmlTextWriter writer, bool selected)
//        {
//            foreach (ListItem item in Items)
//            {
//                if (item.Selected == selected)
//                {
//                    item.Attributes.AddAttributes(writer);
//                    if (!item.Enabled)
//                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
//                    if (!string.IsNullOrEmpty(item.Value))
//                        writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Value);
//                    else
//                        writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Text);
//                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
//                    if (string.IsNullOrEmpty(item.Text))
//                        writer.Write(item.Value);
//                    else
//                        writer.Write(item.Text);
//                    writer.RenderEndTag();
//                }
//            }
//        }

//        private void prepareButtons()
//        {
//            Font textfont = CtrlHelper.ConvertFont(Font);
//            Color fColor = Color.Black;
//            if (ButtonGenerateStyle == Button.GenerateStyles.Vista)
//                fColor = Color.White;

//            _addImg = new Button.GenerateImage[4];
//            _addImg[0] = prepareButton(">", AddImageUrl, Button.GenerateImage.ButtonState.Normal, "addNormImg", textfont, fColor);
//            _addImg[1] = prepareButton(">", AddImageUrl, Button.GenerateImage.ButtonState.Hover, "addHovImg", textfont, fColor);
//            _addImg[2] = prepareButton(">", AddImageUrl, Button.GenerateImage.ButtonState.Clicked, "addClickImg", textfont, fColor);
//            _addImg[3] = prepareButton(">", AddImageUrl, Button.GenerateImage.ButtonState.Disabled, "addDisImg", textfont, fColor);

//            _remImg = new Button.GenerateImage[4];
//            _remImg[0] = prepareButton("<", RemoveImageUrl, Button.GenerateImage.ButtonState.Normal, "remNormImg", textfont, fColor);
//            _remImg[1] = prepareButton("<", RemoveImageUrl, Button.GenerateImage.ButtonState.Hover, "remHovImg", textfont, fColor);
//            _remImg[2] = prepareButton("<", RemoveImageUrl, Button.GenerateImage.ButtonState.Clicked, "remClickImg", textfont, fColor);
//            _remImg[3] = prepareButton("<", RemoveImageUrl, Button.GenerateImage.ButtonState.Disabled, "remDisImg", textfont, fColor);

//            _addAllImg = new Button.GenerateImage[4];
//            _addImg[0] = prepareButton(">>", AddAllImageUrl, Button.GenerateImage.ButtonState.Normal, "addAllNormImg", textfont, fColor);
//            _addImg[1] = prepareButton(">>", AddAllImageUrl, Button.GenerateImage.ButtonState.Hover, "addAllHovImg", textfont, fColor);
//            _addImg[2] = prepareButton(">>", AddAllImageUrl, Button.GenerateImage.ButtonState.Clicked, "addAllClickImg", textfont, fColor);
//            _addImg[3] = prepareButton(">>", AddAllImageUrl, Button.GenerateImage.ButtonState.Disabled, "addAllDisImg", textfont, fColor);

//            _remAllImg = new Button.GenerateImage[4];
//            _remAllImg[0] = prepareButton("<<", RemoveImageUrl, Button.GenerateImage.ButtonState.Normal, "remAllNormImg", textfont, fColor);
//            _remAllImg[1] = prepareButton("<<", RemoveImageUrl, Button.GenerateImage.ButtonState.Hover, "remAllHovImg", textfont, fColor);
//            _remAllImg[2] = prepareButton("<<", RemoveImageUrl, Button.GenerateImage.ButtonState.Clicked, "remAllClickImg", textfont, fColor);
//            _remAllImg[3] = prepareButton("<<", RemoveImageUrl, Button.GenerateImage.ButtonState.Disabled, "remAllDisImg", textfont, fColor);
//        }

//        private Button.GenerateImage prepareButton(string text, string imageUrl, Button.GenerateImage.ButtonState state, string attrName, Font textFont, Color fColor)
//        {
//            if (string.IsNullOrEmpty(imageUrl))
//                text = null;
//            Button.GenerateImage img = new Button.GenerateImage(ButtonGenerateStyle, Button.GenerateImage.ButtonState.Normal, Size.Empty, text, textFont, fColor, imageUrl, CacheName);
//            ScriptManager.RegisterExpandoAttribute(this, ClientID, attrName, img.FileName + WebGfx.Extension, false);
//            return img;
//        }

//        ListItemCollection _items;
//        AdvStyle _panelStyle;
//        AdvStyle _titleStyle;

//        Button.GenerateImage[] _addImg;
//        Button.GenerateImage[] _remImg;
//        Button.GenerateImage[] _addAllImg;
//        Button.GenerateImage[] _remAllImg;

//        #endregion
//    }
//}