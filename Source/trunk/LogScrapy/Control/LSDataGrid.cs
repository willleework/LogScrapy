using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LogScrapy
{
    /// <summary>
    /// DataGrid控件
    /// </summary>
    [TemplatePart(Name = "dataGrid", Type = typeof(DataGrid))]
    public class LSDataGrid : Control
    {
        public EventHandler<IEnumerable> GeneratingColumnsEvent;
        public EventHandler<IEnumerable> BindingDataEvent;
        private DataGrid dataGrid = null;

        private IEnumerable _dataSource = null;
        /// <summary>
        /// 数据源
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                if (value == null|| !IsObjectGenericIEnumerable(value))
                {
                    dataGrid.ItemsSource = null;
                    dataGrid.Columns.Clear();
                    return;                    
                }
                CreateColumns(value);
                BindDatas(value);
            }
        }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <param name="dataSource"></param>
        private void CreateColumns(IEnumerable dataSource)
        {
            if (GeneratingColumnsEvent == null)
            {
                Type type = dataSource.GetType();
                Type[] genericArgTypes = type.GetGenericArguments();
                Type t = genericArgTypes[0];
                PropertyInfo[] PropertyInfos = t.GetProperties();

                foreach (PropertyInfo p in PropertyInfos)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.Header = p.Name;
                    column.Binding = new Binding(p.Name);
                    dataGrid.Columns.Add(column);
                }
            }
            else
            {
                GeneratingColumnsEvent(this.dataGrid, dataSource);
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataSource"></param>
        private void BindDatas(IEnumerable dataSource)
        {
            if (BindingDataEvent == null)
            {
                dataGrid.ItemsSource = dataSource;
            }
            else
            {
                BindingDataEvent(this.dataGrid, dataSource);
            }
        }

        /// <summary>
        /// 数据类型检测
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool IsObjectGenericIEnumerable(object o)
        {
            return o is System.Collections.IEnumerable && (o.GetType().IsGenericType || o is Array);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dataGrid = EnforceInstance<DataGrid>("dataGrid");
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
    }
}
