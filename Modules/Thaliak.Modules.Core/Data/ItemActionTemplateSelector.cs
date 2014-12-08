using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xiv = SaintCoinach.Xiv;

namespace Thaliak.Modules.Core.Data {
    public class ItemActionTemplateSelector : DataTemplateSelector {
        private Dictionary<Type, DataTemplate> _Templates = new Dictionary<Type, DataTemplate>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            if (item == null)
                return new DataTemplate();
            var t = item.GetType();
            if (_Templates.ContainsKey(t))
                return _Templates[t];

            DataTemplate template = new DataTemplate();

            var currentType = t;
            while (currentType != null && currentType.FullName.StartsWith("SaintCoinach.")) {
                var innerControlName = currentType.FullName.Replace("SaintCoinach.", "Thaliak.Modules.Core.Views.Main.");
                var innerControlType = Type.GetType(innerControlName);
                if (innerControlType != null) {
                    var controlFactory = new FrameworkElementFactory(innerControlType);
                    template.VisualTree = controlFactory;
                    break;
                }
                currentType = currentType.BaseType;
            }

            _Templates.Add(t, template);
            return template;
        }
    }
}
