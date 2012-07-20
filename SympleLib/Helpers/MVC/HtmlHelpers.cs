using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SympleLib.Helpers.MVC
{
    public static class HtmlHelpers
    {
        #region uiWidget

        public static HtmlString UiWidgetStart(this HtmlHelper helper, string title)
        {
            return UiWidgetStart(helper, title, "", "");
        }

        public static HtmlString UiWidgetStart(this HtmlHelper helper, string title, string id)
        {
            return UiWidgetStart(helper, title, id, "");
        }

        public static HtmlString UiWidgetStart(this HtmlHelper helper, string title, string id, string adtlClasses)
        {
            var sbWidget = new StringBuilder();
            sbWidget.Append("<div id='" + id + "' class='ui-widget " + adtlClasses + "'>");
            sbWidget.Append("<div class='ui-widget-header ui-corner-top'>" + title + "</div>");
            sbWidget.Append("<div class='ui-widget-content ui-corner-bottom'>");
            return new HtmlString(sbWidget.ToString());
        }

        public static HtmlString UiWidgetEnd(this HtmlHelper helper)
        {
            return new HtmlString("</div></div>");
        }

        #endregion

        #region Data Display Helpers

        public static MvcHtmlString DisplayDataField(this HtmlHelper helper, string label, object data)
        {
            return DisplayDataField(helper, label, data, "");
        }

        public static MvcHtmlString DisplayDataField(this HtmlHelper helper, string label, object data, string htmlToRight)
        {
            var sbOutput = new StringBuilder();
            sbOutput.AppendLine("<div class='display-entry'>");
            sbOutput.AppendLine(String.Format("   <div class='display-label'>{0}</div>", label));
            sbOutput.Append("   <div class='display-field'>");

            data = data ?? "";

            sbOutput.Append(htmlToRight);

            sbOutput.Append(data + "</div>");
            sbOutput.AppendLine("</div>");

            return new MvcHtmlString(sbOutput.ToString());
        }


        //-- Edit Data Field
        public static MvcHtmlString EditDataField(this HtmlHelper helper, string label, MvcHtmlString editor)
        {
            return EditDataField(helper, label, editor, null, null);
        }

        public static MvcHtmlString EditDataField(this HtmlHelper helper, string label, MvcHtmlString editor, MvcHtmlString validator)
        {
            return EditDataField(helper, label, editor, validator, null);
        }

        public static MvcHtmlString EditDataField(this HtmlHelper helper, string label, MvcHtmlString editor, MvcHtmlString validator, MvcHtmlString instructions)
        {
            var sbOutput = new StringBuilder();
            sbOutput.AppendFormat("<div class='control-group'>");
            sbOutput.AppendFormat("<label class='control-label'>{0}</label>", label);
            sbOutput.Append("<div class='controls'>");
            sbOutput.Append(editor);

            sbOutput.Append(validator);

            if (string.IsNullOrEmpty((instructions ?? new MvcHtmlString("")).ToString()) != true)
            {
                sbOutput.AppendFormat("<p class='help-block'>{0}</p>", instructions);
            }

            sbOutput.Append(@"</div>");
            sbOutput.Append(@"</div>");

            return new MvcHtmlString(sbOutput.ToString());
        }

        #endregion

        #region General HTML

        public static MvcHtmlString ToHtmlString(this HtmlHelper helper, String sourceString)
        {
            return new MvcHtmlString(sourceString);
        }

        public static MvcHtmlString TimeSpanString(this HtmlHelper helper, TimeSpan? Span)
        {
            if (Span != null)
            {
                return new MvcHtmlString(new DateTime(Span.Value.Ticks).ToString("h:mm tt"));
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        #endregion
    }
}
