using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LogScrapy
{
    [TemplatePart(Name = "dataGrid", Type = typeof(DataGrid))]
    public class LSDataGrid : Control
    {
        ObservableCollection<dynamic> items = new ObservableCollection<dynamic>();
        private DataGrid dataGrid = null;
        /// <summary>
        /// 数据源
        /// </summary>
        public ObservableCollection<object> DataSource { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dataGrid = EnforceInstance<DataGrid>("dataGrid");
            //InitColumns();
        }

        /// <summary>
        /// 找到模板子元素
        /// </summary>
        /// <typeparam name="T">子元素的类型</typeparam>
        /// <param name="partName">子元素的Name</param>
        /// <returns>返回子元素对象</returns>
        protected T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
        {
            T element = GetTemplateChild(partName) as T ?? new T();
            return element;
        }

        int index = 1;
        public void InitColumns()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    dynamic item = new ExpandoObject();
            //    item.A = "Property A value - " + i.ToString();
            //    item.B = "Property B value - " + i.ToString();
            //    items.Add(item);
            //}

            dataGrid.Columns.Clear();

            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "A" + index.ToString(), Binding = new Binding("A" + index.ToString()) });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "B" + index.ToString(), Binding = new Binding("B" + index.ToString()) });
            index++;
            dataGrid.ItemsSource = items;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomBoundColumn : DataGridBoundColumn
    {
        public string TemplateName { get; set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var binding = new Binding(((Binding)Binding).Path.Path);
            binding.Source = dataItem;

            var content = new ContentControl();

            content.ContentTemplate = (DataTemplate)cell.FindResource(TemplateName);

            content.SetBinding(ContentControl.ContentProperty, binding);

            return content;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateElement(cell, dataItem);
        }
    }
}
