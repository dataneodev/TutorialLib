using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TutorialsLib.UI
{
    internal class VideoListItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VideoItemTemplate { get; set; }
        public DataTemplate FolderItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is VideoItem)
                return this.VideoItemTemplate;

            if (item is FolderItem)
                return this.FolderItemTemplate;

            throw new ArgumentException("Unknow DataTemplate for object");
        }
    }
}
