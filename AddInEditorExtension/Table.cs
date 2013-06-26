using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace AddInEditorExtension
{
    public partial class Table : Form
    {
        IFeatureClass pFeatureClass;
        IFeature pFeature;
        string pCurrentTask;

        public Table()
        {
            InitializeComponent();            
        }
        //获取当前编辑的要素类
        public void Get_FeatureClass(IFeatureClass _FeatureClass)
        {
            pFeatureClass = _FeatureClass;
        }
        //获取当前编辑的要素及任务类型
        public void AddItem(IFeature _Feature, string _CurrentTask)
        {
            pFeature = _Feature;
            pCurrentTask = _CurrentTask;
        }
        //创建表，增加Current Task:与FeatureClass Name:两个字段
        public void createTable()
        {           
            IFields pFields = pFeatureClass.Fields;
            dataGridView1.ColumnCount = pFields.FieldCount + 2;
            for (int i = 0; i < pFields.FieldCount; i++ )
            {
                string fldName = pFields.get_Field(i).Name;
                dataGridView1.Columns[i + 2].Name = fldName;
                dataGridView1.Columns[i + 2].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            dataGridView1.Columns[0].Name = Convert.ToString("Current Task: ");
            dataGridView1.Columns[0].ValueType = System.Type.GetType("System.String");
            dataGridView1.Columns[1].Name = Convert.ToString("FeatureClass Name: ");
            dataGridView1.Columns[1].ValueType = System.Type.GetType("System.String");

            string[] fldValue = new string[pFeatureClass.Fields.FieldCount + 2];
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                string fldName = pFeatureClass.Fields.get_Field(i).Name;
                if (fldName == pFeatureClass.ShapeFieldName)
                {
                    fldValue[i + 2] = Convert.ToString(pFeature.Shape.GeometryType);
                }
                else
                    fldValue[i + 2] = Convert.ToString(pFeature.get_Value(i));
            }
            fldValue[0] = pCurrentTask;
            IDataset dataset = pFeatureClass as IDataset;
            fldValue[1] = Convert.ToString(dataset.Name);
            dataGridView1.Rows.Add(fldValue);

            if (dataGridView1.Rows[0].Cells[0].Selected ==true)//取消DataGridView第一行一列的选中
            {
                dataGridView1.Rows[0].Cells[0].Selected = false;
            }
        }

        private void Table_Load(object sender, EventArgs e)
        {

        }
        //类型转换
        public string ParseFieldType(esriFieldType TableFieldType)
        {
            switch (TableFieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }
    }
}
