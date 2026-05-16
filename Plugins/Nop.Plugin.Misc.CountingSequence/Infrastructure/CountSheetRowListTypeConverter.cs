using Nop.Plugin.Misc.CountingSequence.Models.CountingSequence;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure
{
    public partial class CountSheetRowListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is not string str || string.IsNullOrEmpty(str))
                return null;

            try
            {
                using var tr = new StringReader(str);
                var xmlS = new XmlSerializer(typeof(List<CountSheetModel>));
                return (List<CountSheetModel>)xmlS.Deserialize(tr);
            }
            catch
            {
                return null;
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            if (value is not List<CountSheetModel> list)
                return string.Empty;

            var sb = new StringBuilder();
            using var tw = new StringWriter(sb);
            var xmlS = new XmlSerializer(typeof(List<CountSheetModel>));
            xmlS.Serialize(tw, list);

            return sb.ToString();
        }
    }
}
