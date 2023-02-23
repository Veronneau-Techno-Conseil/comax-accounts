using System;
using System.Reflection;

namespace CommunAxiom.Accounts.Helpers
{
    public static class DateHelper
    {
        public static void Localize<T>(this T obj) where T : class
        {
            var dateType = typeof(DateTime);
            var tp = typeof(T);
            var settables = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach ( var t in settables )
            {
                if(t.PropertyType == dateType)
                {
                    DateTime val = (DateTime)t.GetValue(obj, null);
                    val = DateTime.SpecifyKind(val.ToLocalTime(), DateTimeKind.Local);
                    t.SetValue(obj, val, null);
                }
            }

            var nullDateType = typeof(Nullable<DateTime>);

            settables = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var t in settables)
            {
                if (t.PropertyType == nullDateType)
                {
                    DateTime? val = (DateTime?)t.GetValue(obj, null);
                    if (val.HasValue)
                    {
                        val = DateTime.SpecifyKind(val.Value.ToLocalTime(), DateTimeKind.Local);
                        t.SetValue(obj, val, null);
                    }
                }
            }
        }

        public static void Globalize<T>(this T obj) where T : class
        {
            var dateType = typeof(DateTime);
            var tp = typeof(T);
            var settables = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var t in settables)
            {
                if (t.PropertyType == dateType)
                {
                    DateTime val = (DateTime)t.GetValue(obj, null);
                    val = DateTime.SpecifyKind(val.ToUniversalTime(), DateTimeKind.Utc);
                    t.SetValue(obj, val, null);
                }
            }

            var nullDateType = typeof(Nullable<DateTime>);

            settables = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var t in settables)
            {
                if (t.PropertyType == nullDateType)
                {
                    DateTime? val = (DateTime?)t.GetValue(obj, null);
                    if (val.HasValue)
                    {
                        val = DateTime.SpecifyKind(val.Value.ToUniversalTime(), DateTimeKind.Utc);
                        t.SetValue(obj, val, null);
                    }
                }
            }
        }

    }
}
