using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace AddInEditorExtension
{
    /// <summary>
    /// ValidateFeaturesExtension class implementing custom ESRI Editor Extension functionalities.
    /// </summary>
    public class ValidateFeaturesExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        Table table;

        public ValidateFeaturesExtension()
        {
        }       
        protected override void OnStartup()
        {
            IEditor theEditor = ArcMap.Editor;
            Events.OnStartEditing += new IEditEvents_OnStartEditingEventHandler(Events_OnStartEditing);
            Events.OnStopEditing += new IEditEvents_OnStopEditingEventHandler(Events_OnStopEditing);
        }
        void Events_OnStartEditing()
        {           
            //Wire OnCreateFeature edit event.
            Events.OnCreateFeature += new IEditEvents_OnCreateFeatureEventHandler
                (Events_OnCreateFeature);
            //Wire onChangeFeature edit event.
            Events.OnChangeFeature += new IEditEvents_OnChangeFeatureEventHandler
                (Events_OnChangeFeature);
            //开启编辑时创建Table
            table = new Table();
        }
        //Invoked at the end of an editor session (Editor->Stop Editing).
        void Events_OnStopEditing(bool Save)
        {
            //Unwire OnCreateFeature edit event.
            Events.OnCreateFeature -= new IEditEvents_OnCreateFeatureEventHandler
                (Events_OnCreateFeature);
            //Unwire onChangeFeature edit event.
            Events.OnChangeFeature -= new IEditEvents_OnChangeFeatureEventHandler
                (Events_OnChangeFeature);
        }
        //Invoked when a feature is created.
        void Events_OnCreateFeature(ESRI.ArcGIS.Geodatabase.IObject obj)
        {
            IFeature inFeature = (IFeature)obj;
            IFeatureClass featureClass = inFeature.Class as IFeatureClass;
            table.Get_FeatureClass(featureClass);
            table.AddItem(inFeature, "Create New Feature:");
            table.createTable();
            table.Show();
        }
        //Invoked when a feature is modified.
        void Events_OnChangeFeature(ESRI.ArcGIS.Geodatabase.IObject obj)
        {
            IFeature inFeature = (IFeature)obj;   
            IFeatureClass featureClass = inFeature.Class as IFeatureClass;
            table.Get_FeatureClass(featureClass);
            table.AddItem(inFeature, "Modify Feature:");
            table.createTable();
            table.Show();
        }
        protected override void OnShutdown()
        {
        }
        #region Editor Events

        #region Shortcut properties to the various editor event interfaces
        private IEditEvents_Event Events
        {
            get { return ArcMap.Editor as IEditEvents_Event; }
        }
        private IEditEvents2_Event Events2
        {
            get { return ArcMap.Editor as IEditEvents2_Event; }
        }
        private IEditEvents3_Event Events3
        {
            get { return ArcMap.Editor as IEditEvents3_Event; }
        }
        private IEditEvents4_Event Events4
        {
            get { return ArcMap.Editor as IEditEvents4_Event; }
        }
        #endregion

        void WireEditorEvents()
        {
            //
            //  TODO: Sample code demonstrating editor event wiring
            //
            Events.OnCurrentTaskChanged += delegate
            {
                if (ArcMap.Editor.CurrentTask != null)
                    System.Diagnostics.Debug.WriteLine(ArcMap.Editor.CurrentTask.Name);
            };
            Events2.BeforeStopEditing += delegate(bool save) { OnBeforeStopEditing(save); };
        }

        void OnBeforeStopEditing(bool save)
        {
        }
        #endregion

    }

}
