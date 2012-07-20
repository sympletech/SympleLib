using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SympleLib.Helpers.MVC
{
    public class ListToSelectList
    {
        public static List<SelectListItem> ConvertToSelectList<T>(IEnumerable<T> baseList, string valueFiled, string textField) where T : class
        {
            return ConvertToSelectList(baseList, valueFiled, textField, false, "");
        }

        public static List<SelectListItem> ConvertToSelectList<T>(IEnumerable<T> baseList, string valueFiled, string textField, bool includeFirstEntrySelect, string firstEntrySelectText) where T : class
        {
            var result = new List<SelectListItem>();

            Type ty = typeof(T);
            if (baseList != null)
            {
                foreach (var item in baseList)
                {
                    var valprop = ty.GetProperty(valueFiled);
                    var val = valprop != null ? valprop.GetValue(item, null) : "";
                    var txtprop = ty.GetProperty(textField);
                    var txt = txtprop != null ? txtprop.GetValue(item, null) : "";

                    result.Add(new SelectListItem
                    {
                        Text = txt != null ? txt.ToString() : "",
                        Value = val != null ? val.ToString() : ""
                    });
                }
            }

            result = result.Distinct().OrderBy(x => x.Text).ToList();

            if (includeFirstEntrySelect)
            {
                result.Insert(0, new SelectListItem
                {
                    Text = firstEntrySelectText,
                    Value = ""
                });

            }

            return result;
        }

        public static List<SelectListItem> ConvertToSelectList(Hashtable baseTable, bool includeFirstEntrySelect, string firstEntrySelectText)
        {
            var result = (from DictionaryEntry item in baseTable
                          select new SelectListItem
                              {
                                  Text = item.Value.ToString(),
                                  Value = item.Key.ToString()
                              }).ToList();

            result = result.OrderBy(x => x.Text).ToList();

            if (includeFirstEntrySelect)
            {
                result.Insert(0, new SelectListItem
                {
                    Text = firstEntrySelectText,
                    Value = ""
                });

            }

            return result;
        }

        public static List<SelectListItem> ConvertToSelectList(IEnumerable<object> baseList)
        {
            return baseList.Select(itm => new SelectListItem
                {
                    Text = itm.ToString(), Value = itm.ToString()
                }).ToList();
        }
    }
}
